﻿namespace WebApiMyFinances.WebApi.DTO
{
    public class DTOUserGet
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
