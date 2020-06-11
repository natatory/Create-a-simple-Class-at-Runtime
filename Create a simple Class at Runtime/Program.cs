using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Create_a_simple_Class_at_Runtime
{
    public class SomeClass
    {
        public int SomeInt = 5;
        public string SomeString = "str_value";
    }
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            Type myType1 = typeof(object);
            Type myType2 = typeof(object);

            Dictionary<string, Type> properties;
            SomeClass sc = new SomeClass();
            // Define first class
            properties = new Dictionary<string, Type> { { "SomeInt", typeof(int) }, { "SomeString", typeof(string) }, { "SomeObject", typeof(object) } };
            Kata.DefineClass("SomeClass1", properties, ref myType1);
            // Instantiate first class
            var myInstance1 = Activator.CreateInstance(myType1);

            properties = new Dictionary<string, Type> { { "SomeInt", typeof(int) }, { "SomeString", typeof(string) }, { "SomeObject", typeof(object) } };
            Kata.DefineClass("SomeClass2", properties, ref myType2);
            // Instantiate first class
            var myInstance2 = Activator.CreateInstance(myType2);
            if (myType1.Assembly == myType2.Assembly)
                Console.WriteLine("SomeAssembly");
            else Console.WriteLine("DifferentAssembly");

            PropertyInfo[] propertyInfo = myType1.GetProperties();
            foreach (PropertyInfo pi in propertyInfo)
            {
                Console.Write(pi.Name + " ");
            }
            Console.ReadLine();
        }
    }
}
