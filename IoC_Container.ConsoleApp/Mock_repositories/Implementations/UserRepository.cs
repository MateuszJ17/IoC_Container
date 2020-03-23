using IoC_Container.ConsoleApp.Mock_repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC_Container.ConsoleApp.Mock_repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        public string GetUsername()
        {
            return "Mock User 1";
        }
    }
}
