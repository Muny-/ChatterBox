using System;
namespace ChatterBox.Packets
{
    public class PacketUsers : IPacket
    {
        public string[] Users;

        public PacketUsers(string[] users)
        {
            Users = users;
        }

        public string Command { get { return PacketType.Users; } }

        public string Serialize()
        {
            return Packet.Serialize(Command, Users);
        }
    }
}