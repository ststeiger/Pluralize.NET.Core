using System;
using System.Collections.Generic;
using System.Text;

namespace TestPluralize
{
    public class ThreadSafetyTest
    {


        private static Pluralize.NET.Core.Pluralizer plu = new Pluralize.NET.Core.Pluralizer();


        private static int seed = System.Environment.TickCount;

        private static readonly System.Threading.ThreadLocal<System.Random> random =
            new System.Threading.ThreadLocal<System.Random>(
              delegate ()
              {
                  return new System.Random(System.Threading.Interlocked.Increment(ref seed));
              }
            );


        public static void DoSomething1()
        {
            int rnd = random.Value.Next(0, 100);
            System.Threading.Thread.Sleep(rnd);

            string original = "friend";
            string plural = plu.Pluralize(original);

            System.Console.WriteLine("Plural of friend is " + plural);

        }


        public static void DoSomething2()
        {
            int rnd = random.Value.Next(0, 100);
            System.Threading.Thread.Sleep(rnd);

            string original = "foe";
            string plural = plu.Pluralize(original);

            System.Console.WriteLine("Plural of foe is " + plural);
        }


        public static async System.Threading.Tasks.Task AsyncDoSomething1()
        {
            int rnd = random.Value.Next(0, 100);
            await System.Threading.Tasks.Task.Delay(rnd);

            string original = "friend";
            string plural = plu.Pluralize(original);

            System.Console.WriteLine("Plural of friend is " + plural);
        }


        public static async System.Threading.Tasks.Task AsyncDoSomething2()
        {
            int rnd = random.Value.Next(0, 100);
            await System.Threading.Tasks.Task.Delay(rnd);

            string original = "foe";
            string plural = plu.Pluralize(original);

            System.Console.WriteLine("Plural of foe is " + plural);
        }


    }
}
