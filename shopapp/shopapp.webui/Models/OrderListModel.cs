using System;
using System.Collections.Generic;
using System.Linq;
using shopapp.entity;

namespace shopapp.webui.Models
{
    public class OrderListModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public EnumPaymentTypes PaymentTypes { get; set; }
        public EnumOrderState OrderState { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
        public double TotalPrice(){
            return OrderItems.Sum(i=>i.Price * i.Quantity);
        } 
    }
}