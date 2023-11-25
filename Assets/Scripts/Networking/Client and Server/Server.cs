using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using NetworkingLibrary;
using System.Collections.Generic;

namespace Server
{
    public class Server : MonoBehaviour
    {
        [SerializeField] float tickRate;
        protected bool isCalled;
        protected List<PlayerSocket> clients = new List<PlayerSocket>();

        protected Socket queueSocket;

        public static Action<string, Socket> ClientAdded;
        public Action<string, bool> OnServerLobbyUpdate;
        public Action<bool> UpdatePlayerSceneStatus;

        public static Server Instance;

        [SerializeField] int maxIDRange;

        BasePacket serializedPackets;


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
        }

        protected virtual void Start()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 30);
            isCalled = false;
            queueSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);

            queueSocket.Blocking = false;
            queueSocket.Bind(ipEndPoint);
            queueSocket.Listen(10);
        }

        private void Update()
        {
            if (isCalled) { return; }
            isCalled = true;

            if (clients.Count < 3)
            {
                TryToAcceptClient(queueSocket);
            }

            foreach (PlayerSocket playerSocket in clients)
            {
                (BasePacket packet, byte[] buffer) = ReceiveData(playerSocket.socket);
                if (packet == null)
                {
                    continue;
                }
                switch (packet.packetType)
                {
                    case BasePacket.PacketType.PlayerLobbyPacket:
                        LobbyPacket lobbyPacket = new LobbyPacket().Deserialize(buffer);
                        OnServerLobbyUpdate?.Invoke(lobbyPacket.playerID, lobbyPacket.isReady);
                        break;
                    case BasePacket.PacketType.PlayerInMainScenePacket:
                        PlayerInMainScenePacket playerInMainScenePacket = new PlayerInMainScenePacket().Deserialize(buffer);
                        UpdatePlayerSceneStatus?.Invoke(playerInMainScenePacket.inMainScene);
                        Debug.LogError("Players in main scene packet received");
                        break;
                    case BasePacket.PacketType.Instantiation:
                        InstantiationPacket instantiationPacket = new InstantiationPacket().Deserialize(buffer);
                        playerSocket.socket.Send(instantiationPacket.Serialize());
                        Debug.LogError("SPAWNING OTHER PLAYERS!!!!!!!!!!!!!!!!!");
                        break;

                }
            }
            Invoke(nameof(CallAgain), tickRate);
        }

        private void TryToAcceptClient(Socket _queueSocket)
        {
            try
            {
                Socket newSocket = _queueSocket.Accept();

                PlayerSocket playerSocket = new PlayerSocket(newSocket);
                playerSocket.playerID = GenerateUniqueClientID();

                // Debug.LogError("NEW CLIENT ID IS " + playerSocket.playerID);
                IDPacket packet = new IDPacket(playerSocket.playerID);

                playerSocket.socket.Send(packet.Serialize());
                clients.Add(playerSocket);
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.WouldBlock)
                {
                    Console.WriteLine(e);
                }
            }
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

        public void SendData(byte[] buffer, Socket socket)
        {
            if (isCalled) { return; }
            isCalled = true;
            socket.Send(buffer);
            Invoke(nameof(CallAgain), tickRate);
        }

        void CallAgain()
        {
            isCalled = false;
        }

        string GenerateUniqueClientID()
        {
            Guid guid = Guid.NewGuid();
            //Debug.Log("Generated ID: " + guid.ToString());
            return guid.ToString();
        }

        public void SendToAllClients(byte[] buffer)
        {
            foreach (var client in clients)
            {
                client.socket.Send(buffer);
            }
        }

        private void OnDisable()
        {
            queueSocket.Close();
        }
    }
}
