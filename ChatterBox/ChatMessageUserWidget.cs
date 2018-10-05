using System;
using Gtk;
using Pango;

namespace ChatterBox
{
    public class ChatMessageUserWidget : Label
    {
        public string Username;
        public Gdk.RGBA Color;

        public ChatMessageUserWidget(Gdk.RGBA color, string username) : base($"<b>{username}</b>")
        {
            Color = color;
            Username = username;

            this.UseMarkup = true;

            //this.Expand = false;
            //this.Ellipsize = EllipsizeMode.Middle;

            this.SetPadding(10, 2);


            this.OverrideFont(FontDescription.FromString("'Raleway' Normal Normal Small-Caps Normal 11"));

            this.OverrideColor(StateFlags.Normal, Color);
            this.OverrideBackgroundColor(StateFlags.Normal, ColorHelper.Color8888(100, 0, 0, 0));
        }
    }
}
