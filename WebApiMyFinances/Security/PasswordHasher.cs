﻿using Microsoft.AspNetCore.Identity;
using WebApiMyFinances.Core.Interfaces;

namespace WebApiMyFinances.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        } 
    }
}
