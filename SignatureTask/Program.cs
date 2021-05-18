using System;
using System.IO;

namespace SignatureTask
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Введите путь к файлу:");
                var file = Console.ReadLine();

                //// маленький файл
                //file = @"D:\шаблон.xlsx";

                ////большой файл
                //file = @"D:\torrent\test.mkv";

                if (File.Exists(file))
                {
                    var block = GetFileBlock();

                    ////тестовый блок
                    //block = 512;

                    if (block > 0)
                    {
                        var log = GetEnteringFullLog();

                        var op = new Operator();
                        op.Start(file, block, log);

                        Console.WriteLine("Процесс в работе...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный путь к файлу!");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        private static int GetFileBlock()
        {
            Console.WriteLine("Введите размер блока в байтах:");
            var block = Console.ReadLine();

            if (int.TryParse(block, out var data) && data > 0)
            {
                return data;
            }
            else
            {
                Console.WriteLine("Некорректный размер блока");
                Console.ReadKey();
                return 0;
            }
        }


        private static bool GetEnteringFullLog()
        {
            Console.WriteLine("Требуется ли вывод номера блока с хэш-кодом? Y/N:");
            var answer = Console.ReadLine();

            if (answer == "Y" || answer == "N")
            {
                return answer switch
                {
                    "Y" => true,
                    "N" => false,
                    _ => false,
                };
            }
            else
            {
                Console.WriteLine("Некорректное значение!");
                Console.ReadKey();
                return false;
            }
        }
    }
}
