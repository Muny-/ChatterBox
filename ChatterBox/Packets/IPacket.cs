using System;
namespace ChatterBox.Packets
{
    public interface IPacket
    {
        string Command { get; }

        string Serialize();
    }

    public static class Packet
    {
        public const string Delimeter = "::";

        public static string Serialize(string command)
        {
            return command;
        }

        public static string Serialize(string command, string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].Replace("::", "<unsupported_character>");
            }

            return $"{command}{Delimeter}{String.Join(Delimeter, args)}";
        }
    }

    public static class PacketType
    {
        // Client to server
        public const string Connect = "connect";
        public const string Disconnect = "disconnect";
        public const string SendChat = "send_chat";
        public const string SendWhisper = "send_whisper";
        public const string ListUsers = "list_users";

        // Server to client
        public const string Connected = "connected";
        public const string Disconnected = "disconnected";
        public const string ChatReceived = "chat_received";
        public const string WhisperReceived = "whisper_received";
        public const string WhisperSent = "whisper_sent";
        public const string Users = "users";
        public const string UserJoined = "user_joined";
        public const string UserLeft = "user_left";
        public const string Error = "error";
        public const string FatalError = "fatal_error";
    }
}
