using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using staUtilities;

namespace AppDePrueba
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Main prueba = new Main();
            prueba.ConexionApi();
            Console.WriteLine(prueba.About());
            Console.ReadKey();
        }
    }
}
