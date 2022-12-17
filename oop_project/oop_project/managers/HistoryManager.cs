using System.Collections.Generic;
using Task = oop_project.model.Task;

namespace oop_project.managers
{
    public interface HistoryManager
    {
        void add(Task task);
        void remove(Task task);
        List<Task> getHistory();
        List<int> getHistoryIDs();
    }
}
