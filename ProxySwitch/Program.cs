using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

/* credits
 * source code
 * https://alanbondo.wordpress.com/2008/06/22/creating-a-system-tray-app-with-c/
 * icon credits
 * https://www.iconfinder.com/iconsets/metro-ui-dock
 * http://dakirby309.deviantart.com/
 *
 *
 */

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

            using (StreamReader reader = new StreamReader("config.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string str = reader.ReadLine();
                    trayMenu.MenuItems.Add(str, OnChangeProxy);
                }

                reader.Close();
            }

            trayMenu.MenuItems.Add("Proxy Settings", OnOpenProxySettings);
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon      = new NotifyIcon();
            trayIcon.Text = "ProxySwitch";
            trayIcon.Icon = new Icon("icon.ico", 40, 40);

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

        private void OnChangeProxy(object sender, EventArgs e)
        {
            foreach (MenuItem menuItem in trayMenu.MenuItems)
                menuItem.Checked = false;

            MenuItem menuItemSender = (MenuItem)sender;
            menuItemSender.Checked = true;

            string proxyAddress = menuItemSender.Text;
            SetProxy(proxyAddress);
            Process.Start("inetcpl.cpl", ",4");
        }

        const string PROXY_SERVER_KEY = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings";

        private void SetProxy(string address)
        {
            Registry.SetValue(PROXY_SERVER_KEY, "ProxyServer", address);
        }

        private void OnOpenProxySettings(object sender, EventArgs e)
        {
            Process.Start("inetcpl.cpl", ",4");
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
