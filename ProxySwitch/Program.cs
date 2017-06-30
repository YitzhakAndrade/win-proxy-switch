using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;

// https://alanbondo.wordpress.com/2008/06/22/creating-a-system-tray-app-with-c/

namespace ProxySwitch
{
    public class Program : Form
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Program());
        }

        private NotifyIcon  trayIcon;
        private ContextMenu trayMenu;

        public Program()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("ctnlm", OnChangeProxy_ctnlm);
            trayMenu.MenuItems.Add("239", OnChangeProxy_239);
            trayMenu.MenuItems.Add("224", OnChangeProxy_224);
            trayMenu.MenuItems.Add("225", OnChangeProxy_225);
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon      = new NotifyIcon();
            trayIcon.Text = "MyTrayApp";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible     = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible       = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnChangeProxy_ctnlm(object sender, EventArgs e)
        {
            SetProxy("localhost:3128");
        }

        private void OnChangeProxy_224(object sender, EventArgs e)
        {
            SetProxy("192.168.224.14:8080");
        }

        private void OnChangeProxy_225(object sender, EventArgs e)
        {
            SetProxy("192.168.225.14:8080");
        }

        private void OnChangeProxy_239(object sender, EventArgs e)
        {
            SetProxy("192.168.239.14:8080");
        }

        const string PROXY_SERVER_KEY = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings";

        private void SetProxy(string address)
        {
            Registry.SetValue(PROXY_SERVER_KEY, "ProxyServer", address);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

    }
}
