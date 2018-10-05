using System;
namespace ChatterBox.Packets
{
    // Send message to single user
    public class PacketWhisperReceived : IPacket
    {
        public string FromUser;
        public string Message;

        public PacketWhisperReceived(string fromUser, string message)
        {
            FromUser = fromUser;
            Message = message;
        }

        public string Command { get { return PacketType.WhisperReceived; } }

        public string Serialize()
        {
            return Packet.Serialize(Command, new string[] { FromUser, Message });
        }
    }
}