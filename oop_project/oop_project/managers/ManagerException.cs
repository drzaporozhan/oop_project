using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_project.managers
{
    class ManagerException: Exception
    {
        public ManagerException(string message)
         : base(message) { }
    }
}
