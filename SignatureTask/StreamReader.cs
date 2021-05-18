using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SignatureTask
{
    public class StreamReader
    {
        private FileStream _stream;
        private int _blocksize;
        private int _currentblock;
        private BlockPool _blockPool;

        public event Action BlockReaded;
        public event Action FileReaded;


        public StreamReader(FileStream stream, int blockSize, BlockPool blockPool)
        {
            _stream = stream;
            _blocksize = blockSize;
            _currentblock = 0;
            _blockPool = blockPool;
        }

        private Block Read()
        {
            Block block;
               lock (_stream)
                {
                    if (!_stream.CanRead) return null;

                    var buf = new byte[_blocksize];
                    var realsize = _stream.Read(buf, 0, _blocksize);
                    if (realsize == 0)
                    {
                        _stream.Close();
                        FileReaded.Invoke();
                        return null;
                    }

                block = new Block(realsize)
                {
                    Index = _currentblock,
                    Bytes = buf
                };

                _currentblock++;
                }
            return block;
        }

        private void WaitOnBlock(Block block)
        {
            while(_blockPool.IsQueueFull())
            {
                Logger.Log("Очередь переполнена, ожидается выделение места");
            }

            PlaceBlock(block);
        }

        private void PlaceBlock(Block block)
        {
            _blockPool.AddBlock(block);
            BlockReaded.Invoke();
        }

        public void ProcessRead()
        {
            Block block;
            while((block = Read()) != null)
            {
                if (!_blockPool.IsQueueFull())
                {
                    PlaceBlock(block);
                }
                else
                {
                    WaitOnBlock(block);
                }
            }
        }
    }
}
