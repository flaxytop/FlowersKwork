using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Flowers.Data;
using Flowers.Entities;
using Flowers.Domain.Entities;
using Flowers.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Flowers.Areas.Admin.Pages
{
    public class DetailsModel(IProductService productService, ICategoryService categoryService) : PageModel
    {
        [BindProperty]
        public FN fn { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await productService.GetProductByIdAsync(id);
            fn = response.Data;

            if (fn == null)
            {
                return NotFound();
            }

            // Загружаем категории
            var categoryListData = await categoryService.GetCategoryListAsync();
            ViewData["GroupName"] = new SelectList(categoryListData.Data, "Id",
            "GroupName");

            return Page();
        }



        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            return Page();
        }
    }
}
