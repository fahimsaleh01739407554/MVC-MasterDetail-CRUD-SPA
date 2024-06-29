using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FFXM.Models
{
    public class SaleDetail
    {
        [Key] 
        public int SaleDetailId { get; set; }
        public int? SaleId { get; set; }
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public virtual SaleMaster SaleMasters { get; set; }
    }
}