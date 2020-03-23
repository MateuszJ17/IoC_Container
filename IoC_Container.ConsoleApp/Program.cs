using IoC_Container.ConsoleApp.Mock_repositories.Implementations;
using IoC_Container.ConsoleApp.Mock_repositories.Interfaces;
using IoC_Container.Implementation;
using System;

namespace IoC_Container.ConsoleApp
{
    class Program
    {
        /// <summary>
        /// Sample usage of the container
        /// </summary>
        static void Main(string[] args)
        {
            Container container = new Container();

            container.Register<IUserRepository, UserRepository>();

            container.GetInstance(typeof(IUserRepository));

        }
    }
}
