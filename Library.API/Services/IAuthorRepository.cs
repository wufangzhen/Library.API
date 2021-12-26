﻿using Library.API.Entities;
using Library.API.Helpers;
using System;
using System.Threading.Tasks;

namespace Library.API.Services
{
    public interface IAuthorRepository : IRepositoryBase<Author,Guid> 
    {
        Task<PagedList<Author>> GetAllAsync(AuthorResourceParameters parameters);
    }
}