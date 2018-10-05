using System;
namespace ChatterBox.Packets
{
    // Send message to single user
    public class PacketWhisperSent : IPacket
    {
        public string ToUser;
        public string Message;

        public PacketWhisperSent(string toUser, string message)
        {
            ToUser = toUser;
            Message = message;
        }

        public string Command { get { return PacketType.WhisperSent; } }

        public string Serialize()
        {
            return Packet.Serialize(Command, new string[] { ToUser, Message });
        }
    }
}