using NetworkingLibrary;

public class IDPacket : BasePacket
{
    public IDPacket()
    {

    }
    public IDPacket(string ownershipID)
        : base(PacketType.ID, ownershipID)
    {

    }

    public new byte[] Serialize()
    {
        base.Serialize();
        return writeMemoryStream.ToArray();
    }

    public new IDPacket Deserialize(byte[] dataToDeserialize)
    {
        base.Deserialize(dataToDeserialize);
        return this;
    }
}