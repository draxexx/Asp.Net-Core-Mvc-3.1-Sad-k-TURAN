using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    //localhost:500/home
    public class HomeController:Controller{

        private IProductService _productService;

        public HomeController(IProductService productService){
            this._productService = productService;
        }

        //localhost:500/home/index
        public IActionResult Index(){

            var productViewModel = new ProductListViewModel(){
                Products=_productService.GetHomePageProducts(),
            };
            return View(productViewModel);
        }
        //localhost:500/home/about
        public IActionResult About(){
            return View();
        }
        public IActionResult Contact(){
            return View();
        }
    }
}