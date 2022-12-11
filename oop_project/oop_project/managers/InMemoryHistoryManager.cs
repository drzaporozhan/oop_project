using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = oop_project.model.Task;

namespace oop_project.managers
{
    class InMemoryHistoryManager : HistoryManager
    {
        private Dictionary<int, Task> fortask = new Dictionary<int, Task>();
        private Dictionary<int, ListNode> nodes = new Dictionary<int, ListNode>();
        private ListNode head = null;
        private ListNode tail = null;
        private int ID = 0;

        public void add(Task task)
        {
            if (fortask.Count() > 9)
            {
                fortask.Add(ID, task);
                int key = 0;
                foreach (int i in fortask.Keys)
                {
                    key = i;
                    break;
                }
                removeValueHashed(fortask[key]);
                fortask.Remove(key);
                linkLast(task);

            }
            else
            {
                fortask.Add(ID, task);
                linkLast(task);
            }
            ID++;
        }

        public void remove(Task task)
        {
            removeValueHashed(task);
        }

        public List<Task> getHistory()
        {

            return getValues();
        }

        public List<int> getHistoryIDs()
        {
            List<Task> val = getValues();
            List<int> ids = new List<int>();
            foreach (Task i in val)
            {
                String str = i.ToString();
                String[] split = str.Split(',');
                ids.Add(Int32.Parse(split[0]));
            }
            return ids;
        }

        private void linkLast(Task value)
        {
            if (head == null)
            {
                this.head = createNode(value, ID);
                return;
            }
            ListNode currentNode = this.head;
            while (currentNode.getNext() != null)
            {
                currentNode = currentNode.getNext();
            }
            ListNode newNode = createNode(value, ID);
            currentNode.setNext(newNode);
            newNode.setPrevious(currentNode);
        }

        private void removeValueHashed(Task valueToRemove)
        {
            foreach (int i in fortask.Keys)
            {
                if (fortask[i].Equals(valueToRemove))
                {
                    this.removeNode(this.nodes[i]);
                    this.nodes.Remove(i);
                    break;
                }
            }
        }

        private void removeNode(ListNode nodeToRemove)
        {
            if (nodeToRemove.getPrevious() == null)
            {
                this.head = nodeToRemove.getNext();
                nodeToRemove.getNext().setPrevious(null);
                return;
            }
            nodeToRemove.getPrevious().setNext(nodeToRemove.getNext());
            if (nodeToRemove.getNext() != null)
            {
                nodeToRemove.getNext().setPrevious(nodeToRemove.getPrevious());
            }
        }

        private ListNode createNode(Task value, int ID)
        {
            ListNode newNode = new ListNode(null, null, value);
            this.nodes.Add(ID, newNode);
            return newNode;
        }

        public List<Task> getValues()
        {
            HashSet<Task> uniqueTasks = new HashSet<Task>();
            ListNode currentNode = this.head;
            uniqueTasks.Add(currentNode.getTask());
            while (currentNode.getNext() != null)
            {
                currentNode = currentNode.getNext();
                uniqueTasks.Add(currentNode.getTask());
            }
            List<Task> values = new List<Task>(uniqueTasks);
            if (!values.Any())
            {
                Console.WriteLine("error!");// написать исключение
                return values;// уберем
            }
            else
            {
                return values;
            }

        }
    }
}
