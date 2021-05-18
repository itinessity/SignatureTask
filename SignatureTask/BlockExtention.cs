using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SignatureTask
{
   public static class BlockExtention
    {
        public static void CreateHash(this Block block)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(block.Bytes);
                block.HashString = BitConverter.ToString(hash).Replace("-", "");
                block.IsEncrypted = true;
            }
        }
    }
}
