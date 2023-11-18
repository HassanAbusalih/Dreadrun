using NetworkingLibrary;

public class LobbyPacket : BasePacket
{
    public bool isReady;
    public string playerID { get; private set; }

    public LobbyPacket(bool isReady, string gameObjectID, string playerID) : base(PacketType.PlayerLobbyPacket, gameObjectID)
    {
        this.isReady = isReady;
        this.playerID = playerID;
    }

    public override byte[] Serialize()
    {
        binaryWriter.Write(isReady);
        binaryWriter.Write(playerID);
        return writeMemoryStream.ToArray();
    }

    public override BasePacket Deserialize(byte[] _dataToDeserialize)
    {
        isReady = binaryReader.ReadBoolean();
        playerID = binaryReader.ReadString();
        return this;
    }
}
