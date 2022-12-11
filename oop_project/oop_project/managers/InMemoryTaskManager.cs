using oop_project.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_project.managers
{
    class InMemoryTaskManager : TaskManager
    {
        public void clearTasks()
        {
            throw new NotImplementedException();
        }

        public int createNewTask(model.Task task)
        {
            throw new NotImplementedException();
        }

        public List<Subtask> getAllSubtasksOfEpicByEpicId(int id)
        {
            throw new NotImplementedException();
        }

        public List<model.Task> getListOfAllTasks()
        {
            throw new NotImplementedException();
        }

        public List<model.Task> getPrioritizedTasks()
        {
            throw new NotImplementedException();
        }

        public model.Task getTaskById(int id)
        {
            throw new NotImplementedException();
        }

        public void removeTaskById(int id)
        {
            throw new NotImplementedException();
        }

        public void updateTask(model.Task task)
        {
            throw new NotImplementedException();
        }
    }
}
