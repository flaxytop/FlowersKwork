
using Flowers.Domain.Entities;
using Flowers.Domain.Models;


namespace Flowers.Services
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
            new Category {Id=1, GroupName="Букеты",
            NormalizedName="Композиция"},
            new Category {Id=2, GroupName="Цветы в горшках",
            NormalizedName="Коренные цветы"},
            new Category {Id=3, GroupName="Россыпь",
            NormalizedName="Розничные цветы"}

            };
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }

       
    }
}
