// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Vaxi;

namespace Server
{
    class Program
    {

        const int Port = 50008;
        static void Main(string[] args)
        {
            Console.WriteLine("Holitas de mar");

            Grpc.Core.Server server = null;
            try
            {
                server = new Grpc.Core.Server() { 
                    Services = {PersonaService.BindService(new PersonaServiceImpl())},
                    Ports = {new Grpc.Core.ServerPort("localhost",Port, Grpc.Core.ServerCredentials.Insecure )}
                };

                server.Start();
                Console.WriteLine("El servidor se esta ejecutando en el puerto:" + Port);
                Console.ReadKey();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Errores en el servidor");

                //throw;
            }
            finally 
            {
                if(server != null)
                    server.ShutdownAsync().Wait();
                Console.WriteLine("Fin");
            }
        }
    }
}