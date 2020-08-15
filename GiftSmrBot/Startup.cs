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
using Microsoft.Extensions.Logging;
using System.Linq;
using Telegram.Bot;

namespace GiftSmrBot
{
    public class Startup
    {
        readonly IWebHostEnvironment _env;
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            _logger = loggerFactory.CreateLogger<Startup>();
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

            string connectionPath = _env.IsDevelopment() ? "PostgresDevConnection" : "PostgresProdConnection";
            
            services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString(connectionPath)));

            var botConfigSection = Configuration.GetSection("BotConfiguration");           

            BotSettings botConfig = botConfigSection.Get<BotSettings>();

            var botClient = new TelegramBotClient(botConfig.Token);
            string hook = $@"{botConfig.Host}/api/update";
            botClient.SetWebhookAsync(hook);        

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
