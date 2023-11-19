using System.Net.Sockets;

public class PlayerSocket
{
    public Socket socket;
    public string playerID;

    public PlayerSocket(Socket socket)
    {
        this.socket = socket;
    }
}