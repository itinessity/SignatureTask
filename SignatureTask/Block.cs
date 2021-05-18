using System;
using System.Collections.Generic;
using System.Text;

namespace SignatureTask
{
    public class Block
    {
        /// <summary>
        /// Массив байт для блока. 
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// Номер блока.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Хэш код блока.
        /// </summary>
        public string HashString { get; set; }

        /// <summary>
        /// Признак наличия хэша блока.
        /// </summary>
        public bool IsEncrypted { get; set; }

        /// <summary>
        /// Блок в байтах заданного размера. 
        /// </summary>
        /// <param name="size">Размер блока в байтах</param>
        public Block(int size)
        {
            Bytes = new byte[size];
        }
    }
}
