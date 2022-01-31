using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using NyceLogicTest.Context;
using NyceLogicTest.Models;
using NyceLogicTest.Repository.Interfaces;

namespace NyceLogicTest.Repository
{
    public class UserService : IUserService
    {
        private readonly ShopContext _dbContext;
        public UserService(ShopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Register(LoginModel user)
        {
            var userToCheck = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == user.UserName);
            if (userToCheck != null)
            {
                throw new Exception("User with this name already exists");
            }
            User newUser = new User();
            newUser.Name = user.UserName;
            var hashsalt = EncryptPassword(user.Password);
            newUser.Password = hashsalt.Hash;
            newUser.StoredSalt = hashsalt.Salt;
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<User> Login(LoginModel user)
        {
            var userToCheck = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == user.UserName);
            if (userToCheck == null)
            {
                throw new Exception("No user with this name");
            }
            var isPasswordMatched = VerifyPassword(user.Password, userToCheck.StoredSalt, userToCheck.Password);
            if (isPasswordMatched)
            {
                return userToCheck;
            }
            else
            {
                throw new Exception("Wrong username or password");
            }
        }
        private HashSalt EncryptPassword(string password)
        {
            byte[] salt = new byte[128 / 8]; 
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return new HashSalt { Hash = encryptedPassw, Salt = salt };
        }

        private bool VerifyPassword(string enteredPassword, byte[] salt, string storedPassword)
        {
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return encryptedPassw == storedPassword;
        }
    }
}
