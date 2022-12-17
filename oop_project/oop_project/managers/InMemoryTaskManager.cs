using oop_project.model;
using System;
using System.Collections.Generic;
using System.Linq;


namespace oop_project.managers
{
    public class InMemoryTaskManager : TaskManager
    {
        private Dictionary<int, Task> listOfTasksInMemory = new Dictionary<int, Task>();
        private int idCount = 0;
        private HistoryManager historyManager;
        private SortedSet<Task> listOfTasksWithTime = new SortedSet<Task>(
            new DateComparer());

        public HistoryManager HistoryManager { get => historyManager; set => historyManager = value; }

        public InMemoryTaskManager(HistoryManager historyManager)
        {
            HistoryManager = historyManager;
        }

        internal class DateComparer : IComparer<Task>
        {
            public int Compare(Task o1, Task o2)
            {
                if (o1.Deadline != DateTime.MinValue && o2.Deadline != DateTime.MinValue)
                {
                    return o1.Deadline.CompareTo(o2.Deadline);
                }
                else if (o1.Deadline == DateTime.MinValue && o2.Deadline != DateTime.MinValue)
                {
                    return 1;
                }
                else if (o1.Deadline != DateTime.MinValue && o2.Deadline == DateTime.MinValue)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }

        private int makeNewId()
        {
            return idCount++;
        }

        virtual public void clearTasks()
        {
            listOfTasksInMemory.Clear();
            listOfTasksWithTime.Clear();
        }

        virtual public int createNewTask(model.Task task)
        {
            if (listOfTasksInMemory.ContainsKey(task.Id))
            {
                throw new ManagerException("Ошибка, эта задача уже есть");
            }
            else if (task.getTypeOfTask().Equals(TypeOfTask.SUBTASK))
            {
                Subtask subtask = (Subtask)task;
                int epicId = subtask.EpicId;
                if (!listOfTasksInMemory.ContainsKey(epicId))
                {
                    throw new ManagerException("Ошибка, такого эпика нет в базе");
                }
                subtask.Id = makeNewId();
                listOfTasksInMemory.Add(subtask.Id, subtask);
                Epic epic = (Epic)listOfTasksInMemory[epicId];
                HashSet<int> subtasksIds = epic.SubtasksIds;
                if (subtasksIds.Count == 0)
                {
                    subtasksIds = new HashSet<int>();
                }
                subtasksIds.Add(subtask.Id);
                epic.SubtasksIds = subtasksIds;
                updateEpicStatus(epic);
                return subtask.Id;
            }
            else
            {
                task.Id = makeNewId();
                listOfTasksInMemory.Add(task.Id, task);
                listOfTasksWithTime.Add(task);
                return task.Id;
            }
            throw new ManagerException("Невозможно создать задачу");
        }

        private void updateEpicStatus(Epic epic)
        {
            List<Subtask> subtasks = getAllSubtasksOfEpicByEpicId(epic.Id);
            Dictionary<Status, int> statusCounter = new Dictionary<Status, int>();
            statusCounter.Add(Status.DONE, 0);
            statusCounter.Add(Status.IN_PROCESS, 0);
            statusCounter.Add(Status.NEW, 0);
            int valueOfAllSubtasks = subtasks.Count;
            bool flag = true;
            foreach (Subtask subtask in subtasks)
            {
                statusCounter.Remove(subtask.Status);
                statusCounter.Add(subtask.Status, getOrDefault(subtasks, subtask.Status, 0));

            }
            if (valueOfAllSubtasks == 0 || statusCounter[Status.NEW] == valueOfAllSubtasks)
            {
                epic.Status = Status.NEW;
            }
            else if (statusCounter[Status.DONE] == valueOfAllSubtasks)
            {
                epic.Status = Status.DONE;
            }
            else
            {
                epic.Status = Status.IN_PROCESS;
            }
            foreach (Subtask subtask in subtasks)
            {
                if (subtask.Deadline != DateTime.MinValue && subtask.Duration > 0)
                {
                    epic.Duration = 0;
                    if (subtasks.Count() == 1 || flag)
                    {
                        epic.Deadline = subtask.Deadline;
                        epic.Duration = subtask.Duration;
                        flag = false;
                    }
                    else
                    {
                        if (subtask.Deadline < epic.Deadline)
                        {
                            epic.Deadline = subtask.Deadline;
                        }
                        epic.Duration += subtask.Duration;
                    }
                }

            }
        }

        virtual public List<Subtask> getAllSubtasksOfEpicByEpicId(int id)
        {
            Task task = listOfTasksInMemory[id];
            if (!task.getTypeOfTask().Equals(TypeOfTask.EPIC))
            {
                throw new ManagerException("Ошибка, невозможно получить все подзадачи эпика");
            }
            Epic epic = (Epic)task;
            HashSet<int> subtasksIds = epic.SubtasksIds;
            List<Subtask> subtasks = new List<Subtask>();
            foreach (int subtaskId in subtasksIds)
            {
                Subtask subtask = (Subtask)listOfTasksInMemory[subtaskId];
                subtasks.Add(subtask);
            }
            return subtasks;
        }

        virtual public List<model.Task> getListOfAllTasks()
        {
            List<Task> allTasks = new List<Task>();
            foreach (Task task in listOfTasksInMemory.Values)
            {
                allTasks.Add(task);
            }
            return allTasks;
        }

        virtual public List<model.Task> getPrioritizedTasks() => listOfTasksWithTime.ToList();

        virtual public model.Task getTaskById(int id)
        {
            if (!listOfTasksInMemory.ContainsKey(id))
            {
                throw new ManagerException("Ошибка, этой задачи нет");
            }
            HistoryManager.add(listOfTasksInMemory[id]);
            return listOfTasksInMemory[id];
        }

        virtual public void removeTaskById(int id)
        {
            if (!listOfTasksInMemory.ContainsKey(id))
            {
                throw new ManagerException("Невозможно удалить задачу, её нет");
            }
            Task task = listOfTasksInMemory[id];
            if (task.getTypeOfTask().Equals(TypeOfTask.SUBTASK))
            {
                Subtask subtask = (Subtask)task;
                int epicId = subtask.EpicId;
                Epic epic = (Epic)listOfTasksInMemory[epicId];
                epic.SubtasksIds.Remove(subtask.Id);
                listOfTasksInMemory.Remove(id);
                updateEpicStatus(epic);
            }
            else if (task.getTypeOfTask().Equals(TypeOfTask.EPIC))
            {
                Epic epic = (Epic)task;
                if (!(epic.SubtasksIds.Count == 0 || epic.Status == Status.DONE))
                {
                    throw new ManagerException("Ошибка, нельзя удалять эпик с подзадачами, " +
                        "только если нет подзадач или их статус 'DONE'");
                }
                else
                {
                    listOfTasksInMemory.Remove(id);
                }
            }
            else
            {
                listOfTasksWithTime.Remove(listOfTasksInMemory[id]);
                listOfTasksInMemory.Remove(id);
            }
        }

        virtual public void updateTask(Task task)
        {

            if (!listOfTasksInMemory.ContainsKey(task.Id))
            {
                throw new ManagerException("Ошибка, этой задачи нет ");
            }
            if (task.getTypeOfTask().Equals(TypeOfTask.SUBTASK))
            {
                Subtask subtask = (Subtask)task;
                int epicId = subtask.EpicId;
                if (!listOfTasksInMemory.ContainsKey(epicId))
                {
                    throw new ManagerException("Ошибка, такого эпика нет в базе");
                }
                listOfTasksInMemory.Remove(subtask.Id);
                listOfTasksInMemory.Add(subtask.Id, subtask);
                Epic epic = (Epic)listOfTasksInMemory[epicId];
                updateEpicStatus(epic);
            }
            else
            {
                listOfTasksInMemory.Remove(task.Id);
                listOfTasksInMemory.Add(task.Id, task);
            }
        }

        private int getOrDefault(List<Subtask> dict, Status status, int defaultValue)
        {
            int count = defaultValue;
            foreach (Task st in dict)
            {
                if (st.Status.Equals(status))
                {
                    count++;
                }
            }
            return count;
        }

    }
}
