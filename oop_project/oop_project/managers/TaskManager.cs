using oop_project.model;
using System.Collections.Generic;
using Task = oop_project.model.Task;

namespace oop_project.managers
{
    public interface TaskManager
    {
        Task getTaskById(int id);

        int createNewTask(Task task);

        void updateTask(Task task);

        void removeTaskById(int id);

        void clearTasks();

        List<Subtask> getAllSubtasksOfEpicByEpicId(int id);

        List<Task> getListOfAllTasks();

        List<Task> getPrioritizedTasks();
    }
}
