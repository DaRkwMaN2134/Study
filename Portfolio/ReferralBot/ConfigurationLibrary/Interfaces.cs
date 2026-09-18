using System;
using System.Collections.Generic;
using System.Text;
using DataLibrary;
using DTOLibrary;

namespace ConfigurationLibrary
{
    public class Interfaces
    {
        public interface IBotWrite
        {
            public Task GetOrCreateUserAsync(UserRegistrationDto dto);
        }
    }
}
