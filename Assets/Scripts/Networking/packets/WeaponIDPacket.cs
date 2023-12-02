using NetworkingLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIDPacket : BasePacket
{
    public WeaponIDPacket() : base() { }

    public WeaponIDPacket(string ownershipID)
      : base(PacketType.WeaponID, ownershipID)
    {
    }

    public new byte[] Serialize()
    {
        base.Serialize();
        FinishSerialization();
        return writeMemoryStream.ToArray();
    }

    public new WeaponIDPacket Deserialize(byte[] dataToDeserialize, int index)
    {
        base.Deserialize(dataToDeserialize, index);
        return this;
    }
}
