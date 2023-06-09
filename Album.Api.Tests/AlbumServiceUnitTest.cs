using Album.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Album.Api.Tests.Services
{
    [TestFixture]
    public class AlbumServiceTests
    {
        private Mock<Database.albumContext> _dbContextMock;
        private IAlbumService _albumService;

        [SetUp]
        public void Setup()
        {
            _dbContextMock = new Mock<Database.albumContext>();
            _albumService = new AlbumService(_dbContextMock.Object);
        }

        [Test]
        public async Task GetAllAlbums_ReturnsAllAlbums()
        {
            // Arrange
            var albums = new List<Models.Album>
            {
                new Models.Album { Id = 1, Name = "Album 1", Artist = "Artist 1", ImageUrl = "image1.jpg" },
                new Models.Album { Id = 2, Name = "Album 2", Artist = "Artist 2", ImageUrl = "image2.jpg" }
            };
            var albumSetMock = MockAsyncQueryable(albums.AsQueryable());
            _dbContextMock.Setup(db => db.Albums).Returns(albumSetMock);

            // Act
            var result = await _albumService.GetAllAlbums();

            // Assert
            Assert.AreEqual(albums.Count, result.Count);
            Assert.AreEqual(albums[0].Name, result[0].Name);
            Assert.AreEqual(albums[1].Artist, result[1].Artist);
        }

        [Test]
        public async Task GetAlbumById_ExistingId_ReturnsAlbum()
        {
            // Arrange
            var album = new Models.Album { Id = 1, Name = "Album 1", Artist = "Artist 1", ImageUrl = "image1.jpg" };
            _dbContextMock.Setup(db => db.Albums.FindAsync(1)).ReturnsAsync(album);

            // Act
            var result = await _albumService.GetAlbumById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(album.Name, result.Name);
            Assert.AreEqual(album.Artist, result.Artist);
        }

        [Test]
        public async Task GetAlbumById_NonExistingId_ReturnsNull()
        {
            // Arrange
            _dbContextMock.Setup(db => db.Albums.FindAsync(1)).ReturnsAsync((Models.Album)null);

            // Act
            var result = await _albumService.GetAlbumById(1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateAlbum_AddsAlbumToDbContext()
        {
            // Arrange
            var album = new Models.Album { Name = "New Album", Artist = "New Artist", ImageUrl = "newimage.jpg" };

            // Act
            var result = await _albumService.CreateAlbum(album);

            // Assert
            _dbContextMock.Verify(db => db.Albums.Add(album), Times.Once);
            _dbContextMock.Verify(db => db.SaveChangesAsync(CancellationToken.None), Times.Once);
            Assert.AreEqual(album, result);
        }

        [Test]
        public async Task UpdateAlbum_ExistingId_UpdatesAlbumInDbContext()
        {
            // Arrange
            var existingAlbum = new Models.Album { Id = 1, Name = "Album 1", Artist = "Artist 1", ImageUrl = "image1.jpg" };
            var updatedAlbum = new Models.Album { Id = 1, Name = "Updated Album", Artist = "Updated Artist", ImageUrl = "updatedimage.jpg" };
            _dbContextMock.Setup(db => db.Albums.FindAsync(1)).ReturnsAsync(existingAlbum);

            // Act
            var result = await _albumService.UpdateAlbum(1, updatedAlbum);

            // Assert
            _dbContextMock.Verify(db =>
                db.SaveChangesAsync(CancellationToken.None), Times.Once);
            Assert.AreEqual(updatedAlbum.Name, existingAlbum.Name);
            Assert.AreEqual(updatedAlbum.Artist, existingAlbum.Artist);
            Assert.AreEqual(updatedAlbum.ImageUrl, existingAlbum.ImageUrl);
            Assert.AreEqual(existingAlbum, result);
        }


        [Test]
        public async Task UpdateAlbum_NonExistingId_ReturnsNull()
        {
            // Arrange
            var album = new Models.Album { Id = 1, Name = "Updated Album", Artist = "Updated Artist", ImageUrl = "updatedimage.jpg" };
            _dbContextMock.Setup(db => db.Albums.FindAsync(1)).ReturnsAsync((Models.Album)null);

            // Act
            var result = await _albumService.UpdateAlbum(1, album);

            // Assert
            _dbContextMock.Verify(db =>
                db.SaveChangesAsync(CancellationToken.None), Times.Never);
            Assert.IsNull(result);
        }

        [Test]
        public async Task DeleteAlbum_ExistingId_RemovesAlbumFromDbContext()
        {
            // Arrange
            var album = new Models.Album { Id = 1, Name = "Album 1", Artist = "Artist 1", ImageUrl = "image1.jpg" };
            _dbContextMock.Setup(db => db.Albums.FindAsync(1)).ReturnsAsync(album);

            // Act
            var result = await _albumService.DeleteAlbum(1);

            // Assert
            _dbContextMock.Verify(db =>
                db.Albums.Remove(album), Times.Once);
            _dbContextMock.Verify(db =>
                db.SaveChangesAsync(CancellationToken.None), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAlbum_NonExistingId_ReturnsFalse()
        {
            // Arrange
            _dbContextMock.Setup(db => db.Albums.FindAsync(1)).ReturnsAsync((Models.Album)null);

            // Act
            var result = await _albumService.DeleteAlbum(1);

            // Assert
            _dbContextMock.Verify(db =>
                db.Albums.Remove(It.IsAny<Models.Album>()), Times.Never);
            _dbContextMock.Verify(db =>
                db.SaveChangesAsync(CancellationToken.None), Times.Never);
            Assert.IsFalse(result);
        }

        private static DbSet<T> MockAsyncQueryable<T>(IQueryable<T> data)
            where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                .Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));
            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(data.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet.Object;
        }
    }
}
