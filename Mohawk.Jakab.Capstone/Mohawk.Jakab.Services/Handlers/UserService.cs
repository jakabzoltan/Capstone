using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mohawk.Jakab.Quizzard.Domain;
using Mohawk.Jakab.Quizzard.Services.Interfaces;

namespace Mohawk.Jakab.Quizzard.Services.Handlers
{
    public class UserService
    {
        private QuizzardContext _context;
        public UserService()
        {
            _context = QuizzardContext.Create();
        }

        public string GetFullName(string id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) return "";
            if (user.FirstName != null)
            {
                return $"{user.FirstName} {user.LastName}";
            }
            return user.UserName;
        }

        public string GetSecurityQuestion(string id)
        {
            return _context.Users.Where(x => x.Id == id).Select(x => x.SecurityQuestion).FirstOrDefault();
        }

        public bool UserIsVerified(string id)
        {
            return _context.Users.Where(x => x.Id == id).Select(x => x.EmailConfirmed).FirstOrDefault();
        }
    }
}
