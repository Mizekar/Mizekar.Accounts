using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mizekar.Accounts.Services
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;

        public SmsService(IConfiguration configuration, ILogger<Startup> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<bool> SendAsync(string phoneNumber, string body)
        {
            try
            {
                var smsServiceOption = _configuration.GetSection("SMS").Get<SmsServiceOption>();

                if (!smsServiceOption.EnableSms)
                {
                    return true;
                }

                //var client = new SmsOnlineService.SendSoapClient();

                //var result = await client.SendSmsAsync(username, password, new ArrayOfString() { phoneNumber }, sender, body, false, "", null, null);
                //switch (result.Body.SendSmsResult)
                //{
                //    case 1:
                //        return true;
                //    case 0:
                //        _logger.LogError("send sms failed. InvalidUserPass with code 0");
                //        break;
                //    case 2:
                //        _logger.LogError("send sms failed. NoCredit with code 2");
                //        break;
                //    case 3:
                //        _logger.LogError("send sms failed. DailyLimit with code 3");
                //        break;
                //    case 4:
                //        _logger.LogError("send sms failed. SendLimit with code 4");
                //        break;
                //    case 5:
                //        _logger.LogError("send sms failed. InvalidNumber with code 5");
                //        break;
                //    default:
                //        _logger.LogError("send sms failed. with code " + result.Body.SendSmsResult);
                //        break;
                //}
                return false;
            }
            catch (Exception e)
            {
                _logger.LogCritical("send sms exception." + e.ToString());
                return false;
            }
        }

    }
}
