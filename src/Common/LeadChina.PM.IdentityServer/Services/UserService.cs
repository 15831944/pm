﻿using LeadChina.PM.IdentityServer.Interfaces.Repositories;
using LeadChina.PM.IdentityServer.Interfaces.Services;
using LeadChina.PM.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LeadChina.PM.IdentityServer.Services
{
    /// <summary>
    /// 用户服务类
    /// </summary>
    public class UserService : IUserService
    {
        public IUserRepository UserRepository { get; private set; }

        public UserService(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }
        public Task AddAsync(User entity)
        {
            //this one wont be needed for a bit
            throw new NotImplementedException();
        }

        public async Task AddAsync(User entity, string password)
        {

            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)}");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException($"{nameof(password)}");
            }

            await UserRepository.AddAsync(entity, password);
        }

        public async Task<User> AutoProvisionUserAsync(string provider, string userId, List<Claim> claims)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            await UserRepository.DeleteAsync(id);
        }

        public async Task<User> FindByExternalProviderAsync(string provider, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetAsync(int id)
        {
            User user = await UserRepository.GetAsync(id);
            return user;
        }

        public async Task<User> GetAsync(string username)
        {
            var user = await UserRepository.GetAsync(username);

            return user;
        }

        public async Task<User> GetAsync(string username, string password)
        {
            var user = await UserRepository.GetAsync(username, password);
            return user;
        }

        public async Task UpdateAsync(User entity)
        {
           await  UserRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 验证用户名和密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var user = await UserRepository.GetAsync(username, password);
            return user != null;
        }
    }
}
