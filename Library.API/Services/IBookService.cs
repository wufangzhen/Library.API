using Library.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Services
{
    public interface IBookService
    {
        
        Task<Book> GetBookAsync(Guid authorId, Guid bookId);


        Task<IEnumerable<Book>> GetBooksAsync(Guid authorId);


        Task<bool> IsExistAsync(Guid authorId, Guid bookId);

    }
}
