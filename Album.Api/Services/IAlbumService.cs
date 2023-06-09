namespace Album.Api.Services;
public interface IAlbumService
{
    Task<List<Models.Album>> GetAllAlbums();
    Task<Models.Album> GetAlbumById(int id);
    Task<Models.Album> CreateAlbum(Models.Album album);
    Task<Models.Album> UpdateAlbum(int id, Models.Album album);
    Task<bool> DeleteAlbum(int id);
}
