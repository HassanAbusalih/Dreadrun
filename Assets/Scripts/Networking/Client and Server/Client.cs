using System;
using System.Net.Sockets;
using System.Net;
using NetworkingLibrary;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

namespace ClientLibrary
{
    public class Client : MonoBehaviour
    {
        [SerializeField] float tickRate;
        bool isConnected;

        protected Socket socket;
        public NetworkComponent networkComponent;
        public static List<NetworkComponent> allNetworkObjects = new List<NetworkComponent>();

        public static Client Instance;

        public Action ConnectedToServerEvent;
        public Action<List<string>, List<bool>> OnLobbyUpdate;

        [SerializeField] string mainScene;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            TryGetComponent(out networkComponent);
        }

        public void ConnectToServer(string _ipAddress)
        {
            try
            {
                socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

                //Debug.LogError("Client is trying to connect to server");
                socket.Connect(new IPEndPoint(IPAddress.Parse(_ipAddress), 30));
                socket.Blocking = false;
                isConnected = true;

                if (isConnected) ConnectedToServerEvent?.Invoke();
                //Debug.LogError("Client connected!");
                TryGetComponent(out networkComponent);
                StartCoroutine(Tick());
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
        }

        IEnumerator Tick()
        {
            while (true)
            {
                SendPacket(new IDPacket(networkComponent.ClientID).Serialize());
                try
                {
                    if (socket.Available > 0)
                    {
                        byte[] buffer = new byte[socket.Available];
                        socket.Receive(buffer);
                        int index = 0;
                        while (index < buffer.Length)
                        {
                            BasePacket packet = new BasePacket().Deserialize(buffer, index);
                            if (packet != null)
                            {
                                SwitchCaseHell(packet, buffer, index);
                                index += packet.packetSize;
                            }
                        }
                    }
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode != SocketError.WouldBlock)
                    {
                        Console.WriteLine(e);
                    }
                }
                yield return new WaitForSeconds(tickRate);
            }
        }

        public void SendPacket(byte[] buffer)
        {
            socket.Send(buffer);
        }

        private void OnDisable()
        {
            socket?.Dispose();
        }

        private void SwitchCaseHell(BasePacket packet, byte[] buffer, int index)
        {
            switch (packet.packetType)
            {
                case BasePacket.PacketType.ScenePacket:
                    Debug.LogError("Scene Packet Received! Scene name is: ");
                    ScenePacket scenePacket = new ScenePacket().Deserialize(buffer, index);
                    SceneManager.LoadScene(scenePacket.sceneName);
                    if (scenePacket.sceneName == mainScene)
                    {
                        PlayerInMainScenePacket playerInMainScene = new PlayerInMainScenePacket(true);

                        SendPacket(playerInMainScene.Serialize());
                        Debug.LogError("Players in main scene packet is sent");
                    }
                    break;

                case BasePacket.PacketType.ID:
                    networkComponent.ClientID = packet.gameObjectID;
                    break;

                case BasePacket.PacketType.Instantiation:
                    InstantiationPacket instantiationPacket = new InstantiationPacket().Deserialize(buffer, index);
                    Debug.LogError("Player Spawned");
                    GameObject objectToSpawn = Instantiate(Resources.Load(instantiationPacket.prefabName) as GameObject, 
                        instantiationPacket.position, 
                        instantiationPacket.rotation);
                    objectToSpawn.GetComponent<NetworkComponent>().SetIDs(instantiationPacket.OwnershipID, instantiationPacket.gameObjectID);
                    allNetworkObjects.Add(objectToSpawn.GetComponent<NetworkComponent>());
                   Debug.LogError("Sending Spawned player back to server");
                    break;

                case BasePacket.PacketType.ServerLobbyPacket:
                    LobbyStatusPacket lobbyStatusPacket = new LobbyStatusPacket().Deserialize(buffer, index);
                    //Debug.LogError("Server Lobby Packet Received! Player count is: " + lobbyStatusPacket.playerIDs.Count);
                    OnLobbyUpdate?.Invoke(lobbyStatusPacket.playerIDs, lobbyStatusPacket.playerStatuses);
                    break;
                case BasePacket.PacketType.Position:
                    PositionPacket positionPacket = new PositionPacket().Deserialize(buffer, index);
                    PlayerNetworkComponent component = FindNetworkComponent(positionPacket.gameObjectID) as PlayerNetworkComponent;
                    if (component != null)
                    {
                        component.SetTargetPosition(positionPacket.position);
                    }
                    break;
            }
        }

        NetworkComponent FindNetworkComponent(string gameObjectId)
        {
            foreach (NetworkComponent networkComponent in allNetworkObjects)
            {
                if (gameObjectId == networkComponent.GameObjectId)
                {
                    return networkComponent;
                }
            }
            return null;
        }
    }
}