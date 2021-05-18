using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SignatureTask
{
    public class ThreadPool
    {
        public List<Thread> Threads { get; }

        public Queue<Action> Tasks { get; }

        public ThreadPool(int count)
        {
            Threads = new List<Thread>();
            Tasks = new Queue<Action>();

            Inizialize(count);
        }

        private void Inizialize(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var thread = new Thread(ListenQue)
                {
                    IsBackground = true,
                    Name = i.ToString()
                };
                thread.Start();
                Threads.Add(thread);
            }
        }

        private void ListenQue()
        {
            while (true)
            {
                if (Tasks.Count != 0)
                {
                    lock (Tasks)
                    {
                        if (Tasks.TryDequeue(out var Task))
                        {
                            Task();
                        }

                        Monitor.Pulse(Tasks);
                    }
                }
            }
        }

        public void AddTask(Action task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            lock (Tasks)
            {
                Tasks.Enqueue(task);
                Monitor.Pulse(Tasks);
            }
        }

    }
}
