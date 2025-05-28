using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }



        [HttpPost]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            // Map DTO to Domain Model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            await categoryRepository.CreateAsync(category);
            // Domain model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }

        [HttpGet]

        //GET: https://localhost:7165/api/Categories
        public async Task<IActionResult> GetCategories()
        {
            var categories = await categoryRepository.GetAllAsync();
            var response = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                UrlHandle = c.UrlHandle
            });
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingcategory=await categoryRepository.GetById(id);
            if (existingcategory == null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = existingcategory.Id,
                Name = existingcategory.Name,
                UrlHandle = existingcategory.UrlHandle
            };
            return Ok(response);    

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto request)
        {
            //Convert DTO to DomainModel
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            category=await categoryRepository.UpdateAsync(category);
            if (category == null)
            {
                return NotFound();
            }
            //convert DOmain to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            }; 
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
           var category= await categoryRepository.DeleteAsync(id);
            if(category == null)
            {
                return NotFound();
            }
            // Convert Domain to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }
    }
}
