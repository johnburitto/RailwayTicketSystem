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
    public class TrainServiceTests
    {
        [Fact]
        public async void GetAllAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainProfile())));
            var underTest = new TrainService(context.Object, mapper);
            #endregion
            List<Train> trains = new List<Train>()
            {
                new Train
                {
                    Id = 1,
                    Number = "Number",
                    RouteId = 1,
                    Route = new Route { },
                    TrainCars = new List<TrainCar> { new TrainCar { } }
                }
            };
            context.Setup(x => x.Trains).ReturnsDbSet(trains);

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(trains.Count, result.Count);
            Assert.Equal(trains, result);

            foreach (var el in result)
            {
                Assert.NotNull(el.Route);
                Assert.NotNull(el.TrainCars);
            }
        }

        [Fact]
        public async void GetAllAsyncTest_EmptyDatabase()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainProfile())));
            var underTest = new TrainService(context.Object, mapper);
            #endregion
            List<Train> trains = new List<Train>()
            {
                new Train
                {
                    Id = 1,
                    Number = "Number",
                    RouteId = 1,
                    Route = new Route { },
                    TrainCars = new List<TrainCar> { new TrainCar { } }
                }
            };
            context.Setup(x => x.Trains).ReturnsDbSet(new List<Train>());

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.NotEqual(trains, result);
        }

        [Fact]
        public async void GetRawAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainProfile())));
            var underTest = new TrainService(context.Object, mapper);
            #endregion
            List<Train> trains = new List<Train>()
            {
                new Train
                {
                    Id = 1,
                    Number = "Number",
                    RouteId = 1
                }
            };
            context.Setup(x => x.Trains).ReturnsDbSet(trains);

            // When
            var result = await underTest.GetAllRawAsync();

            // Then
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(trains.Count, result.Count);
            Assert.Equal(trains, result);

            foreach (var el in result)
            {
                Assert.Null(el.Route);
                Assert.Null(el.TrainCars);
            }
        }

        [Fact]
        public async void GetRawAsyncTest_EmptyDatabase()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainProfile())));
            var underTest = new TrainService(context.Object, mapper);
            #endregion
            List<Train> trains = new List<Train>()
            {
                new Train
                {
                    Id = 1,
                    Number = "Number",
                    RouteId = 1
                }
            };
            context.Setup(x => x.Trains).ReturnsDbSet(new List<Train>());

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.NotEqual(trains, result);
        }

        [Fact]
        public async void GetByIdAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainProfile())));
            var underTest = new TrainService(context.Object, mapper);
            #endregion
            var departureTime = DateTime.Now;
            var arrivalTime = DateTime.Now;
            List<Train> trains = new List<Train>()
            {
                new Train
                {
                    Id = 1,
                    Number = "Number",
                    RouteId = 1,
                    Route = new Route { },
                    TrainCars = new List<TrainCar> { new TrainCar { } }
                },
                new Train
                {
                    Id = 4,
                    Number = "Number",
                    RouteId = 1,
                    Route = new Route { },
                    TrainCars = new List<TrainCar> { new TrainCar { } }
                },
                new Train
                {
                    Id = 11,
                    Number = "Number",
                    RouteId = 1,
                    Route = new Route { },
                    TrainCars = new List<TrainCar> { new TrainCar { } }
                },
                new Train
                {
                    Id = 6,
                    Number = "Number",
                    RouteId = 1,
                    Route = new Route { },
                    TrainCars = new List<TrainCar> { new TrainCar { } }
                }
            };
            context.Setup(x => x.Trains).ReturnsDbSet(trains);

            // When
            var result = await underTest.GetByIdAsync(6);

            // Then
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal("Number", result.Number);
            Assert.Equal(1, result.RouteId);
            Assert.Contains(result, trains);
        }

        [Fact]
        public async void GetByIdAsyncTest_TryGetUnregisteredId()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainProfile())));
            var underTest = new TrainService(context.Object, mapper);
            #endregion
            List<Train> trains = new List<Train>()
            {
                new Train
                {
                    Id = 1,
                    Number = "Number",
                    RouteId = 1,
                    Route = new Route { },
                    TrainCars = new List<TrainCar> { new TrainCar { } }
                },
                new Train
                {
                    Id = 4,
                    Number = "Number",
                    RouteId = 1,
                    Route = new Route { },
                    TrainCars = new List<TrainCar> { new TrainCar { } }
                },
                new Train
                {
                    Id = 11,
                    Number = "Number",
                    RouteId = 1,
                    Route = new Route { },
                    TrainCars = new List<TrainCar> { new TrainCar { } }
                },
                new Train
                {
                    Id = 6,
                    Number = "Number",
                    RouteId = 1,
                    Route = new Route { },
                    TrainCars = new List<TrainCar> { new TrainCar { } }
                }
            };
            context.Setup(x => x.Trains).ReturnsDbSet(trains);

            // When
            var result = await underTest.GetByIdAsync(200);

            // Then
            Assert.Null(result);
            Assert.DoesNotContain(result, trains);
        }

        [Fact]
        public async void CreateAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainProfile())));
            var underTest = new TrainService(context.Object, mapper);
            #endregion
            var dto = new TrainCreateDto
            {
                Number = "Number",
                RouteId = 1
            };
            context.Setup(x => x.Trains).ReturnsDbSet(new List<Train> { });

            // When 
            var result = await underTest.CreateAsync(dto);

            // Then
            context.Verify(x => x.Trains.AddAsync(It.IsAny<Train>(), It.IsAny<CancellationToken>()), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async void UpdateAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainProfile())));
            var underTest = new TrainService(context.Object, mapper);
            #endregion
            var train = new Train
            {
                Id = 1,
                Number = "Number",
                RouteId = 1,
                Route = new Route { },
                TrainCars = new List<TrainCar> { new TrainCar { } }
            };
            var dto = new TrainUpdateDto
            {
                Id = 1,
                Number = "New Number",
                RouteId = 1
            };

            context.Setup(x => x.Trains).ReturnsDbSet(new[] { train }.AsQueryable());
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            var result = await underTest.UpdateAsync(dto);

            //Then
            context.Verify(x => x.Update(train), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async void DeleteAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainProfile())));
            var underTest = new TrainService(context.Object, mapper);
            #endregion
            var train = new Train
            {
                Id = 1,
                Number = "Number",
                RouteId = 1,
                Route = new Route { },
                TrainCars = new List<TrainCar> { new TrainCar { } }
            };

            context.Setup(x => x.Trains).ReturnsDbSet(new[] { train }.AsQueryable());
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            await underTest.DeleteAsync(train);

            //Then
            context.Verify(x => x.Trains.Remove(train), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
