using System;

namespace oop_project.managers
{
    class ManagerException : Exception
    {
        public ManagerException(string message)
         : base(message) { }
    }
}
