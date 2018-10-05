using System;
namespace ChatterBox.Packets
{
    public class PacketConnect : IPacket
    {
        public string Username;

        public PacketConnect(string username)
        {
            Username = username;
        }

        public string Command { get { return PacketType.Connect; } }

        public string Serialize()
        {
            return Packet.Serialize(Command, new string[] { Username });
        }
    }
}