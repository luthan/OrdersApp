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
    
    public partial class Order
    {
        //public Order()
        //{
        //    this.Items = new HashSet<Item>();
        //}
    
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string PoNumber { get; set; }
        [Required(ErrorMessage="Required")]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Vendor { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Notes { get; set; }
        public string UserName { get; set; }
    
        public ICollection<Item> Items { get; set; }
    }
}
