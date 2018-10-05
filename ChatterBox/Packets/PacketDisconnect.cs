using System;
namespace ChatterBox.Packets
{
    public class PacketDisconnect : IPacket
    {
        public string Command { get { return PacketType.Disconnect; } }

        public string Serialize()
        {
            return Packet.Serialize(Command);
        }
    }
}