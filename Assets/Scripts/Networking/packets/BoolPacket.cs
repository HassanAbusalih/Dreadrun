using NetworkingLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolPacket : BasePacket
{
    public bool isReady;

    public BoolPacket(bool isReady, string gameObjectID) : base(PacketType.Lobby, gameObjectID)
    {
        this.isReady = isReady;
    }

    public override byte[] Serialize()
    {
        base.Serialize();
        binaryWriter.Write(isReady);
        return writeMemoryStream.ToArray();
    }

    public override BasePacket Deserialize(byte[] _dataToDeserialize)
    {
        base.Deserialize(_dataToDeserialize);
        isReady = binaryReader.ReadBoolean();
        return this;
    }
}
