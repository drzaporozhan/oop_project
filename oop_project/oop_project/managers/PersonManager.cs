using oop_project.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Task = oop_project.model.Task;

namespace oop_project.managers
{
    public class PersonManager
    {
        private List<Person> listOfPersons = new List<Person>();
        private FileTaskManager fileTaskManager;

        internal FileTaskManager FileTaskManager { get => fileTaskManager; set => fileTaskManager = value; }

        public void createUser(string login, string password)
        {
            loadUsersFromFile();
            foreach (Person i in listOfPersons)
            {
                if (i.Login.Equals(login))
                {
                    throw new ManagerException("Такой пользователь уже существует");
                }
            }
            FileTaskManager = new FileTaskManager(login, new InMemoryHistoryManager());
            listOfPersons.Add(new Person(login, password));
            personInFile();
        }

        public Task getTaskById(int id)
        {
            return fileTaskManager.getTaskById(id);
        }

        public int createNewTask(Task task)
        {
            return fileTaskManager.createNewTask(task);
        }

        public void updateTask(Task task)
        {
            fileTaskManager.updateTask(task);
        }

        public void removeTaskById(int id)
        {
            fileTaskManager.removeTaskById(id);
        }

        public void clearTasks()
        {
            fileTaskManager.clearTasks();
        }

        public List<Subtask> getAllSubtasksOfEpicByEpicId(int id)
        {
            return fileTaskManager.getAllSubtasksOfEpicByEpicId(id);
        }

        public List<Task> getListOfAllTasks()
        {
            return fileTaskManager.getListOfAllTasks();
        }

        public List<Task> getListOfHistory()
        {
            return fileTaskManager.HistoryManager.getHistory();
        }

        private void personInFile()
        {
            StreamWriter fileWriter = new StreamWriter("users.csv", true);
            fileWriter.Write(listOfPersons.Last().ToString());
            fileWriter.Close();
        }

        private void loadUsersFromFile()
        {
            if (!File.Exists("users.csv")) File.Create("users.csv").Close();
            else
            {
                StreamReader sr = new StreamReader("users.csv");
                while (sr.Peek() >= 0)
                {
                    String line = sr.ReadLine();
                    listOfPersons.Add(fromString(line));
                }
                sr.Close();
            }
        }

        private static Person fromString(string value)
        {
            string[] split = value.Split(';');
            return new Person(split[0], split[1]);
        }

        public void loggingUser(Person person)
        {
            loadUsersFromFile();
            if (listOfPersons.Contains(person))
            {
                FileTaskManager = FileTaskManager.loadFromFile(person.Login);
            }
        }
    }
}

