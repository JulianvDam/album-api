using System.Linq;
using Album.Api.Database;

namespace Album.Api.Database
{
    public static class DBInitializer
    {
        public static void Initialize(Database.albumContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Check if the Albums table is empty
            if (!context.Albums.Any())
            {
                // Add initial data to the Albums table
                var albums = new[]
                {
                    new Models.Album { Id = 1, Name = "Initialized:Revolver", Artist = "TheBeatles", ImageUrl = "https://m.media-amazon.com/images/I/91ffeWzPNpL._UF1000,1000_QL80_.jpg" },
                    new Models.Album { Id = 1, Name = "Initialized:RevolverDeluxeEdition", Artist = "TheBeatles", ImageUrl = "https://m.media-amazon.com/images/I/91ffeWzPNpL._UF1000,1000_QL80_.jpg" }
                };

                context.Albums.AddRange(albums);
                context.SaveChanges();
            }
        }
    }
}
