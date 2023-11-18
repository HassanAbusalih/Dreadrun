using NetworkingLibrary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IDPacket : BasePacket
{

    public IDPacket()
    {

    }
    public IDPacket(string ownershipID)
        : base(PacketType.ID, ownershipID)
    {

    }

    public override byte[] Serialize()
    {
        base.Serialize();
        return writeMemoryStream.ToArray();
    }

    public override BasePacket Deserialize(byte[] dataToDeserialize)
    {
        base.Deserialize(dataToDeserialize);
        return this;
    }
}
