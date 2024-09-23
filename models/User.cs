
namespace DPowerAPI.models
{
    public class User
    {
        public int ID { get; set; }
        public string? userName { get; set; }
        public string? password { get; set; }
    }
    public class LoginModel : User
    {
        //public string? UserName { get; set; }
        //public string? Password { get; set; }
    }
}
