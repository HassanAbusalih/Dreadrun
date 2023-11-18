using NetworkingLibrary;
public class PlayerPacket
{
    public BasePacket packet;
    public string iD;

    public PlayerPacket(BasePacket basePacket, string iD)
    {
        this.packet = basePacket;
        this.iD = iD;
    }
}
