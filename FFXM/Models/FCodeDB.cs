using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FFXM.Models
{
    public class FCodeDB : DbContext
    {
        public DbSet<SaleMaster> SaleMasters { get; set;}
        public DbSet<SaleDetail> SaleDetails { get; set;}
    }
}