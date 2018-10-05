using System;
namespace ChatterBox.Packets
{
    public class PacketUserLeft : IPacket
    {
        public string Username;

        public PacketUserLeft(string username)
        {
            Username = username;
        }

        public string Command { get { return PacketType.UserLeft; } }

        public string Serialize()
        {
            return Packet.Serialize(Command, new string[] { Username });
        }
    }
}