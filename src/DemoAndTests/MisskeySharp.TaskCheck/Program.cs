namespace MisskeySharp.TaskCheck
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var source = new CancellationTokenSource();
            var token = source.Token;

            var task = new Task(() => TaskProc(), token);
            task.Start();
            
            //Task.Run(() => TaskProc(), token);
            
            Task.Delay(3500).Wait();
            source.Cancel();

            Console.WriteLine("Main: Task cancelled");
            Console.ReadLine();
        }


        static void TaskProc()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("TaskProc: Running !!");
                    Task.Delay(1000).Wait();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("TaskProc: Exception = ", ex.ToString());
            }
        }
    }
}