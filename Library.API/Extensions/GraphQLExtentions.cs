using GraphQL;
using GraphQL.Types;
using Library.API.GraphQLSchema;
using Microsoft.Extensions.DependencyInjection;

namespace Library.API.Extensions
{
    public static class GraphQLExtensions
    {
        public static void AddGraphQLSchemaAndTypes(this IServiceCollection services)
        {
            services.AddScoped<AuthorType>();
            services.AddScoped<BookType>();
            services.AddScoped<LibraryQuery>();
            services.AddScoped<ISchema, LibrarySchema>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            //services.AddSingleton<IDocumentWriter, DocumentWriter>();
            services.AddScoped<IDependencyResolver>(provider => new FuncDependencyResolver(type => provider.GetRequiredService(type)));
        }
    }
}