
namespace TestPluralize
{


    /// <summary>
    /// Specifies that this field is a primary key in the database
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class KeyAttribute
        : System.Attribute
    {


        public KeyAttribute(string name, params string[] fields)
        {
            this.Name = name;
            this.Fields = fields;
        }


        // https://stackoverflow.com/questions/4606973/how-to-get-name-of-property-which-our-attribute-is-set
        public KeyAttribute(string name, [System.Runtime.CompilerServices.CallerMemberName] string field = null)
            : this(name, new string[] { field })
        { }


        public string Name;
        public string[] Fields;

    }


    public class foobar
    {
        [KeyAttribute("noob")]
        public string Foo { get; set; }
    }


    class Helper
    {



        public static T GetCustomAttribute<T>(System.Reflection.MemberInfo mi)
        {
            object[] objs = mi.GetCustomAttributes(typeof(T), false);

            if (objs == null || objs.Length < 1)
                return default(T);

            T attr = (T)objs[0];
            return attr;
        }


        public static void Test()
        {
            KeyAttribute attr = GetCustomAttribute<KeyAttribute>(
                typeof(foobar).GetProperty("foo", System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase)
                );
            System.Console.WriteLine(attr.Name);
            System.Console.WriteLine(attr.Fields[0]);
        }


    }
}
