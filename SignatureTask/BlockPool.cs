using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SignatureTask.Handlers;

namespace SignatureTask
{
    public class BlockPool
    {
        public ConcurrentQueue<Block> Pool { get; set; }

        public bool AllPoolRecived { get; set; }

        public event BlockAction ShowEncyptedBlock;

        public int AllBlockCount { get; set; }

        /// <summary>
        /// Максимальное количество блоков на основании оперативной памяти. 
        /// </summary>
        public int MaxCapacity { get; set; }

        public BlockPool(int capacity)
        {
            MaxCapacity = capacity;
            Pool = new ConcurrentQueue<Block>();
            AllBlockCount = 0;
        }

        public IEnumerable<Block> GetBlocks()
        {
            return Pool;
        }

        public void AddBlock(Block block)
        {
            Pool.Enqueue(block);
            AllBlockCount++;
        }

        public bool IsQueueFull()
        {
            return Pool.Count >= MaxCapacity;
        }

        public void WorkOnBlock()
        {
            if (Pool.TryDequeue(out var block))
            {
               Encrypt(block);
            }
        }


        public void Encrypt(Block block)
        {
           block.CreateHash();
           ShowEncyptedBlock.Invoke(block);
        }

    }
}
