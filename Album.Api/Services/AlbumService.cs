using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Album.Api.Models;
using Album.Api.Services;
using Swashbuckle.AspNetCore.Annotations;


namespace Album.Api.Services {
    public class AlbumService : IAlbumService
    {
        private readonly Database.albumContext _dbContext;

        public AlbumService(Database.albumContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Models.Album>> GetAllAlbums()
        {
            return await _dbContext.Albums.ToListAsync();
        }


        public async Task<Models.Album> GetAlbumById(int id)
        {
            return await _dbContext.Albums.FindAsync(id);
        }

        public async Task<Models.Album> CreateAlbum(Models.Album album)
        {
            _dbContext.Albums.Add(album);
            await _dbContext.SaveChangesAsync();
            return album;
        }

        public async Task<Models.Album> UpdateAlbum(int id, Models.Album album)
        {
            var existingAlbum = await _dbContext.Albums.FindAsync(id);
            if (existingAlbum == null)
            {
                return null;
            }

            existingAlbum.Name = album.Name;
            existingAlbum.Artist = album.Artist;
            existingAlbum.ImageUrl = album.ImageUrl;

            await _dbContext.SaveChangesAsync();
            return existingAlbum;
        }

        public async Task<bool> DeleteAlbum(int id)
        {
            var album = await _dbContext.Albums.FindAsync(id);
            if (album == null)
            {
                return false;
            }

            _dbContext.Albums.Remove(album);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}


namespace Album.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        // GET: api/Album
        [HttpGet]
        [SwaggerOperation("GetAllAlbums")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the list of albums", typeof(IEnumerable<Models.Album>))]
        public async Task<ActionResult<IEnumerable<Models.Album>>> GetAlbums()
        {
            var albums = await _albumService.GetAllAlbums();
            return Ok(albums);
        }

        // GET: api/Album/5
        [HttpGet("{id}")]
        [SwaggerOperation("GetAlbumById")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the album with the specified ID", typeof(Models.Album))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No album found with the specified ID")]
        public async Task<ActionResult<Models.Album>> GetAlbum(int id)
        {
            var album = await _albumService.GetAlbumById(id);

            if (album == null)
            {
                return NotFound();
            }

            return Ok(album);
        }

        // POST: api/Album
        [HttpPost]
        [SwaggerOperation("CreateAlbum")]
        [SwaggerResponse(StatusCodes.Status201Created, "Creates a new album", typeof(Models.Album))]
        public async Task<ActionResult<Models.Album>> CreateAlbum([FromBody] Models.Album album)
        {
            var createdAlbum = await _albumService.CreateAlbum(album);
            return CreatedAtAction(nameof(GetAlbum), new { id = createdAlbum.Id }, createdAlbum);
        }

        // PUT: api/Album/5
        [HttpPut("{id}")]
        [SwaggerOperation("UpdateAlbum")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Updates the album with the specified ID")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No album found with the specified ID")]
        public async Task<IActionResult> UpdateAlbum(int id, [FromBody] Models.Album album)
        {
            if (id != album.Id)
            {
                return BadRequest();
            }

            var updatedAlbum = await _albumService.UpdateAlbum(id, album);

            if (updatedAlbum == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Album/5
        [HttpDelete("{id}")]
        [SwaggerOperation("DeleteAlbum")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Deletes the album with the specified ID")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No album found with the specified ID")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var deletedAlbum = await _albumService.DeleteAlbum(id);

            if (deletedAlbum == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}