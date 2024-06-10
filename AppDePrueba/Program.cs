using System;
using staUtilities;

namespace AppDePrueba
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Main pruebaDLL = new staUtilities.Main();
            //Metodos de Prueba de Cotizacion Dolar
            //float respuesta = pruebaDLL.ObtieneCotizacionDolar();
            //pruebaDLL.GuardaDatos();
            //Console.WriteLine($"Cotizacion Dolar: ${respuesta}");
            //
            //Metodos de Prueba LiveConnect
            //string respuesta = pruebaDLL.ObtieneDataLiveConnect();
            //Console.WriteLine($"Profession: {respuesta}");
            Console.WriteLine(pruebaDLL.About());
            Console.ReadKey();
        }
    }
}
