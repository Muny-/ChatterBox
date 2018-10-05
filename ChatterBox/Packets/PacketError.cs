using System;
namespace ChatterBox.Packets
{
    public class PacketFatalError : IPacket
    {
        public string Message;

        public PacketFatalError(string message)
        {
            Message = message;
        }

        public string Command { get { return PacketType.FatalError; } }

        public string Serialize()
        {
            return Packet.Serialize(Command, new string[] { Message });
        }
    }
}