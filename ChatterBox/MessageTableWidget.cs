using System;
using Gtk;
using System.Collections.Generic;
namespace ChatterBox
{
    public class MessageTableWidget : Table
    {
        public MessageTableWidget() : base(0, 2, false)
        {
            
        }

        private List<(ChatMessageUserWidget, ChatMessageMessageWidget)> MessageList = new List<(ChatMessageUserWidget, ChatMessageMessageWidget)>();

        public void NewMessage(Gdk.RGBA fromUserColor, string fromUser, string message)
        {
            ChatMessageUserWidget userWidget = new ChatMessageUserWidget(fromUserColor, fromUser);

            ChatMessageMessageWidget messageWidget = new ChatMessageMessageWidget(fromUser, message);

            this.NRows += 1;


            this.Attach(userWidget, 0, 1, this.NRows - 2, this.NRows - 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            this.Attach(messageWidget, 1, 2, this.NRows - 2, this.NRows - 1, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);

            this.ShowAll();



            this.CheckResize();
            //this.QueueResize();
            //this.QueueDraw();

            //messageWidget.CheckResize();
            //messageWidget.QueueResize();
            //messageWidget.QueueDraw();

            MessageList.Add((userWidget, messageWidget));
        }
    }
}
