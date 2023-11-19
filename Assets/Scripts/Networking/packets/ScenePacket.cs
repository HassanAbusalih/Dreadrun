using NetworkingLibrary;
public class ScenePacket : BasePacket
{
    public string sceneName;

    public ScenePacket()
    {

    }
    public ScenePacket(string sceneName) : base(PacketType.ScenePacket)
    {
        this.sceneName = sceneName;
    }

    public new byte[] Serialize()
    {
        base.Serialize();
        binaryWriter.Write(sceneName);
        return writeMemoryStream.ToArray();
    }

    public new ScenePacket Deserialize(byte[] _dataToDeserialize)
    {
        base.Deserialize(_dataToDeserialize);
        sceneName = binaryReader.ReadString();
        return this;
    }
}
