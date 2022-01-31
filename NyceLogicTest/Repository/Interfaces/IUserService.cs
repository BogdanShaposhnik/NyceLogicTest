using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NyceLogicTest.Models;

namespace NyceLogicTest.Repository.Interfaces
{
    public interface IUserService
    {
        public abstract Task<bool> Register(LoginModel user);
        public abstract Task<User> Login(LoginModel user);
    }
}
