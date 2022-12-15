using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_project.model
{
    class Person
    {
        private string login;
        private string password;

        public Person(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }

        public override bool Equals(object obj)
        {
            return obj is Person person &&
                   Login == person.Login &&
                   Password == person.Password;
        }

        public override string ToString()
        {
            return login+";"+password+";"+"\n";
        }
    }
}
