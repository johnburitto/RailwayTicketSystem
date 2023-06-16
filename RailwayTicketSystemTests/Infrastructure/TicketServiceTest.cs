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
    public class TicketServiceTest
    {
        [Fact]
        public async void GetAllAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TicketProfile())));
            var underTest = new TicketService(context.Object, mapper);
            #endregion
            List<Ticket> tickets = new List<Ticket>()
            {
                new Ticket
                {
                    Id = 1,
                    BookDate = DateTime.Now,
                    PlaceId = 1,
                    Place = new Place { }
                }
            };
            context.Setup(x => x.Tickets).ReturnsDbSet(tickets);

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(tickets.Count, result.Count);
            Assert.Equal(tickets, result);

            foreach (var el in result)
            {
                Assert.NotNull(el.Place);
            }
        }

        [Fact]
        public async void GetAllAsyncTest_EmptyDatabase()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TicketProfile())));
            var underTest = new TicketService(context.Object, mapper);
            #endregion
            List<Ticket> tickets = new List<Ticket>()
            {
                new Ticket
                {
                    Id = 1,
                    BookDate = DateTime.Now,
                    PlaceId = 1,
                    Place = new Place { }
                }
            };
            context.Setup(x => x.Tickets).ReturnsDbSet(new List<Ticket>());

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.NotEqual(tickets, result);
        }

        [Fact]
        public async void GetRawAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TicketProfile())));
            var underTest = new TicketService(context.Object, mapper);
            #endregion
            List<Ticket> tickets = new List<Ticket>()
            {
                new Ticket
                {
                    Id = 1,
                    BookDate = DateTime.Now,
                    PlaceId = 1
                }
            };
            context.Setup(x => x.Tickets).ReturnsDbSet(tickets);

            // When
            var result = await underTest.GetAllRawAsync();

            // Then
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(tickets.Count, result.Count);
            Assert.Equal(tickets, result);

            foreach (var el in result)
            {
                Assert.Null(el.Place);
            }
        }

        [Fact]
        public async void GetRawAsyncTest_EmptyDatabase()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TicketProfile())));
            var underTest = new TicketService(context.Object, mapper);
            #endregion
            List<Ticket> tickets = new List<Ticket>()
            {
                new Ticket
                {
                    Id = 1,
                    BookDate = DateTime.Now,
                    PlaceId = 1
                }
            };
            context.Setup(x => x.Tickets).ReturnsDbSet(new List<Ticket>());

            // When
            var result = await underTest.GetAllAsync();

            // Then
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.NotEqual(tickets, result);
        }

        [Fact]
        public async void GetByIdAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TicketProfile())));
            var underTest = new TicketService(context.Object, mapper);
            #endregion
            var bookDate = DateTime.Now;
            List<Ticket> tickets = new List<Ticket>()
            {
                new Ticket
                {
                    Id = 1,
                    BookDate = DateTime.Now,
                    PlaceId = 1,
                    Place = new Place { }
                },
                new Ticket
                {
                    Id = 4,
                    BookDate = bookDate,
                    PlaceId = 1,
                    Place = new Place { }
                },
                new Ticket
                {
                    Id = 7,
                    BookDate = DateTime.Now,
                    PlaceId = 1,
                    Place = new Place { }
                },
                new Ticket
                {
                    Id = 78,
                    BookDate = DateTime.Now,
                    PlaceId = 1,
                    Place = new Place { }
                }
            };
            context.Setup(x => x.Tickets).ReturnsDbSet(tickets);

            // When
            var result = await underTest.GetByIdAsync(4);

            // Then
            Assert.NotNull(result);
            Assert.Equal(4, result.Id);
            Assert.Equal(bookDate, result.BookDate);
            Assert.Equal(1, result.PlaceId);
            Assert.Contains(result, tickets);
        }

        [Fact]
        public async void GetByIdAsyncTest_TryGetUnregisteredId()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TicketProfile())));
            var underTest = new TicketService(context.Object, mapper);
            #endregion
            List<Ticket> tickets = new List<Ticket>()
            {
                new Ticket
                {
                    Id = 1,
                    BookDate = DateTime.Now,
                    PlaceId = 1,
                    Place = new Place { }
                },
                new Ticket
                {
                    Id = 4,
                    BookDate = DateTime.Now,
                    PlaceId = 1,
                    Place = new Place { }
                },
                new Ticket
                {
                    Id = 7,
                    BookDate = DateTime.Now,
                    PlaceId = 1,
                    Place = new Place { }
                },
                new Ticket
                {
                    Id = 78,
                    BookDate = DateTime.Now,
                    PlaceId = 1,
                    Place = new Place { }
                }
            };
            context.Setup(x => x.Tickets).ReturnsDbSet(tickets);

            // When
            var result = await underTest.GetByIdAsync(200);

            // Then
            Assert.Null(result);
            Assert.DoesNotContain(result, tickets);
        }

        [Fact]
        public async void CreateAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TicketProfile())));
            var underTest = new TicketService(context.Object, mapper);
            #endregion
            var dto = new TicketCreateDto
            {
                BookDate = DateTime.Now,
                PlaceId = 1
            };
            context.Setup(x => x.Tickets).ReturnsDbSet(new List<Ticket> { });

            // When 
            var result = await underTest.CreateAsync(dto);

            // Then
            context.Verify(x => x.Tickets.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async void UpdateAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TicketProfile())));
            var underTest = new TicketService(context.Object, mapper);
            #endregion
            var ticket = new Ticket
            {
                Id = 1,
                BookDate = DateTime.Now,
                PlaceId = 1,
                Place = new Place { }
            };
            var dto = new TicketUpdateDto
            {
                Id = 1,
                BookDate = DateTime.Now,
                PlaceId = 1
            };

            context.Setup(x => x.Tickets).ReturnsDbSet(new[] { ticket }.AsQueryable());
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            var result = await underTest.UpdateAsync(dto);

            //Then
            context.Verify(x => x.Update(ticket), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async void DeleteAsyncTest_NormalFlow()
        {
            // Given
            #region Setup
            var context = new Mock<AppDbContext>();
            var mapper = new Mapper(new MapperConfiguration(confiration => confiration.AddProfile(new TicketProfile())));
            var underTest = new TicketService(context.Object, mapper);
            #endregion
            var ticket = new Ticket
            {
                Id = 1,
                BookDate = DateTime.Now,
                PlaceId = 1,
                Place = new Place { }
            };

            context.Setup(x => x.Tickets).ReturnsDbSet(new[] { ticket }.AsQueryable());
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // When
            await underTest.DeleteAsync(ticket);

            //Then
            context.Verify(x => x.Tickets.Remove(ticket), Times.Once());
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
