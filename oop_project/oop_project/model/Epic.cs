using System;
using System.Collections.Generic;

namespace oop_project.model
{
    public class Epic : Task
    {
        private HashSet<int> subtasksIds;

        public Epic(string name, string description, HashSet<int> subtasksIds) : base(name, description, Status.NEW)
        {
            SubtasksIds = subtasksIds;
        }

        public Epic(string name, string description) : base(name, description, Status.NEW)
        {
            subtasksIds = new HashSet<int>();
        }

        public HashSet<int> SubtasksIds { get => subtasksIds; set => subtasksIds = value; }

        override public TypeOfTask getTypeOfTask()
        {
            return TypeOfTask.EPIC;
        }

        public override string ToString()
        {
            if (Deadline != DateTime.MinValue)
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
