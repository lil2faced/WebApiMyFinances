namespace WebApiMyFinances.WebApi.DTO
{
    public class DTOUserAPIJwt
    {
        public int Id { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DTOUserApiRole Role { get; set; } = new();
    }
}
