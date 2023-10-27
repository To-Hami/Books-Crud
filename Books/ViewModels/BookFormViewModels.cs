using Books.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Books.ViewModels
{
    public class BookFormViewModels
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string Author { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Display(Name ="Select Category")]
        public byte CategoryId { get; set; }
      
        public IEnumerable<Category> Categories { get; set;}
    }
}