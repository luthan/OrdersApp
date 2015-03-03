//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrdersWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Item
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ItemDescription { get; set; }
        [Required(ErrorMessage = "Required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Required")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CategoryId { get; set; }
        public int BillingCategoryId { get; set; }
        public int OrderId { get; set; }
        [Display(Name = "Billing Category")]
        public virtual BillingCategory BillingCategory { get; set; }
        public virtual Category Category { get; set; }
        public virtual Order Order { get; set; }
    }
}
