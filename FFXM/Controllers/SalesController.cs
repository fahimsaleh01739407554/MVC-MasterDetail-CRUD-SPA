using FFXM.Models;
using FFXM.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FFXM.Controllers
{
    public class SalesController : Controller
    {
        FCodeDB db = new FCodeDB();
        [HttpGet]
        public ActionResult Single(int? id)
        {
            var item = new List<SelectListItem>
            {
                new SelectListItem {Value="TV", Text="TV"},
                new SelectListItem {Value="Mobile", Text="Mobile"}
            };
            var oSale = db.SaleMasters
                .Where(x => x.SaleId == id)
                .Select(oSM => new VmSale 
                { 
                    SaleId = oSM.SaleId,
                    CustomerName = oSM.CustomerName,
                    Gender = oSM.Gender,
                    CreateDate = oSM.CreateDate,
                    Photo = oSM.Photo,
                    SaleDetails = oSM.SaleDetails
                    .Select(oSD => new VmSale.VmSaleDetail
                    {
                        SaleId = oSD.SaleId,
                        SaleDetailId = oSD.SaleDetailId,
                        ProductName = oSD.ProductName,
                        Price = oSD.Price
                    })
                    .ToList()
                })
                .FirstOrDefault();
            ViewData["List"] = db.SaleMasters.ToList();
            ViewData["item"] = item;
            return View(oSale??new VmSale());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Single(VmSale model, HttpPostedFileBase img, FormCollection form)
        {
            var oSaleMaster = db.SaleMasters
                .Include("SaleDetails")
                .FirstOrDefault(x => x.SaleId == model.SaleId);

            string filename = img?.FileName;
            if(img != null)
            {
                string path = Path.Combine(Server.MapPath("~/Picture"), filename);
                img.SaveAs(path);
            }
            
            if(oSaleMaster == null)
            {
                oSaleMaster = new SaleMaster
                {
                    CustomerName = model.CustomerName,
                    Gender = model.Gender,
                    CreateDate = model.CreateDate,
                    ProductType = form["Select"],
                    Photo = "/Picture/" + filename
                };
                db.SaleMasters.Add(oSaleMaster);
                ViewBag.Message = "Insert";

                var listSaleDetail = model.ProductName
                    .Where((name, i)=> !string.IsNullOrEmpty(name))
                    .Select((name,i)=> new SaleDetail
                    {
                        ProductName = name,
                        Price = model.Price[i],
                        SaleId = oSaleMaster.SaleId
                    })
                    .ToList();
                if(listSaleDetail.Any())
                {
                    db.SaleDetails.AddRange(listSaleDetail);
                    db.SaveChanges();
                }
            }
            else
            {
                oSaleMaster.CustomerName = model.CustomerName;
                oSaleMaster.Gender = model.Gender;
                oSaleMaster.CreateDate = model.CreateDate;
                oSaleMaster.ProductType = form["Select"];
                oSaleMaster.Photo = (filename != null) ? "/Picture/" + filename : oSaleMaster.Photo;
                ViewBag.Message = "Update";

                db.SaleDetails.RemoveRange(oSaleMaster.SaleDetails);

                var listSaleDetail = model.ProductName
                    .Where((name, i) => !string.IsNullOrEmpty(name))
                    .Select((name, i) => new SaleDetail
                    {
                        ProductName = name,
                        Price = model.Price[i],
                        SaleId = oSaleMaster.SaleId
                    })
                    .ToList();
                if (listSaleDetail.Any())
                {
                    db.SaleDetails.AddRange(listSaleDetail);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Single");
        }
        [HttpGet]
        // GET: Items/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SaleDetail saleDetail)
        {
         
            return PartialView();
        }
        public ActionResult SingleDelete(int id)
        {
            var oSaleMaster = db.SaleMasters
                .Include("SaleDetails")
                .FirstOrDefault(x => x.SaleId == id);
            if (oSaleMaster != null)
            {
                db.SaleDetails.RemoveRange(oSaleMaster.SaleDetails);
                db.SaleMasters.Remove(oSaleMaster); 
                db.SaveChanges();
            }
            return RedirectToAction("Single");
        }
    }
}