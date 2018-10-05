using System;
using Gtk;
using Pango;

namespace ChatterBox
{
    public class ChatMessageMessageWidget : TextView
    {
        public string Username;
        public string Message;

        public ChatMessageMessageWidget(string username, string message)
        {
            this.OverrideFont(FontDescription.FromString("normal 10"));


            Username = username;
            Message = message;

            this.Buffer.Text = message;

            //this.UseMarkup = true;
            //this.SetAlignment(0, 0.5f);

            //this.Selectable = true;

            //this.SetPadding(10, 0);

            this.WrapMode = Gtk.WrapMode.Word;

            this.Editable = false;

            this.LeftMargin = 5;
            this.RightMargin = 5;

            this.MarginTop = 5;
            this.MarginBottom = 5;

            this.Valign = Align.Center;
        }
    }
}
