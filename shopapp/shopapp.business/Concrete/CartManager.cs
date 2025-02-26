using System.Collections.Generic;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.entity;

namespace shopapp.business.Concrete
{
    public class CartManager : ICartService
    {
        private ICartRepository _cartRepository;

        public CartManager(ICartRepository cartRepository)
        {
            this._cartRepository = cartRepository;
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);

            if(cart!=null){
                //eklenecek ürün sepette var mı? (güncelleme)

                //eklenecek ürün sepette var ve yeni kayıt oluştur (ekleme)

                var index = cart.CartItems.FindIndex(i=>i.ProductId==productId);
                //index 0'dan küçük ise aranan ürün yok demektir
                if(index<0){
                    cart.CartItems.Add(new CartItem(){
                        ProductId = productId,
                        Quantity = quantity,
                        CartId = cart.Id,
                    });
                }else{
                    cart.CartItems[index].Quantity += quantity;
                }
                _cartRepository.Update(cart);
            }
        }

        public void ClearCart(int cartId)
        {
            _cartRepository.ClearCart(cartId);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);
            if(cart!=null){
                _cartRepository.DeleteFromCart(cart.Id,productId);
            } 
        }

        public Cart GetCartByUserId(string userId)
        {
            return _cartRepository.GetByUserId(userId);
        }

        public void InitializeCart(string userId)
        {
            _cartRepository.Create(new Cart(){
                UserId = userId,
            });
        }
    }
}