using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace Decode
{
    public partial class Decode_F : Form
    {
        public Decode_F()
        {
            InitializeComponent();
        }
        const string pathRegistryKeyStartup ="SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        const string applicationName = "Decode";
        String value="";
        private void Decode_F_Load(object sender, EventArgs e)
        {
            try
            {
                SystemArgs.Decode = new Decode();
                SystemArgs.Path = new PathProgramm();
                SystemArgs.Server = new Server();
                if (SystemArgs.Server.GetParametersConnect())
                {
                    if (SystemArgs.Server.Start())
                    {
                        SystemArgs.Server.Load += AppendToTB;
                        SystemArgs.PrintLog(Directory.GetCurrentDirectory());
                        using (RegistryKey registryKeyStartup = Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup, true))
                        {
                            value = (String)registryKeyStartup.GetValue("Decode");
                        }
                        if (!String.IsNullOrEmpty(value))
                        {
                            AddAutoRun_TSM.Text = "Удалить из автозагрузки";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Сервер не был запущен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        SystemArgs.PrintLog("Ошибка запуска сервера");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось прочитать данные для запуска сервера", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SystemArgs.PrintLog("Не удалось прочитать данные для запуска сервера");
                }
            }
            catch(Exception E)
            {
                MessageBox.Show(E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SystemArgs.PrintLog(E.ToString());
            }
        }

        private void Setting_TSM_Click(object sender, EventArgs e)
        {
                Setting_F Dialog = new Setting_F();
                Dialog.IP_TB.Text = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
                Dialog.Port_TB.Text = SystemArgs.Server._Port;
                if (Dialog.ShowDialog() == DialogResult.OK)
                {

                }
        }
        private void AppendToTB(String Text)
        {
            Status_TB.Invoke((MethodInvoker)delegate ()
            {
                Status_TB.AppendText(Text+Environment.NewLine);
                SystemArgs.PrintLog(Text);
            });
        }
        private void Closing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipText = "Нажмите, чтобы отобразить окно";
            notifyIcon1.BalloonTipTitle = "Подсказка";
            notifyIcon1.ShowBalloonTip(12);
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            e.Cancel = true;
            SystemArgs.PrintLog("Сервер свернут в трей");
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            SystemArgs.PrintLog("Сервер развернут из трея");
        }

        private void Exit_TSM_Click(object sender, EventArgs e)
        {
            SystemArgs.PrintLog("Закрытие сервера");
            Environment.Exit(0);
        }

        private void AddAutoRun_TSM_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(value))
                {
                    using (RegistryKey registryKeyStartup = Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup, true))
                    {
                        registryKeyStartup.DeleteValue(applicationName, false);
                    }
                    value = "";
                    AddAutoRun_TSM.Text = "Добавить в автозагрузку";
                }
                else
                {
                    using (RegistryKey registryKeyStartup = Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup, true))
                    {
                        registryKeyStartup.SetValue(applicationName, Application.ExecutablePath);
                    }
                    value = "Decode";
                    AddAutoRun_TSM.Text = "Удалить из автозагрузки";
                }
            }
            catch(Exception E)
            {
                MessageBox.Show("Не удалось изменить параметры автозагрузки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SystemArgs.PrintLog(E.ToString());
            }
        }
    }
}
