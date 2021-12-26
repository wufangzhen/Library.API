﻿using Library.API.Middlewares;
using Library.API.SocketsManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Library.API.Extensions
{
    public static class SocketsExtension
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<SocketsManager.SocketsManager>();
            var exportedTypes = Assembly.GetEntryAssembly()?.ExportedTypes;
            if (exportedTypes == null) return services;

            foreach (var type in exportedTypes)
                if (type.GetTypeInfo().BaseType == typeof(SocketsHandler))
                    services.AddSingleton(type);

            return services;
        }

        public static IApplicationBuilder MapSockets(this IApplicationBuilder app, PathString path,
            SocketsHandler socket)
        {
            return app.Map(path, x => x.UseMiddleware<SocketsMiddleware>(socket));
        }
    }
}
