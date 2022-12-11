using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_project.model
{
    class Epic : Task
    {
        private HashSet<int> subtasksIds;

        public Epic(string name, string description, HashSet<int> subtasksIds) : base(name, description, Status.NEW)
        {
            this.SubtasksIds = subtasksIds;
        }

        public HashSet<int> SubtasksIds { get => subtasksIds; set => subtasksIds = value; }

        public override string ToString()
        {
            if (Deadline != null)
            {
                return Id +
                    "," + TypeOfTask.EPIC +
                    "," + Name +
                    "," + Status +
                    "," + Description +
                    "," + Deadline +
                    "," + Duration;
            }
            else
            {
                return Id +
                    "," + TypeOfTask.EPIC +
                    "," + Name +
                    "," + Status +
                    "," + Description;
            }
        }
    }
}
