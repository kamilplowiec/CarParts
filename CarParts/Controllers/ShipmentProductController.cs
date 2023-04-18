using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using CarParts.Models;
using System.Linq.Expressions;
using CarParts.Attributes;
using CarParts.Models.Product;
using CarParts.Enums;

namespace CarParts.Controllers
{  
    public partial class ShipmentProductController : Controller
    {
        [AuthRole(Roles = new Role[] { Role.Admin, Role.Warehouseman, Role.Seller })]
        public ActionResult Index(int shipmentId)
        {
            return View(shipmentId);
        }

        [HttpPost]
        public JsonResult ShipmentProductsJSON(int shipmentId, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            IList<ShipmentProductModel> retList = new List<ShipmentProductModel>();
            int totalRecordCount = 0;

            using (var db = new CarPartsEntities())
            {
                totalRecordCount = db.ShipmentProducts.Where(x => !x.Deleted && x.ShipmentId == shipmentId).Count();

                var list = db.ShipmentProducts.Where(x => !x.Deleted && x.ShipmentId == shipmentId).OrderBy(x => x.Id).Skip(jtStartIndex).Take(jtPageSize).ToList();

                foreach (var li in list)
                {
                    var product = new ShipmentProductModel()
                    {
                        Id = li.Id,
                        Name = li.Products.Name,
                        Details = li.Products.Details,
                        Price = li.Products.Price.ToString(),
                        QuantityInWarehouse = li.Products.QuantityInWarehouse,
                        Count = li.Count,
                        ProductId = li.ProductId,
                        ShipmentId = li.ShipmentId,
                        InShipment = li.InShipment
                    };

                    retList.Add(product);
                }
            }
            var tableResult = new TableResult<ShipmentProductModel>() { Records = retList, TotalRecordCount = totalRecordCount };

            return Json(tableResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductPackedInShipmentJSON(int id, bool inShipment)
        {
            try
            {
                using (var db = new CarPartsEntities())
                {
                    var shipmentProduct = db.ShipmentProducts.First(x => x.Id == id);

                    if (shipmentProduct != null)
                    {
                        if (inShipment && shipmentProduct.Products.QuantityInWarehouse < shipmentProduct.Count)
                        {
                            return Json(new { Result = "ERROR", Message = "Zbyt mało towaru na magazynie" });
                        }

                        if (inShipment)
                        {
                            shipmentProduct.Products.QuantityInWarehouse -= shipmentProduct.Count;
                        }
                        else
                        {
                            shipmentProduct.Products.QuantityInWarehouse += shipmentProduct.Count;
                        }

                        shipmentProduct.InShipment = inShipment;

                        int allProducts = shipmentProduct.Shipments.ShipmentProducts.Count(x => !x.Deleted);
                        int packedProducts = shipmentProduct.Shipments.ShipmentProducts.Count(x => !x.Deleted && x.InShipment);

                        db.SaveChanges();

                        return Json(new { Result = "OK", Data = new { All = allProducts, Packed = packedProducts } });
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "Nie znaleziono produktu" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetProductsInShipmentJSON(int shipmentId)
        {
            try
            {
                using (var db = new CarPartsEntities())
                {
                    var allProducts = db.Shipments.FirstOrDefault(x => x.Id == shipmentId).ShipmentProducts.Count(x => !x.Deleted);
                    var packedProducts = db.Shipments.FirstOrDefault(x => x.Id == shipmentId).ShipmentProducts.Count(x => !x.Deleted && x.InShipment);

                    var price = db.Shipments.FirstOrDefault(x => x.Id == shipmentId).ShipmentProducts.Where(x => !x.Deleted).Sum(x => x.Count * x.Products.Price);

                    return Json(new { Result = "OK", Data = new { All = allProducts, Packed = packedProducts, Price = price } });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateShipmentProductJSON(ShipmentProductModel model)
        {
            try
            {
                using (var db = new CarPartsEntities())
                {
                    var shipmentProduct = db.ShipmentProducts.First(x => x.Id == model.Id);

                    if (shipmentProduct != null)
                    {
                        shipmentProduct.Count = model.Count;

                        db.SaveChanges();
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "Nie znaleziono produktu" });
                    }
                }

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteShipmentProductJSON(int id)
        {
            try 
            {
                using (var db = new CarPartsEntities())
                {
                    var shipmentProduct = db.ShipmentProducts.First(x => x.Id == id);

                    if (shipmentProduct != null)
                    {
                        shipmentProduct.Deleted = true;

                        db.SaveChanges();
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "Nie znaleziono produktu w paczce" });
                    }
                }

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }        

        [HttpPost]
        public JsonResult CreateShipmentProductJSON(ShipmentProductModel model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    using (CarPartsEntities db = new CarPartsEntities())
                    {
                        db.ShipmentProducts.Add(
                            new ShipmentProducts()
                            {
                                ProductId = model.ProductId,
                                ShipmentId = model.ShipmentId,
                                Count = model.Count
                            });
                        db.SaveChanges();
                    }
                //}
                //else
                //{
                //    var message = string.Join("<br>", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                //    return Json(new { Result = "ERROR", Message = message });
                //}

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}
