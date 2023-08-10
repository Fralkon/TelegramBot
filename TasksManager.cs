using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    internal class TasksManager
    {
        private List<Task> PoolTasks = new List<Task>();
        public TasksManager() {  }
        public void Add(Task task)
        {
            lock(PoolTasks)
            {
                PoolTasks.Add(task);
                if (PoolTasks.Count == 1)
                    Task.Factory.StartNew(() => { Start(); });
            }
        }
        private void Start()
        {
            while (PoolTasks.Count > 0)
            {
                PoolTasks[0].Start();
                PoolTasks[0].Wait();
                lock (PoolTasks)
                {
                    PoolTasks.RemoveAt(0);
                }
            }
        }
    }
}
