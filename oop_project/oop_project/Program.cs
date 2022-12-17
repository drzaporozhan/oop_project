using oop_project.managers;
using oop_project.model;
using System;
using System.Collections.Generic;
using Task = oop_project.model.Task;

namespace oop_project
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogInMenu());*/

            PersonManager pm = new PersonManager();
            /*pm.createUser("user1", "nadezhniyparol");
            int id1 = pm.createNewTask(new Task("task1", "very hard", Status.NEW));
            pm.getTaskById(id1);
            int id2 = pm.createNewTask(new Epic("epic1", "not very hard"));
            int id3 = pm.createNewTask(new Subtask("sub1", "not hard", Status.NEW, id2));
            int id4 = pm.createNewTask(new Subtask("sub1", "easy", Status.DONE, 20, DateTime.Now, id2));
            pm.getTaskById(id3);
            pm.getTaskById(id4);
            pm.getTaskById(id2);
            pm.updateTask(new Task(id1, "task1", "now easy", Status.DONE));*/
            pm.loggingUser(new Person("user1", "nadezhniyparol"));
            List<Task> l1 = pm.getListOfAllTasks();
            List<Task> l2 = pm.getListOfHistory();
        }
    }
}
