using oop_project.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = oop_project.model.Task;

namespace oop_project.managers
{
    class PersonManager
    {
        private LinkedList<Person> listOfPersons = new LinkedList<Person>();
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
            listOfPersons.AddLast(new Person(login, password));
        }

        public Task getTaskById(int id)
        {
            return fileTaskManager.getTaskById(id);
        }

        public int createNewTask(Task task)
        {
            return fileTaskManager.createNewTask(task); ;
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

        public void personInFile()
        {
            try
            {
                if (!File.Exists(Path.GetFullPath("users.csv")))
                {
                    StreamWriter fileWriter = new StreamWriter("users.csv", true);
                    fileWriter.Write(listOfPersons.ElementAt<Person>(0).ToString());
                }
                else
                {
                    StreamWriter fileWriter = new StreamWriter("users.csv", true);
                    fileWriter.Write(listOfPersons.Last?.Value.ToString());
                }
            }
            catch (NullReferenceException e)
            {
                throw new ManagerException("Файла не существует");
            }
          
        }

        private void loadUsersFromFile()
        {
            try
            {
                StreamReader sr = new StreamReader("users.csv");
                while (sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    listOfPersons.AddLast(fromString(line));
                }
            }
            catch (IOException)
            {
                throw new ManagerException("Файла не существует");
            }
        }

        private static Person fromString(string value)
        {
            Person task = null;
            string[] split = value.Split(';');
            return new Person(split[0],split[1]);
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

