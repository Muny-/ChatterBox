using System;
namespace ChatterBox.Packets
{
    public class PacketError : IPacket
    {
        public string Message;

        public PacketError(string message)
        {
            Message = message;
        }

        public string Command { get { return PacketType.Error; } }

        public string Serialize()
        {
            return Packet.Serialize(Command, new string[] { Message });
        }
    }
}