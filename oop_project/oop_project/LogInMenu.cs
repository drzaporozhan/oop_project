using oop_project.managers;
using oop_project.model;
using System;
using System.Windows.Forms;

namespace oop_project
{
    public partial class LogInMenu : Form
    {
        public LogInMenu()
        {
            InitializeComponent();
        }

        private void LoginBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void PasswordBox_TextChanged(object sender, EventArgs e)
        {

        }

        public void Apply_Click(object sender, EventArgs e)
        {
            try
            {
                PersonManager pm = new PersonManager();
                pm.createUser(LoginBox.Text, PasswordBox.Text);
                Choice choice = new Choice(pm);
                choice.Show();
            }
            catch (ManagerException exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void Apply2_Click(object sender, EventArgs e)
        {
            PersonManager pm = new PersonManager();
            pm.loggingUser(new Person(LoginBox.Text, PasswordBox.Text));
            Choice choice = new Choice(pm);
            choice.Show();
        }
    }
}
