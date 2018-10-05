using System;
namespace ChatterBox.Packets
{
    public class PacketUserJoined : IPacket
    {
        public string Username;

        public PacketUserJoined(string username)
        {
            Username = username;
        }

        public string Command { get { return PacketType.UserJoined; } }

        public string Serialize()
        {
            return Packet.Serialize(Command, new string[] { Username });
        }
    }
}