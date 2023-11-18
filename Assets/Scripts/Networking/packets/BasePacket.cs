using System.IO;

namespace NetworkingLibrary
{
    public class BasePacket
    {
        public enum PacketType
        {
            unknown = -1,
            none,
            Position,
            Rotation,
            Instantiation,
            ID,
            Lobby
        }

        public PacketType packetType { get; private set; }

        protected MemoryStream writeMemoryStream;
        protected MemoryStream readMemoryStream;
        protected BinaryWriter binaryWriter;
        protected BinaryReader binaryReader;

        public BasePacket()
        {
            packetType = PacketType.none;
        }
        public BasePacket(PacketType _packetType)
        {
            packetType = _packetType;
        }

        protected void SerializePacketType()
        {
            writeMemoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(writeMemoryStream);

            binaryWriter.Write((int)packetType);
        }
        public virtual byte[] Serialize()
        {
            byte[] data = new byte[0];
            return data;
        }
        public virtual BasePacket Deserialize(byte[] dataToDeserialize)
        {
            return new BasePacket();
        }

        public BasePacket DeserializePacketType(byte[] _dataToDeserialize)
        {
            readMemoryStream = new MemoryStream(_dataToDeserialize);
            binaryReader = new BinaryReader(readMemoryStream);

            packetType = (PacketType)binaryReader.ReadInt32();
            return this;
        }
    }
}
