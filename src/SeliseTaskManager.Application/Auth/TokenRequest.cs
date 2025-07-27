namespace SeliseTaskManager.Application.Auth
{
    public class TokenRequest
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
