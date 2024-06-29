using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FFXM.Models
{
    public class SaleMaster
    {
        [Key] 
        public int SaleId { get; set; }
        public string CustomerName { get; set; }
        public string Gender { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Photo { get; set; }
        public string ProductType { get; set; }
        public virtual IList<SaleDetail> SaleDetails { get; set; }
    }
}