
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;

class Servidor
{
    public void abrirServer()
    {
        Socket socketServidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Nota: "IPAddress.Any, obliga al servidor a escuchar las peticiones de conexion provenientes de cualquier ip"
        IPEndPoint miDireccion = new IPEndPoint(IPAddress.Any, 1234);
        string textoRecibido = "";
        int enteroMensajeRecibido; 
        //declaramos un array de bytes para recibir con un tamaño preestablecido (idealmente es el lenght del mensaje)
        byte[] msgRecibido = new byte[255];

        try
        {
            //Prepara la direccion IP para cuando le llegue una solicitud de conexion
            socketServidor.Bind(miDireccion);
            //Establece el numero de conexiones permitidas
            socketServidor.Listen(1);
            Console.WriteLine("Esperando Conexion...");
            //Declara un nuevo socket local con el socket de la conexion entrante
            Socket Escuchar = socketServidor.Accept();
    
            if (checkearWhitelist(Escuchar))
            {
            Console.WriteLine("Conectado con exito");
            //los parametros del metodo Recibir son los mismos que el de enviar
            enteroMensajeRecibido = Escuchar.Receive(msgRecibido, 0, msgRecibido.Length, 0);

            //Reajustamos el tamaño del array para que coincida con el del mensaje y evitar vacios innecesarios
            Array.Resize(ref msgRecibido, enteroMensajeRecibido);

            //Transformamos el array de bytes a string
            textoRecibido = Encoding.Default.GetString(msgRecibido);
            Console.WriteLine("Mensaje recibido: {0}", textoRecibido);
            }
            
        }
        catch (Exception error)
        {
            Console.WriteLine("Error: {0}", error.ToString());
        }
        Console.WriteLine("Presione cualquier tecla para continuar");
        Console.ReadLine();
    }

    private Boolean checkearWhitelist(Socket Escuchar)
    {
        //path del Whitelist
        //Si falla el Firewall, reemplazar la ruta por la local
        string nombreDeArchivo = "C:\\Users\\Lakshmana\\source\\repos\\Tp Whitelist Servidor\\Tp Whitelist Servidor\\whitelist.txt";

        try
        {
            //Toma la IP del cliente y la evalua con cada ip en la whitelist
            StreamReader sr = new StreamReader(nombreDeArchivo);
            IPEndPoint clienteEndpoint = (IPEndPoint)Escuchar.RemoteEndPoint;
            string linea = "";
            string clienteIp = clienteEndpoint.Address.ToString();
            while (linea != null)
            {
                linea = sr.ReadLine();
                if (clienteIp == linea)
                {
                    Console.WriteLine("Se han validado las credenciales del cliente");
                    return true;
                }
            }
            Console.WriteLine("El cliente no tiene permitida la conexion");
            Escuchar.Close();
            return false;
        } catch
        {
            Console.WriteLine("hubo un error en el firewall");
        }
        return false;
    }
}
//Obtiene la direccion IP de la conexion del cliente y la imprime
// EndPEndPoint clientIp = Escuchar.RemoteEndPoint;
// Console.WriteLine("Conexion desde: {0}", clientIp);
