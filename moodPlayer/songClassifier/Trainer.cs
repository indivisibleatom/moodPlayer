using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace songClassifier
{
    public partial class Trainer : Form
    {
        public Trainer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;
                label1.Text = "Status: Training classifier. This can take some time";

                // Start the child process.
                Process p = new Process();
                string pathExec = Application.ExecutablePath;
                string rootDir = new FileInfo(pathExec).Directory.FullName;

                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = rootDir + @"\train.bat";
                p.StartInfo.Arguments = "\"trainerInvoker '" + path + "'\"";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
            }
            label1.Text = "Stauts: Training complete! Press back button to go back";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Trainer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form mainScreen = Application.OpenForms[0];
            mainScreen.Show();
        }
    }
}
