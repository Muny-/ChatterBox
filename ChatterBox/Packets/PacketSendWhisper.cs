using System;
namespace ChatterBox.Packets
{
    // Send message to single user
    public class PacketSendWhisper : IPacket
    {
        public string TargetUser;
        public string Message;

        public PacketSendWhisper(string targetUser, string message)
        {
            TargetUser = targetUser;
            Message = message;
        }

        public string Command { get { return PacketType.SendWhisper; } }

        public string Serialize()
        {
            return Packet.Serialize(Command, new string[] { TargetUser, Message });
        }
    }
}