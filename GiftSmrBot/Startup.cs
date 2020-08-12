using GiftSmrBot.Core;
using GiftSmrBot.Core.DataInterfaces;
using GiftSmrBot.Core.Interfaces;
using GiftSmrBot.Services;
using GiftSmrBot.Services.Commands;
using GiftSmrBot.Services.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace GiftSmrBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
             .AddControllers()
             .AddNewtonsoftJson();

            services.AddCors();

            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.Configure<SecretLogin>(Configuration.GetSection("SecretLogin"));

            services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PostgresDevConnection")));

            var botConfig = new BotSettings();
            Configuration.GetSection("BotConfiguration").Bind(botConfig);

            var botClient = new TelegramBotClient(botConfig.Token);
            string hook = $@"{botConfig.Host}/api/update";
            botClient.SetWebhookAsync(hook);

            //services.Configure<BotSettings>(Configuration.GetSection("BotConfiguration"));
            //services.Configure<DbSettings>(options => options.Path = Configuration.GetConnectionString("JsonConnection"));           

            services.AddSingleton<ITelegramBotClient>(botClient);
            services.AddSingleton<IStateMachine, PollStateMachine>();

            services.AddScoped<IGiftService, GiftService>();
            services.AddScoped<IUserService, UserService>();
                
            services.AddScoped<IStateFactory, PollStateFactory>();

            services.AddScoped<ICommand, StartCommand>();
            services.AddScoped<ICommand, LoginCommand>();
            services.AddScoped<ICommand, CreateGiftCommand>();
            services.AddScoped<ICommand, ShowGiftsCommand>();

            services.AddScoped<ICommandService, CommandsService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
