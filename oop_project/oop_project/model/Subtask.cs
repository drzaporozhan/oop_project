using System;

namespace oop_project.model
{
    public class Subtask : Task
    {
        private int epicId;

        public Subtask(string name, string description, Status status, int epicId) : base(name, description, status)
        {
            this.EpicId = epicId;
        }

        public Subtask(int id, string name, string description, Status status, int epicId) : base(id, name, description, status)
        {
            this.EpicId = epicId;
        }

        public Subtask(string name, string description, Status status, int duration, DateTime deadline, int epicId) : base(name, description, status, duration, deadline)
        {
            this.EpicId = epicId;
        }

        public Subtask(int id, string name, string description, Status status, int duration, DateTime deadline, int epicId) : base(id, name, description, status, duration, deadline)
        {
            this.EpicId = epicId;
        }

        public int EpicId { get => epicId; set => epicId = value; }

        override public TypeOfTask getTypeOfTask()
        {
            return TypeOfTask.SUBTASK;
        }

        public override string ToString()
        {
            if (Deadline != DateTime.MinValue)
            {
                return Id +
                    "," + TypeOfTask.SUBTASK +
                    "," + Name +
                    "," + Status +
                    "," + Description +
                    "," + epicId +
                    "," + Deadline +
                    "," + Duration;
            }
            else
            {
                return Id +
                    "," + TypeOfTask.SUBTASK +
                    "," + Name +
                    "," + Status +
                    "," + Description +
                    "," + epicId;
            }
        }
    }

}
