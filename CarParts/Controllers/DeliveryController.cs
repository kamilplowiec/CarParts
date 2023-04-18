using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using CarParts.Attributes;
using CarParts.Models.Delivery;
using CarParts.Models;
using CarParts.Enums;

namespace CarParts.Controllers
{  
    public partial class DeliveryController : Controller
    {
        [AuthRole(Roles = new Role[] { Role.Admin })]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DeliveriesJSON(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            IList<DeliveryModel> retList = new List<DeliveryModel>();
            int totalRecordCount = 0;

            using (var db = new CarPartsEntities())
            {
                totalRecordCount = db.Deliveries.Where(x => !x.Deleted).Count();

                var list = db.Deliveries.Where(x => !x.Deleted).OrderBy(x => x.Id).Skip(jtStartIndex).Take(jtPageSize).ToList();

                foreach (var li in list)
                {
                    var product = new DeliveryModel()
                    {
                        Id = li.Id,
                        Name = li.Name,
                        TrackUrl = li.TrackUrl,
                        PriceForWeight = li.PriceForWeight.ToString(),
                        InsurancePercentCost = li.InsurancePercentCost
                    };

                    retList.Add(product);
                }
            }
            var tableResult = new TableResult<DeliveryModel>() { Records = retList, TotalRecordCount = totalRecordCount };

            return Json(tableResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateDeliveryJSON(DeliveryModel model)
        {
            try
            {
                using (var db = new CarPartsEntities())
                {
                    var delivery = db.Deliveries.First(x => x.Id == model.Id);

                    if (delivery != null)
                    {
                        delivery.Name = model.Name;
                        delivery.TrackUrl = model.TrackUrl;
                        delivery.PriceForWeight = decimal.Parse(model.PriceForWeight);
                        delivery.InsurancePercentCost = model.InsurancePercentCost;

                        db.SaveChanges();
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "Nie znaleziono dostawy" });
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
        public JsonResult DeleteDeliveryJSON(int id)
        {
            try 
            {
                using (var db = new CarPartsEntities())
                {
                    var delivery = db.Deliveries.First(x => x.Id == id);

                    if (delivery != null)
                    {
                        delivery.Deleted = true;

                        db.SaveChanges();
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "Nie znaleziono dostawy" });
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
        public JsonResult CreateDeliveryJSON(DeliveryModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (CarPartsEntities db = new CarPartsEntities())
                    {
                        db.Deliveries.Add(
                            new Deliveries()
                            {
                                Name = model.Name,
                                TrackUrl = model.TrackUrl,
                                PriceForWeight = decimal.Parse(model.PriceForWeight),
                                InsurancePercentCost = model.InsurancePercentCost
                            });
                        db.SaveChanges();
                    }
                }
                else
                {
                    var message = string.Join("<br>", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return Json(new { Result = "ERROR", Message = message });
                }

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}
