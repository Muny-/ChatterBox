using System;
namespace ChatterBox.Packets
{
    public class PacketDisconnected : IPacket
    {
        public string Command { get { return PacketType.Disconnected; } }

        public string Serialize()
        {
            return Packet.Serialize(Command);
        }
    }
}