using GraphQL.Types;
using Library.API.Entities;
using Library.API.Services;
using System;

namespace Library.API.GraphQLSchema
{
    public class LibraryQuery : ObjectGraphType
    {
        public LibraryQuery(IRepositoryBase<Author, Guid> authorRepository)
        {
            Field<ListGraphType<AuthorType>>("authors", resolve: context =>
                authorRepository.GetAllAsync().Result);

            Field<AuthorType>("author", arguments: new QueryArguments(new QueryArgument<IdGraphType>()
            {
                Name = "id",
            }),
                resolve: context =>
                {
                    Guid id = Guid.Empty;
                    if (context.Arguments.ContainsKey("id"))
                    {
                        id = new Guid(context.Arguments["id"].ToString());
                    }

                    return authorRepository.GetByIdAsync(id).Result;
                });
        }
    }
}