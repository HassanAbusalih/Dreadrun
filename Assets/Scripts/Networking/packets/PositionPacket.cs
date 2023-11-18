using System;
using UnityEngine;

namespace NetworkingLibrary
{
    public class PositionPacket : BasePacket
    {
        public Vector3 position { get; private set; }


        public PositionPacket()
        {
            position = Vector3.zero;
        }
        public PositionPacket(Vector3 _position, string gameObjectID) : base(PacketType.Position, gameObjectID)
        {
            position = _position;
        }

        public override byte[] Serialize()
        {
            base.Serialize();
            binaryWriter.Write(position.x);
            binaryWriter.Write(position.y);
            binaryWriter.Write(position.z);

            return writeMemoryStream.ToArray();
        }

        public override BasePacket Deserialize(byte[] dataToDeserialize)
        {
            base.Deserialize(dataToDeserialize);

            float x = binaryReader.ReadSingle();
            float y = binaryReader.ReadSingle();
            float z = binaryReader.ReadSingle();

            position = new Vector3(x, y, z);
            return this;
        }
    }
}
