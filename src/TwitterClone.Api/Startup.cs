using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using TwitterClone.Api.Config;
using TwitterClone.Data;
using TwitterClone.Data.Backends.DynamoDb;
using TwitterClone.Services;

namespace TwitterClone.Api
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
            services.Configure<RedisSettings>(Configuration.GetSection("Redis"));

            // Cloud API settings and clients
            services.Configure<AWSCredentials>(Configuration.GetSection("AWS"));
            services.Configure<CosmosDBSettings>(Configuration.GetSection("CosmosDB"));

            // Infra services (mem cache etc)
            services.AddSingleton<IDatabase>(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<RedisSettings>>().Value;
                var redisConnection = ConnectionMultiplexer.Connect(settings.ConnectionString);

                return redisConnection.GetDatabase();
            });

            services.AddTransient<IMemoryCacheService, MemoryCacheService>();

            // Data services
            services.AddTransient<AmazonDynamoDBClient>();
            services.AddSingleton<CosmosClient>(sp => {
                var settings = sp.GetRequiredService<IOptions<CosmosDBSettings>>().Value;
                return new CosmosClient(settings.Endpoint, settings.AuthKey);
            });

            // Data access services
            // Swap out Data.Backends.DynamoDb with your implementation. Should ideally do this in code
            services.AddTransient<ITweetDataAccess, Data.Backends.CosmosDb.TweetDataAccess>();
            services.AddTransient<IUserDataAccess, Data.Backends.CosmosDb.UserDataAccess>();
            services.AddTransient<IHashtagDataAccess, Data.Backends.CosmosDb.HashtagDataAccess>();

            // Top level services
            services.AddTransient<TweetService>();
            services.AddTransient<UserService>();
            services.AddTransient<HashtagService>();

            services.AddCors(options =>
            {
                options.AddPolicy("DevPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TwitterClone.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("DevPolicy");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TwitterClone.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
