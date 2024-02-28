using System.Threading;
namespace Module_15_Thread_Lessons_Console
{
    class Program
    {
        static object someObject = new object();
        static void Main(string[] args)
        {
            ConsoleCenselToken();
        }
        public static void SomeFuncThread()
        {
            var th = Thread.CurrentThread;

            th.Name = "SomeThread";

            for (int i = 0; i < 100; i++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("+ ");

                Thread.Sleep(20);
            }
            Console.WriteLine($"{th.Name}  {th.GetHashCode()} {th.ManagedThreadId}");
        }
        public static void ConsoleView()
        {
            //ThreadStart ts = new ThreadStart(SomeFuncThread);
            Thread th = new Thread(SomeFuncThread);
            th.Start();

            th = Thread.CurrentThread;

            th.Name = "MainThread";

            for (int i = 0; i < 100; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("- ");

                Thread.Sleep(40);
            }
            Console.WriteLine($"{th.Name}  {th.GetHashCode()} {th.ManagedThreadId}");
        }


        public static void MyPrintBag(object obj)
        {
            Bag? bag = obj as Bag;

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{bag.MyInt} {bag.MyString} {bag.MyDouble}");
                Thread.Sleep(20);
            }
        }
        public static void Print(object obj)
        {
            while (true)
            {
                Console.WriteLine($"{obj}");
                Thread.Sleep(10);
            }
        }
        public static void LockMethod(object objText)
        {
            lock (someObject)
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"{objText}");
                    Thread.Sleep(20);
                }

            }
        }
        public static void ConsoleLockMethod()
        {
            Thread tr = new Thread(LockMethod);
            tr.Start("action 1");

            Thread tr1 = new Thread(LockMethod);
            tr1.Start("action 2");

            Thread tr2 = new Thread(LockMethod);
            tr2.Start("action 3");
        }

        public static void JoinMethod1_1(object objText)
        {
            Thread tr1 = new Thread(JoinMethod1_2);
            tr1.Start("action 2");
            tr1.Join();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{objText}");
                Thread.Sleep(20);
            }
        }
        public static void JoinMethod1_2(object objText)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{objText}");
                Thread.Sleep(20);
            }
        }
        public static void JoinMethod1_3(object objText)
        {

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{objText}");
                Thread.Sleep(20);
            }

        }
        public static void InterruptMethod()
        {
            for (int i = 0; i < 1000; i++)
            {
                try
                {
                    Console.WriteLine($"action -{i}");
                    Thread.Sleep(10);
                }
                catch (ThreadInterruptedException trI)
                {
                    Console.WriteLine(trI.Message);

                    if (Console.ReadLine() == "1")
                    {

                    }
                }
            }
        }
        public static void ConsoleInterruptMethod()
        {
            Thread tr = new Thread(InterruptMethod);

            tr.Start();
            Thread.Sleep(5000);

            tr.Interrupt();
        }
        public static void ConcoleViewBag()
        {
            ParameterizedThreadStart prBag = new ParameterizedThreadStart(MyPrintBag);
            Thread thread = new Thread(prBag);
            thread.Start(new Bag(10, "My bag", 20.06));

            ParameterizedThreadStart prPrint = new ParameterizedThreadStart(Print);
            Thread thread2 = new Thread(prPrint);
            thread2.Start(" +");

            thread2.IsBackground = true;
        }
        
        public static void ConsoleJoinMethod()
        {
            Thread tr = new Thread(JoinMethod1_1);
            tr.Start("action 1");
            tr.Join();

            Thread tr2 = new Thread(JoinMethod1_3);
            tr2.Start("action 3");
            tr2.Join();
        }


        static Random random = new Random();
        static void ThreadTask(object obj)
        {
            Console.WriteLine($"Thread #{Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(random.Next(2000, 3000));
        }
        static void GetThreadPullInformation()
        {
            int workerThreadsAvilable;
            int cmplitionPortThreadAvilable;
            ThreadPool.GetAvailableThreads(out workerThreadsAvilable, out cmplitionPortThreadAvilable);

            int workerThreadMax;
            int cmplitionPortThreadMax;
            ThreadPool.GetMaxThreads(out workerThreadMax, out cmplitionPortThreadMax);

            Console.WriteLine($"Free Threads in ThreadPool: {workerThreadsAvilable} for {workerThreadMax}");
            Console.WriteLine($"Free Thread of IN and OUT {cmplitionPortThreadAvilable} for {cmplitionPortThreadMax}\n");
        }
        static void ConsoleViewThreadPool()
        {
            GetThreadPullInformation();

            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadTask));
                GetThreadPullInformation();
                Thread.Sleep(500);
            }
        }



        static int count;
        static Random r = new Random();
        static void GhangeCount()
        {
            Interlocked.Increment(ref count); // count ++;

            Thread.Sleep(10);

            Interlocked.Decrement(ref count);//count --;
        }
        static void ReadCount()
        {
            while(true)
            {
                Console.WriteLine($"count {count}");
                Thread.Sleep(10);
            }
        }
        static void ConsoleAtomOperation()
        {
            Thread tReadCount = new Thread(ReadCount) { IsBackground = true};
            tReadCount.Start();
            count = 0;

            Thread[] threads = new Thread[1000];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(GhangeCount);
                threads[i].Start();
            }
        }

        static void TaskFunction()
        {
            for (int i = 0; i < 50; i++)
            {      
                Console.WriteLine($"counter {i} {Thread.GetCurrentProcessorId()}");
                Thread.Sleep(100);
            }
        }
        static void ConsoleTask()
        {
            var t = new Action(TaskFunction);
            var t1 = new Task(TaskFunction);
            //var t = Task.Factory.StartNew(TaskFunction);
            //var t1 = Task.Factory.StartNew(TaskFunction);

            var task = new Task(t);

            t1.Start();
            task.Start();

            Thread.Sleep(5000);

            t1.Wait();
            task.Wait();
            Task.WaitAll(t1, task);

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"Main counter {i} {Thread.GetCurrentProcessorId()}");
                Thread.Sleep(50);
            }
            
            Thread.Sleep(5000);
        }

        static  int TaskResult()
        {
            Thread.Sleep(1000);
            return 2024;
        }
        static double Sum (object arg)
        {
            int obj = (int)arg;
            double sum = 0;

            while (obj-- > 0)
            {
                sum += obj;
            }
            Thread.Sleep(3000);

            return sum;
        }
        static void ConsoleArgumentTask()
        {
            Task<int> task1 = Task<int>.Factory.StartNew(TaskResult);
            Thread.Sleep(1000);
            Console.WriteLine($"result work {task1.Result}");

            Task<double> task2 = Task<double>.Factory.StartNew(Sum, 10);
            Console.WriteLine($"result_2 work {task2.Result}");

            task1.Dispose();
            task2.Dispose();

        }

        static void TaskFunctionCansel(object cansel)
        {
            var token = (CancellationToken)cansel;

            token.ThrowIfCancellationRequested();

            for (int i = 0; i < 5000; i++)
            {   
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested ();
                }
                Console.WriteLine($"counter {i} {Thread.GetCurrentProcessorId()}");
                Thread.Sleep(100);
            }
        }
        static void ConsoleCenselToken()
        {
            var canselToken = new CancellationTokenSource();

            Task task = Task.Factory.StartNew(TaskFunctionCansel, canselToken.Token, canselToken.Token);

            Thread.Sleep(3000);

            try
            {
                canselToken.Cancel();
                task.Wait();
            }
            catch(AggregateException ae)
            {
                if (task.IsCanceled)
                {
                    Console.WriteLine("Stoped Task");
                }
                Console.WriteLine(ae.InnerException.Message);
            }
            finally
            {
                task.Dispose();
                canselToken.Dispose();
            }
        }
        class Bag
        {
            public int MyInt { get; set; }
            public string MyString { get; set; }
            public double MyDouble { get; set; }
            public Bag(int i, string s, double d)
            {
                this.MyInt = i;
                this.MyString = s;
                this.MyDouble = d;
            }

        }
    }
}
