using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using RailwayTicketSystemTests.Profiles;
using Security.Services.Impls;
using Security.Dto;
using MockQueryable.Moq;

namespace RailwayTicketSystemTests.Security
{
    public class RoleServiceTests
    {
        [Fact]
        public async void GetAllAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var roleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(cofiguration => cofiguration.AddProfile(new RoleProfile())));
            var underTest = new RoleService(roleManager.Object, mapper);
            #endregion
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Role",
                    NormalizedName = "ROLE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            }.AsQueryable().BuildMock();
            roleManager.Setup(x => x.Roles).Returns(roles);
            
            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(roles.Count(), result.Count);
            Assert.Equal(roles, result);
        }

        [Fact]
        public async void GetAllAsyncTest_EmptyDatabase()
        {
            // Given
            #region Setup
            var roleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(cofiguration => cofiguration.AddProfile(new RoleProfile())));
            var underTest = new RoleService(roleManager.Object, mapper);
            #endregion
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Role",
                    NormalizedName = "ROLE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            };
            roleManager.Setup(x => x.Roles).Returns(new List<IdentityRole>().AsQueryable().BuildMock());
            
            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.NotEqual(roles, result);
        }

        [Fact]
        public async void AddAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var roleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(cofiguration => cofiguration.AddProfile(new RoleProfile())));
            var underTest = new RoleService(roleManager.Object, mapper);
            #endregion
            var dto = new RoleDto
            {
                RoleName = "Admin"
            };
            roleManager.Setup(x => x.Roles).Returns(new List<IdentityRole>().AsQueryable().BuildMock());
            roleManager.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);
            
            // When
            var result = await underTest.AddAsync(dto);

            // Then
            Assert.Equal($"Role {dto.RoleName} successfully added to database.", result);
        }

        [Fact]
        public async void AddAsyncTest_RoleDoesntExistInSystem()
        {
            // Given
            #region Setup
            var roleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(cofiguration => cofiguration.AddProfile(new RoleProfile())));
            var underTest = new RoleService(roleManager.Object, mapper);
            #endregion
            var dto = new RoleDto
            {
                RoleName = "Role"
            };
            roleManager.Setup(x => x.Roles).Returns(new List<IdentityRole>().AsQueryable().BuildMock());
            roleManager.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

            // When
            var result = await underTest.AddAsync(dto);

            // Then
            Assert.Equal($"System doesn't have role {dto.RoleName}.", result);
        }

        [Fact]
        public async void AddAsyncTest_RoleAlreadyExist()
        {
            // Given
            #region Setup
            var roleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(cofiguration => cofiguration.AddProfile(new RoleProfile())));
            var underTest = new RoleService(roleManager.Object, mapper);
            #endregion
            var roles = new List<IdentityRole> 
            {
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "Admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            }.AsQueryable().BuildMock();
            var dto = new RoleDto
            {
                RoleName = "Admin"
            };
            roleManager.Setup(x => x.Roles).Returns(roles);
            roleManager.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

            // When
            var result = await underTest.AddAsync(dto);

            // Then
            Assert.Equal($"Role {dto.RoleName} already exist in database.", result);
        }

        [Fact]
        public async void AddAsyncTest_InternalErrorHapends()
        {
            // Given
            #region Setup
            var roleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(cofiguration => cofiguration.AddProfile(new RoleProfile())));
            var underTest = new RoleService(roleManager.Object, mapper);
            #endregion
            var dto = new RoleDto
            {
                RoleName = "Admin"
            };
            roleManager.Setup(x => x.Roles).Returns(new List<IdentityRole>().AsQueryable().BuildMock());
            roleManager.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Failed());

            // When
            var result = await underTest.AddAsync(dto);

            // Then
            Assert.Equal($"Some internal error has occured.", result);
        }

        [Fact]
        public async void RemoveAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var roleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(cofiguration => cofiguration.AddProfile(new RoleProfile())));
            var underTest = new RoleService(roleManager.Object, mapper);
            #endregion
            var roles = new List<IdentityRole> 
            {
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "Admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            }.AsQueryable().BuildMock();
            var dto = new RoleDto
            {
                RoleName = "Admin"
            };
            roleManager.Setup(x => x.Roles).Returns(roles);
            roleManager.Setup(x => x.DeleteAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

            // When
            var result = await underTest.RemoveAsync(dto);

            // Then
            Assert.Equal($"Role {dto.RoleName} successfully removed from database.", result);
        }

        [Fact]
        public async void RemoveAsyncTest_RoleDoesntExistInSystem()
        {
            // Given
            #region Setup
            var roleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(cofiguration => cofiguration.AddProfile(new RoleProfile())));
            var underTest = new RoleService(roleManager.Object, mapper);
            #endregion
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "Admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            }.AsQueryable().BuildMock();
            var dto = new RoleDto
            {
                RoleName = "Role"
            };
            roleManager.Setup(x => x.Roles).Returns(roles);
            roleManager.Setup(x => x.DeleteAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

            // When
            var result = await underTest.RemoveAsync(dto);

            // Then
            Assert.Equal($"System doesn't have role {dto.RoleName}.", result);
        }

        [Fact]
        public async void RemoveAsyncTest_RoleDoesntExistInDatabase()
        {
            // Given
            #region Setup
            var roleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(cofiguration => cofiguration.AddProfile(new RoleProfile())));
            var underTest = new RoleService(roleManager.Object, mapper);
            #endregion
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "Admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            }.AsQueryable().BuildMock();
            var dto = new RoleDto
            {
                RoleName = "User"
            };
            roleManager.Setup(x => x.Roles).Returns(roles);
            roleManager.Setup(x => x.DeleteAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

            // When
            var result = await underTest.RemoveAsync(dto);

            // Then
            Assert.Equal($"Role {dto.RoleName} doesn't exist in database.", result);
        }

        [Fact]
        public async void RemoveAsyncTest_InternalErrorHapends()
        {
            // Given
            #region Setup
            var roleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            var mapper = new Mapper(new MapperConfiguration(cofiguration => cofiguration.AddProfile(new RoleProfile())));
            var underTest = new RoleService(roleManager.Object, mapper);
            #endregion
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "Admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            }.AsQueryable().BuildMock();
            var dto = new RoleDto
            {
                RoleName = "Admin"
            };
            roleManager.Setup(x => x.Roles).Returns(roles);
            roleManager.Setup(x => x.DeleteAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Failed());

            // When
            var result = await underTest.RemoveAsync(dto);

            // Then
            Assert.Equal($"Some internal error has occured.", result);
        }
    }
}
