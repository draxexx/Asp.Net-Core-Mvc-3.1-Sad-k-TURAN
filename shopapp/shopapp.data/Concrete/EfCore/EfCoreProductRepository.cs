using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        public Product GetByIdWithCategories(int id)
        {
            using(var context = new ShopContext()){
                return context.Products
                                .Where(i=>i.ProductId==id)
                                .Include(i=>i.ProductCategories)
                                .ThenInclude(i=>i.Category)
                                .FirstOrDefault();
            }
        }

        public int GetCountByCategory(string category)
        {
            using(var context = new ShopContext()){
                var products = context.Products.Where(i=>i.IsApproved).AsQueryable(); //filtreleme işlemi yapılacağı için

                if(!string.IsNullOrEmpty(category)){
                    products = products
                                .Include(i=>i.ProductCategories)
                                .ThenInclude(i=>i.Category) //ilgili kayıtın referanslarına ulaştığımız için where şartını sonra ekliyoruz
                                .Where(i=>i.ProductCategories.Any(a=>a.Category.Url.ToLower() == category.ToLower())); //kategorileri, parametreden gelen isim ile aynı ise o ürünü products içerisine atıyoruz
                }

                //buralarda istediğin kadar filtre ekleyebilirsin toList() yapmadan önce, fiyat, isim vb...
                return products.Count();
            }
        }

        public List<Product> GetHomePageProducts()
        {
            using(var context = new ShopContext()){
                return context.Products.Where(i=>i.IsApproved && i.IsHome==true).ToList();
            }
        }
        public Product GetProductDetails(string url)
        {
            using(var context = new ShopContext()){
                return context.Products
                                .Where(i=>i.Url==url) //id'si eşleşen product aldık
                                .Include(i=>i.ProductCategories) //product içerisinden ProductCategories'lere geldik
                                .ThenInclude(i=>i.Category) //ProductCategories içerisinden product'ın bağlı olduğu kategorileri aldık
                                .FirstOrDefault();
            }
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            using(var context = new ShopContext()){
                var products = context.Products.Where(i=>i.IsApproved).AsQueryable(); //filtreleme işlemi yapılacağı için

                if(!string.IsNullOrEmpty(name)){
                    products = products
                                .Include(i=>i.ProductCategories)
                                .ThenInclude(i=>i.Category) //ilgili kayıtın referanslarına ulaştığımız için where şartını sonra ekliyoruz
                                .Where(i=>i.ProductCategories.Any(a=>a.Category.Url.ToLower() == name.ToLower())); //kategorileri, parametreden gelen isim ile aynı ise o ürünü products içerisine atıyoruz
                }

                //buralarda istediğin kadar filtre ekleyebilirsin toList() yapmadan önce, fiyat, isim vb...
                return products.Skip((page-1)*pageSize).Take(pageSize).ToList();
            }
        }

        public List<Product> GetSearchResult(string search)
        {
            using(var context = new ShopContext()){
                var products = context.Products
                                        .Where(i=>i.IsApproved && (i.Name.ToLower().Contains(search.ToLower()) || i.Description.ToLower().Contains(search.ToLower())))
                                        .AsQueryable();

                return products.ToList();
            }
        }

        public List<Product> GetTop5Products()
        {
            throw new System.NotImplementedException();
        }

        public void Update(Product entity, int[] categoryIds)
        {
            using(var context = new ShopContext()){
                var product = context.Products
                                .Include(i=>i.ProductCategories)
                                .FirstOrDefault(i=>i.ProductId==entity.ProductId);

                if(product!=null){
                    product.Name = entity.Name;
                    product.Price = entity.Price;
                    product.Description = entity.Description;
                    product.Url = entity.Url;
                    product.ImageUrl = entity.ImageUrl;
                    product.IsApproved = entity.IsApproved;
                    product.IsHome = entity.IsHome;
                    product.ProductCategories = categoryIds.Select(catid=>new ProductCategory(){
                        ProductId = entity.ProductId,
                        CategoryId = catid,
                    }).ToList();

                    context.SaveChanges();
                }
            }
        }
    }
}