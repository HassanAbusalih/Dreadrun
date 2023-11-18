using NetworkingLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPacket : BasePacket
{
    public bool isReady;
    public string playerID { get; private set; }

    public LobbyPacket(bool isReady, string gameObjectID, string playerID) : base(PacketType.Lobby, gameObjectID)
    {
        this.isReady = isReady;
        this.playerID = playerID;
    }

    public override byte[] Serialize()
    {
        base.Serialize();
        binaryWriter.Write(isReady);
        binaryWriter.Write(playerID);
        return writeMemoryStream.ToArray();
    }

    public override BasePacket Deserialize(byte[] _dataToDeserialize)
    {
        base.Deserialize(_dataToDeserialize);
        isReady = binaryReader.ReadBoolean();
        playerID = binaryReader.ReadString();
        return this;
    }
}
