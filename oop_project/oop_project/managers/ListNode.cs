using Task = oop_project.model.Task;

namespace oop_project.managers
{
    public class ListNode
    {
        private ListNode previous;
        private ListNode next;
        private Task task = null;

        public ListNode(ListNode obj, ListNode obj1, Task task)
        {
            this.previous = obj;
            this.next = obj1;
            this.task = task;
        }
        public ListNode getPrevious()
        {
            return previous;
        }

        public void setPrevious(ListNode previous)
        {
            this.previous = previous;
        }

        public ListNode getNext()
        {
            return next;
        }

        public void setNext(ListNode next)
        {
            this.next = next;
        }

        public Task getTask()
        {
            return task;
        }

        public void setTask(Task task)
        {
            this.task = task;
        }
    }
}
