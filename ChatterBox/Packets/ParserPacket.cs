using System;
using System.Linq;
using ChatterBox.Exceptions;
namespace ChatterBox.Packets
{
    public static class ParserPacket
    {
        public static IPacket ParsePacket(string rawPacket)
        {
            string[] parts = rawPacket.Split(new string[] { "::" }, StringSplitOptions.None);

            if (parts.Length == 0)
                throw new ParsePacketException("bad format -- empty command");

            string command = parts[0];

            string[] args = parts.Skip(1).ToArray();

            IPacket pak;

            switch(command)
            {
                case PacketType.Connect:
                    {
                        AssertArgsCountEquals(args, 1);

                        string username = args[0];

                        pak = new PacketConnect(username);
                    }
                    break;

                case PacketType.Disconnect:
                    {
                        AssertArgsCountEquals(args, 0);

                        pak = new PacketDisconnect();
                    }
                    break;

                case PacketType.Connected:
                    {
                        AssertArgsCountEquals(args, 0);

                        pak = new PacketConnected();
                    }
                    break;

                case PacketType.Disconnected:
                    {
                        AssertArgsCountEquals(args, 0);

                        pak = new PacketDisconnected();
                    }
                    break;

                case PacketType.ChatReceived:
                    {
                        AssertArgsCountEquals(args, 2);

                        string from = args[0];
                        string message = args[1];

                        pak = new PacketChatReceived(from, message);
                    }
                    break;

                case PacketType.WhisperReceived:
                    {
                        AssertArgsCountEquals(args, 2);

                        string from = args[0];
                        string message = args[1];

                        pak = new PacketWhisperReceived(from, message);
                    }
                    break;

                case PacketType.WhisperSent:
                    {
                        AssertArgsCountEquals(args, 2);

                        string to = args[0];
                        string message = args[1];

                        pak = new PacketWhisperSent(to, message);
                    }
                    break;

                case PacketType.Users:
                    {
                        string[] users = args;
                        pak = new PacketUsers(users);
                    }
                    break;

                case PacketType.UserJoined:
                    {
                        AssertArgsCountEquals(args, 1);

                        string username = args[0];

                        pak = new PacketUserJoined(username);
                    }
                    break;

                case PacketType.UserLeft:
                    {
                        AssertArgsCountEquals(args, 1);

                        string username = args[0];

                        pak = new PacketUserLeft(username);
                    }
                    break;

                case PacketType.Error:
                    {
                        AssertArgsCountEquals(args, 1);

                        string message = args[0];

                        pak = new PacketError(message);
                    }
                    break;

                case PacketType.FatalError:
                    {
                        AssertArgsCountEquals(args, 1);

                        string message = args[0];

                        pak = new PacketFatalError(message);
                    }
                    break;

                default:
                    throw new ParsePacketException("unrecognized packet");
            }

            return pak;
        }

        static void AssertArgsCountEquals(string[] arr, int numArgs)
        {
            if (arr.Length != numArgs)
                throw new ParsePacketException("invalid argument count");
        }
    }
}
