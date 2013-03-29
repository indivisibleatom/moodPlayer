using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace songClassifier
{
    public partial class MoodPlayerClassifier : Form
    {
        public MoodPlayerClassifier()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            SongSelector selector = new SongSelector();
            selector.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Trainer trainer = new Trainer();
            trainer.Show();
        }
    }
}
