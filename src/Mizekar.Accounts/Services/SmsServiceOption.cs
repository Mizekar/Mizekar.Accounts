using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mizekar.Accounts.Services
{
    public class SmsServiceOption
    {
        public bool EnableSms { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
    }
}
