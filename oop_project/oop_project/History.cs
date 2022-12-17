using oop_project.managers;
using System;
using System.Windows.Forms;

namespace oop_project
{
    public partial class History : Form
    {
        private PersonManager pm;
        public History(PersonManager pm)
        {
            InitializeComponent();
            this.pm = pm;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.DataSource = pm.getListOfHistory();
        }
    }
}
