using System;

namespace DiscomonProject
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            if(message is null) 
                throw new ArgumentException("message cannot be null.");
                
            Console.WriteLine($"[{DateTime.Now:dd/M HH:mmtt}] - {message}");
        }
    }
}