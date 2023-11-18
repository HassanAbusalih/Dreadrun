using System;
using System.Net.Sockets;
using System.Net;
using NetworkingLibrary;
using UnityEngine;
using System.Text;

namespace Client
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

                Debug.Log("Client is trying to connect to server");
                socket.Connect(new IPEndPoint(IPAddress.Parse(_ipAddress), 3000));
                socket.Blocking = false;
                isConnected = true;

                if (isConnected) ConnectedToServerEvent?.Invoke();
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
                ReceiveData();
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


        BasePacket ReceiveData()
        {
            byte[] buffer = new byte[socket.Available];
            socket.Receive(buffer);
            BasePacket basePacket = new BasePacket();
            basePacket.Deserialize(buffer);
            return basePacket;
        }

        void SendPacket(BasePacket basePacket)
        {
            socket.Send(basePacket.Serialize());
        }


        void CallAgain()
        {
            isCalled = false;
        }
    }
}
