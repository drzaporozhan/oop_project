using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_project.managers
{
    class ManagerSaveException: Exception
    {
        public ManagerSaveException(string message)
         : base(message) { }
    }
}
