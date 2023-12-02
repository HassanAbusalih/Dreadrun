using NetworkingLibrary;

public class IDPacket : BasePacket
{
    public bool isHost { get; private set; }
    public IDPacket()
    {

    }
    public IDPacket(string ownershipID, bool isHost)
        : base(PacketType.ID, ownershipID)
    {
        this.isHost = isHost;
    }

    public new byte[] Serialize()
    {
        base.Serialize();
        binaryWriter.Write(isHost);
        FinishSerialization();
        return writeMemoryStream.ToArray();
    }

    public new IDPacket Deserialize(byte[] dataToDeserialize, int index)
    {
        base.Deserialize(dataToDeserialize, index);
        isHost = binaryReader.ReadBoolean();
        return this;
    }
}