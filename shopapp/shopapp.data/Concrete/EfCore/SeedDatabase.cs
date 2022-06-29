using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Concrete.EfCore;
using shopapp.entity;

namespace shopapp.data.EfCore
{
    public static class SeedDatabase{
        public static void Seed(){
            var context = new ShopContext();

            if(context.Database.GetPendingMigrations().Count() == 0){
                if(context.Categories.Count() == 0){
                    context.Categories.AddRange(Categories);
                }

                if(context.Products.Count() == 0){
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategories);
                }
            }
            context.SaveChanges();
        }

        private static Category[] Categories = {
            new Category(){Name="Telefon",Url="telefon"},
            new Category(){Name="Bilgisayar",Url="bilgisayar"},
            new Category(){Name="Elektronik",Url="elektronik"},
            new Category(){Name="Beyaz Eşya",Url="beyaz-esya"},
        };

        private static Product[] Products = {
            new Product(){Name="Samsung S5", Url="samsung-s5", Price=2000, ImageUrl="1.jpg", Description="İyi telefon", IsApproved=true},
            new Product(){Name="Samsung S6", Url="samsung-s6", Price=3000, ImageUrl="2.jpg", Description="harika telefon", IsApproved=true},
            new Product(){Name="Samsung S7", Url="samsung-s7", Price=4000, ImageUrl="3.jpg", Description="mükemmel telefon", IsApproved=true},
            new Product(){Name="Samsung S8", Url="samsung-s8", Price=5000, ImageUrl="4.jpg", Description="süper telefon", IsApproved=false},
            new Product(){Name="ASUS", Url="asus", Price=6000, ImageUrl="5.jpg", Description="İyi bilgisayar", IsApproved=true},
            new Product(){Name="Lenova", Url="lenova", Price=7000, ImageUrl="6.jpg", Description="harika bilgisayar", IsApproved=true},
        };

        private static ProductCategory[] ProductCategories={
            new ProductCategory(){Product=Products[0],Category=Categories[0]},
            new ProductCategory(){Product=Products[0],Category=Categories[2]},
            new ProductCategory(){Product=Products[1],Category=Categories[0]},
            new ProductCategory(){Product=Products[1],Category=Categories[2]},
            new ProductCategory(){Product=Products[2],Category=Categories[0]},
            new ProductCategory(){Product=Products[3],Category=Categories[0]},
            new ProductCategory(){Product=Products[4],Category=Categories[1]},
            new ProductCategory(){Product=Products[5],Category=Categories[1]},
            new ProductCategory(){Product=Products[5],Category=Categories[2]},
        };
    }
}