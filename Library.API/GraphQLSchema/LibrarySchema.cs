using GraphQL;
using GraphQL.Types;
//using System.Web.Http.Dependencies;

namespace Library.API.GraphQLSchema
{
    public class LibrarySchema : Schema
    {
        public LibrarySchema(LibraryQuery query, IDependencyResolver dependencyResolver)
        {
            Query = query;
            
            DependencyResolver = dependencyResolver;

        }
    }
}