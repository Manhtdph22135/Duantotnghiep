namespace API.DOT
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int RoleId { get; set; } // Chọn Role khi đăng ký
    }
}
