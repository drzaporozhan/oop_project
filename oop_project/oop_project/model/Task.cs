using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_project.model
{
    class Task
    {
        private int id;
        private string name;
        private string description;
        private Status status;
        private int duration;
        private DateTime deadline;

        public Task(string name, string description, Status status)
        {
            this.Name = name;
            this.Description = description;
            this.Status = status;
        }

        //пока временно
        public Task(int id, string name, string description, Status status)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Status = status;
        }

        public Task(string name, string description, Status status, int duration, DateTime deadline) : this(name, description, status)
        {
            this.Duration = duration;
            this.Deadline = deadline;
        }

        //пока временно
        public Task(int id, string name, string description, Status status, int duration, DateTime deadline) : this(id, name, description, status)
        {
            this.Duration = duration;
            this.Deadline = deadline;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public int Duration { get => duration; set => duration = value; }
        public DateTime Deadline { get => deadline; set => deadline = value; }
        public Status Status { get => status; set => status = value; }

        virtual public TypeOfTask getTypeOfTask()
        {
            return TypeOfTask.TASK;
        }


        public override bool Equals(object obj)
        {
            return obj is Task task &&
                   Id == task.Id &&
                   Name == task.Name &&
                   Description == task.Description &&
                   Status == task.Status;
        }

        public override int GetHashCode()
        {
            int hashCode = 489239526;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + Status.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            if (Deadline != null)
            {
                return Id +
                    "," + TypeOfTask.TASK +
                    "," + Name +
                    "," + Status +
                    "," + Description +
                    "," + Deadline +
                    "," + Duration;
            }
            else
            {
                return Id +
                    "," + TypeOfTask.TASK +
                    "," + Name +
                    "," + Status +
                    "," + Description;
            }
        }
    }
}