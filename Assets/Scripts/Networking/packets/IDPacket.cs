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
        FinishSerialization();
        return writeMemoryStream.ToArray();
    }

    public new IDPacket Deserialize(byte[] dataToDeserialize, int index)
    {
        base.Deserialize(dataToDeserialize, index);
        return this;
    }
}