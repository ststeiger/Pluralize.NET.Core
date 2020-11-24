
namespace TestPluralize
{


    // https://stackoverflow.com/questions/9468800/determining-thread-safety-in-unit-tests
    // Proving that something is thread safe is tricky - probably halting-problem hard.
    // You can show that a race condition is easy to produce, or that it is hard to produce. 
    // But not producing a race condition doesn't mean it isn't there.
    public class ThreadSafety
    {

        private static Pluralize.NET.Core.Pluralizer plu = new Pluralize.NET.Core.Pluralizer();


        public static bool IsBad()
        {
            return false;
        }


        public static void Test()
        {
            bool failed = false;
            int iterations = 100;

            // threads interact with some object - either 
            System.Threading.Thread thread1 = new System.Threading.Thread(
                new System.Threading.ThreadStart(
                    delegate () 
                    {
                        for (int i = 0; i < iterations; i++)
                        {
                            ThreadSafetyTest.DoSomething1(); // call unsafe code

                            // check that object is not out of synch due to other thread
                            if (IsBad())
                            {
                                failed = true;
                            }
                        }
                    })
            );

            System.Threading.Thread thread2 = new System.Threading.Thread(
                new System.Threading.ThreadStart(
                    delegate () 
                    {
                        for (int i = 0; i < iterations; i++)
                        {
                            ThreadSafetyTest.DoSomething2(); // call unsafe code
                            
                            // check that object is not out of synch due to other thread
                            if (IsBad())
                            {
                                failed = true;
                            }
                        }
                    })
            );

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
            System.Diagnostics.Debug.Assert(failed == false, "The code was thread safe");
        }


    }


}
