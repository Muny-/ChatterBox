using System;
namespace ChatterBox.Packets
{
    public class PacketListUsers : IPacket
    {
        public string Command { get { return PacketType.ListUsers; } }

        public string Serialize()
        {
            return Packet.Serialize(Command);
        }
    }
}