using System;
using System.Net.Sockets;
using System.Net;
using NetworkingLibrary;
using UnityEngine;
using System.Text;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace ClientLibrary
{
    public class Client : MonoBehaviour
    {
        [SerializeField] float tickRate;
        [SerializeField] bool isCalled;
        bool isConnected;

        protected Socket socket;
        public NetworkComponent networkComponent;

        public static Client Instance;

        public Action ConnectedToServerEvent;
        public Action<List<string>, List<bool>> OnLobbyUpdate;

        //127.0.0.1
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

        protected virtual void Start()
        {
            isCalled = false;
        }


        public void ConnectToServer(string _ipAddress)
        {
            try
            {
                socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

                Debug.LogError("Client is trying to connect to server");
                socket.Connect(new IPEndPoint(IPAddress.Parse(_ipAddress), 3000));
                socket.Blocking = false;
                isConnected = true;

                if (isConnected) ConnectedToServerEvent?.Invoke();
                Debug.LogError("Client connected!");
                TryGetComponent(out networkComponent);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
        }

        private void Update()
        {
            ProcessOfClientToConnectAndReceiveData();
        }

        void ProcessOfClientToConnectAndReceiveData()
        {
            if (isCalled || !isConnected) { return; }
            isCalled = true;

            try
            {
                (BasePacket packet, byte[] buffer) = ReceiveData(socket);
                if (packet != null)
                {
                    switch (packet.packetType)
                    {
                        case BasePacket.PacketType.unknown:
                            break;
                        case BasePacket.PacketType.none:
                            break;
                        case BasePacket.PacketType.Position:
                            break;
                        case BasePacket.PacketType.Rotation:
                            break;
                        case BasePacket.PacketType.Instantiation:
                            break;
                        case BasePacket.PacketType.ID:
                            networkComponent.ClientID = packet.gameObjectID;
                            break;
                        case BasePacket.PacketType.ServerLobbyPacket:
                            LobbyStatusPacket lobbyStatusPacket = new LobbyStatusPacket().Deserialize(buffer);
                            Debug.LogError("Server Lobby Packet Received! Player count is: " + lobbyStatusPacket.playerIDs.Count);
                            OnLobbyUpdate?.Invoke(lobbyStatusPacket.playerIDs, lobbyStatusPacket.playerStatuses);
                            break;
                        case BasePacket.PacketType.ScenePacket:
                            ScenePacket scenePacket = new ScenePacket().Deserialize(buffer);
                            Debug.LogError("Scene Packet Received! Scene name is: " + scenePacket.sceneName);
                            SceneManager.LoadScene(scenePacket.sceneName);
                            break;
                        case BasePacket.PacketType.Destruction:
                            break;
                        default:
                            break;
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
            Invoke(nameof(CallAgain), tickRate);
        }


        private (BasePacket, byte[]) ReceiveData(Socket socket)
        {
            if (socket.Available > 0)
            {
                byte[] buffer = new byte[socket.Available];
                socket.Receive(buffer);
                BasePacket packet = new BasePacket().Deserialize(buffer);
                return (packet, buffer);
            }
            return (null, null);
        }

        public void SendPacket(byte[] buffer)
        {
            socket.Send(buffer);
        }

        void CallAgain()
        {
            isCalled = false;
        }
    }
}
