using oop_project.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Task = oop_project.model.Task;

namespace oop_project.managers
{
    public class FileTaskManager : InMemoryTaskManager
    {
        string fileName;

        public string FileName { get => fileName; set => fileName = value; }

        public FileTaskManager(string path, HistoryManager historyManager) : base(historyManager)
        {
            FileName = path + ".csv";
        }

        override public Task getTaskById(int id)
        {
            Task task = base.getTaskById(id);
            save();
            return task;
        }

        override public int createNewTask(Task task)
        {
            int id = base.createNewTask(task);
            save();
            return id;
        }

        override public void updateTask(Task task)
        {
            base.updateTask(task);
            save();
        }

        override public void removeTaskById(int id)
        {
            base.removeTaskById(id);
            save();
        }

        override public void clearTasks()
        {
            base.clearTasks();
            save();
        }

        override public List<Subtask> getAllSubtasksOfEpicByEpicId(int id)
        {
            return base.getAllSubtasksOfEpicByEpicId(id);
        }

        override public List<Task> getListOfAllTasks()
        {
            return base.getListOfAllTasks();
        }

        private void save()
        {
            List<Task> tasks = getListOfAllTasks();
            StringBuilder bd = new StringBuilder();
            try
            {
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }
            }
            catch (NullReferenceException)
            {
                throw new ManagerException("Файла не существует");
            }

            foreach (Task task in tasks)
            {
                string str = task.ToString();
                bd = parser(str, bd);
            }
            bd.Append("\n");
            if (HistoryManager.getHistoryIDs().Any())
            {
                List<int> ids = HistoryManager.getHistoryIDs();
                bd = parserForIDs(ids, bd);
            }
            try
            {
                StreamWriter fileWriter = new StreamWriter(FileName, false);
                fileWriter.Write(bd.ToString());
                fileWriter.Close();
            }
            catch (IOException)
            {
                throw new ManagerException("Файла не существует");
            }
        }
        private StringBuilder parser(string str, StringBuilder bd)
        {
            string[] split = str.Split(',');
            if (split.Length == 7)
            {
                int count = 0;
                for (int i = 0; i < 10; i++)
                {
                    if (i < 5)
                    {
                        bd.Append(split[i]);
                        count++;
                        bd.Append(';');
                    }
                    else if (i > 5)
                    {
                        bd.Append(split[i - 1]);
                        if (count++ == split.Length - 1)
                        {
                            break;
                        }
                        bd.Append(';');
                    }
                    else
                    {
                        bd.Append("");
                        bd.Append(";");
                    }
                }
                bd.Append("\n");
            }
            else
            {
                int count = 0;
                foreach (string j in split)
                {
                    bd.Append(j);
                    if (count++ == split.Length - 1)
                    {
                        break;
                    }
                    bd.Append(';');
                }
                bd.Append("\n");
            }
            return bd;
        }

        private StringBuilder parserForIDs(List<int> ids, StringBuilder bd)
        {
            int count = 0;
            foreach (int j in ids)
            {
                string str = j.ToString();
                bd.Append(str);
                if (count++ == ids.Count - 1)
                {
                    break;
                }
                bd.Append(';');
            }
            bd.Append("\n");
            return bd;
        }

        public static FileTaskManager loadFromFile(string path)
        {
            FileTaskManager taskManager = new FileTaskManager(path, new InMemoryHistoryManager());
            List<string> listForTasks = new List<string>();
            try
            {
                StreamReader sr = new StreamReader(taskManager.FileName);
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    listForTasks.Add(line);
                }
                sr.Close();
                for (int i = 0; i < listForTasks.Count - 2; i++)
                {
                    taskManager.createNewTask(fromString(listForTasks[i]));
                }
                string[] idsForHistory = listForTasks[listForTasks.Count - 1].Split(';');
                foreach (string j in idsForHistory)
                {
                    taskManager.getTaskById(int.Parse(j));
                }
                return taskManager;
            }
            catch (IOException)
            {
                throw new ManagerException("Файла не существует");
            }
        }
        private static Task fromString(string value)
        {
            Task task = null;
            string[] split = value.Split(';');
            if (split[1].Equals("TASK"))
            {
                if (split[3].Equals("NEW"))
                {
                    task = new Task(split[2], split[4], Status.NEW);
                }
                else if (split[3].Equals("IN_PROGRESS"))
                {
                    task = new Task(split[2], split[4], Status.IN_PROCESS);
                }
                else
                {
                    task = new Task(split[2], split[4], Status.DONE);
                }
            }
            else if (split[1].Equals("EPIC"))
            {
                task = new Epic(split[2], split[4]);
            }
            else if (split[1].Equals("SUBTASK"))
            {
                if (split[3].Equals("NEW"))
                {
                    task = new Subtask(split[2], split[4], Status.NEW, int.Parse(split[5]));
                }
                else if (split[3].Equals("IN_PROGRESS"))
                {
                    task = new Subtask(split[2], split[4], Status.IN_PROCESS, int.Parse(split[5]));
                }
                else
                {
                    task = new Subtask(split[2], split[4], Status.DONE, int.Parse(split[5]));
                }
            }
            return task;
        }
    }
}
