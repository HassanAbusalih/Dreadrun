using NetworkingLibrary;

public class PlayerInMainScenePacket : BasePacket
{

    public bool inMainScene;

    public PlayerInMainScenePacket()
    {

    }

    public PlayerInMainScenePacket(bool inMainScene) : base(PacketType.PlayerInMainScenePacket)
    {
        this.inMainScene = inMainScene;
    }

    public new byte[] Serialize()
    {
        base.Serialize();
        binaryWriter.Write(inMainScene);
        FinishSerialization();
        return writeMemoryStream.ToArray();
    }

    public new PlayerInMainScenePacket Deserialize(byte[] _dataToDeserialize, int index)
    {
        base.Deserialize(_dataToDeserialize, index);
        inMainScene = binaryReader.ReadBoolean();
        return this;
    }
}
