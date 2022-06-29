using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetByIdWithProduct(int categoryId);
        void DeleteFromCategory(int productId,int categortId);
    }
}