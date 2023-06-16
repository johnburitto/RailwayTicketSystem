using AutoMapper;
using Core.Dtos.Create;
using Core.Dtos.Update;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Services.Impls;
using Moq;
using Moq.EntityFrameworkCore;
using RailwayTicketSystemTests.Profiles;

namespace RailwayTicketSystemTests.Infrastructure
{
    public class RouteServiceTests
    {
        [Fact]
        public async void GetAllAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new RouteProfile())));
            var underTest = new RouteService(context.Object, mapper);
            #endregion
            List<Route> routes = new List<Route>()
            {
                new Route
                {
                    Id = 1,
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now,
                    FromCity = "CityOne",
                    ToCity = "CityTwo",
                    Trains = new List<Train> { new Train { } }
                }
            };
            context.Setup(x => x.Routes).ReturnsDbSet(routes);

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(routes.Count, result.Count);
            Assert.Equal(routes, result);

            foreach (var el in result)
            {
                Assert.NotNull(el.Trains);
            }
        }

        [Fact]
        public async void GetAllAsyncTest_EmptyDatabase()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new RouteProfile())));
            var underTest = new RouteService(context.Object, mapper);
            #endregion
            List<Route> routes = new List<Route>()
            {
                new Route
                {
                    Id = 1,
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now,
                    FromCity = "CityOne",
                    ToCity = "CityTwo",
                    Trains = new List<Train> { new Train { } }
                }
            };
            context.Setup(x => x.Routes).ReturnsDbSet(new List<Route> ());

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.NotEqual(routes, result);
        }

        [Fact]
        public async void GetRawAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new RouteProfile())));
            var underTest = new RouteService(context.Object, mapper);
            #endregion
            List<Route> routes = new List<Route>()
            {
                new Route
                {
                    Id = 1,
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now,
                    FromCity = "CityOne",
                    ToCity = "CityTwo"
                }
            };
            context.Setup(x => x.Routes).ReturnsDbSet(routes);

            // When
            var result = await underTest.GetAllRawAsync();

            // Then
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(routes.Count, result.Count);
            Assert.Equal(routes, result);

            foreach (var el in result)
            {
                Assert.Null(el.Trains);
            }
        }

        [Fact]
        public async void GetRawAsyncTest_EmptyDatabase()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new RouteProfile())));
            var underTest = new RouteService(context.Object, mapper);
            #endregion
            List<Route> routes = new List<Route>()
            {
                new Route
                {
                    Id = 1,
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now,
                    FromCity = "CityOne",
                    ToCity = "CityTwo"
                }
            };
            context.Setup(x => x.Routes).ReturnsDbSet(new List<Route>());

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.NotEqual(routes, result);
        }

        [Fact]
        public async void GetByIdAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new RouteProfile())));
            var underTest = new RouteService(context.Object, mapper);
            #endregion
            var departureTime = DateTime.Now;
            var arrivalTime = DateTime.Now;
            List<Route> routes = new List<Route>()
            {
                new Route
                {
                    Id = 1,
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now,
                    FromCity = "CityOne",
                    ToCity = "CityTwo",
                    Trains = new List<Train> { new Train { } }
                },
                new Route
                {
                    Id = 2,
                    DepartureTime = departureTime,
                    ArrivalTime = arrivalTime,
                    FromCity = "CityOne",
                    ToCity = "CityTwo",
                    Trains = new List<Train> { new Train { } }
                },
                new Route
                {
                    Id = 3,
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now,
                    FromCity = "CityOne",
                    ToCity = "CityTwo",
                    Trains = new List<Train> { new Train { } }
                },
                new Route
                {
                    Id = 11,
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now,
                    FromCity = "CityOne",
                    ToCity = "CityTwo",
                    Trains = new List<Train> { new Train { } }
                }
            };
            context.Setup(x => x.Routes).ReturnsDbSet(routes);

            // When
            var result = await underTest.GetByIdAsync(2);

            // Then
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal(departureTime, result.DepartureTime);
            Assert.Equal(arrivalTime, result.ArrivalTime);
            Assert.Equal("CityOne", result.FromCity);
            Assert.Equal("CityTwo", result.ToCity);
            Assert.Contains(result, routes);
        }

        [Fact]
        public async void GetByIdAsyncTest_TryGetUnregisteredId()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new RouteProfile())));
            var underTest = new RouteService(context.Object, mapper);
            #endregion
            List<Route> routes = new List<Route>()
            {
                new Route
                {
                    Id = 1,
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now,
                    FromCity = "CityOne",
                    ToCity = "CityTwo",
                    Trains = new List<Train> { new Train { } }
                },
                new Route
                {
                    Id = 2,
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now,
                    FromCity = "CityOne",
                    ToCity = "CityTwo",
                    Trains = new List<Train> { new Train { } }
                },
                new Route
                {
                    Id = 3,
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now,
                    FromCity = "CityOne",
                    ToCity = "CityTwo",
                    Trains = new List<Train> { new Train { } }
                },
                new Route
                {
                    Id = 11,
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now,
                    FromCity = "CityOne",
                    ToCity = "CityTwo",
                    Trains = new List<Train> { new Train { } }
                }
            };
            context.Setup(x => x.Routes).ReturnsDbSet(routes);

            // When
            var result = await underTest.GetByIdAsync(200);

            // Then
            Assert.Null(result);
            Assert.DoesNotContain(result, routes);
        }

        [Fact]
        public async void CreateAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new RouteProfile())));
            var underTest = new RouteService(context.Object, mapper);
            #endregion
            var dto = new RouteCreateDto
            {
                DepartureTime = DateTime.Now,
                ArrivalTime = DateTime.Now,
                FromCity = "CityOne",
                ToCity = "CityTwo"
            };
            context.Setup(x => x.Routes).ReturnsDbSet(new List<Route> { });

            // When 
            var result = await underTest.CreateAsync(dto);

            // Then
            context.Verify(x => x.Routes.AddAsync(It.IsAny<Route>(), It.IsAny<CancellationToken>()), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async void UpdateAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new RouteProfile())));
            var underTest = new RouteService(context.Object, mapper);
            #endregion
            var route = new Route
            {
                Id = 1,
                DepartureTime = DateTime.Now,
                ArrivalTime = DateTime.Now,
                FromCity = "CityOne",
                ToCity = "CityTwo",
                Trains = new List<Train> { new Train { } }
            };
            var dto = new RouteUpdateDto
            {
                Id = 1,
                DepartureTime = DateTime.Now,
                ArrivalTime = DateTime.Now,
                FromCity = "CityThree",
                ToCity = "CityFour"
            };

            context.Setup(x => x.Routes).ReturnsDbSet(new[] { route }.AsQueryable());
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            var result = await underTest.UpdateAsync(dto);

            //Then
            context.Verify(x => x.Update(route), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async void DeleteAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new RouteProfile())));
            var underTest = new RouteService(context.Object, mapper);
            #endregion
            var route = new Route
            {
                Id = 1,
                DepartureTime = DateTime.Now,
                ArrivalTime = DateTime.Now,
                FromCity = "CityOne",
                ToCity = "CityTwo",
                Trains = new List<Train> { new Train { } }
            };

            context.Setup(x => x.Routes).ReturnsDbSet(new[] { route }.AsQueryable());
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            await underTest.DeleteAsync(route);

            //Then
            context.Verify(x => x.Routes.Remove(route), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
