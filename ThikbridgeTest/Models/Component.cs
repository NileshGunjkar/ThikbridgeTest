using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThikbridgeTest.Models
{
    public class Component
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required...")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required...")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required...")]
        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:#.#}")]
        [RegularExpression(@"^\d+(\.\d{1,2})?", ErrorMessage = "Please enter Valid Price..")]
        public decimal Price { get; set; }
        public string Picture { get; set; }
    }
    public class ComponentViewModel
    {
        public Component componentModel { get; set; }
        public List<Component> componentModelList { get; set; }
    }
}