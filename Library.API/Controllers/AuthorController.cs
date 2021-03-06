using AutoMapper;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    //[EnableCors]
    [Route("api/authors")]
    [ApiController]
    //[Authorize]
    public class AuthorController : ControllerBase
    {
        public AuthorController(IAuthorService authorService,
            IRepositoryBase<Author, Guid> authorRepository,
            IMapper mapper,
            ILogger<AuthorController> logger,
            IDistributedCache distributedCache,
            IHashFactory hashFactory)
        {
            AuthorService = authorService;
            Mapper = mapper;
            Logger = logger;
            DistributedCache = distributedCache;
            HashFactory = hashFactory;
            AuthorRepository = authorRepository;
        }

        public IDistributedCache DistributedCache { get; }
        public IHashFactory HashFactory { get; }
        public ILogger<AuthorController> Logger { get; }
        public IMapper Mapper { get; }

        public IRepositoryBase<Author, Guid> AuthorRepository { get; }
        public IAuthorService AuthorService { get; }

        /// <summary>
        /// 添加一个作者
        /// </summary>
        /// <param name="authorForCreationDto">作者</param>
        /// <remarks>
        /// 添加作者的请求：        
        /// 
        ///     POST api/authors
        ///     {
        ///         "name" : "Author1",
        ///         "birthplace" : "Beijing",
        ///         "dateOfBirth" : "1980/1/1",
        ///         "email" : "xxx@xxx.com"
        ///     }
        /// </remarks>
        /// <returns>添加结果</returns>
        /// <response code="201">返回新创建的资源</response>
        /// <response code="400">提交请求时的信息不正确</response>
        [HttpPost(Name = nameof(CreateAuthorAsync))]
        //[ProducesResponseType(201, Type = typeof(AuthorDto))]
        //[ProducesResponseType(400, Type = typeof(void))]
        //[ApiConventionMethod(typeof(DefaultApiConventions),
        //             nameof(DefaultApiConventions.Create))]        
        public async Task<ActionResult> CreateAuthorAsync(AuthorForCreationDto authorForCreationDto)
        {
            var author = Mapper.Map<Author>(authorForCreationDto);

            AuthorRepository.Create(author);
            var result = await AuthorRepository.SaveAsync();
            if (!result)
            {
                throw new Exception("创建资源author失败");
            }

            var authorCreated = Mapper.Map<AuthorDto>(author);
            return CreatedAtRoute(nameof(GetAuthorAsync),
                new { authorId = authorCreated.Id },
                CreateLinksForAuthor(authorCreated));
        }

        [EnableCors("AllowAllMethodsPolicy")]
        [HttpDelete("{authorId}", Name = nameof(DeleteAuthorAsync))]
        public async Task<ActionResult> DeleteAuthorAsync(Guid authorId)
        {
            
            var author = await AuthorRepository.GetByIdAsync(authorId);
            if (author == null)
            {
                return NotFound();
            }

            AuthorRepository.Delelte(author);
            var result = await AuthorRepository.SaveAsync();
            if (!result)
            {
                throw new Exception("删除资源author失败");
            }

            return NoContent();
        }

        
        [HttpGet("{authorId}", Name = nameof(GetAuthorAsync))]
        //[ResponseCache(Duration = 60,Location =ResponseCacheLocation.Client)]
        public async Task<ActionResult<AuthorDto>> GetAuthorAsync(Guid authorId)
        {
            var author = await AuthorRepository.GetByIdAsync(authorId);
            if (author == null)
            {
                return NotFound();
            }

            var entityHash = HashFactory.GetHash(author);
            Response.Headers[HeaderNames.ETag] = entityHash;
            if (Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var requestETag)
                && entityHash == requestETag)
            {
                return StatusCode(StatusCodes.Status304NotModified);
            }

            var authorDto = Mapper.Map<AuthorDto>(author);
            return CreateLinksForAuthor(authorDto);
        }

        [AllowAnonymous]
        [HttpGet(Name = nameof(GetAuthorsAsync))]
        // [ResponseCache(Duration = 60, VaryByQueryKeys = new string[] { "sortBy", "searchQuery" })]
        public async Task<ActionResult<ResourceCollection<AuthorDto>>> GetAuthorsAsync(
            [FromQuery]AuthorResourceParameters parameters)
        {
            PagedList<Author> pagedList = null;

            // 为了简单，仅当请求中不包含过滤和搜索查询字符串时，
            // 才进行缓存，实际情况不应有此限制
            if (string.IsNullOrWhiteSpace(parameters.BirthPlace)
                && string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                string cacheKey = $"authors_page_{parameters.PageNumber}_pageSize_{parameters.PageSize}_{parameters.SortBy}";
                string cachedContent = await DistributedCache.GetStringAsync(cacheKey);

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new PagedListConverter<Author>());
                settings.Formatting = Formatting.Indented;

                if (string.IsNullOrWhiteSpace(cachedContent))
                {
                    pagedList = await AuthorService.GetAllAsync(parameters);
                    DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(2)
                    };

                    var serializedContent = JsonConvert.SerializeObject(pagedList, settings);
                    await DistributedCache.SetStringAsync(cacheKey, serializedContent);
                }
                else
                {
                    pagedList = JsonConvert.DeserializeObject<PagedList<Author>>(cachedContent, settings);
                }
            }
            else
            {
                pagedList = await AuthorService.GetAllAsync(parameters);
            }

            var paginationMetadata = new
            {
                totalCount = pagedList.TotalCount,
                pageSize = pagedList.PageSize,
                currentPage = pagedList.CurrentPage,
                totalPages = pagedList.TotalPages,
                previousePageLink = pagedList.HasPrevious ? Url.Link(nameof(GetAuthorsAsync), new
                {
                    pageNumber = pagedList.CurrentPage - 1,
                    pageSize = pagedList.PageSize,
                    birthPlace = parameters.BirthPlace,
                    serachQuery = parameters.SearchQuery,
                    sortBy = parameters.SortBy,
                }) : null,
                nextPageLink = pagedList.HasNext ? Url.Link(nameof(GetAuthorsAsync), new
                {
                    pageNumber = pagedList.CurrentPage + 1,
                    pageSize = pagedList.PageSize,
                    birthPlace = parameters.BirthPlace,
                    serachQuery = parameters.SearchQuery,
                    sortBy = parameters.SortBy,
                }) : null
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            var authorDtoList = Mapper.Map<IEnumerable<AuthorDto>>(pagedList);

            authorDtoList = authorDtoList.Select(author => CreateLinksForAuthor(author));

            var resourceList = new ResourceCollection<AuthorDto>(authorDtoList.ToList());
            return Ok(CreateLinksForAuthors(resourceList, parameters, paginationMetadata));
        }

        private AuthorDto CreateLinksForAuthor(AuthorDto author)
        {
            author.Links.Clear();
            author.Links.Add(new Link(HttpMethods.Get,
                "self",
                Url.Link(nameof(GetAuthorAsync), new { authorId = author.Id })));
            author.Links.Add(new Link(HttpMethods.Delete,
                "delete author",
                Url.Link(nameof(DeleteAuthorAsync), new { authorId = author.Id })));
            author.Links.Add(new Link(HttpMethods.Get,
               "author's books",
               Url.Link(nameof(BookController.GetBooksAsync), new { authorId = author.Id })));

            return author;
        }

        private ResourceCollection<AuthorDto> CreateLinksForAuthors(ResourceCollection<AuthorDto> authors,
            AuthorResourceParameters parameters = null,
            dynamic paginationData = null)
        {
            authors.Links.Add(new Link(HttpMethods.Get,
                "self",
                Url.Link(nameof(GetAuthorsAsync), parameters)));

            authors.Links.Add(new Link(HttpMethods.Post,
                "create author",
                Url.Link(nameof(CreateAuthorAsync), null)));

            if (paginationData != null)
            {
                if (paginationData.previousePageLink != null)
                {
                    authors.Links.Add(new Link(HttpMethods.Get,
                        "previous page",
                        paginationData.previousePageLink));
                }

                if (paginationData.nextPageLink != null)
                {
                    authors.Links.Add(new Link(HttpMethods.Get,
                        "next page",
                        paginationData.nextPageLink));
                }
            }

            return authors;
        }
    }
}