using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Flowers.Domain.Entities;
using Flowers.Services;
using Microsoft.AspNetCore.Authorization;

namespace Flowers.Areas.Admin.Pages
{
    [Authorize(Policy = "admin")]
    public class CreateModel(ICategoryService categoryService, IProductService productService) : PageModel
    {
     
            public async Task<IActionResult> OnGet()
            {


                var categoryListData = await categoryService.GetCategoryListAsync();
                ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id",
                "GroupName");
                return Page();


            }
            [BindProperty]
            public FN fn { get; set; } = default!;
            [BindProperty]
            public IFormFile? Image { get; set; }

            public async Task<IActionResult> OnPostAsync()
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                await productService.CreateProductAsync(fn, Image);
                return RedirectToPage("./Index");
            }
        }
    }
