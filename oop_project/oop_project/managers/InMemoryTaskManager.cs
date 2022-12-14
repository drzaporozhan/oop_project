using oop_project.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace oop_project.managers
{
    class InMemoryTaskManager : TaskManager
    {
        private Dictionary<int, Task> listOfTasksInMemory = new Dictionary<int, Task>();
        private int idCount = 0;
        private HistoryManager historyManager;
        private SortedSet<Task> listOfTasksWithTime = new SortedSet<Task>(
            new DateComparer());

        internal HistoryManager HistoryManager { get => historyManager; set => historyManager = value; }

        public InMemoryTaskManager(HistoryManager historyManager)
        {
            this.HistoryManager = historyManager;
        }

        internal class DateComparer : IComparer<Task>
        {
            public int Compare(Task o1, Task o2)
            {
                if (o1.Deadline != null && o2.Deadline!=null)
                {
                    return o1.Deadline.CompareTo(o2.Deadline);
                } else if (o1.Deadline == null && o2.Deadline != null)
                {
                    return 1;
                } else if(o1.Deadline != null && o2.Deadline == null)
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
            foreach (int id in listOfTasksInMemory.Keys)
            {
                removeTaskById(id);
            }
        }

        virtual public int createNewTask(model.Task task)
        {
            if (listOfTasksInMemory.ContainsKey(task.Id))
            {
                Console.WriteLine("Ошибка, эта задача уже есть");
            }
            else if (task.getTypeOfTask().Equals(TypeOfTask.SUBTASK))
            {
                Subtask subtask = (Subtask)task;
                int epicId = subtask.EpicId;
                if (!listOfTasksInMemory.ContainsKey(epicId))
                {
                    Console.WriteLine("Ошибка, такого epicId нет в базе");
                }
                subtask.Id=makeNewId();
                listOfTasksInMemory.Add(subtask.Id, subtask);
                Epic epic = (Epic)listOfTasksInMemory[epicId];
                HashSet<int> subtasksIds = epic.SubtasksIds;
                subtasksIds.Add(subtask.Id);
                updateEpicStatus(epic);
                return subtask.Id;
            }
            else
            {
                task.Id=makeNewId();
                listOfTasksInMemory.Add(task.Id, task);
                listOfTasksWithTime.Add(task);
                return task.Id;
            }
            return -100;
            //exception
        }

        private void updateEpicStatus(Epic epic)
        {
            List<Subtask> subtasks = getAllSubtasksOfEpicByEpicId(epic.Id);
            Dictionary<Status, int> statusCounter = new Dictionary<Status, int>();
            int valueOfAllSubtasks = 0;
            int statusCount;
            bool flag = true;
            foreach (Subtask subtask in subtasks)
            {
                statusCount = getOrDefault(statusCounter, subtask.Status, 0);
                statusCount++;
                statusCounter.Add(subtask.Status, statusCount);
                valueOfAllSubtasks++;
            }
            if (valueOfAllSubtasks == 0 || getOrDefault(statusCounter, Status.NEW, 0) == valueOfAllSubtasks)
            {
                epic.Status=Status.NEW;
            }
            else if (getOrDefault(statusCounter, Status.DONE, 0) == valueOfAllSubtasks)
            {
                epic.Status = Status.DONE;
            }
            else
            {
                epic.Status = Status.IN_PROCESS;
            }
            epic.Duration = 0;
            foreach (Subtask subtask in  subtasks)
            {
                if (subtask.Deadline != null && subtask.Duration>0)
                {
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
                Console.WriteLine("Ошибка нельзя получить подзадачи не через id эпика");
            }
            Epic epic = (Epic)task;
            HashSet<int> subtasksIds = epic.SubtasksIds;
            List<Subtask> subtasks = new List<Subtask>();
            foreach(int subtaskId in subtasksIds)
            {
                Subtask subtask = (Subtask)listOfTasksInMemory[subtaskId] ;
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
                Console.WriteLine("Ошибка, этой задачи нет");
                // add Exception, make try catch
            }
            HistoryManager.add(getTaskById(id));
            return listOfTasksInMemory[id];
        }

        virtual public void removeTaskById(int id)
        {
            if (!listOfTasksInMemory.ContainsKey(id))
            {
                Console.WriteLine("Ошибка, этой задачи нет ");
                return;
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
                if (!(epic.SubtasksIds.Count==0 || epic.Status == Status.DONE))
                {
                    Console.WriteLine("Ошибка, нельзя удалять эпик с подзадачами, " +
                            "только если нет подзадач или их статус 'DONE'");
                    return;
                }
                else
                {
                    listOfTasksInMemory.Remove(id);
                }
            }
            else
            {
                listOfTasksWithTime.Remove(getTaskById(id));
                listOfTasksInMemory.Remove(id);
            }
        }

        virtual public void updateTask(model.Task task)
        {
           
            if (!listOfTasksInMemory.ContainsKey(task.Id))
            {
                Console.WriteLine("Ошибка, этой задачи нет ");
                return;
            }
            if (task.getTypeOfTask().Equals(TypeOfTask.SUBTASK))
            {
                Subtask subtask = (Subtask)task;
                int epicId = subtask.EpicId;
                if (!listOfTasksInMemory.ContainsKey(epicId))
                {
                    Console.WriteLine("Ошибка, такого epicId нет в базе");
                    return;
                }
                listOfTasksInMemory.Add(subtask.Id, subtask);
                Epic epic = (Epic)listOfTasksInMemory[epicId];
                updateEpicStatus(epic);
            }
            else
            {
                listOfTasksInMemory.Add(task.Id, task);
            }
        }

        private int getOrDefault(Dictionary<Status, int> dict,Status status,  int defaultValue)
        {
            int count = defaultValue;
            foreach(Status st in dict.Keys)
            {
                if (st.Equals(status))
                {
                    count++;
                }
            }
            return count;
        }

    }
}
