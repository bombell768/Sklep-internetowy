using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lista10_v2.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "To short name")]
        [Display(Name = "Article name")]
        [MaxLength(20, ErrorMessage = " To long name, do not exceed {0}")]
        public string Name { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public Category()
        {
        }

        public Category(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

    }
}
