using GraphQL.Types;
using Library.API.Entities;
using Library.API.Services;

namespace Library.API.GraphQLSchema
{
    public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType(IBookService bookService)
        {
            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.Name);
            Field(x => x.BirthDate);
            Field(x => x.BirthPlace);
            Field(x => x.Email);
            Field<ListGraphType<BookType>>("books", resolve: context =>
            {
                return bookService.GetBooksAsync(context.Source.Id).Result;
            });
        }
    }
}