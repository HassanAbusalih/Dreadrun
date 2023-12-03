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
        [SerializeField] float ticksPerSecond = 30;
        float tickRate;
        bool isConnected;

        public bool isHost;

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
            tickRate = 1 / ticksPerSecond;
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
                    ScenePacket scenePacket = new ScenePacket().Deserialize(buffer, index);
                    SceneManager.LoadScene(scenePacket.sceneName);
                    if (scenePacket.sceneName == mainScene)
                    {
                        PlayerInMainScenePacket playerInMainScene = new PlayerInMainScenePacket(true);
                        SendPacket(playerInMainScene.Serialize());
                    }
                    break;

                case BasePacket.PacketType.ID:
                    IDPacket _idPacket = new IDPacket().Deserialize(buffer, index); 
                    isHost = _idPacket.isHost;
                    networkComponent.ClientID = packet.gameObjectID;
                    break;

                case BasePacket.PacketType.Instantiation:
                    InstantiationPacket instantiationPacket = new InstantiationPacket().Deserialize(buffer, index);
                    GameObject objectToSpawn = Instantiate(Resources.Load(instantiationPacket.prefabName) as GameObject, 
                        instantiationPacket.position, 
                        instantiationPacket.rotation);

                    objectToSpawn.GetComponent<NetworkComponent>().SetIDs(instantiationPacket.OwnershipID, instantiationPacket.gameObjectID);
                    allNetworkObjects.Add(objectToSpawn.GetComponent<NetworkComponent>());
                    break;

                case BasePacket.PacketType.ServerLobbyPacket:
                    LobbyStatusPacket lobbyStatusPacket = new LobbyStatusPacket().Deserialize(buffer, index);
                    OnLobbyUpdate?.Invoke(lobbyStatusPacket.playerIDs, lobbyStatusPacket.playerStatuses);
                    break;

                case BasePacket.PacketType.Position:
                    PositionPacket positionPacket = new PositionPacket().Deserialize(buffer, index);
                    PlayerNetworkComponent component = FindNetworkComponent(positionPacket.gameObjectID) as PlayerNetworkComponent;
                    if (component != null)
                    {
                        component.SetTargetPosition(positionPacket.position, positionPacket.rotation);
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