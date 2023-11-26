using System;
using UnityEngine;

namespace NetworkingLibrary
{
    public class PositionPacket : BasePacket
    {
        public Vector3 position { get; private set; }
        public Quaternion rotation { get; private set; }


        public PositionPacket()
        {
            position = Vector3.zero;
        }
        public PositionPacket(Vector3 _position, Quaternion rotation, string gameObjectID) : base(PacketType.Position, gameObjectID)
        {
            position = _position;
            this.rotation = rotation;
        }

        public new byte[] Serialize()
        {
            base.Serialize();
            binaryWriter.Write(position.x);
            binaryWriter.Write(position.y);
            binaryWriter.Write(position.z);

            binaryWriter.Write(rotation.x);
            binaryWriter.Write(rotation.y);
            binaryWriter.Write(rotation.z);
            binaryWriter.Write(rotation.w);

            FinishSerialization();
            return writeMemoryStream.ToArray();
        }

        public new PositionPacket Deserialize(byte[] dataToDeserialize, int index)
        {
            base.Deserialize(dataToDeserialize, index);

            float x = binaryReader.ReadSingle();
            float y = binaryReader.ReadSingle();
            float z = binaryReader.ReadSingle();

            float rx = binaryReader.ReadSingle();
            float ry = binaryReader.ReadSingle();
            float rz = binaryReader.ReadSingle();
            float rw = binaryReader.ReadSingle();

            position = new Vector3(x, y, z);
            rotation = new Quaternion(rx, ry, rz, rw);
            return this;
        }
    }
}
