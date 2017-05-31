using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThrearTask
{
    //public class X : INotifyPropertyChanged
    //{
    //    private int kills = 0;

    //    public int Kills
    //    {
    //        get
    //        {
    //            return kills;
    //        }
    //        set
    //        {
    //            this.kills = value;
    //            this.OnPropertyChanged();
    //        }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
    //    {
    //        PropertyChangedEventHandler pc = this.PropertyChanged;

    //        if (pc != null)
    //        {
    //            pc(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }
    //}



    class OnDataChanged
    {
        public EventHandler<DataEventArgs> Chandged;

        private ThreadTask.StringProvider provider;
        private string oldProviderData;
        public OnDataChanged(ThreadTask.StringProvider provider)
        {
            this.provider = provider;
            oldProviderData = provider.Data;
            Thread dataThread = new Thread(DataHandler);
            dataThread.Start();
        }

        private void DataHandler()
        {
            while(true)
            {
                if (oldProviderData != provider.Data)
                {
                    Chandged?.Invoke(this, new DataEventArgs(provider.Data));
                    oldProviderData = provider.Data;                  
                }
            }
        }


    }
    class Program
    {
        public static OnDataChanged handler;
        static void Main(string[] args)
        {
            ThreadTask.StringProvider provider = new ThreadTask.StringProvider();
            handler = new OnDataChanged(provider);
            handler.Chandged += DataWasChanged;
            ThreadPool.QueueUserWorkItem(HelloWorldThread);
        }

        private static void HelloWorldThread(object state)
        {
            while (true)
            {
                Console.WriteLine("             Hello world");
                Thread.Sleep(1000);
            }
        }


        private static void DataWasChanged(object sender, DataEventArgs e)
        {
            Console.WriteLine(e.Message);
            ThreadPool.QueueUserWorkItem(o => AddFile100(e.Message));
            //ThreadPool.QueueUserWorkItem(AddFile100);
        }

        private static void AddFile100(string name)
        {
            string[] arr = new string[10];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = "1";
            File.WriteAllLines(name, arr);
        }
    }
}
