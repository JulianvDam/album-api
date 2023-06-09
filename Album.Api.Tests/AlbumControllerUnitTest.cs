using System.Collections.Generic;
using System.Threading.Tasks;
using Album.Api.Controllers;
using Album.Api.Models;
using Album.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Album.Api.Tests.Controllers
{
    public class AlbumControllerTests
    {
        private readonly Mock<IAlbumService> _albumServiceMock;
        private readonly AlbumController _controller;

        public AlbumControllerTests()
        {
            _albumServiceMock = new Mock<IAlbumService>();
            _controller = new AlbumController(_albumServiceMock.Object);
        }

        [Fact]
        public async Task GetAlbums_ReturnsListOfAlbums()
        {
            // Arrange
            var albums = new List<Album> { /* mock album data */ };
            _albumServiceMock.Setup(s => s.GetAllAlbums()).ReturnsAsync(albums);

            // Act
            var result = await _controller.GetAlbums();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAlbums = Assert.IsAssignableFrom<IEnumerable<Album>>(okResult.Value);
            Assert.Equal(albums.Count, returnedAlbums.Count);
        }

        [Fact]
        public async Task GetAlbum_ExistingId_ReturnsAlbum()
        {
            // Arrange
            var albumId = 1;
            var album = new Album { /* mock album data */ };
            _albumServiceMock.Setup(s => s.GetAlbumById(albumId)).ReturnsAsync(album);

            // Act
            var result = await _controller.GetAlbum(albumId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAlbum = Assert.IsType<Album>(okResult.Value);
            Assert.Equal(albumId, returnedAlbum.Id);
        }

        [Fact]
        public async Task GetAlbum_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var albumId = 1;
            _albumServiceMock.Setup(s => s.GetAlbumById(albumId)).ReturnsAsync(null as Album);

            // Act
            var result = await _controller.GetAlbum(albumId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task UpdateAlbum_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var albumId = 1;
            var album = new Album { Id = albumId, /* mock album data */ };
            _albumServiceMock.Setup(s => s.UpdateAlbum(album)).ReturnsAsync(album);

            // Act
            var result = await _controller.UpdateAlbum(albumId, album);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateAlbum_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var albumId = 1;
            var album = new Album { Id = albumId, /* mock album data */ };
            _albumServiceMock.Setup(s => s.UpdateAlbum(album)).ReturnsAsync(null as Album);

            // Act
            var result = await _controller.UpdateAlbum(albumId, album);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAlbum_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var albumId = 1;
            var album = new Album { Id = albumId, /* mock album data */ };
            _albumServiceMock.Setup(s => s.DeleteAlbum(albumId)).ReturnsAsync(album);

            // Act
            var result = await _controller.DeleteAlbum(albumId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAlbum_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var albumId = 1;
            _albumServiceMock.Setup(s => s.DeleteAlbum(albumId)).ReturnsAsync(null as Album);

            // Act
            var result = await _controller.DeleteAlbum(albumId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}