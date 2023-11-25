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
        return writeMemoryStream.ToArray();
    }

    public new PlayerInMainScenePacket Deserialize(byte[] _dataToDeserialize)
    {
        base.Deserialize(_dataToDeserialize);
        inMainScene = binaryReader.ReadBoolean();
        return this;
    }
}
