using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoAPi_Dapper.Implements;
using TodoAPi_Dapper.Models.Persistance;
using Grpc;
using Grpc.Core;

namespace TodoAPi_Dapper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const int PORT = 5000;
            TodoImplements todoImplements = new TodoImplements(new UnitOfWork());
            Server server = new Server
            {
                Services = {
                    Todos.Todos.BindService(todoImplements)
                },
                Ports = { new ServerPort("0.0.0.0", PORT, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("gRPC Todo server listening on port " + PORT);
            Console.WriteLine("Press any key to stop the server...");
            Console.Read();

            server.ShutdownAsync().Wait();
            // CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
