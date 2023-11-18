using NetworkingLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiationPacket : BasePacket
{
    public string prefabName;
    public Vector3 position;
    public Quaternion rotation;

    public InstantiationPacket()
    {
        prefabName = string.Empty;
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }

    public InstantiationPacket(string _prefabName, Vector3 _position, Quaternion _rotation)
        : base(PacketType.Instantiation)
    {
        prefabName = _prefabName;
        position = _position;
        rotation = _rotation;
    }

    public override byte[] Serialize()
    {
        SerializePacketType();

        binaryWriter.Write(prefabName);

        binaryWriter.Write(position.x);
        binaryWriter.Write(position.y);
        binaryWriter.Write(position.z);

        binaryWriter.Write(rotation.x);
        binaryWriter.Write(rotation.y);
        binaryWriter.Write(rotation.z);
        binaryWriter.Write(rotation.w);

        return writeMemoryStream.ToArray();
    }

    public override BasePacket Deserialize(byte[] dataToDeserialize)
    {
        DeserializePacketType(dataToDeserialize);

        prefabName = binaryReader.ReadString();
        position = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        rotation = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());

        return this;
    }
}