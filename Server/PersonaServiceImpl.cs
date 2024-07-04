using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaxi;
using static Vaxi.PersonaService;

namespace Server
{
    public class PersonaServiceImpl : PersonaServiceBase
    {
        public override Task<PersonaResponse> RegistroPersona(PersonaRequest request, ServerCallContext context)
        {
            string mensaje = "Se insertó correctamente el usuario: " + request.Persona.Nombre + " - " + request.Persona.Email;

            PersonaResponse response = new PersonaResponse()
            {
                Resultado = mensaje
            };

            return Task.FromResult(response);
            //return base.RegistroPersona(request, context);
        }

        public override async Task RegistroPersonaServidorMultiple(ServerMultiplePersonaRequest request, IServerStreamWriter<ServerMultiplePersonaResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("El servidor recibio el request del cliente" +  request.ToString());
            string mensaje = "Se insertó correctamente el usuario: " + request.Persona.Nombre + " - " + request.Persona.Email;

            foreach (int  i in Enumerable.Range(1,10))
            {
                ServerMultiplePersonaResponse response = new ServerMultiplePersonaResponse()
                {
                    Resultado = string.Format("El response {0} tiene contenido {1}", i, mensaje)
                };

                await responseStream.WriteAsync(response);
            }

            //return base.RegistroPersonaServidorMultiple(request, responseStream, context);  
        }

        public override async Task<ClientMultiplePersonaResponse> RegistroPersonaClienteMultiple(IAsyncStreamReader<ClientMultiplePersonaRequest> requestStream, ServerCallContext context)
        {
            Console.WriteLine("RegistroPersonaClienteMultiple");
            string resultado = "";


            while(await requestStream.MoveNext())
            {
                resultado += string.Format("Request Mensaje en el servidor {0} {1}", requestStream.Current.Persona.Email, Environment.NewLine);
            }

            var responseMessage = new ClientMultiplePersonaResponse() { Resultado = resultado };

            return responseMessage;

            //return base.RegistroPersonaClienteMultiple(requestStream, context);
        }

        public override async Task RegistroPersonaBidireccional(IAsyncStreamReader<BidireccionalPersonaRequest> requestStream, IServerStreamWriter<BidireccionalPersonaResponse> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var mensaje = string.Format("Comunicación bidireccional: {0} {1}", requestStream.Current.Persona.Email, Environment.NewLine);
                Console.WriteLine(mensaje);
                var response = new BidireccionalPersonaResponse()
                {
                    Resultado = mensaje
                };

                await responseStream.WriteAsync(response);
            }

            //return base.RegistroPersonaBidireccional(requestStream, responseStream, context);
        }
    }
}
