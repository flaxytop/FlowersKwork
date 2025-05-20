using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Flowers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Wlowers.API.Data
{

    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {

            // Uri проекта
            var uri = "https://localhost:7002/";
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //Выполнение миграций
            await context.Database.MigrateAsync();

            if (!context.Categories.Any() && !context.FN.Any())
            {
                var categories = new Category[]
            {
             new Category {GroupName="Букеты",
            NormalizedName="Композиция"},
            new Category {GroupName="Цветы в горшках",
            NormalizedName="Коренные цветы"},
            new Category {GroupName="Россыпь",
            NormalizedName="Розничные цветы"}
            };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();


               var _fn = new List<FN>
        {
            new FN {Name="Роза красная",
            Description="Цветок",
            Price = 15,
            Image= uri + "Images/001.jpg",
            Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("Розничные цветы"))},

             new FN {Name="Букет в конверте",
            Description="Подарочный набор",
            Price = 25,
            Image= uri + "Images/002.jpg",
            Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("Композиция"))},

            };

                await context.FN.AddRangeAsync(_fn);
                await context.SaveChangesAsync();

            }
        }
    }
}

