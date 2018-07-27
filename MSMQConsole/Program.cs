using System;
using System.Collections.Generic;
using System.Messaging;
using System.Threading;

namespace Practice.WindowsServices
{
    class Program
    {
        private const string QUEUE_ROOT = @".\private$\";
        private static List<string> VALID_OPTIONS = new List<string> { "1", "2", "3", "e", };

        private static MessageQueue QueueLogQueue = new MessageQueue();
        private static string Input = string.Empty;

        static void Main()
        {

            while (Input.ToLower() != "e")
            {
                Console.WriteLine("1) Create queue");
                Console.WriteLine("2) List queues");
                Console.WriteLine("3) Delete queue");
                Console.WriteLine("Press to execute, or press 'e' to exists(case-insensitive): ");
                Console.Write(">>");

                Input = Console.ReadLine();
                if (false == VALID_OPTIONS.Exists(item => item == Input))
                {
                    Console.WriteLine($"Input invalid."); Console.WriteLine();
                }
                else
                {
                    switch (Input)
                    {
                        case "1":
                            CreateQueue();
                            break;
                        case "2":
                            ListQueues();
                            break;
                        case "3":
                            DeleteQueue();
                            break;
                    }
                }
            }
            
            Console.WriteLine("This window will close automatically in");
            var counter = 3;
            do
            {
                Console.WriteLine($"{counter}...");
                Thread.Sleep(1000);
            } while (--counter > 0) ;
            return;
        }

        private static void CreateQueue()
        {
            var queuePath = string.Empty;

            Console.WriteLine("Enter queue name: ");
            Console.Write(">>");

            Input = Console.ReadLine();
            while(string.IsNullOrWhiteSpace(Input))
            {
                Console.WriteLine($"Input invalid.");
                Console.WriteLine("Enter queue name: ");
                Console.Write(">>");

                Input = Console.ReadLine();
            }

            queuePath = QUEUE_ROOT + Input;
            if (MessageQueue.Exists(queuePath))
            {
                Console.WriteLine($"Queue: {Input} already exists");
            }
            else
            {
                MessageQueue.Create(queuePath);
                Console.WriteLine($"Queue: {Input} created");
            }

            Console.WriteLine();
        }

        private static void ListQueues()
        {
            var queues = MessageQueue.GetPrivateQueuesByMachine(".");
            foreach (var queue in queues)
            {
                Console.WriteLine(queue.QueueName);
            }
            Console.WriteLine();
        }

        private static void DeleteQueue()
        {
            var queuePath = string.Empty;

            Console.WriteLine("Enter queue name: ");
            Console.Write(">>");

            Input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(Input))
            {
                Console.WriteLine($"Input invalid.");
                Console.WriteLine("Enter queue name: ");
                Console.Write(">>");

                Input = Console.ReadLine();
            }

            queuePath = QUEUE_ROOT + Input;
            if (!MessageQueue.Exists(queuePath))
            {
                Console.WriteLine($"Queue: {Input} not exists");
            }
            else
            {
                MessageQueue.Delete(queuePath);
                Console.WriteLine($"Queue: {Input} deleted");
            }

            Console.WriteLine();
        }
    }

    class QueueMessage
    {
        public string Label { get; set; }
        public string Message { get; set; }
        public bool IsImportant { get; set; }

        public QueueMessage(string label, string message, bool isImportant)
        {
            Label = label;
            Message = message;
            IsImportant = isImportant;
        }
    }
}
