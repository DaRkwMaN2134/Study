using ConfigurationLibrary;
using DataLibrary;
using DTOLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using static ConfigurationLibrary.Interfaces;

namespace BotStart
{
    public class BotDataOutput: IBotWrite
    {
        private readonly AppDbContext _dbContext;

        public BotDataOutput(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task GetOrCreateUserAsync(UserRegistrationDto dto)
        {
            var chatId = dto.ChatId;
 
            var user = await _dbContext.Users.FirstOrDefaultAsync(c => c.ChatId == chatId);
            if (user == null)
            {
                user = new User
                {
                    ChatId = dto.ChatId,
                    Username = dto.Username,
                    FirstName = dto.FirstName,
                    Points = 0,
                    PremiumUntil = null,
                    RegisteredAt = DateTime.UtcNow,
                    IsAdmin = false,
                    IsBlocked = false
                };
                _dbContext.Users.Add(user);
            }
            else
            {
                if (user.Username != dto.Username || user.FirstName != dto.FirstName)
                {
                    user.Username = dto.Username;
                    user.FirstName = dto.FirstName;
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
