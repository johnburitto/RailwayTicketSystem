﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Configurations;
using Security.Data;
using Security.Dto;
using Security.Entities;
using Security.Services.Interfaces;

namespace Security.Services.Impls
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _manager;
        private readonly SecurityDbContext _context;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> manager, SecurityDbContext context, IMapper mapper)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _manager.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _manager.FindByIdAsync(id);
        }

        public async Task<ResponseType> RegisterAsync(UserRegistrationDto dto)
        {
            var expectedUser = await _manager.FindByEmailAsync(dto.Email);

            if (expectedUser != null)
            {
                return ResponseType.AlreadyRegistered;
            }

            var user = _mapper.Map<User>(dto);
            var result = await _manager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _manager.AddToRoleAsync(user, Authorization.DEFAULT_ROLE.ToString());

                return ResponseType.Registered;
            }
            else
            {
                return ResponseType.InternalError;
            }
        }

        public async Task<ResponseType> CreateAsync(UserCreateDto dto)
        {
            var expectedUser = await _manager.FindByEmailAsync(dto.Email);

            if (expectedUser != null)
            {
                return ResponseType.AlreadyRegistered;
            }

            var user = _mapper.Map<User>(dto);
            var result = await _manager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _manager.AddToRoleAsync(user, dto.Role.ToString());

                return ResponseType.Created;
            }
            else
            {
                return ResponseType.InternalError;
            }
        }

        public async Task<ResponseType> UpdateAsync(UserUpdateDto dto)
        {
            var expectedUser = await _manager.FindByIdAsync(dto.Id);

            if (expectedUser == null)
            {
                return ResponseType.NotRegistered;
            }
            else
            {
                expectedUser.FirstName = dto.FirstName;
                expectedUser.MiddleName = dto.MiddleName;
                expectedUser.LastName = dto.LastName;
                expectedUser.UserName = dto.UserName;
                expectedUser.Email = dto.Email;
                expectedUser.PhoneNumber = dto.PhoneNumber;

                _context.Update(expectedUser);
                await _context.SaveChangesAsync();
            }

            var oldRoles = await _manager.GetRolesAsync(expectedUser);
            var result = await _manager.RemoveFromRolesAsync(expectedUser, oldRoles);
            
            if (result.Succeeded)
            {
                await _manager.AddToRoleAsync(expectedUser, dto.Role.ToString());
            }
            else
            {
                Console.WriteLine("Role issue");

                return ResponseType.InternalError;
            }

            result = await _manager.RemovePasswordAsync(expectedUser);

            if (result.Succeeded)
            {
                await _manager.AddPasswordAsync(expectedUser, dto.Password);
            }
            else
            {
                Console.WriteLine("Password issue");

                return ResponseType.InternalError;
            }

            return ResponseType.Updated;

            //result = await _manager.UpdateAsync(expectedUser);

            //if (result.Succeeded)
            //{
            //    return ResponseType.Updated;
            //}
            //else
            //{
            //    Console.WriteLine("Update issue");

            //    return ResponseType.InternalError;
            //}
        }

        public async Task<ResponseType> LoginAsync(UserLoginDto dto)
        {
            var expectedUser = await _manager.FindByNameAsync(dto.UserName);

            if (expectedUser != null)
            {
                return await _manager.CheckPasswordAsync(expectedUser, dto.Password) ? ResponseType.Logined : ResponseType.BadCredentials;
            }
            else
            {
                return ResponseType.NotRegistered;
            }
        }

        public async Task<List<string>> GetUserRolesAsync(string id)
        {
            var user = await _manager.FindByIdAsync(id);

            return (await _manager.GetRolesAsync(user ?? throw new ArgumentNullException(nameof(user)))).ToList();
        }

        public async Task DeleteByIdAsync(string id)
        {
            var user = await _manager.FindByIdAsync(id);

            await _manager.DeleteAsync(user ?? throw new ArgumentNullException(nameof(user)));
        }
    }
}
