using Books_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Books_Shop.Helpers
{
    public class CodeGenerator
    {
        private static Random _random = new Random();
        private static AppDbContext _appDbContext = new AppDbContext();
        public static string generate(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string Code = new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
            List<User> users = _appDbContext.Users.ToList();
            if (users != null)
            {
                int i = 0;
                foreach (var user in users)
                {
                    if ( user.Code == Code)
                    {
                        Code = new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
                    }
                    else{
                        return Code;
                    }
                    i++;
                }
            }
            return Code;

        }
    }
}