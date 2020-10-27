using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Concrete;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Redis
{
    class Program
    {
        private static IServiceProvider _ServiceProvider;

        private static async System.Threading.Tasks.Task Main(string[] args)
        {

            Startup();

            var odp = _ServiceProvider.GetService<IProductRepository>();

            var userDetailFactory = _ServiceProvider.GetService<UserDetailFactory>();
            var redisUserDetailRepositoryType = userDetailFactory(UserDetailRepositoryType.Redis);
            var efUserDetailRepositoryType = userDetailFactory(UserDetailRepositoryType.EF);

            var cacheHandle= _ServiceProvider.GetService<CacheInit>();            

            var userDetails = efUserDetailRepositoryType.UserDetails.ToList();
            Console.Clear();
            Console.WriteLine(string.Format("{0} records of UserDetail have been found", userDetails.Count.ToString()));
            Console.WriteLine("Press enter to creat cache. . .");
            Console.ReadLine();
            var skip = 0;
            var take = 20;
            while (skip < userDetails.Count())
            {
                var subOfUserDetails = userDetails.Skip(skip).Take(take).ToList();
                skip += take;
                cacheHandle.CreateUserDetailRedisCache(subOfUserDetails);
                Console.Write($"\r {skip} records has been created...");
            }
            DisposeServices();
            Console.WriteLine("END");
        }

        private static void Startup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("redissettings.json", optional: true, reloadOnChange: true)
                .Build();
            var redisConfiguration = configuration.GetSection("Redis").Get<RedisConfiguration>();

            _ServiceProvider = new ServiceCollection()
                .AddScoped<IProductRepository, EFProductRepository>()
                .AddScoped<EFUserDetailRepository>()
                .AddScoped<RedisUserDetailRepository>()
                .AddScoped<CacheInit>()
                .AddTransient<UserDetailFactory>(serviceProvider => userDetailRepositoryType =>
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
                })
                .AddDbContext<EFDbContext>(options => options.UseSqlServer(configuration["ConnectionStrings:DBContext"]))
                .AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration)
                .BuildServiceProvider();
        }
        private static void DisposeServices()
        {
            if (_ServiceProvider == null)
            {
                return;
            }
            if (_ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

    }
}
