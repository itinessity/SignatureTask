using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SignatureTask
{
    public class Operator
    {
        private string _filepath;
        private int _blocksize;

        private int _processersCount;

        private bool _logging;

        private FileStream _stream;

        private BlockPool _blockPool;
        private ThreadPool _threadpool;

        private int _queCapacity;

        
        public Operator()
        {
            _processersCount = Environment.ProcessorCount;
        }

        private void CalcCapacity()
        {
            //Вычисление общего объема оперативной памяти ПК в байтах
            var processes = Process.GetProcesses();
            long memoryUse = 0;

            foreach (Process p in processes)
            {
                memoryUse += p.PagedMemorySize64;
            }

            //вычисляем кратность блоку с погрешностью вниз
            var multiplicity = Math.Floor(Convert.ToDecimal(memoryUse / _blocksize));

            _queCapacity = Convert.ToInt32(multiplicity);

        }

        public void Start(string filepath, int blocksize, bool logging)
        {
            _filepath = filepath;
            _blocksize = blocksize;
            _logging = logging;

            CalcCapacity();

            _stream = new FileStream(_filepath, FileMode.Open);

            SetPool();
            SetTasks();
        }

        private void SetPool()
        {
            _blockPool = new BlockPool(_queCapacity);
            _threadpool = new ThreadPool(_processersCount);

            ////тестовые значения
            //_blockPool = new BlockPool(10);
            //_threadpool = new ThreadPool(3);
        }

        private void SetTasks()
        {
            var reader = new StreamReader(_stream, _blocksize, _blockPool);
            reader.BlockReaded += DoWorkWithBlock;
            reader.FileReaded += SetAllBlockRecived;

            _blockPool.ShowEncyptedBlock += ShowOnScreen;

            _threadpool.AddTask(reader.ProcessRead);
        }

        public void DoWorkWithBlock()
        {
            _threadpool.AddTask(() => _blockPool.WorkOnBlock());
        }

        public void SetAllBlockRecived()
        {
            _blockPool.AllPoolRecived = true;

            var percent = Math.Round((Convert.ToDecimal(_blockPool.AllBlockCount) / Convert.ToDecimal(_blockPool.MaxCapacity)) * 100, 3);
            var message = $"Файл прочтен. \n Максимальная вместимость пула памяти: {_blockPool.MaxCapacity} \n Количество блоков файла: { _blockPool.AllBlockCount} ({percent} %)";
            Logger.Log(message);
        }

        public void ShowOnScreen(Block item)
        {
            if (string.IsNullOrEmpty(item.HashString))
            {
                var errmessage = "Encrypted Error. Block number: " + item.Index.ToString();
                throw new Exception(errmessage);
            }

            if (_logging)
            {
                var message = "Номер блока: " + item.Index.ToString() + " Hash: " + item.HashString;
                Logger.Log(message);
            }
        }
    }
}
