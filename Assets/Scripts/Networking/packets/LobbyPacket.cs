using NetworkingLibrary;
using UnityEngine;

public class LobbyPacket : BasePacket
{
    public bool isReady;
    public string playerID { get; private set; }

    public LobbyPacket()
    {
    }

    public LobbyPacket(bool isReady, string gameObjectID, string playerID) : base(PacketType.PlayerLobbyPacket, gameObjectID)
    {
        this.isReady = isReady;
        this.playerID = playerID;
    }

    public new byte[] Serialize()
    {
        base.Serialize();
        binaryWriter.Write(isReady);
        binaryWriter.Write(playerID);
        FinishSerialization();
        return writeMemoryStream.ToArray();
    }

    public new LobbyPacket Deserialize(byte[] _dataToDeserialize, int index)
    {
        base.Deserialize(_dataToDeserialize, index);
        isReady = binaryReader.ReadBoolean();
        playerID = binaryReader.ReadString();

        return this;
    }
}
