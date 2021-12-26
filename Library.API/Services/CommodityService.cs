using Library.API.Entities;
using Library.API.Extensions;
using Library.API.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Library.API.Services
{
    public class CommodityService:ICommodityService
    {
        private Dictionary<string, PropertyMapping> mappingDict = null;
        private readonly IRepositoryBase<Commodity, Guid> BookRepository;
        public CommodityService(IRepositoryBase<Commodity, Guid> bookRepository)
        {
            BookRepository = bookRepository;
            mappingDict = new Dictionary<string, PropertyMapping>(StringComparer.OrdinalIgnoreCase);
            mappingDict.Add("Name", new PropertyMapping("Name"));
        }
        public Task<PagedList<Commodity>> GetAllAsync(ResourceParameters parameters)
        {
            IQueryable<Commodity> queryableCommodities = BookRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                queryableCommodities = queryableCommodities.Where(
                    m => m.Tag.ToLower().Contains(parameters.SearchQuery.ToLower())
                    || m.Name.ToLower().Contains(parameters.SearchQuery.ToLower()));
            }

            //return PagedList<Author>.Create(queryableAuthors, parameters.PageNumber, parameters.PageSize);

            //queryableAuthors = queryableAuthors.OrderBy(parameters.SortBy);
            //return PagedList<Author>.Create(queryableAuthors, parameters.PageNumber, parameters.PageSize);

            var orderedCommodities = queryableCommodities.Sort(parameters.SortBy, mappingDict);
            return PagedList<Commodity>.CreateAsync(orderedCommodities,
                parameters.PageNumber,
                parameters.PageSize);
        }
    }
}
