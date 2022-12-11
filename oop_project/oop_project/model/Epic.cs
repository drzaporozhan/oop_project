using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_project.model
{
    class Epic: Task
    {
        private HashSet<int> subtasksIds;

        public override TypeOfTask getTypeOfTask()
        {
            return TypeOfTask.EPIC;
        }

        public Epic(string name, string description, HashSet<int> subtasksIds) : base(name, description, Status.NEW)
        {
            this.SubtasksIds = subtasksIds;
        }

        public HashSet<int> SubtasksIds { get => subtasksIds; set => subtasksIds = value; }

        // добавить toString;
    }
}
