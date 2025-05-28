using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public Task<Category?> GetById(Guid id)
        {
            return dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingcategory = await dbContext.Categories.FirstOrDefaultAsync(x=>x.Id == category.Id);
            if (existingcategory != null) 
            {
                dbContext.Entry(existingcategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return category;
            }
            return null;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
           var existingCategory=await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCategory != null)
            {
                dbContext.Categories.Remove(existingCategory);
                await dbContext.SaveChangesAsync();
                return existingCategory;
            }
            return null;
        }
    }
}
