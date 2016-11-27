using System.Collections.Generic;

namespace Jarvis.Tests.Models
{
    public class User
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<string> Roles { get; set; }
    }
}