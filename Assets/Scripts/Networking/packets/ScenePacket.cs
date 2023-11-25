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
        FinishSerialization();
        return writeMemoryStream.ToArray();
    }

    public new ScenePacket Deserialize(byte[] _dataToDeserialize, int index)
    {
        base.Deserialize(_dataToDeserialize, index);
        sceneName = binaryReader.ReadString();
        return this;
    }
}