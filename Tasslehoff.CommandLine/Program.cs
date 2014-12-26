namespace Tasslehoff.CommandLine
{
    using System;
    using Library.Services;

    public class Program
    {
        public static void Main(string[] args)
        {
            BlankServiceContainer container = new BlankServiceContainer("root");

            BlankServiceContainer c1 = new BlankServiceContainer("c1");
            container.AddChild(c1);
            
            BlankServiceContainer c1_1 = new BlankServiceContainer("c1_1");
            c1.AddChild(c1_1);

            BlankServiceContainer c1_2 = new BlankServiceContainer("c1_2");
            c1.AddChild(c1_2);

            BlankServiceContainer c2 = new BlankServiceContainer("c2");
            container.AddChild(c2);

            IService service = container.Find("c2");

            Console.WriteLine(service.Name);

            Console.Read();
        }
    }
}
