using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using oop_project.model;
using Task = oop_project.model.Task;

namespace oop_project.managers
{
    interface TaskManager
    {
        Task getTaskById(int id) ;

        int createNewTask(Task task) ;

        void updateTask(Task task) ;

        void removeTaskById(int id) ;

        void clearTasks();

        List<Subtask> getAllSubtasksOfEpicByEpicId(int id);

        List<Task> getListOfAllTasks();

        //HistoryManager getHistoryManager();

        List<Task> getPrioritizedTasks();
    }
}
