﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Decode
{
    public partial class Setting_F : Form
    {
        public Setting_F()
        {
            InitializeComponent();
        }

        private void OK_B_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(IP_TB.Text))
                {
                    IP_TB.Focus();
                    throw new Exception("Необходимо ввести IP - адрес");
                }

                if (String.IsNullOrEmpty(Port_TB.Text))
                {
                    Port_TB.Focus();
                    throw new Exception("Необходимо ввести порт");
                }
                Int32 Port = Convert.ToInt32(Port_TB.Text);

                SystemArgs.Server._Port = Port_TB.Text.Trim();

                if (SystemArgs.Server.SetParametersConnect())
                {
                    SystemArgs.PrintLog("Обновлены параметры сервера порт: " + SystemArgs.Server._Port + " " + DateTime.Now.ToString());
                    MessageBox.Show("Параметры подключения успешно обновлены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FormatException)
            {
                Port_TB.Focus();
                MessageBox.Show("Порт подключения должен состоять из целых цифр", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.Message);
                MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Setting_F_Load(object sender, EventArgs e)
        {

        }
    }
}
