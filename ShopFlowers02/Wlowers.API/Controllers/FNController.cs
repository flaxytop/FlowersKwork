using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wlowers.API.Data;
using Flowers.Domain.Entities;
using Flowers.Domain.Models;

namespace Wlowers.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FNController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public FNController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/FN
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<FN>>>> GetFN(
        string? category,
        int pageNo = 1,
        int pageSize = 3)
        {
            // Создать объект результата
            var result = new ResponseData<ListModel<FN>>();
            // Фильтрация по категории загрузка данных категории
            var data = _context.FN
            .Include(d => d.Category)
            .Where(d => String.IsNullOrEmpty(category)
            || d.Category.NormalizedName.Equals(category));
            // Подсчет общего количества страниц
            int totalPages = (int)Math.Ceiling(data.Count() / (double)pageSize);

            if (pageNo > totalPages)
                pageNo = totalPages;
            // Создание объекта ProductListModel с нужной страницей данных
            var listData = new ListModel<FN>()
            {
                Items = await data
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };
            // поместить данные в объект результата
            result.Data = listData;
            // Если список пустой
            if (data.Count() == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории";
            }
            return result;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> SaveImage(int id, IFormFile image)
        {
            // Найти объект по Id
            var dish = await _context.FN.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            // Путь к папке wwwroot/Images
            var imagesPath = Path.Combine(_env.WebRootPath, "Images");
            // получить случайное имя файла
            var randomName = Path.GetRandomFileName();
            // получить расширение в исходном файле
            var extension = Path.GetExtension(image.FileName);
            // задать в новом имени расширение как в исходном файле
            var fileName = Path.ChangeExtension(randomName, extension);
            // полный путь к файлу
            var filePath = Path.Combine(imagesPath, fileName);
            // создать файл и открыть поток для записи
            using var stream = System.IO.File.OpenWrite(filePath);
            // скопировать файл в поток
            await image.CopyToAsync(stream);
            // получить Url хоста
            var host = "https://" + Request.Host;
            // Url файла изображения
            var url = $"{host}/Images/{fileName}";
            // Сохранить url файла в объекте
            dish.Image = url;
            await _context.SaveChangesAsync();
            return Ok();
        }


            // GET: api/FN/5
            [HttpGet("{id}")]
        public async Task<ActionResult<FN>> GetFN(int id)
        {
            var fN = await _context.FN.FindAsync(id);

            if (fN == null)
            {
                return NotFound();
            }

            return fN;
        }

        // PUT: api/FN/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFN(int id, [FromBody] FN fN)
        {
            if (id != fN.Id)
            {
                return BadRequest();
            }

            var existingFN = await _context.FN.FindAsync(id);
            if (existingFN == null)
            {
                return NotFound();
            }

            // Сохранение обновленных данных
            existingFN.Name = fN.Name;
            existingFN.Description = fN.Description;
            existingFN.Price = fN.Price;
            existingFN.CategoryId = fN.CategoryId;

            if (!string.IsNullOrEmpty(fN.Image))
            {
                existingFN.Image = fN.Image; // Сохранение фото
            }

            _context.Entry(existingFN).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FNExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingFN);
        }

        // POST: api/FN
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FN>> PostFN(FN fN)
        {
            _context.FN.Add(fN);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFN", new { id = fN.Id }, fN);
        }

        // DELETE: api/FN/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFN(int id)
        {
            var fN = await _context.FN.FindAsync(id);
            if (fN == null)
            {
                return NotFound();
            }

            _context.FN.Remove(fN);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FNExists(int id)
        {
            return _context.FN.Any(e => e.Id == id);
        }

        [HttpPost("{id}/upload")]
        public async Task<IActionResult> UploadImage(int id, IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("Файл не был загружен.");
            }

            var filePath = Path.Combine("wwwroot/Images", image.FileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            var fn = await _context.FN.FindAsync(id);
            if (fn == null)
            {
                return NotFound();
            }

            // Формируем полный URL изображения
            var host = $"{Request.Scheme}://{Request.Host}";
            fn.Image = $"{host}/Images/{image.FileName}"; // Обновляем поле `Image`

            _context.FN.Update(fn);
            await _context.SaveChangesAsync(); // Сохранение изменений

            return Ok(new { Image = fn.Image });
        }



    }
}
