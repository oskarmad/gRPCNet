// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Grpc.Core;
using System.IO;
using Vaxi;

namespace Client
{
    class Program
    {

        const string serverPoint = "127.0.0.1:50008";
        static async Task Main(string[] args)
        {
            Console.WriteLine("Holitas desde client ");

            Grpc.Core.Channel canal = new Grpc.Core.Channel(serverPoint,Grpc.Core.ChannelCredentials.Insecure);
            try
            {
                await canal.ConnectAsync().ContinueWith((task) =>
                {
                    if (task.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        Console.WriteLine("El cliente se conecto al servidor GRPC correctamente");
                    }
                });

                //var persona = new Persona()
                //{
                //    Nombre = "Oscar",
                //    Apellido = "Diaz",
                //    Email = "oscarmd@outlook.com"
                //};

                //************** UNARY

                //var request = new PersonaRequest() {
                //    Persona = persona
                //};

                //var response = client.RegistroPersona(request);
                //Console.WriteLine(response.Resultado);
                //var client = new Operaciones.OperacionesClient(canal);

                //************** SERVER REQUEST STREAMING API

                //var request = new ServerMultiplePersonaRequest()
                //{
                //    Persona = persona
                //};

                //var client = new PersonaService.PersonaServiceClient(canal);
                //var response = client.RegistroPersonaServidorMultiple(request);

                //while (await response.ResponseStream.MoveNext())
                //{
                //    Console.WriteLine(response.ResponseStream.Current.Resultado);
                //    await Task.Delay(250);
                //}

                //************** CLIENT REQUEST STREAMING API


                //var request = new ClientMultiplePersonaRequest()
                //{
                //    Persona = persona
                //};

                //var client = new PersonaService.PersonaServiceClient(canal);
                //var stream = client.RegistroPersonaClienteMultiple();

                //foreach (int i in Enumerable.Range(1,10))
                //{
                //    await stream.RequestStream.WriteAsync(request);
                //}

                //await stream.RequestStream.CompleteAsync();

                //var response = await stream.ResponseAsync;
                //Console.WriteLine(response.Resultado);

                //************** BIDIRECCIONAL

                var client = new PersonaService.PersonaServiceClient(canal);

                Persona[] personaCollection =
                {
                    new () {Email = "oscarmd@outlook.com"},
                    new () {Email = "oskarmad@gmail.com"},
                    new () {Email = "maryory812@hotmail.com"},
                    new () {Email = "anniediaz2403@gmail.com"},
                    new () {Email = "oscardiaz@hotmail.com"},
                };

                var stream = client.RegistroPersonaBidireccional();

                foreach (var persona in personaCollection)
                {
                    Console.Write("Enviado al servidor:" + persona.Email);
                    var request = new BidireccionalPersonaRequest()
                    {
                        Persona = persona
                    };

                    await stream.RequestStream.WriteAsync(request);
                }

                await stream.RequestStream.CompleteAsync();

                var responseCollection = Task.Run(async () =>
                {
                    while (await stream.ResponseStream.MoveNext())
                    {
                        Console.Write("El cliente esta recibiendo del servidor {0} {1}", stream.ResponseStream.Current.Resultado,Environment.NewLine );

                    }
                });

                await responseCollection;

                //**************



                canal.ShutdownAsync().Wait();
                Console.ReadKey();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Errores en el servidor");

                //throw;
            }
            finally
            {
            }
        }
    }
}