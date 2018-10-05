using System;
using Gtk;
using System.Collections.Generic;
using ChatterBox.Packets;
using Gdk;

namespace ChatterBox
{
    public class ChatWidget : VBox
    {
        public bool IsBroadcast;
        public string TargetUser;
        public int TotalUnread = 0;

        public Dictionary<string, Gdk.RGBA> UserColors = new Dictionary<string, Gdk.RGBA>();

        public Label PageLabel
        {
            get
            {
                string label = TargetUser;
                if (TotalUnread > 0)
                    label = $"<b>{TargetUser} ({TotalUnread})</b>";

                Label thelb = new Label(label);
                thelb.UseMarkup = true;

                return thelb;
            }
        }

        private ScrolledWindow chatScroller;
        private MessageTableWidget MessagesTable;

        private Entry messageEntry;

        private Action<IPacket> RequestPacketSend;

        public ChatWidget(bool isBroadcast, string targetUser, Action<IPacket> requestPacketSend)
        {
            RequestPacketSend = requestPacketSend;

            chatScroller = new ScrolledWindow();
            chatScroller.SizeAllocated += ChatScroller_SizeAllocated;
            //chatScroller.SetPolicy(PolicyType.Never, PolicyType.Always);

            messageEntry = new Entry();
            messageEntry.Activated += MessageEntry_Activated;

            IsBroadcast = isBroadcast;
            TargetUser = targetUser;

            this.CanFocus = false;

            /****/
            MessagesTable = new MessageTableWidget();

            chatScroller.Add(MessagesTable);

            /*TreeView messagesTree = new TreeView();

            TreeViewColumn usernameColumn = new TreeViewColumn();
            usernameColumn.Title = "usernames";

            TreeViewColumn messageColumn = new TreeViewColumn();
            messageColumn.Title = "messages";

            //CellRendererText usernameCell = new CellRendererText();
            //usernameColumn.PackStart(usernameCell, true);

            CellRendererWidget usernameCell = new CellRendererWidget();
            usernameColumn.PackStart(usernameCell, true);



            CellRendererText messageCell = new CellRendererText();
            messageColumn.PackStart(messageCell, true);

            messagesTree.AppendColumn(usernameColumn);
            messagesTree.AppendColumn(messageColumn);

            usernameColumn.AddAttribute(usernameCell, "widget", 0);
            messageColumn.AddAttribute(messageCell, "markup", 1);

            ListStore ls = new ListStore(typeof(Widget), typeof(string));

            ls.AppendValues(new Label("BT"), "Circles<span fgcolor='#ff0000'>lol test</span>");
            ls.AppendValues(new Label("Daft Punk"), "Technologic");
            ls.AppendValues(new Label("Daft Punk"), "Digital Love");
            ls.AppendValues(new Label("The Crystal Method"), "PHD");
            ls.AppendValues(new Label("The Crystal Method"), "Name of the game");
            ls.AppendValues(new Label("The Chemical Brothers"), "Galvanize");

            TreeModelFilter filter = new TreeModelFilter(ls, null);

            filter.VisibleFunc = new TreeModelFilterVisibleFunc(FilterTree);

            usernameColumn.SetCellDataFunc(usernameCell, new TreeCellDataFunc(RenderUsername));
            messageColumn.SetCellDataFunc(messageCell, new TreeCellDataFunc(RenderMessage));

            messagesTree.Model = filter;

            chatScroller.Add(messagesTree);*/

            this.PackStart(chatScroller, true, true, 0);
            this.PackStart(messageEntry, false, false, 0);

            this.ShowAll();

        }

        private void RenderUsername(TreeViewColumn column, CellRenderer cell, ITreeModel model, Gtk.TreeIter iter)
        {
            Widget widget = (Widget)model.GetValue(iter, 0);

            /*if (song.Artist.StartsWith("X") == true)
            {
                (cell as Gtk.CellRendererText).Foreground = "red";
            }
            else
            {
                (cell as Gtk.CellRendererText).Foreground = "darkgreen";
            }*/


            ((CellRendererWidget)cell).widget = widget;
        }

        private void RenderMessage(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            string message = (string)model.GetValue(iter, 1);
            ((CellRendererText)cell).Markup = message;
        }

        private bool FilterTree(ITreeModel model, TreeIter iter)
        {
            string username = model.GetValue(iter, 0).ToString();

            /*if (filterEntry.Text == "")
                return true;

            if (artistName.IndexOf(filterEntry.Text) > -1)
                return true;
            else
                return false;*/

            return true;
        }

        protected void MessageEntry_Activated(object sender, EventArgs e)
        {
            if (messageEntry.Text != "")
            {
                IPacket pak;

                if (IsBroadcast)
                {
                    pak = new PacketSendChat(messageEntry.Text);
                }
                else
                {
                    pak = new PacketSendWhisper(TargetUser, messageEntry.Text);
                }

                RequestPacketSend(pak);
                messageEntry.Text = "";
            }
        }

        void ChatScroller_SizeAllocated(object o, SizeAllocatedArgs args)
        {
            if (doScroll)
                chatScroller.Vadjustment.Value = chatScroller.Vadjustment.Upper - chatScroller.Vadjustment.PageSize;
        }

        bool doScroll = false;

        public void NewMessage(string fromUser, string message)
        {
            double diff = Math.Abs(chatScroller.Vadjustment.Value - (chatScroller.Vadjustment.Upper - chatScroller.Vadjustment.PageSize));
            //Console.WriteLine($"Math.Abs(Vadj.Val[{Vadjustment.Value}] - (Vadj.Upper[{Vadjustment.Upper}] - Vadj.PageSize[{Vadjustment.PageSize}])) == {diff}");
            doScroll = diff < 1;

            if (!UserColors.ContainsKey(fromUser))
                UserColors.Add(fromUser, ColorHelper.RandomColor());

            MessagesTable.NewMessage(UserColors[fromUser], fromUser, message);

            if (!this.IsDrawable)
            {
                TotalUnread++;
                Notebook parent = (Notebook)this.Parent;

                parent.SetTabLabel(this, this.PageLabel);
            }
            else
            {
                
            }
        }

        public void MarkRead()
        {
            TotalUnread = 0;

            Notebook parent = (Notebook)this.Parent;

            parent.SetTabLabel(this, this.PageLabel);
        }
    }
}
