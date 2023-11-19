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
        binaryWriter.Write(playerStatuses.Count);
        for (int i = 0; i < playerStatuses.Count; i++)
        {
            binaryWriter.Write(playerStatuses[i]);
            binaryWriter.Write(playerIDs[i]);
        }
        return writeMemoryStream.ToArray();
    }

    public new LobbyStatusPacket Deserialize(byte[] _dataToDeserialize)
    {
        base.Deserialize(_dataToDeserialize);
        int count = binaryReader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            playerStatuses.Add(binaryReader.ReadBoolean());
            playerIDs.Add(binaryReader.ReadString());
        }
        return this;
    }
}