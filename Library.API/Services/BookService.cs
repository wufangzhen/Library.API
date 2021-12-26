using Library.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Services
{
    public class BookService:IBookService
    {
        private readonly IRepositoryBase<Book, Guid> BookRepository;
        public BookService(IRepositoryBase<Book, Guid> bookRepository)
        {
            BookRepository = bookRepository;
        }

        public async Task<Book> GetBookAsync(Guid authorId, Guid bookId)
        {
            return await BookRepository.GetAll()
                .SingleOrDefaultAsync(book => book.AuthorId == authorId && book.Id == bookId);
        }

        public Task<IEnumerable<Book>> GetBooksAsync(Guid authorId)
        {
            return Task.FromResult(BookRepository.GetAll().Where(book => book.AuthorId == authorId).AsEnumerable());
        }

        public async Task<bool> IsExistAsync(Guid authorId, Guid bookId)
        {
            return await BookRepository.GetAll().
                AnyAsync(book => book.AuthorId == authorId && book.Id == bookId);
        }
    }
}