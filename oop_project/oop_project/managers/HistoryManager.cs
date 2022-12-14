using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = oop_project.model.Task;

namespace oop_project.managers
{
    interface HistoryManager
    {
        void add(Task task);
        void remove(Task task);
        List<Task> getHistory();
        List<int> getHistoryIDs();
    }
}
