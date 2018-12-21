using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestIService;

namespace TestService
{
    public class UserService : IUserService
    {
        public INewsService newsService { get; set; }

        public bool CheckLogin(string userName, string password)
        {
            newsService.AddNews(userName, password);
            return true;
        }

        public bool CheckUserNameExists(string userName)
        {
            return false;
        }
    }
}
