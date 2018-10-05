using System;
namespace ChatterBox.Packets
{
    // Broadcast message to all users
    public class PacketSendChat : IPacket
    {
        public string Message;

        public PacketSendChat(string message)
        {
            Message = message;
        }

        public string Command { get { return PacketType.SendChat; } }

        public string Serialize()
        {
            return Packet.Serialize(Command, new string[] { Message });
        }
    }
}