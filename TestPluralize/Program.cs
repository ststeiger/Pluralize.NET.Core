
namespace TestPluralize
{
    class Program
    {
        static void Main(string[] args)
        {
            Pluralize.NET.Core.Pluralizer plu = new Pluralize.NET.Core.Pluralizer();

            string original = "wife";
            original = "man";
            original = "woman";
            original = "mountainman";
            original = "mountainwoman";
            original = "mountainwoman";
            original = "truss";
            original = "city";
            original = "boy";
            original = "potato";
            original = "analysis";
            original = "ellipsis";
            original = "criterion";
            original = "sheep";
            original = "phenomenon";
            original = "focus";
            original = "cactus";

            original = "gas"; // bug - fixed 
            original = "fez"; // bug - fixed 

            original = "puppy";
            original = "mountainfez"; // bug - doesn't recognize ending


            // original = original.Trim();
            string plural = plu.Pluralize(original);
            string singular = plu.Singularize(plural);

            System.Console.WriteLine(plural);
            System.Console.WriteLine(singular);

            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        }
    }
}
