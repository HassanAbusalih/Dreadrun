using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using NetworkingLibrary;
using System.Collections.Generic;
using System.Collections;

namespace Server
{
    public class Server : MonoBehaviour
    {
        [SerializeField] float tickRate;
        protected List<PlayerSocket> clients = new();
        protected List<string> clientGameObjectIDs = new();

        protected Socket queueSocket;

        public static Action<string, Socket> ClientAdded;
        public Action<string, bool> OnServerLobbyUpdate;
        public Action<bool> UpdatePlayerSceneStatus;

        public static Server Instance;

        [SerializeField] int maxIDRange;

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
            queueSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);

            queueSocket.Blocking = false;
            queueSocket.Bind(ipEndPoint);
            queueSocket.Listen(10);

            StartCoroutine(Tick());
        }

        IEnumerator Tick()
        {
            while (true)
            {
                if (clients.Count < 3)
                {
                    TryToAcceptClient(queueSocket);
                }

                foreach (PlayerSocket playerSocket in clients)
                {
                    if (playerSocket.socket.Available > 0)
                    {
                        byte[] buffer = new byte[playerSocket.socket.Available];
                        playerSocket.socket.Receive(buffer);
                        int index = 0;
                        while (index < buffer.Length)
                        {
                            BasePacket packet = new BasePacket().Deserialize(buffer, index);
                            if (packet != null)
                            {
                                SwitchCaseHell(playerSocket, buffer, packet, index);
                                index += packet.packetSize;
                            }
                        }
                    }
                }


                yield return new WaitForSeconds(tickRate);
            }
        }

        private void SwitchCaseHell(PlayerSocket playerSocket, byte[] buffer, BasePacket packet, int index)
        {
            switch (packet.packetType)
            {
                case BasePacket.PacketType.PlayerLobbyPacket:
                    LobbyPacket lobbyPacket = new LobbyPacket().Deserialize(buffer, index);
                    OnServerLobbyUpdate?.Invoke(lobbyPacket.playerID, lobbyPacket.isReady);
                    break;
                case BasePacket.PacketType.PlayerInMainScenePacket:
                    PlayerInMainScenePacket playerInMainScenePacket = new PlayerInMainScenePacket().Deserialize(buffer, index);
                    UpdatePlayerSceneStatus?.Invoke(playerInMainScenePacket.inMainScene);
                    Debug.LogError("Players in main scene packet received");
                    break;
                case BasePacket.PacketType.Instantiation:
                    InstantiationPacket instantiationPacket = new InstantiationPacket().Deserialize(buffer, index);
                    playerSocket.socket.Send(instantiationPacket.Serialize());
                    Debug.LogError("SPAWNING OTHER PLAYERS!!!!!!!!!!!!!!!!!");
                    break;
                case BasePacket.PacketType.Position:
                    PositionPacket positionPacket = new PositionPacket().Deserialize(buffer, index);
                    SendToAllClientsExcept(positionPacket.Serialize(), playerSocket.socket);
                    break;
            }
        }

        private void TryToAcceptClient(Socket _queueSocket)
        {
            try
            {
                Socket newSocket = _queueSocket.Accept();

                PlayerSocket playerSocket = new PlayerSocket(newSocket);
                playerSocket.playerID = GenerateUniqueClientID();

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

        public void SendData(byte[] buffer, Socket socket)
        {
            socket.Send(buffer);
        }

        string GenerateUniqueClientID()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        public void SendToAllClients(byte[] buffer)
        {
            foreach (var client in clients)
            {
                client.socket.Send(buffer);
            }
        }


        public void SendToAllClientsExcept(byte[] buffer, Socket socket)
        {
            foreach (var client in clients)
            {
                if (client.socket != socket)
                {
                    client.socket.Send(buffer);
                }
            }
        }

        public void SpawnPlayerObjects(string realPrefab, string fakePrefab, int index)
        {
            string gameObjectID = GenerateUniqueClientID();
            InstantiationPacket realInstantiationPacket = new InstantiationPacket(realPrefab, Vector3.zero, Quaternion.identity, gameObjectID, clients[index].playerID);
            InstantiationPacket fakeInstantiationPacket = new InstantiationPacket(fakePrefab, Vector3.zero, Quaternion.identity, gameObjectID, clients[index].playerID);

            foreach (var client in clients)
            {
                if (client.playerID == clients[index].playerID)
                {
                    SendData(realInstantiationPacket.Serialize(), client.socket);
                    clientGameObjectIDs.Add(gameObjectID);
                }
                else
                {
                    SendData(fakeInstantiationPacket.Serialize(), client.socket);
                }
            }
        }

        public void SendPosition(Vector3 position)
        {
          
        }

        private void OnDisable()
        {
            queueSocket.Close();
        }
    }
}
