using System.Threading.Tasks;
using GiftSmrBot.Core.DataInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace GiftSmrBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly ILogger<UpdateController> _logger;

        public UpdateController(ICommandService commandService, ILogger<UpdateController> logger)
        {
            _commandService = commandService;
            _logger = logger;
        }       

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            if (update == null)
                return Ok();

            if (update.Message != null)
            {
                await _commandService.TryExecuteCommandFromMessage(update.Message);
            }
            else if (update.CallbackQuery != null)
            {
                await _commandService.HandleCallbackQuery(update.CallbackQuery);
            }

            return Ok();
        }
    }
}
