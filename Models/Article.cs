using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Lista10_v2.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "To short name")]
        [Display(Name = "Article name")]
        [MaxLength(20, ErrorMessage = " To long name, do not exceed {0}")]
        public string Name { get; set; } 

        [Required]
        public double Price { get; set; }

        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpirationDate { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        
        public string ImagePath { get; set; }

        // modyfikacja
        public bool Promo { get; set; }

        public virtual Category Category { get; set; }
        [NotMapped]
        public virtual IFormFile FormFile { get; set; }


        public Article()
        {
        }

        public Article(int id, string name, double price, DateTime expirationDate, int categoryId, string imagePath)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.ExpirationDate = expirationDate;
            //this.Category = category;
            this.CategoryId = categoryId;
            this.ImagePath = imagePath;
        }

    }
}
