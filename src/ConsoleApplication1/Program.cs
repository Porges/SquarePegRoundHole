using System;
using System.IO;
using SquarePegRoundHole.Dynamic;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Coerce.To<Func<StreamWriter, double, StreamWriter>>("(sw, x) => {sw.WriteLine(x); return sw;}")
                (Coerce.To<StreamWriter>("out.txt"), Coerce.To<double>("314e-2"))
                .Dispose(); // everything about this is best practice
        }
    }
}
