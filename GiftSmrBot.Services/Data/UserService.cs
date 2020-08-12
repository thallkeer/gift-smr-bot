using GiftSmrBot.Core;
using GiftSmrBot.Core.DataInterfaces;
using GiftSmrBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GiftSmrBot.Services.Data
{
    public class UserService : EntityService<User, long>, IUserService
    {
        public UserService(ApplicationContext context) : base(context)
        {
        }
    }
}
