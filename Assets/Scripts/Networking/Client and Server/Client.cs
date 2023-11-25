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
                try
                {
                    if (socket.Available > 0)
                    {
                        byte[] buffer = new byte[socket.Available];
                        socket.Receive(buffer);
                        BasePacket packet = new BasePacket().Deserialize(buffer);
                        if (packet != null)
                        {
                            SwitchCaseHell(packet, buffer);
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

        private void SwitchCaseHell(BasePacket packet, byte[] buffer)
        {
            switch (packet.packetType)
            {
                case BasePacket.PacketType.ScenePacket:
                    Debug.LogError("Scene Packet Received! Scene name is: ");
                    ScenePacket scenePacket = new ScenePacket().Deserialize(buffer);
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

                case BasePacket.PacketType.ServerLobbyPacket:
                    LobbyStatusPacket lobbyStatusPacket = new LobbyStatusPacket().Deserialize(buffer);
                    //Debug.LogError("Server Lobby Packet Received! Player count is: " + lobbyStatusPacket.playerIDs.Count);
                    OnLobbyUpdate?.Invoke(lobbyStatusPacket.playerIDs, lobbyStatusPacket.playerStatuses);
                    break;
            }
        }
    }
}