using System;
using Gtk;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using ChatterBox;
using ChatterBox.Packets;
using System.Diagnostics;
using System.Collections.Generic;
using ChatterBox.Exceptions;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void ConnectButton_Click(object sender, EventArgs e)
    {
        string host = "127.0.0.1";
        int port = 1337;

        if (serverEntry.Text.Contains(":"))
        {
            string[] parts = serverEntry.Text.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                // invalid address format
                // TODO: show error
                return;
            }
            else
            {
                host = parts[0];
                if (!int.TryParse(parts[1], out port))
                {
                    // bad port format
                    // TODO: show error
                    return;
                }
            }
        }
        else if (!String.IsNullOrEmpty(serverEntry.Text))
        {
            host = serverEntry.Text;
        }

        try
        {
            ServerPageWidget serverWidget = new ServerPageWidget(usernameEntry.Text, host, port);

            AddServer(serverWidget);
        }
        catch (Exception ex)
        {
            // error connecting
            // TODO: show error
            throw ex;
        }
    }

    void AddServer(ServerPageWidget serverWidget)
    {
        ServersNotebook.AppendPage(serverWidget, serverWidget.PageLabel);
        ServersNotebook.ShowAll();



        // select server page
    }

    protected void ServersNotebook_SwitchPage(object o, SwitchPageArgs args)
    {
        ServerPageWidget widg = GetSelectedServerWidget();

        if (widg != null)
            widg.UpdateLabels();
    }

    ServerPageWidget GetSelectedServerWidget()
    {
        if (ServersNotebook.Children.Length > 1 && ServersNotebook.CurrentPage != -1 && ServersNotebook.CurrentPage != 0)
        {
            var selectedChild = ServersNotebook.Children[ServersNotebook.CurrentPage];

            if (selectedChild.GetType() == typeof(ServerPageWidget))
            {
                return (ServerPageWidget)selectedChild;
            }
            else
                return null;
        }
        else
        {
            return null;
        }
    }
}