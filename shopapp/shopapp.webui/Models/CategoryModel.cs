using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using shopapp.entity;

namespace shopapp.webui.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage="Name zorunlu bir alan.")]
        [StringLength(10,MinimumLength=5,ErrorMessage="Kategori ismi 5-10 karakter aralığında olmalıdır.")]
        public string Name { get; set; }

        [Required(ErrorMessage="Url zorunlu bir alan.")]
        public string Url { get; set; }
        public List<Product> Products { get; set; }
    }
}