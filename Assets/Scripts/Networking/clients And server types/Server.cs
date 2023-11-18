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

        [SerializeField] int maxIDRange;


        BasePacket serializedPackets;

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
            StartTheProcessOfServerToConnectAndSendData();
        }


        void StartTheProcessOfServerToConnectAndSendData()
        {
            if (isCalled) { return; }
            isCalled = true;

            TryToAcceptClient(queueSocket);
            List<PlayerPacket> packets = new List<PlayerPacket>();
            foreach (PlayerSocket playerSocket in clients)
            {
                packets.Add(new PlayerPacket(ReceiveData(playerSocket.socket), playerSocket.playerID));
            }
            foreach (var packet in packets)
            {
                switch (packet.packet.packetType)
                {
                    case BasePacket.PacketType.Lobby:
                        break;
                }
            }
            Invoke(nameof(CallAgain), tickRate);
        }

        private void TryToAcceptClient(Socket _queueSocket)
        {
            try
            {
                Socket socket = _queueSocket.Accept();
                if (socket != null)
                {
                    clients.Add(new PlayerSocket(socket));

                    string _clientID = GenerateUniqueClientID();
                    clients[clients.Count - 1].playerID = _clientID;
                    IDPacket idPacket = new IDPacket(_clientID);
                    SendData(idPacket, clients[clients.Count - 1].socket);

                    Debug.LogError(_clientID);
                }
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.WouldBlock)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private BasePacket ReceiveData(Socket socket)
        {
            if (socket.Available > 0)
            {
                byte[] buffer = new byte[socket.Available];
                socket.Receive(buffer);
                BasePacket packet = new BasePacket();
                packet.Deserialize(buffer);
                return packet;
            }
            return null;
        }

        private void SendData(BasePacket packet, Socket socket)
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
            Debug.Log("Generated ID: " + guid.ToString());
            return guid.ToString();
        }
    }
}
