using System;
using System.Collections.Generic;
using Gtk;
using System.Linq;
using ChatterBox.Packets;
using ChatterBox.Exceptions;
using System.Diagnostics;

namespace ChatterBox
{
    public class ServerPageWidget : VBox
    {
        public string ServerName
        {
            get
            {
                return $"{Host}:{Port}";
            }
        }

        private string Host;
        private int Port;
        private string Username;

        Client client;

        private Notebook ChatsNotebook;
        Dictionary<string, ChatWidget> ConnectedUsers = new Dictionary<string, ChatWidget>();

        public Label PageLabel
        {
            get
            {
                /*EventBox evBox = new EventBox();

                evBox.ButtonPressEvent += EvBox_ButtonPressEvent;*/

                string label = ServerName;

                if (TotalUnread > 0)
                    label = $"<b>{ServerName} ({TotalUnread})</b>";

                Label thelb = new Label(label);
                thelb.UseMarkup = true;

                thelb.HasWindow = true;

                thelb.ButtonPressEvent += Thelb_ButtonPressEvent;

                return thelb;
            }
        }

        void Thelb_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            
        }

        public int TotalUnread
        {
            get
            {
                return ConnectedUsers.Sum(pair => pair.Value.TotalUnread);
            }
        }

        public ServerPageWidget(string username, string host, int port)
        {
            Username = username;
            Host = host;
            Port = port;
            ChatsNotebook = new Notebook();
            ChatsNotebook.TabPos = PositionType.Right;
            ChatsNotebook.Scrollable = true;
            ChatsNotebook.SwitchPage += ChatsNotebook_SwitchPage;

            this.PackStart(ChatsNotebook, true, true, 0);

            this.ShowAll();

            client = new Client(host, port);

            client.DataReceived += Client_DataReceived;
            client.Disconnected += Client_Disconnected;

            client.SendPacket(new PacketConnect(username));
        }

        void Client_DataReceived(object sender, DataReceivedEventArgs e)
        {
            Gtk.Application.Invoke(delegate
            {
                Console.WriteLine($"[<-] {e.Data}");

                Packets.IPacket packet;

                try
                {
                    packet = ParserPacket.ParsePacket(e.Data);
                }
                catch (ParsePacketException ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }

                switch (packet.Command)
                {
                    case PacketType.Connected:
                        {
                            ChatWidget broadcastWidget = new ChatWidget(true, "Broadcast", client.SendPacket);
                            AddChatWidget(broadcastWidget);

                            // we want the list of users...

                            client.SendPacket(new PacketListUsers());
                        }
                        break;

                    case PacketType.Disconnected:
                        {
                            // handle leaving server
                        }
                        break;

                    case PacketType.ChatReceived:
                        {
                            PacketChatReceived chatPak = (PacketChatReceived)packet;

                            ShowBroadcastMessage(chatPak.FromUser, chatPak.Message);
                        }
                        break;

                    case PacketType.WhisperReceived:
                        {
                            PacketWhisperReceived whispPak = (PacketWhisperReceived)packet;

                            if (!ConnectedUsers.ContainsKey(whispPak.FromUser))
                                AddChatWidget(new ChatWidget(false, whispPak.FromUser, client.SendPacket));

                            ConnectedUsers[whispPak.FromUser].NewMessage(whispPak.FromUser, whispPak.Message);
                            UpdatePageLabel();
                        }
                        break;

                    case PacketType.WhisperSent:
                        {
                            PacketWhisperSent whispPak = (PacketWhisperSent)packet;

                            ConnectedUsers[whispPak.ToUser].NewMessage(Username, whispPak.Message);
                            UpdatePageLabel();
                        }
                        break;

                    case PacketType.Users:
                        {
                            PacketUsers usersPak = (PacketUsers)packet;

                            foreach (string user in usersPak.Users)
                            {
                                ChatWidget widget = new ChatWidget(false, user, client.SendPacket);
                                AddChatWidget(widget);
                            }
                        }
                        break;

                    case PacketType.UserJoined:
                        {
                            PacketUserJoined usrJnPak = (PacketUserJoined)packet;

                            if (!ConnectedUsers.ContainsKey(usrJnPak.Username))
                            {
                                AddChatWidget(new ChatWidget(false, usrJnPak.Username, client.SendPacket));

                                ShowBroadcastMessage("server", $"User '{usrJnPak.Username}' joined the server.");
                            }
                        }
                        break;

                    case PacketType.UserLeft:
                        {
                            PacketUserLeft usrLfPak = (PacketUserLeft)packet;

                            if (ConnectedUsers.ContainsKey(usrLfPak.Username))
                            {
                                ShowBroadcastMessage("server", $"User '{usrLfPak.Username}' left the server.");

                                RemoveChatWidget(ConnectedUsers[usrLfPak.Username]);
                            }
                        }
                        break;

                    case PacketType.Error:
                        {
                            PacketError errPak = (PacketError)packet;

                            ShowBroadcastMessage("ERROR", errPak.Message);
                        }
                        break;

                    case PacketType.FatalError:
                        {
                            PacketFatalError errPak = (PacketFatalError)packet;

                            ShowBroadcastMessage("FATAL ERROR", errPak.Message);
                        }
                        break;
                }
            });

        }

        void UpdatePageLabel()
        {
            Notebook parent = (Notebook)this.Parent;
            parent.SetTabLabel(this, this.PageLabel);
        }

        void ShowBroadcastMessage(string from, string message)
        {
            if (ConnectedUsers.ContainsKey("Broadcast"))
            {
                ConnectedUsers["Broadcast"].NewMessage(from, message);
                UpdatePageLabel();
            }
            else
            {
                Console.WriteLine($"hmm...no broadcast key...size of connectedusers: {ConnectedUsers.Count}");
            }
        }

        void Client_Disconnected(object sender, EventArgs e)
        {
            Console.WriteLine($"Disconnected");
        }

        void AddChatWidget(ChatWidget widget)
        {
            ConnectedUsers.Add(widget.TargetUser, widget);
            ChatsNotebook.AppendPage(widget, widget.PageLabel);
            ChatsNotebook.ShowAll();
        }

        void RemoveChatWidget(ChatWidget widget)
        {
            ConnectedUsers.Remove(widget.TargetUser);
            ChatsNotebook.Remove(widget);
        }

        ChatWidget GetSelectedChat()
        {
            if (ChatsNotebook.Children.Length > 0 && ChatsNotebook.CurrentPage != -1)
            {
                var selectedChild = ChatsNotebook.Children[ChatsNotebook.CurrentPage];

                if (selectedChild.GetType() == typeof(ChatWidget))
                {
                    return (ChatWidget)selectedChild;
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        protected void ChatsNotebook_SwitchPage(object o, SwitchPageArgs args)
        {
            UpdateLabels();
        }

        public void UpdateLabels()
        {
            ChatWidget widg = GetSelectedChat();

            if (widg != null)
                widg.MarkRead();

            UpdatePageLabel();
        }
    }
}
