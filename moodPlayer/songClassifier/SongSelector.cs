using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace songClassifier
{
    public partial class SongSelector : Form
    {
        private ClassfierAutomater m_classifierAutomater;

        public SongSelector()
        {
            InitializeComponent();
            listView1.View = View.List;
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "MP3 Files|*.mp3";
            m_classifierAutomater = new ClassfierAutomater();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string[] names = openFileDialog1.FileNames;
                foreach (string name in names)
                {
                    ListViewItem item = new ListViewItem(name);
                    listView1.Items.Add(item);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] names = openFileDialog1.FileNames;
            foreach (string name in names)
            {
                this.statusLabel.Text = "Status : Processing file " + name;
                m_classifierAutomater.perFormClassification(name);
            }
            this.statusLabel.Text = "Status : Processing complete";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SongSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form mainScreen = Application.OpenForms[0];
            mainScreen.Show();
        }
    }
}
