using System;

namespace ThrearTask
{
    public class DataEventArgs:EventArgs
    {
        public string Message { get; set; }
        public DataEventArgs(string message)
        {
            Message = message;
        }
    }
}