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
    public class TrainCarCarServiceTest
    {
        [Fact]
        public async void GetAllAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainCarProfile())));
            var underTest = new TrainCarService(context.Object, mapper);
            #endregion
            List<TrainCar> trainCars = new List<TrainCar>()
            {
                new TrainCar
                {
                    Id = 1,
                    Number = "Number",
                    TrainId = 1,
                    Train = new Train { },
                    Places = new List<Place> { new Place { } }
                }
            };
            context.Setup(x => x.TrainCars).ReturnsDbSet(trainCars);

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(trainCars.Count, result.Count);
            Assert.Equal(trainCars, result);

            foreach (var el in result)
            {
                Assert.NotNull(el.Train);
                Assert.NotNull(el.Places);
            }
        }

        [Fact]
        public async void GetAllAsyncTest_EmptyDatabase()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainCarProfile())));
            var underTest = new TrainCarService(context.Object, mapper);
            #endregion
            List<TrainCar> trainCars = new List<TrainCar>()
            {
                new TrainCar
                {
                    Id = 1,
                    Number = "Number",
                    TrainId = 1,
                    Train = new Train { },
                    Places = new List<Place> { new Place { } }
                }
            };
            context.Setup(x => x.TrainCars).ReturnsDbSet(new List<TrainCar>());

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.NotEqual(trainCars, result);
        }

        [Fact]
        public async void GetRawAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainCarProfile())));
            var underTest = new TrainCarService(context.Object, mapper);
            #endregion
            List<TrainCar> trainCars = new List<TrainCar>()
            {
                new TrainCar
                {
                    Id = 1,
                    Number = "Number",
                    TrainId = 1
                }
            };
            context.Setup(x => x.TrainCars).ReturnsDbSet(trainCars);

            // When
            var result = await underTest.GetAllRawAsync();

            // Then
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(trainCars.Count, result.Count);
            Assert.Equal(trainCars, result);

            foreach (var el in result)
            {
                Assert.Null(el.Train);
                Assert.Null(el.Places);
            }
        }

        [Fact]
        public async void GetRawAsyncTest_EmptyDatabase()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainCarProfile())));
            var underTest = new TrainCarService(context.Object, mapper);
            #endregion
            List<TrainCar> trainCars = new List<TrainCar>()
            {
                new TrainCar
                {
                    Id = 1,
                    Number = "Number",
                    TrainId = 1
                }
            };
            context.Setup(x => x.TrainCars).ReturnsDbSet(new List<TrainCar>());

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.NotEqual(trainCars, result);
        }

        [Fact]
        public async void GetByIdAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainCarProfile())));
            var underTest = new TrainCarService(context.Object, mapper);
            #endregion
            var departureTime = DateTime.Now;
            var arrivalTime = DateTime.Now;
            List<TrainCar> trainCars = new List<TrainCar>()
            {
                new TrainCar
                {
                    Id = 1,
                    Number = "Number",
                    TrainId = 1,
                    Train = new Train { },
                    Places = new List<Place> { new Place { } }
                },
                new TrainCar
                {
                    Id = 12,
                    Number = "Number",
                    TrainId = 1,
                    Train = new Train { },
                    Places = new List<Place> { new Place { } }
                },
                new TrainCar
                {
                    Id = 3,
                    Number = "Number",
                    TrainId = 1,
                    Train = new Train { },
                    Places = new List<Place> { new Place { } }
                },
                new TrainCar
                {
                    Id = 7,
                    Number = "Number",
                    TrainId = 1,
                    Train = new Train { },
                    Places = new List<Place> { new Place { } }
                }
            };
            context.Setup(x => x.TrainCars).ReturnsDbSet(trainCars);

            // When
            var result = await underTest.GetByIdAsync(3);

            // Then
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
            Assert.Equal("Number", result.Number);
            Assert.Equal(1, result.TrainId);
            Assert.Contains(result, trainCars);
        }

        [Fact]
        public async void GetByIdAsyncTest_TryGetUnregisteredId()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainCarProfile())));
            var underTest = new TrainCarService(context.Object, mapper);
            #endregion
            List<TrainCar> trainCars = new List<TrainCar>()
            {
                new TrainCar
                {
                    Id = 1,
                    Number = "Number",
                    TrainId = 1,
                    Train = new Train { },
                    Places = new List<Place> { new Place { } }
                },
                new TrainCar
                {
                    Id = 12,
                    Number = "Number",
                    TrainId = 1,
                    Train = new Train { },
                    Places = new List<Place> { new Place { } }
                },
                new TrainCar
                {
                    Id = 3,
                    Number = "Number",
                    TrainId = 1,
                    Train = new Train { },
                    Places = new List<Place> { new Place { } }
                },
                new TrainCar
                {
                    Id = 7,
                    Number = "Number",
                    TrainId = 1,
                    Train = new Train { },
                    Places = new List<Place> { new Place { } }
                }
            };
            context.Setup(x => x.TrainCars).ReturnsDbSet(trainCars);

            // When
            var result = await underTest.GetByIdAsync(200);

            // Then
            Assert.Null(result);
            Assert.DoesNotContain(result, trainCars);
        }

        [Fact]
        public async void CreateAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainCarProfile())));
            var underTest = new TrainCarService(context.Object, mapper);
            #endregion
            var dto = new TrainCarCreateDto
            {
                Number = "Number",
                TrainId = 1
            };
            context.Setup(x => x.TrainCars).ReturnsDbSet(new List<TrainCar> { });

            // When 
            var result = await underTest.CreateAsync(dto);

            // Then
            context.Verify(x => x.TrainCars.AddAsync(It.IsAny<TrainCar>(), It.IsAny<CancellationToken>()), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async void UpdateAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainCarProfile())));
            var underTest = new TrainCarService(context.Object, mapper);
            #endregion
            var trainCar = new TrainCar
            {
                Id = 1,
                Number = "Number",
                TrainId = 1,
                Train = new Train { },
                Places = new List<Place> { new Place { } }
            };
            var dto = new TrainCarUpdateDto
            {
                Id = 1,
                Number = "New Number",
                TrainId = 1
            };

            context.Setup(x => x.TrainCars).ReturnsDbSet(new[] { trainCar }.AsQueryable());
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            var result = await underTest.UpdateAsync(dto);

            //Then
            context.Verify(x => x.Update(trainCar), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async void DeleteAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TrainCarProfile())));
            var underTest = new TrainCarService(context.Object, mapper);
            #endregion
            var trainCar = new TrainCar
            {
                Id = 1,
                Number = "Number",
                TrainId = 1,
                Train = new Train { },
                Places = new List<Place> { new Place { } }
            };

            context.Setup(x => x.TrainCars).ReturnsDbSet(new[] { trainCar }.AsQueryable());
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            await underTest.DeleteAsync(trainCar);

            //Then
            context.Verify(x => x.TrainCars.Remove(trainCar), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
