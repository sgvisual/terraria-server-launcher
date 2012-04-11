using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace TerrariaServerLauncher
{
    public partial class Form1 : Form
    {
        NotifyIcon m_notificationIcon;
        ContextMenu m_trayMenu;
        Process m_serverProcess;

        public Form1()
        {
            InitializeComponent();

            m_trayMenu = new ContextMenu();
            m_trayMenu.MenuItems.Add("Start", OnStart);
            m_trayMenu.MenuItems.Add("Stop", OnStop);
            m_trayMenu.MenuItems.Add("-");
            m_trayMenu.MenuItems.Add("Save", OnSave);

            MenuItem[] timeSubMenu = new MenuItem[4];
            timeSubMenu[0] = new MenuItem("Dawn", OnTimeDawn);
            timeSubMenu[1] = new MenuItem("Noon", OnTimeNoon);
            timeSubMenu[2] = new MenuItem("Dusk", OnTimeDusk);
            timeSubMenu[3] = new MenuItem("Midnight", OnTimeMidnight);
            m_trayMenu.MenuItems.Add("Time", timeSubMenu);

            m_trayMenu.MenuItems.Add("Exit", OnExit);



            m_notificationIcon = new NotifyIcon();
            m_notificationIcon.Text = "Terraria Server";
            
            
            Bitmap iconBitmap = Properties.Resources.icon;
            IntPtr hIcon = iconBitmap.GetHicon();
            Icon icon = Icon.FromHandle(hIcon);
            
            m_notificationIcon.Icon = new Icon(icon, 40, 40);
            m_notificationIcon.ContextMenu = m_trayMenu;
            m_notificationIcon.Visible = true;

        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;

            base.OnLoad(e);
        }

        private void OnTimeDawn(object snder, EventArgs e)
        {
            if (m_serverProcess != null)
                m_serverProcess.StandardInput.WriteLine("dawn");
        }

        private void OnTimeNoon(object snder, EventArgs e)
        {
            if (m_serverProcess != null)
                m_serverProcess.StandardInput.WriteLine("noon");
        }

        private void OnTimeDusk(object snder, EventArgs e)
        {
            if (m_serverProcess != null)
                m_serverProcess.StandardInput.WriteLine("dusk");
        }

        private void OnTimeMidnight(object snder, EventArgs e)
        {
            if (m_serverProcess != null)
                m_serverProcess.StandardInput.WriteLine("midnight");
        }

        private void OnStart(object sender, EventArgs e)
        {
            if (m_serverProcess != null)
                return;

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "TerrariaServer.exe";
            startInfo.Arguments = "-config serverconfig.txt";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardInput = true;
            m_serverProcess = Process.Start(startInfo);

        }

        void Stop()
        {
            if (m_serverProcess != null)
            {
                m_serverProcess.StandardInput.WriteLine("exit");
                m_serverProcess.Close();
                m_serverProcess.WaitForExit();
                m_serverProcess = null;
            }
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (m_serverProcess != null)
                m_serverProcess.StandardInput.WriteLine("save");
        }

        private void OnStop(object sender, EventArgs e)
        {
            Stop();
        }

        private void OnExit(object sender, EventArgs e)
        {
            Stop();

            Application.Exit();

            m_notificationIcon.Dispose();
        }

        

    }
}
