using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Extensions;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{   
    [Authorize(Roles="admin")] //admin controller ulaşmak istenildiğinde yetkilendirilmiş kullanıcı olması gerekir
    //localhost:500/home
    public class AdminController:Controller{
        private IProductService _productService;
        private ICategoryService _categoryService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public AdminController(IProductService productService, ICategoryService categoryService, 
        RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        public async Task<IActionResult> UserEdit(string id){
            var user = await _userManager.FindByIdAsync(id);
            if(user!=null){
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i=>i.Name);

                ViewBag.Roles = roles;
                return View(new UserDetailsModel(){
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles,
                });
            }
            return Redirect("~/admin/user/list");
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles){
            if(ModelState.IsValid){
                var user = await _userManager.FindByIdAsync(model.UserId);
                if(user!=null){
                    user.FirstName = model.UserName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result  = await _userManager.UpdateAsync(user);

                    if(result.Succeeded){
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] {};
                        await _userManager.AddToRolesAsync(user,selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user,userRoles.Except(selectedRoles).ToArray<string>());

                        return Redirect("/admin/user/list");
                    }
                }
                return Redirect("/admin/user/list");
            }
            return View(model);
        }

        public IActionResult UserList(){
            return View(_userManager.Users);
        }

        public async Task<IActionResult> RoleEdit(string id){
            var role = await _roleManager.FindByIdAsync(id);
            var members = new List<User>();
            var nonmembers = new List<User>();

            foreach(var user in _userManager.Users){
                var list = await _userManager.IsInRoleAsync(user,role.Name) ? members : nonmembers;
                list.Add(user);
            }
            var model = new RoleDetails(){
                Role = role,
                Members = members,
                NonMembers = nonmembers,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model){
            if(ModelState.IsValid){
                foreach(var userId in model.IdsToAdd ?? new string[]{}){
                    var user = await _userManager.FindByIdAsync(userId);
                    if(user!=null){
                        var result = await _userManager.AddToRoleAsync(user,model.RoleName);
                        if(!result.Succeeded){
                            foreach(var error in result.Errors){
                                ModelState.AddModelError("",error.Description);
                            }
                        }
                    }
                }

                foreach(var userId in model.IdsToDelete ?? new string[]{}){
                    var user = await _userManager.FindByIdAsync(userId);
                    if(user!=null){
                        var result = await _userManager.RemoveFromRoleAsync(user,model.RoleName);
                        if(!result.Succeeded){
                            foreach(var error in result.Errors){
                                ModelState.AddModelError("",error.Description);
                            }
                        }
                    }
                }
            }
            return Redirect("/admin/role/"+model.RoleId);
        }

        public IActionResult RoleList(){
            return View(_roleManager.Roles);
        }

        public IActionResult RoleCreate(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model){

            if(ModelState.IsValid){
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if(result.Succeeded){
                    return RedirectToAction("RoleList");
                }else{
                    foreach(var error in result.Errors){ //hata mesajlarını gösterir
                        ModelState.AddModelError("",error.Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult ProductList(){
            return View(new ProductListViewModel(){
                Products = _productService.GetAll(),
            });
        }

        public IActionResult CategoryList(){
            return View(new CategoryListViewModel(){
                Categories = _categoryService.GetAll(),
            });
        }

        [HttpGet]
        public IActionResult ProductCreate(){
            return View();
        }

        [HttpPost]
        public IActionResult ProductCreate(ProductModel model)
        {

            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Url = model.Url,
                    Price = model.Price,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl,
                };
                if (_productService.Create(entity))
                {
                    TempData.Put("message",new AlertMessage(){
                    Title = "Ürün Eklendi",
                    Message = $"{entity.Name} isimli ürün eklendi.",
                    AlertType = "success",
                    });

                    return RedirectToAction("ProductList");
                }
                TempData.Put("message",new AlertMessage(){
                    Title = "Ürün Eklenemedi",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger",
                    });
                return View(model);
            }
            return View(model);
        }

        public IActionResult CategoryCreate(){
            return View();
        }

        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model){

            if(ModelState.IsValid){
                var entity = new Category(){
                Name = model.Name,
                Url = model.Url,
            };
                _categoryService.Create(entity);

                var msg = new AlertMessage(){
                    Message = $"{entity.Name} isimli kategori eklendi.",
                    AlertType = "success",
                };

                TempData["message"] = JsonConvert.SerializeObject(msg);

                return RedirectToAction("CategoryList");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ProductEdit(int? id){

            if(id==null){
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);

            if(entity == null){
                return NotFound();
            }

            var model = new ProductModel(){
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Price = entity.Price,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(i=>i.Category).ToList(),
            };

            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel model, int[] categoryIds, IFormFile file){

            if(ModelState.IsValid){
                var entity = _productService.GetById(model.ProductId);

                if(entity == null){
                    return NotFound();
                }

                entity.Name = model.Name;
                entity.Price = model.Price;
                entity.Url = model.Url;
                entity.Description = model.Description;
                entity.IsHome = model.IsHome;
                entity.IsApproved = model.IsApproved;

                if(file!=null){
                    var extention = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extention}"); //random ad ve dosya uzantısı
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\img",randomName);

                    using(var stream = new FileStream(path,FileMode.Create)){
                        await file.CopyToAsync(stream);
                    }
                }

                if(_productService.Update(entity,categoryIds)){
                    TempData.Put("message",new AlertMessage(){
                    Title = "Ürün Güncellendi",
                    Message = $"{entity.Name} isimli ürün güncellendi.",
                    AlertType = "success",
                });
                    return RedirectToAction("ProductList");
                }
                TempData.Put("message",new AlertMessage(){
                    Title = "Ürün Güncellenemedi",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger",
                    });
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        [HttpGet]
        public IActionResult CategoryEdit(int? id){

            if(id==null){
                return NotFound();
            }

            var entity = _categoryService.GetByIdWithProducts((int)id);

            if(entity == null){
                return NotFound();
            }

            var model = new CategoryModel(){
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(p=>p.Product).ToList(),
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model){

            if(ModelState.IsValid){
                var entity = _categoryService.GetById(model.CategoryId);

                if(entity == null){
                    return NotFound();
                }

                entity.Name = model.Name;
                entity.Url = model.Url;

                _categoryService.Update(entity);

                TempData.Put("message",new AlertMessage(){
                    Title = "Ürün Güncellendi",
                    Message = $"{entity.Name} isimli kategori güncellendi.",
                    AlertType = "success",
                    });

                return RedirectToAction("CategoryList");
            }
            if (model.Products==null)
            {
                model.Products=new List<Product>();
            }
            return View(model);
        }

        public IActionResult DeleteProduct(int productId){
            var entity = _productService.GetById(productId);
            
            if(entity!=null){
                _productService.Delete(entity);
            }

            TempData.Put("message",new AlertMessage(){
                    Title = "Ürün Silindi",
                    Message = $"{entity.Name} isimli ürün silindi.",
                    AlertType = "danger",
                    });

            return RedirectToAction("ProductList");
        }

        public IActionResult DeleteCategory(int categoryId){
            var entity = _categoryService.GetById(categoryId);
            
            if(entity!=null){
                _categoryService.Delete(entity);
            }

            TempData.Put("message",new AlertMessage(){
                    Title = "Kategori Silindi",
                    Message = $"{entity.Name} isimli kategori silindi.",
                    AlertType = "danger",
                    });

            return RedirectToAction("CategoryList");
        }
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId){
            _categoryService.DeleteFromCategory(productId,categoryId);
            return Redirect("/admin/categories/"+categoryId);
        }
    
    }
}