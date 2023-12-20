using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Models
{

    public class Product
    {

        public Product()
        {
            TempSqFt = 1;
        }

        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ShortDesc { get; set; }

        public double Price { get; set; }
        public string Image { get; set; }
        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name="Application Type")]
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public virtual ApplicationType ApplicationType { get; set; }

        [NotMapped]
        [Range(1,10000, ErrorMessage = "Sqft must be greater than 0")]
        public int TempSqFt { get; set; }
    }
}
