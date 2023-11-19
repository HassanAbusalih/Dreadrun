using System.IO;

namespace NetworkingLibrary
{
    public class BasePacket
    {
        public string gameObjectID;
        public enum PacketType
        {
            unknown = -1,
            none,
            Position,
            Rotation,
            Instantiation,
            ID,
            PlayerLobbyPacket,
            ServerLobbyPacket,
            ScenePacket,
            Destruction
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
        public BasePacket(PacketType _packetType, string gameObjectID = "")
        {
            packetType = _packetType;
            this.gameObjectID = gameObjectID;
        }

        public byte[] Serialize()
        {
            writeMemoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(writeMemoryStream);
            binaryWriter.Write((int)packetType);
            binaryWriter.Write(gameObjectID);
            return writeMemoryStream.ToArray();
        }
        public BasePacket Deserialize(byte[] dataToDeserialize)
        {
            readMemoryStream = new MemoryStream(dataToDeserialize);
            binaryReader = new BinaryReader(readMemoryStream);
            packetType = (PacketType)binaryReader.ReadInt32();
            gameObjectID = binaryReader.ReadString();
            return this;
        }
    }
}
