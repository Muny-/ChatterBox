using System;
namespace ChatterBox.Packets
{
    public class PacketConnected : IPacket
    {
        public string Command { get { return PacketType.Connected; } }

        public string Serialize()
        {
            return Packet.Serialize(Command);
        }
    }
}