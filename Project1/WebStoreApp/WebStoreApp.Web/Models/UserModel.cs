using System.ComponentModel.DataAnnotations;

namespace WebStoreApp.Web.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}