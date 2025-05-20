using Flowers.Domain.Entities;
using Flowers.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Flowers.Services
{
    
        public class MemoryProductService: IProductService
        {
            List<FN> _fn;
            List<Category> _categories;
            IConfiguration _config;

            public MemoryProductService(ICategoryService categoryService, [FromServices] IConfiguration config)
            {
                _config = config;
                _categories = categoryService.GetCategoryListAsync()
                    .Result
                    .Data;

                SetupData();
            }

        /// <summary>
        /// Инициализация списков
        /// </summary>
        public void SetupData()
        {

            _fn = new List<FN>
        {
            new FN {Id = 1, Name="Роза красная",
            Description="Цветок",
            Image="/Images/001.jpg",
            CategoryId = _categories.Find(c=>c.NormalizedName.Equals("Розничные цветы")).Id},

            new FN {Id = 2, Name="Букет в конверте",
            Description="Подарочный набор",
            Image="/Images/002.jpg",
            CategoryId = _categories.Find(c=>c.NormalizedName.Equals("Композиция")).Id}

            };

        }
        Task<ResponseData<ListModel<FN>>> IProductService.GetProductListAsync(string? categoryNormalizedName, int pageNo=1)
        {

                // Создать объект результата
                var result = new ResponseData<ListModel<FN>>();

                // Id категории для фильрации
                int? categoryId = null;

                // если требуется фильтрация, то найти Id категории
                // с заданным categoryNormalizedName
                if (categoryNormalizedName != null)
                    categoryId = _categories
                    .Find(c =>
                    c.NormalizedName.Equals(categoryNormalizedName))
                    ?.Id;

            // Выбрать объекты, отфильтрованные по Id категории,
            // если этот Id имеется
            var data = _fn.Where(d => categoryNormalizedName == null || d.CategoryId == categoryId).ToList();


            // получить размер страницы из конфигурации
            int pageSize = _config.GetSection("ItemsPerPage").Get<int>();


                // получить общее количество страниц
                int totalPages = (int)Math.Ceiling(data.Count / (double)pageSize);

                // получить данные страницы
                var listData = new ListModel<FN>()
                {
                    Items = data.Skip((pageNo - 1) *
                pageSize).Take(pageSize).ToList(),
                    CurrentPage = pageNo,
                    TotalPages = totalPages
                };

                // поместить ранные в объект результата
                result.Data = listData;

                // Если список пустой
                if (data.Count == 0)
                {
                    result.Success = false;
                    result.ErrorMessage = "Нет объектов в выбраннной категории";
                }
                // Вернуть результат
                return Task.FromResult(result);

            }

        public Task<ResponseData<FN>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

       

        public Task<ResponseData<FN>> CreateProductAsync(FN product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task GetProductListAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(FN fn, int id)
        {
            throw new NotImplementedException();
        }

        Task<ResponseData<FN>> IProductService.UpdateProductAsync(int id, FN product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
    }
