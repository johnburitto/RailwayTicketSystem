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
    public class PlaceServiceTests
    {
        [Fact]
        public async void GetAllAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new PlaceProfile())));
            var underTest = new PlaceService(context.Object, mapper);
            #endregion
            List<Place> places = new List<Place>()
            {
                new Place
                {
                    Id = 1,
                    Price = 100,
                    PlaceType = PlaceType.S1,
                    IsAvaliable = true,
                    TrainCarId = 1,
                    TrainCar = new TrainCar { },
                    Tickets = new List<Ticket> { new Ticket { } }
                }
            };
            context.Setup(x => x.Places).ReturnsDbSet(places);

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(places.Count, result.Count);
            Assert.Equal(places, result);

            foreach (var el in result)
            {
                Assert.NotNull(el.TrainCar);
                Assert.NotNull(el.Tickets);
            }
        }

        [Fact]
        public async void GetAllAsyncTest_EmptyDatabase()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new PlaceProfile())));
            var underTest = new PlaceService(context.Object, mapper);
            #endregion
            List<Place> places = new List<Place>()
            {
                new Place
                {
                    Id = 1,
                    Price = 100,
                    PlaceType = PlaceType.S1,
                    IsAvaliable = true,
                    TrainCarId = 1,
                    TrainCar = new TrainCar { },
                    Tickets = new List<Ticket> { new Ticket { } }
                }
            };
            context.Setup(x => x.Places).ReturnsDbSet(new List<Place>());

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.NotEqual(places, result);
        }

        [Fact]
        public async void GetRawAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new PlaceProfile())));
            var underTest = new PlaceService(context.Object, mapper);
            #endregion
            List<Place> places = new List<Place>()
            {
                new Place
                {
                    Id = 1,
                    Price = 100,
                    PlaceType = PlaceType.S1,
                    IsAvaliable = true,
                    TrainCarId = 1
                }
            };
            context.Setup(x => x.Places).ReturnsDbSet(places);

            // When
            var result = await underTest.GetAllRawAsync();

            // Then
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(places.Count, result.Count);
            Assert.Equal(places, result);

            foreach (var el in result)
            {
                Assert.Null(el.TrainCar);
                Assert.Null(el.Tickets);
            }
        }

        [Fact]
        public async void GetRawAsyncTest_EmptyDatabase()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new PlaceProfile())));
            var underTest = new PlaceService(context.Object, mapper);
            #endregion
            List<Place> places = new List<Place>()
            {
                new Place
                {
                    Id = 1,
                    Price = 100,
                    PlaceType = PlaceType.S1,
                    IsAvaliable = true,
                    TrainCarId = 1
                }
            };
            context.Setup(x => x.Places).ReturnsDbSet(new List<Place>());

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.NotEqual(places, result);
        }

        [Fact]
        public async void GetByIdAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new PlaceProfile())));
            var underTest = new PlaceService(context.Object, mapper);
            #endregion
            var departureTime = DateTime.Now;
            var arrivalTime = DateTime.Now;
            List<Place> places = new List<Place>()
            {
                new Place
                {
                    Id = 1,
                    Price = 100,
                    PlaceType = PlaceType.S1,
                    IsAvaliable = true,
                    TrainCarId = 1,
                    TrainCar = new TrainCar { },
                    Tickets = new List<Ticket> { new Ticket { } }
                },
                new Place
                {
                    Id = 3,
                    Price = 100,
                    PlaceType = PlaceType.S1,
                    IsAvaliable = true,
                    TrainCarId = 1,
                    TrainCar = new TrainCar { },
                    Tickets = new List<Ticket> { new Ticket { } }
                },
                new Place
                {
                    Id = 11,
                    Price = 100,
                    PlaceType = PlaceType.S1,
                    IsAvaliable = true,
                    TrainCarId = 1,
                    TrainCar = new TrainCar { },
                    Tickets = new List<Ticket> { new Ticket { } }
                },
                new Place
                {
                    Id = 100,
                    Price = 100,
                    PlaceType = PlaceType.S1,
                    IsAvaliable = true,
                    TrainCarId = 1,
                    TrainCar = new TrainCar { },
                    Tickets = new List<Ticket> { new Ticket { } }
                }
            };
            context.Setup(x => x.Places).ReturnsDbSet(places);

            // When
            var result = await underTest.GetByIdAsync(3);

            // Then
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
            Assert.Equal(100, result.Price);
            Assert.Equal(PlaceType.S1, result.PlaceType);
            Assert.True(result.IsAvaliable);
            Assert.Equal(1, result.TrainCarId);
            Assert.Contains(result, places);
        }

        [Fact]
        public async void GetByIdAsyncTest_TryGetUnregisteredId()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new PlaceProfile())));
            var underTest = new PlaceService(context.Object, mapper);
            #endregion
            List<Place> places = new List<Place>()
            {
                new Place
                {
                    Id = 1,
                    Price = 100,
                    PlaceType = PlaceType.S1,
                    IsAvaliable = true,
                    TrainCarId = 1,
                    TrainCar = new TrainCar { },
                    Tickets = new List<Ticket> { new Ticket { } }
                },
                new Place
                {
                    Id = 3,
                    Price = 100,
                    PlaceType = PlaceType.S1,
                    IsAvaliable = true,
                    TrainCarId = 1,
                    TrainCar = new TrainCar { },
                    Tickets = new List<Ticket> { new Ticket { } }
                },
                new Place
                {
                    Id = 11,
                    Price = 100,
                    PlaceType = PlaceType.S1,
                    IsAvaliable = true,
                    TrainCarId = 1,
                    TrainCar = new TrainCar { },
                    Tickets = new List<Ticket> { new Ticket { } }
                },
                new Place
                {
                    Id = 100,
                    Price = 100,
                    PlaceType = PlaceType.S1,
                    IsAvaliable = true,
                    TrainCarId = 1,
                    TrainCar = new TrainCar { },
                    Tickets = new List<Ticket> { new Ticket { } }
                }
            };
            context.Setup(x => x.Places).ReturnsDbSet(places);

            // When
            var result = await underTest.GetByIdAsync(200);

            // Then
            Assert.Null(result);
            Assert.DoesNotContain(result, places);
        }

        [Fact]
        public async void CreateAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new PlaceProfile())));
            var underTest = new PlaceService(context.Object, mapper);
            #endregion
            var dto = new PlaceCreateDto
            {
                Price = 100,
                PlaceType = PlaceType.Compartment,
                IsAvaliable = true,
                TrainCarId = 1
            };
            context.Setup(x => x.Places).ReturnsDbSet(new List<Place> { });

            // When 
            var result = await underTest.CreateAsync(dto);

            // Then
            context.Verify(x => x.Places.AddAsync(It.IsAny<Place>(), It.IsAny<CancellationToken>()), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async void UpdateAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new PlaceProfile())));
            var underTest = new PlaceService(context.Object, mapper);
            #endregion
            var place = new Place
            {
                Id = 1,
                Price = 100,
                PlaceType = PlaceType.Compartment,
                IsAvaliable = true,
                TrainCarId = 1,
                TrainCar = new TrainCar { },
                Tickets = new List<Ticket> { new Ticket { } }
            };
            var dto = new PlaceUpdateDto
            {
                Id = 1,
                Price = 100,
                PlaceType = PlaceType.S1,
                IsAvaliable = true,
                TrainCarId = 1
            };

            context.Setup(x => x.Places).ReturnsDbSet(new[] { place }.AsQueryable());
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            var result = await underTest.UpdateAsync(dto);

            //Then
            context.Verify(x => x.Update(place), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async void DeleteAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new PlaceProfile())));
            var underTest = new PlaceService(context.Object, mapper);
            #endregion
            var place = new Place
            {
                Id = 1,
                Price = 100,
                PlaceType = PlaceType.Compartment,
                IsAvaliable = true,
                TrainCarId = 1,
                TrainCar = new TrainCar { },
                Tickets = new List<Ticket> { new Ticket { } }
            };

            context.Setup(x => x.Places).ReturnsDbSet(new[] { place }.AsQueryable());
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            await underTest.DeleteAsync(place);

            //Then
            context.Verify(x => x.Places.Remove(place), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
