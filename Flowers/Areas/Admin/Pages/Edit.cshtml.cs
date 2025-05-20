using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flowers.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Flowers.Domain.Entities;
using Flowers.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Flowers.Areas.Admin.Pages
{
    public class EditModel(IProductService productService, ICategoryService categoryService) : PageModel
    {
        [BindProperty]
        public FN fn { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await productService.GetProductByIdAsync(id);
            fn = response?.Data;

            if (fn == null)
            {
                return NotFound();
            }

            var categoryListData = await categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id",
            "GroupName");

           
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await productService.UpdateProductAsync(id, fn, Image);
            return RedirectToPage("./Index");
        }

    }
}