using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Concrete;
using WebApiNinjectStudio.Domain.Entities;
using WebApiNinjectStudio.Services;
using WebApiNinjectStudio.V1.Dtos;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace WebApiNinjectStudio
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

            #region Injection Custome Service

            //Account handle
            services.AddTransient<AccountService>();
            //AES Security handle
            services.AddTransient<AESSecurity>();
            //Pbkdf2 Security handle, especially used in password
            services.AddTransient<Pbkdf2Security>();

            #endregion

            #region Redis Cache
            var redisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);
            #endregion

            #region Injection Repository

            services.AddScoped<IProductRepository, EFProductRepository>();
            services.AddScoped<IUserRepository, EFUserRepository>();
            services.AddScoped<IRoleRepository, EFRoleRepository>();
            services.AddScoped<ICategoryRepository, EFCategoryRepository>();
            services.AddScoped<IProductCategoryRepository, EFProductCategoryRepository>();
            services.AddScoped<IRouteBusRepository, EFRouteBusRepository>();
            services.AddScoped<INumberOfPassengerRepository, EFNumberOfPassengerRepository>();
            services.AddScoped<IRouteRepository, EFRouteRepository>();
            services.AddScoped<IRouteBusStopRepository, EFRouteBusStopRepository>();
            services.AddScoped<IBusStopRepository, EFBusStopRepository>();
            services.AddScoped<IBusRepository, EFBusRepository>();
            services.AddScoped<IBusDriverRepository, EFBusDriverRepository>();            
            services.AddScoped<IBusModelRepository, EFBusModelRepository>();

            //services.AddScoped<IRouteBusRepository, EFRouteBusRepository>();


            services.AddScoped<EFUserDetailRepository>();
            services.AddScoped<RedisUserDetailRepository>();
            services.AddTransient<UserDetailFactory>(serviceProvider => userDetailRepositoryType =>
            {
                switch (userDetailRepositoryType)
                {
                    case UserDetailRepositoryType.EF:
                        return serviceProvider.GetService<EFUserDetailRepository>();
                    case UserDetailRepositoryType.Redis:
                        return serviceProvider.GetService<RedisUserDetailRepository>();
                    default:
                        throw new KeyNotFoundException();
                }
            });

            services.AddScoped<EFRouteBusRepository>();
            services.AddScoped<RedisRouteBusRepository>();
            services.AddTransient<RouteBusFactory>(serviceProvider => routeBusRepositoryType =>
            {
                switch (routeBusRepositoryType)
                {
                    case RouteBusRepositoryType.EF:
                        return serviceProvider.GetService<EFRouteBusRepository>();
                    case RouteBusRepositoryType.Redis:
                        return serviceProvider.GetService<RedisRouteBusRepository>();
                    default:
                        throw new KeyNotFoundException();
                }
            });

            #endregion

            #region JWT Token and Authorization

            //JWT Token, Authorization permission efter user role i database
            //Create Authentication requirement after permission 
            var permissionRequirement = new AuthPolicyRequirement();
            //Get signing secret key to JWT token
            var secureKeyOfToken = Configuration["TokenSettings:SecretKey"];
            services.AddAuthorization(
                options => options.AddPolicy("Permission", policy => policy.Requirements.Add(permissionRequirement))
            )
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secureKeyOfToken)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //Injection AuthorizationHandler
            //Handlers that use Entity Framework shouldn't be registered as singletons.
            services.AddSingleton(permissionRequirement);
            services.AddSingleton<IAuthorizationHandler, AuthPolicyHandler>();

            #endregion

            #region Swagger
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();

                // integrate xml comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath, true);

                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {securityScheme, new string[] { }}
                    });
            });
            #endregion

            #region ApiVersioning            
            services.AddApiVersioning(options =>
                options.ReportApiVersions = true);
            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });
            #endregion

            //AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            //Model validation fails, Validation failure error response
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    //var result = new BadRequestObjectResult(context.ModelState);

                    var errors = new List<string>();
                    foreach (var state in context.ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            errors.Add(error.ErrorMessage);
                        }
                    }
                    var result = new BadRequestObjectResult(new { Message = errors });                    
                    result.ContentTypes.Add(MediaTypeNames.Application.Json);
                    result.ContentTypes.Add(MediaTypeNames.Application.Xml);
                    return result;
                };
            });

            //Database connetion string
            services.AddDbContext<EFDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DBContext"))
            );

            //Newtonsoft json package
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            //Lower case for Api Urls
            services.AddRouting(options =>
                options.LowercaseUrls = true
            );

            services.AddHttpContextAccessor();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                //Swagger - UI configure
                options.DefaultModelRendering(ModelRendering.Example);
                options.DefaultModelExpandDepth(-1);
                options.DefaultModelsExpandDepth(-1);
                options.DocExpansion(DocExpansion.List);
                options.EnableFilter();
                options.EnableValidator();

                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            #endregion

            app.UseRouting();

            //Global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("x-pagination"));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
