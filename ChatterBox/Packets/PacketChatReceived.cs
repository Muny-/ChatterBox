using System;
namespace ChatterBox.Packets
{
    // Send message to single user
    public class PacketChatReceived : IPacket
    {
        public string FromUser;
        public string Message;

        public PacketChatReceived(string fromUser, string message)
        {
            FromUser = fromUser;
            Message = message;
        }

        public string Command { get { return PacketType.ChatReceived; } }

        public string Serialize()
        {
            return Packet.Serialize(Command, new string[] { FromUser, Message });
        }
    }
}