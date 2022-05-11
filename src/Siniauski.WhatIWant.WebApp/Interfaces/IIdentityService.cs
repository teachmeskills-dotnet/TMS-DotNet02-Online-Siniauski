using Siniauski.WhatIWant.WebData.Models;

namespace Siniauski.WhatIWant.WebApp.Interfaces
{
    public interface IIdentityService
    {
        Task<UserAuthModel> LoginAsync(object value);

        Task<UserAuthModel> RegisterAsync(object value);
    }
}