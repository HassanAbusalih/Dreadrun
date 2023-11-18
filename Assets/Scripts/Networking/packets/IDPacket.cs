using NetworkingLibrary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IDPacket : BasePacket
{
    public string ID;


    public IDPacket()
    {
        ID = "";
    }
    public IDPacket(string _id)
        : base(PacketType.ID)
    {
        ID = _id;
    }

    public override byte[] Serialize()
    {
        SerializePacketType();
        binaryWriter.Write(ID);
        return writeMemoryStream.ToArray();
    }

    public override BasePacket Deserialize(byte[] dataToDeserialize)
    {
        DeserializePacketType(dataToDeserialize);
        ID = binaryReader.ReadString();
        return this;
    }
}
