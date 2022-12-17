using oop_project.managers;
using System;
using System.Windows.Forms;

namespace oop_project
{
    public partial class Choice : Form
    {
        private PersonManager pm;
        public Choice(PersonManager pm)
        {
            InitializeComponent();
            this.pm = pm;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            History history = new History(pm);
            history.Show();
        }
    }
}
