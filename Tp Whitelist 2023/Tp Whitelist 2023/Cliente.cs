using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

class Cliente
{
    //Funcion para mandar solicitud de conexion al servidor usando sockets
    public void Conectar()
    {
        //Creamos un objeto socket con las especificaciones por defecto
        Socket socketCliente = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Declaramos una variable IPEndPoint donde guarda la ip del cliente y su puerto de conexion
        //NOTA: la direcion ip reservada "127.0.0.1" es la localhost (la propia maquina)
        IPEndPoint miDireccion = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
        string texto;
        //Declaramos un array de bytes que contenga nuestro mensaje codificado en bytes
        byte[] msg;
        try
        {
            //El socket intenta establecer una conexion con la ip brindada
            socketCliente.Connect(miDireccion);

            //si nada falla esto se ejecutaria
            Console.WriteLine("Conectado con exito");

            //solicitamos el mensaje
            Console.WriteLine("Ingrese el texto a enviar al servidor:");
            texto = Console.ReadLine();
            
            //Transformamos el texto a un array de bytes
            msg = Encoding.Default.GetBytes(texto); 

            //El socket envia un mensaje con la siguiente estrucctura (a, b, c, d)
            /*
             * a: Es el array de bytes que vamos a enviar
             * b: es desde que posicion del array va a comenzar (usamos 0 porque inicia desde ahi a contar los arrays)
             * c: tomamos el tamaño del array como referencia para el tamaño del mensaje
             * d: establecemos el modo de operacion que por defecto es 0
             */
            socketCliente.Send(msg, 0, msg.Length, 0);
            Console.WriteLine("Enviado exitosamente");
            socketCliente.Close();
        }
        catch (Exception error)
        {
            Console.WriteLine("Error: {0}", error.ToString());
        }
        Console.WriteLine("Presione cualquier tecla para terminar");
        Console.ReadLine();
    }
}