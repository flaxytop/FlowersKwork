using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Flowers.Data;
using Flowers.Domain.Entities;
using Wlowers.API.Data;
using Microsoft.EntityFrameworkCore.Internal;
using Flowers.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Flowers.Areas.Admin.Pages
{
    public class DeleteModel(IProductService productService, ICategoryService categoryService) : PageModel
    {
        [BindProperty]
        public FN fn { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await productService.GetProductByIdAsync(id);
            fn = response.Data;
            if (fn == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await productService.DeleteProductAsync(fn, id);
            return RedirectToPage("./Index");
        }
    }
}

