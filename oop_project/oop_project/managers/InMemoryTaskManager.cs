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



        private int makeNewId()
        {
            return idCount++;
        }

        public void clearTasks()
        {
            List<int> list = new List<int>();
            foreach (int i in listOfTasksInMemory.Keys)
            {
                list.Add(i);
            }
            foreach (int id in list)
            {
                removeTaskById(id);
            }
        }

        public int createNewTask(model.Task task)
        {
            if (task.Id>-1)
            {
                Console.WriteLine("Ошибка, задача с недопустимым id");
            }
            else if (listOfTasksInMemory.ContainsKey(task.Id))
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
              //  tasksWithTime.add(task);
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
            DateTime start;
            DateTime end;
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
            /*foreach (Subtask subtask in  subtasks)
            {
                if (subtask.getStartTime() != null && subtask.getDuration() != null)
                {
                    if (subtasks.size() == 1 || flag)
                    {
                        epic.setStartTime(subtask.getStartTime());
                        epic.setEndTime(subtask.getEndTime());
                        epic.setDuration(Duration.between(subtask.getStartTime(), subtask.getEndTime()));
                        flag = false;
                    }
                    else
                    {
                        if (subtask.getStartTime().isBefore(epic.getStartTime()))
                        {
                            epic.setStartTime(subtask.getStartTime());
                        }
                        if (subtask.getEndTime().isAfter(epic.getEndTime()))
                        {
                            epic.setEndTime(subtask.getEndTime());
                        }
                        epic.setDuration(Duration.between(subtask.getStartTime(), subtask.getEndTime()));
                    }
                }

            }*/
        }

        public List<Subtask> getAllSubtasksOfEpicByEpicId(int id)
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

        public List<model.Task> getListOfAllTasks()
        {
            List<Task> allTasks = new List<Task>();
            foreach (Task task in listOfTasksInMemory.Values)
            {
                allTasks.Add(task);
            }
            return allTasks;
        }

        public List<model.Task> getPrioritizedTasks()
        {
            throw new NotImplementedException();
        }

        public model.Task getTaskById(int id)
        {
            if (!listOfTasksInMemory.ContainsKey(id))
            {
                Console.WriteLine("Ошибка, этой задачи нет");
                // add Exception, make try catch
            }
            // add history manager
            return listOfTasksInMemory[id];
        }

        public void removeTaskById(int id)
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
                //tasksWithTime.remove(getTaskById(id));
                listOfTasksInMemory.Remove(id);
            }
        }

        public void updateTask(model.Task task)
        {
           // if (task.Id == null)
           // {
           //     System.out.println("Ошибка, задача с пустым id");
           //     return;
           // }
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
