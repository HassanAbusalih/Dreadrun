using NetworkingLibrary;
using System.Collections.Generic;

public class LobbyStatusPacket : BasePacket
{
    public List<bool> playerStatuses = new();
    public List<string> playerIDs = new();

    public LobbyStatusPacket() { }

    public LobbyStatusPacket(List<bool> playerStatuses, List<string> playerIDs) : base(PacketType.ServerLobbyPacket, "")
    {
        this.playerStatuses = playerStatuses;
        this.playerIDs = playerIDs;
    }

    public new byte[] Serialize()
    {
        base.Serialize();
        for (int i = 0; i < 2; i++)
        {
            binaryWriter.Write(playerStatuses[i]);
            binaryWriter.Write(playerIDs[i]);
        }
        return writeMemoryStream.ToArray();
    }

    public new LobbyStatusPacket Deserialize(byte[] _dataToDeserialize)
    {
        base.Deserialize(_dataToDeserialize);
        for (int i = 0; i < 2; i++)
        {
            playerStatuses[i] = binaryReader.ReadBoolean();
            playerIDs[i] = binaryReader.ReadString();
        }
        return this;
    }
}