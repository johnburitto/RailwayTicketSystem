using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using RailwayTicketSystemTests.Profiles;
using Security.Data;
using Security.Dto;
using Security.Entities;
using Security.Services.Impls;
using Security.Services.Interfaces;
using static Security.Configurations.Authorization;

namespace RailwayTicketSystemTests.Security
{
    public class UserServiceTests
    {
        [Fact]
        public async void GetAllAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var users = new List<User>()
            {
                new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "John",
                    MiddleName = "",
                    LastName = "Buritto",
                    Email = "johnburitto@gmail.com",
                    UserName = "johnburitto",
                    PhoneNumber = "1234567890"
                }
            }.AsQueryable().BuildMock();
            userManager.Setup(x => x.Users).Returns(users);

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(users.Count(), result.Count);
            Assert.Equal(users, result);
        }

        [Fact]
        public async void GetAllAsyncTest_EmptyDatabase()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var users = new List<User>()
            {
                new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "John",
                    MiddleName = "",
                    LastName = "Buritto",
                    Email = "johnburitto@gmail.com",
                    UserName = "johnburitto",
                    PhoneNumber = "1234567890"
                }
            }.AsQueryable().BuildMock();
            userManager.Setup(x => x.Users).Returns(new List<User>().AsQueryable().BuildMock());

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.NotEqual(users, result);
        }

        [Fact]
        public async void GetByIdAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            string id = Guid.NewGuid().ToString();
            var user = new User
            {
                Id = id,
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                Email = "johnburitto@gmail.com",
                UserName = "johnburitto",
                PhoneNumber = "1234567890"
            };
            userManager.Setup(x => x.FindByIdAsync(id)).Returns(Task.FromResult<User?>(user));

            // When
            var result = await underTest.GetByIdAsync(id);

            // Then
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("", result.MiddleName);
            Assert.Equal("Buritto", result.LastName);
            Assert.Equal("johnburitto@gmail.com", result.Email);
            Assert.Equal("johnburitto", result.UserName);
            Assert.Equal("1234567890", result.PhoneNumber);
            Assert.Equal(result, user);
        }

        [Fact]
        public async void GetByIdAsyncTest_TryGetUnregisteredId()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            string id = Guid.NewGuid().ToString();
            var users = new List<User>()
            {
                new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "John",
                    MiddleName = "",
                    LastName = "Buritto",
                    Email = "johnburitto@gmail.com",
                    UserName = "johnburitto",
                    PhoneNumber = "1234567890"
                }
            }.AsQueryable().BuildMock();
            userManager.Setup(x => x.FindByIdAsync(id)).Returns(Task.FromResult<User?>(null));

            // When
            var result = await underTest.GetByIdAsync(id);

            // Then
            Assert.Null(result);
            Assert.DoesNotContain(result, users);
        }

        [Fact]
        public async void RegisterAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var dto = new UserRegistrationDto
            {
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890"
            };
            userManager.Setup(x => x.FindByEmailAsync(dto.Email)).Returns(Task.FromResult<User?>(null));
            userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));

            // When
            var result = await underTest.RegisterAsync(dto);

            // Then
            Assert.Equal(ResponseType.Registered, result);
        }

        [Fact]
        public async void RegisterAsyncTest_UserAlreadyRegistered()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var dto = new UserRegistrationDto
            {
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890"
            };
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                Email = "johnburitto@gmail.com",
                UserName = "johnburitto",
                PhoneNumber = "1234567890"
            };
            userManager.Setup(x => x.FindByEmailAsync(dto.Email)).Returns(Task.FromResult<User?>(user));

            // When
            var result = await underTest.RegisterAsync(dto);

            // Then
            Assert.Equal(ResponseType.AlreadyRegistered, result);
        }

        [Fact]
        public async void RegisterAsyncTest_InternalError()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var dto = new UserRegistrationDto
            {
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890"
            };
            userManager.Setup(x => x.FindByEmailAsync(dto.Email)).Returns(Task.FromResult<User?>(null));
            userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Failed()));

            // When
            var result = await underTest.RegisterAsync(dto);

            // Then
            Assert.Equal(ResponseType.InternalError, result);
        }

        [Fact]
        public async void CreateAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var dto = new UserCreateDto
            {
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890",
                Role = Role.User
            };
            userManager.Setup(x => x.FindByEmailAsync(dto.Email)).Returns(Task.FromResult<User?>(null));
            userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));

            // When
            var result = await underTest.CreateAsync(dto);

            // Then
            Assert.Equal(ResponseType.Created, result);
        }

        [Fact]
        public async void CreateAsyncTest_UserAlreadyRegistered()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var dto = new UserCreateDto
            {
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890",
                Role = Role.User
            };
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                Email = "johnburitto@gmail.com",
                UserName = "johnburitto",
                PhoneNumber = "1234567890"
            };
            userManager.Setup(x => x.FindByEmailAsync(dto.Email)).Returns(Task.FromResult<User?>(user));

            // When
            var result = await underTest.CreateAsync(dto);

            // Then
            Assert.Equal(ResponseType.AlreadyRegistered, result);
        }

        [Fact]
        public async void CreateAsyncTest_BadRole()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var dto = new UserCreateDto
            {
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890",
                Role = (Role)100
            };
            userManager.Setup(x => x.FindByEmailAsync(dto.Email)).Returns(Task.FromResult<User?>(null));
            userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));

            // When
            var result = await underTest.CreateAsync(dto);

            // Then
            Assert.Equal(ResponseType.BadRole, result);
        }

        [Fact]
        public async void CreateAsyncTest_InternalError()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var dto = new UserCreateDto
            {
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890",
                Role = (Role)100
            };
            userManager.Setup(x => x.FindByEmailAsync(dto.Email)).Returns(Task.FromResult<User?>(null));
            userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Failed()));
            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));

            // When
            var result = await underTest.CreateAsync(dto);

            // Then
            Assert.Equal(ResponseType.InternalError, result);
        }

        [Fact]
        public async void UpdateAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var id = Guid.NewGuid().ToString();
            var dto = new UserUpdateDto
            {
                Id = id,
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890",
                Role = Role.Admin
            };
            var user = new User
            {
                Id = id,
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890"
            };
            userManager.Setup(x => x.FindByIdAsync(dto.Id)).Returns(Task.FromResult<User?>(user));
            userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()));
            userManager.Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>())).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            userManager.Setup(x => x.RemovePasswordAsync(It.IsAny<User>())).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(x => x.AddPasswordAsync(It.IsAny<User>(), It.IsAny<string>()));
            context.Setup(x => x.Update(It.IsAny<User>()));
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            var result = await underTest.UpdateAsync(dto);

            // Then
            Assert.Equal(ResponseType.Updated, result);
        }

        [Fact]
        public async void UpdateAsyncTest_UserDoesntRegistered()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var id = Guid.NewGuid().ToString();
            var dto = new UserUpdateDto
            {
                Id = id,
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890",
                Role = Role.Admin
            };
            userManager.Setup(x => x.FindByIdAsync(dto.Id)).Returns(Task.FromResult<User?>(null));

            // When
            var result = await underTest.UpdateAsync(dto);

            // Then
            Assert.Equal(ResponseType.NotRegistered, result);
        }

        [Fact]
        public async void UpdateAsyncTest_BadRole()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var id = Guid.NewGuid().ToString();
            var dto = new UserUpdateDto
            {
                Id = id,
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890",
                Role = (Role) 100
            };
            var user = new User
            {
                Id = id,
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890"
            };
            userManager.Setup(x => x.FindByIdAsync(dto.Id)).Returns(Task.FromResult<User?>(user));
            userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()));
            userManager.Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>())).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            userManager.Setup(x => x.RemovePasswordAsync(It.IsAny<User>())).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(x => x.AddPasswordAsync(It.IsAny<User>(), It.IsAny<string>()));
            context.Setup(x => x.Update(It.IsAny<User>()));
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            var result = await underTest.UpdateAsync(dto);

            // Then
            Assert.Equal(ResponseType.BadRole, result);
        }

        [Fact]
        public async void UpdateAsyncTest_InternalErrorRoleCause()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var id = Guid.NewGuid().ToString();
            var dto = new UserUpdateDto
            {
                Id = id,
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890",
                Role = Role.Admin
            };
            var user = new User
            {
                Id = id,
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890"
            };
            userManager.Setup(x => x.FindByIdAsync(dto.Id)).Returns(Task.FromResult<User?>(user));
            userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()));
            userManager.Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>())).Returns(Task.FromResult(IdentityResult.Failed()));
            context.Setup(x => x.Update(It.IsAny<User>()));
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            var result = await underTest.UpdateAsync(dto);

            // Then
            Assert.Equal(ResponseType.InternalErrorRoleCause, result);
        }

        [Fact]
        public async void UpdateAsyncTest_InternalErrorPasswordCause()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var id = Guid.NewGuid().ToString();
            var dto = new UserUpdateDto
            {
                Id = id,
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890",
                Role = Role.Admin
            };
            var user = new User
            {
                Id = id,
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890"
            };
            userManager.Setup(x => x.FindByIdAsync(dto.Id)).Returns(Task.FromResult<User?>(user));
            userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()));
            userManager.Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>())).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            userManager.Setup(x => x.RemovePasswordAsync(It.IsAny<User>())).Returns(Task.FromResult(IdentityResult.Failed()));
            context.Setup(x => x.Update(It.IsAny<User>()));
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            var result = await underTest.UpdateAsync(dto);

            // Then
            Assert.Equal(ResponseType.InternalErrorPasswordCause, result);
        }

        [Fact]
        public async void LoginAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var dto = new UserLoginDto
            {
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd"
            };
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890"
            };
            userManager.Setup(x => x.FindByNameAsync(dto.UserName)).Returns(Task.FromResult<User?>(user));
            userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(true));

            // When
            var result = await underTest.LoginAsync(dto);

            // Then
            Assert.Equal(ResponseType.Logined, result);
        }

        [Fact]
        public async void LoginAsyncTest_UserDoesntRegistered()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var dto = new UserLoginDto
            {
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd"
            };
            userManager.Setup(x => x.FindByNameAsync(dto.UserName)).Returns(Task.FromResult<User?>(null));
            userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(true));

            // When
            var result = await underTest.LoginAsync(dto);

            // Then
            Assert.Equal(ResponseType.NotRegistered, result);
        }
        
        [Fact]
        public async void LoginAsyncTest_BadCredentials()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var dto = new UserLoginDto
            {
                UserName = "johnburitto",
                Password = "Str0ngPa$$w0rd"
            };
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890"
            };
            userManager.Setup(x => x.FindByNameAsync(dto.UserName)).Returns(Task.FromResult<User?>(user));
            userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            // When
            var result = await underTest.LoginAsync(dto);

            // Then
            Assert.Equal(ResponseType.BadCredentials, result);
        }

        [Fact]
        public async void GetUserRolesAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var id = Guid.NewGuid().ToString();
            var user = new User
            {
                Id = id,
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890"
            };
            var roles = new List<string>
            {
                "Admim"
            };
            userManager.Setup(x => x.FindByIdAsync(id)).Returns(Task.FromResult<User?>(user));
            userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).Returns(Task.FromResult<IList<string>>(roles));

            // When
            await underTest.GetUserRolesAsync(id);

            // Then
            userManager.Verify(x => x.GetRolesAsync(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async void GetUserRolesAsyncTest_TryToRolesOfUnredisteredUser()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var id = Guid.NewGuid().ToString();
            var roles = new List<string>
            {
                "Admim"
            };
            userManager.Setup(x => x.FindByIdAsync(id)).Returns(Task.FromResult<User?>(null));
            userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).Returns(Task.FromResult<IList<string>>(roles));

            // When

            // Then
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await underTest.GetUserRolesAsync(id));
            userManager.Verify(x => x.GetRolesAsync(It.IsAny<User>()), Times.Never());
        }

        [Fact]
        public async void DeleteByIdTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var id = Guid.NewGuid().ToString();
            var user = new User
            {
                Id = id,
                FirstName = "John",
                MiddleName = "",
                LastName = "Buritto",
                UserName = "johnburitto",
                Email = "johnburitto@gmail.com",
                PhoneNumber = "1234567890"
            };
            userManager.Setup(x => x.FindByIdAsync(id)).Returns(Task.FromResult<User?>(user));
            userManager.Setup(x => x.DeleteAsync(It.IsAny<User>()));

            // When
            await underTest.DeleteByIdAsync(id);

            // Then
            userManager.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async void DeleteByIdTest_TryToDeleteUnredisteredUser()
        {
            // Given
            #region Setup
            var context = new Mock<SecurityDbContext>();
            var userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(configuration => configuration.AddProfile(new UserProfile())));
            var underTest = new UserService(userManager.Object, context.Object, mapper);
            #endregion
            var id = Guid.NewGuid().ToString();
            userManager.Setup(x => x.FindByIdAsync(id)).Returns(Task.FromResult<User?>(null));
            userManager.Setup(x => x.DeleteAsync(It.IsAny<User>()));

            // When

            // Then
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await underTest.DeleteByIdAsync(id));
            userManager.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Never());
        }
    }
}
