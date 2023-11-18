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
            isCalled = false;
            queueSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);

            queueSocket.Blocking = false;
            queueSocket.Bind(new IPEndPoint(IPAddress.Any, 3000));
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
                        LobbyPacket lobbyPacket = packet as LobbyPacket;
                        lobbyPacket.Deserialize(buffer);
                        OnServerLobbyUpdate?.Invoke(lobbyPacket.playerID, lobbyPacket.isReady);
                        break;
                }
                foreach(PlayerSocket player in clients)
                {
                    if(player == playerSocket)
                    {
                        continue;
                    }
                    SendData(packet, player.socket);

                }
            }
            Invoke(nameof(CallAgain), tickRate);
        }

        private void TryToAcceptClient(Socket _queueSocket)
        {
            try
            {
                //Debug.LogError("IM IN THE TRY TO ACCEPT CLIENT BABY");
                Socket newSocket = _queueSocket.Accept();

                //Debug.LogError("MY FAMILY ACTUALLY ACCEPTED ME BUT MY GROUP DID NOT :(");

                PlayerSocket playerSocket = new PlayerSocket(newSocket);
                playerSocket.playerID = GenerateUniqueClientID();

                //Debug.LogError("MY CLIENT ID " + playerSocket.playerID);

                playerSocket.socket.Send(new IDPacket(playerSocket.playerID).Serialize());
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
                BasePacket packet = new BasePacket();
                packet.Deserialize(buffer);
                return (packet, buffer);
            }
            return (null, null);
        }

        public void SendData(BasePacket packet, Socket socket)
        {
            if (isCalled) { return; }
            isCalled = true;
            byte[] serializedPacket = packet.Serialize();
            socket.Send(serializedPacket);
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

        public void SendToAllClients(BasePacket packet)
        {
            foreach (var client in clients)
            {
                SendData(packet, client.socket);
            }
        }
    }
}
