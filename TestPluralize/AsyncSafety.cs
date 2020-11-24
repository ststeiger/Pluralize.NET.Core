
namespace TestPluralize
{


    // https://stackoverflow.com/questions/9468800/determining-thread-safety-in-unit-tests
    // Proving that something is thread safe is tricky - probably halting-problem hard.
    // You can show that a race condition is easy to produce, or that it is hard to produce. 
    // But not producing a race condition doesn't mean it isn't there.
    public class AsyncSafety
    {

        private static int seed = System.Environment.TickCount;

        private static readonly System.Threading.ThreadLocal<System.Random> random =
            new System.Threading.ThreadLocal<System.Random>(
              delegate ()
              {
                  return new System.Random(System.Threading.Interlocked.Increment(ref seed));
              }
            );


        public static async System.Threading.Tasks.Task DoSomething()
        {
            int rnd = random.Value.Next(0, 200);
            await System.Threading.Tasks.Task.Delay(rnd);

            if (rnd % 2 == 0)
                await ThreadSafetyTest.AsyncDoSomething1();
            else
                await ThreadSafetyTest.AsyncDoSomething2();
        }


        public static async System.Threading.Tasks.Task<(bool IsSuccess, System.Exception Error)> TestAsync()
        {
            (bool IsSuccess, System.Exception Error) a = await RunTaskInParallel(DoSomething, 100);

            System.Console.WriteLine(a.IsSuccess);
            if (a.Error != null)
            {
                System.Console.WriteLine(a.Error.Message);
            }

            return a;
        }

        public static void Test()
        {
            (bool IsSuccess, System.Exception Error) a = TestAsync().Result;
            System.Console.WriteLine(a);
        }



        public static System.Threading.Tasks.ParallelOptions GetParallelLoopOptions(
            System.Threading.CancellationTokenSource tokenSource)
        {
            System.Threading.Tasks.ParallelOptions po = new System.Threading.Tasks.ParallelOptions();
            po.CancellationToken = tokenSource.Token;
            po.MaxDegreeOfParallelism = 100;

            return po;
        }


        /// <summary>
        ///  https://stackoverflow.com/questions/9468800/determining-thread-safety-in-unit-tests
        /// </summary>
        /// <param name="task"></param>
        /// <param name="numberOfParallelExecutions"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task<(bool IsSuccess, System.Exception Error)> 
            RunTaskInParallel(
            System.Func<System.Threading.Tasks.Task> task, int numberOfParallelExecutions = 2)
        {
            var cancellationTokenSource = new System.Threading.CancellationTokenSource();
            System.Exception error = null;
            int tasksCompletedCount = 0;
            var result = System.Threading.Tasks.Parallel.For(0, numberOfParallelExecutions, GetParallelLoopOptions(cancellationTokenSource),
                          async delegate (int index)
                          {
                              try
                              {
                                  await task();
                              }
                              catch (System.Exception ex)
                              {
                                  error = ex;
                                  cancellationTokenSource.Cancel();
                              }
                              finally
                              {
                                  tasksCompletedCount++;
                              }

                          });

            int spinWaitCount = 0;
            int maxSpinWaitCount = 100;
            while (numberOfParallelExecutions > tasksCompletedCount && error is null && spinWaitCount < maxSpinWaitCount)
            {
                await System.Threading.Tasks.Task.Delay(System.TimeSpan.FromMilliseconds(100));
                spinWaitCount++;
            }

            return (error == null, error);
        }


    }


}
