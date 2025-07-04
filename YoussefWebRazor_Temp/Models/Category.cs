﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YoussefWebRazor_Temp.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Order value must be between 1 and 100")]
        public int DisplayOrder { get; set; }
    }
}
