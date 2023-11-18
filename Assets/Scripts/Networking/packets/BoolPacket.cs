using NetworkingLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolPacket : BasePacket
{
    public bool isReady;

    public BoolPacket(bool isReady) : base(PacketType.Lobby)
    {
        this.isReady = isReady;
    }

    public override byte[] Serialize()
    {
        SerializePacketType();
        binaryWriter.Write(isReady);
        return writeMemoryStream.ToArray();
    }

    public override BasePacket Deserialize(byte[] _dataToDeserialize)
    {
        DeserializePacketType(_dataToDeserialize);
        isReady = binaryReader.ReadBoolean();
        return this;
    }
}
