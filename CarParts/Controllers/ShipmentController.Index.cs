using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using CarParts.Models;
using System.Linq.Expressions;
using CarParts.Attributes;
using CarParts.Models.Product;
using CarParts.Models.Shipment;
using CarParts.Models.Customer;
using CarParts.Enums;
using System.Data.Entity;
using CarParts.Helpers;

namespace CarParts.Controllers
{
    public partial class ShipmentController : Controller
    {
        [AuthRole(Roles = new Role[] { Role.Admin, Role.Warehouseman, Role.Seller })]
        public ActionResult Index()
        {
            using (var db = new CarPartsEntities())
            {
                //todo: warunek dla usunietego klienta
                ViewBag.CustomerList = db.Customers.Where(x => !x.Deleted).Select(x => new SelectListItem() { Text = string.IsNullOrEmpty(x.CompanyName) ? x.Name + " " + x.Surname : x.CompanyName, Value = x.Id.ToString() }).ToList();

                int [] allowedStatuses = Helpers.CurrentUser.AllowedShipmentStatuses().ToArray();
                ViewBag.ShipmentStatusList = db.ShipmentStatuses.Where(x => !x.Deleted && allowedStatuses.Any(s => s == x.Id)).Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
            }
            return View();
        }

        [HttpPost]
        public JsonResult ShipmentsJSON(ShipmentFilterModel filterModel, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            IList<ShipmentModel> retList = new List<ShipmentModel>();
            int totalRecordCount = 0;

            using (var db = new CarPartsEntities())
            {
                var list = GetShipmentsFiltered(filterModel, jtSorting, jtStartIndex, jtPageSize, out totalRecordCount);

                foreach (var li in list)
                {
                    var Shipment = new ShipmentModel()
                    {
                        Id = li.Id,
                        CreateDate = li.CreateDate,
                        DeliveryCost = li.DeliveryCost.HasValue ? li.DeliveryCost.Value.ToString() : string.Empty,
                        DeliveryDate = li.DeliveryDate,
                        ShipmentStatus = li.ShipmentStatusId,
                        Insurance = li.Insurance,
                        Price = li.Price.HasValue ? li.Price.Value.ToString() : string.Empty,
                        ReceiptDate = li.ReceiptDate,
                        ShippingNumber = li.ShippingNumber,
                        CustomerId = li.CustomerId,
                        //ShipmentTrackingLink = li.DeliveryId.HasValue ? li.Deliveries.TrackUrl + li.ShippingNumber : ""
                    };

                    retList.Add(Shipment);
                }
            }
            var tableResult = new TableResult<ShipmentModel>() { Records = retList, TotalRecordCount = totalRecordCount };

            return Json(tableResult, JsonRequestBehavior.AllowGet);
        }

        private List<Shipments> GetShipmentsFiltered(ShipmentFilterModel filterModel, string sortOrder, int start, int length, out int totalRecordCount)
        {
            int[] allowedStatuses = Helpers.CurrentUser.AllowedShipmentStatuses().ToArray();

            using (var db = new CarPartsEntities())
            {
                var result = db.Shipments.Where(x => !x.Deleted &&
                    (string.IsNullOrEmpty(filterModel.FilterShippingNumber) || x.ShippingNumber.Contains(filterModel.FilterShippingNumber))
                    && (string.IsNullOrEmpty(filterModel.FilterCustomer) || (x.Customers.Name + " " + x.Customers.Surname + " " + x.Customers.CompanyName).Contains(filterModel.FilterCustomer))
                    && (filterModel.FilterCreateDate == DateTime.MinValue || DbFunctions.TruncateTime(x.CreateDate) == filterModel.FilterCreateDate)
                    && (filterModel.FilterShipmentStatus == 0 || x.ShipmentStatusId == filterModel.FilterShipmentStatus)
                    && (allowedStatuses.Any(s => s == x.ShipmentStatusId))
                    ).ToList();

                totalRecordCount = result.Count;

                result = result.Skip(start).Take(length).ToList();

                switch (sortOrder)
                {
                    case "ShipmentStatus ASC":
                        result = result.OrderBy(a => a.ShipmentStatusId).ToList();
                        break;
                    case "ShipmentStatus DESC":
                        result = result.OrderByDescending(a => a.ShipmentStatusId).ToList();
                        break;

                    case "CreateDate ASC":
                        result = result.OrderBy(a => a.CreateDate).ToList();
                        break;
                    case "CreateDate DESC":
                        result = result.OrderByDescending(a => a.CreateDate).ToList();
                        break;

                    case "ReceiptDate ASC":
                        result = result.OrderBy(a => a.ReceiptDate).ToList();
                        break;
                    case "ReceiptDate DESC":
                        result = result.OrderByDescending(a => a.ReceiptDate).ToList();
                        break;

                    case "DeliveryDate ASC":
                        result = result.OrderBy(a => a.DeliveryDate).ToList();
                        break;
                    case "DeliveryDate DESC":
                        result = result.OrderByDescending(a => a.DeliveryDate).ToList();
                        break;

                    case "CustomerId ASC":
                        result = result.OrderBy(a => a.Customers.CompanyName).ThenBy(x => x.Customers.Name).ThenBy(x => x.Customers.Surname).ToList();
                        break;
                    case "CustomerId DESC":
                        result = result.OrderByDescending(a => a.Customers.CompanyName).ThenByDescending(x => x.Customers.Name).ThenByDescending(x => x.Customers.Surname).ToList();
                        break;

                    default:
                        result = result.AsQueryable().ToList();
                        break;
                }
                return result.ToList();
            }
        }

        [HttpPost]
        public JsonResult UpdateShipmentJSON(ShipmentModel model)
        {
            try
            {
                using (var db = new CarPartsEntities())
                {
                    var shipment = db.Shipments.First(x => x.Id == model.Id);

                    if (shipment != null)
                    {
                        shipment.CustomerId = model.CustomerId;

                        db.SaveChanges();
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "Nie znaleziono zamówienia" });
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
        public JsonResult DeleteShipmentJSON(int id)
        {
            try
            {
                using (var db = new CarPartsEntities())
                {
                    var shipment = db.Shipments.First(x => x.Id == id);

                    if (shipment != null)
                    {
                        shipment.Deleted = true;

                        db.SaveChanges();
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "Nie znaleziono zamówienia" });
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
        public JsonResult CreateShipmentJSON(ShipmentModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (CarPartsEntities db = new CarPartsEntities())
                    {
                        var newShipment = new Shipments()
                        {
                            CreateDate = DateTime.Now,
                            CreatedBy = CurrentUser.Id,
                            CustomerId = model.CustomerId,
                            ShipmentStatusId = (int)ShipmentStatus.New,
                            Insurance = false
                        };

                        db.Shipments.Add(newShipment);
                        db.SaveChanges();

                        model.Id = newShipment.Id;
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
