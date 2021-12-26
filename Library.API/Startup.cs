using AutoMapper;
using Library.API.Conventions;
using Library.API.Entities;
using Library.API.Extensions;
using Library.API.Extensions.Middlewares;
using Library.API.Filters;
using Library.API.Helpers;
using Library.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCore.AutoRegisterDi;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            BaseConfig.TargerUrl = Configuration["TargerUrl"];
            services.AddControllers(config=>
            {
                config.Filters.Add<JsonExceptionFilter>();

                config.ReturnHttpNotAcceptable = true;
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());

                config.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        Duration = 60
                    });

                config.CacheProfiles.Add("Never",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
                config.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
            });
            //跨域
            services.AddCors(options =>
            {
                //options.AddPolicy("AllowAllMethodsPolicy", builder => builder.WithOrigins("https://localhost:8888").AllowAnyMethod());

                options.AddPolicy("AllowAnyOriginPolicy", builder => builder.SetIsOriginAllowed(_=>true).AllowAnyHeader().AllowAnyMethod().AllowCredentials());

                options.AddDefaultPolicy(builder => builder.WithOrigins(BaseConfig.TargerUrl).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            });
            //登录扩展
            services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
            }).AddGitHub(options =>
            {
                options.ClientId = Configuration["Authentication:GitHub:ClientId"];
                options.ClientSecret = Configuration["Authentication:GitHub:ClientSecret"];
            }).AddQQ(options =>
            {
                options.ClientId = Configuration["Authentication:QQ:ClientSecret"];
                options.ClientSecret = Configuration["Authentication:QQ:ClientSecret"];
            });

            //映射
            services.AddAutoMapper(Assembly.Load("Library.API"), Assembly.Load("Library.API"));
            //数据库
            services.AddDbContext<LibraryDbContext>(
                config => config.UseMySql(Configuration["ConnectionStrings:DefaultConnection"]
                ,new MySqlServerVersion(new Version(8, 0, 21))));
            //仓储
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
            //自动注入服务到依赖注入容器
            var assembliesToScan = new[]   {
                 Assembly.GetExecutingAssembly(),
                 //Assembly.GetAssembly(typeof(PagedResultDto<>)),//因为PagedResultDto<>在MockSchoolManagement.Application类库中，所以通过PagedResultDto<>获取程序集信息
            };
            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)//将获取到的程序集信息注册到我们的依赖注入容器中
             .Where(c => c.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            //检查作者是否存在
            services.AddScoped<CheckAuthorExistFilterAttribute>();

            //GraphQL
            services.AddGraphQLSchemaAndTypes();

            //hash工厂
            services.AddSingleton<IHashFactory, HashFactory>();

            //响应缓存
            services.AddResponseCaching();

            //缓存
            services.AddMemoryCache();

            //Redis
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration["Caching:Host"];
                //options.InstanceName = Configuration["Caching:Instance"];
            });

            //websock
            services.AddWebSocketManager();

            //SignalR
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserBasedUserIdProvider>();
            //APi版本管理
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;

                //options.ApiVersionReader = new QueryStringApiVersionReader("ver");
                //options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                //options.ApiVersionReader = new MediaTypeApiVersionReader();

                options.ApiVersionReader = ApiVersionReader.Combine(
                    new MediaTypeApiVersionReader(),
                    new QueryStringApiVersionReader("api-version"));

                options.Conventions.Controller<Controllers.V1.ProjectController>()
                    .HasApiVersion(new ApiVersion(1, 0));

                options.Conventions.Controller<Controllers.V2.ProjectController>()
                    .HasApiVersion(new ApiVersion(2, 0));
            });

            //HSTS
            services.AddHsts(option =>
            {
                option.MaxAge = TimeSpan.FromDays(5);
                option.Preload = true;
                option.IncludeSubDomains = true;
                option.ExcludedHosts.Clear();
            });



            //Identity
            services.AddIdentity<User, Role>(
                opts =>
                {
                    opts.Password.RequiredLength = 6;
                    opts.Password.RequireDigit = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<LibraryDbContext>()
                .AddDefaultTokenProviders();


            //JWT认证
            var tokenSection = Configuration.GetSection("Security:Token");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  options.Audience = "Audience";
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuer = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = tokenSection["Issuer"],
                      ValidAudience = tokenSection["Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSection["Key"])),
                      ClockSkew = TimeSpan.Zero
                  };
                  //options.Events = new JwtBearerEvents
                  //{
                  //    OnMessageReceived = context =>
                  //    {
                  //        var accessToken = context.Request.Query["access_token"];

                  //        // If the request is for our hub...
                  //        var path = context.HttpContext.Request.Path;
                  //        if (!string.IsNullOrEmpty(accessToken) &&
                  //            (path.StartsWithSegments("/hub")))
                  //        {
                  //            // Read the token out of the query string
                  //            context.Token = accessToken;
                  //        }
                  //        return Task.CompletedTask;
                  //    }
                  //};
              });

            //services.AddComplexAuthenticate();
            //swagger
            services.AddSwaggerGen(c =>
            {
                
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library API", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "Library API", Version = "v2" });

                var xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                c.IncludeXmlComments(xmlFile);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IMapper Mapper,
            ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    //c.RoutePrefix = string.Empty;
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library.API v1");
                }
                
                ) ; ;
            }
            else
            {
                app.UseHsts();
            }
            app.UseWebSockets();
            app.MapSockets("/ws", serviceProvider.GetService<WebSocketMessageHandler>());

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseCors("AllowAnyOriginPolicy");
            app.UseResponseCaching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapHub<SignalRHub>("/hub");
            });
        }
    }
}
