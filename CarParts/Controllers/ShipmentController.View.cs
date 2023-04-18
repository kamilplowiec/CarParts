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

namespace CarParts.Controllers
{  
    public partial class ShipmentController : Controller
    {
        [AuthRole(Roles = new Role[] { Role.Admin, Role.Warehouseman, Role.Seller })]
        public ActionResult View(int id)
        {
            var model = new ShipmentModel();

            using (CarPartsEntities db = new CarPartsEntities())
            {
                var li = db.Shipments.First(x => x.Id == id);

                if(li.Deleted)
                {
                    TempData["ErrorMessage"] = "Zamówienie usunięte. Nie można do niego przejść.";
                    return RedirectToAction("Index", "Home");
                }

                model = new ShipmentModel()
                {
                    Id = li.Id,
                    CreateDate = li.CreateDate,
                    CreatedByName = li.Accounts.FirstName + " " + li.Accounts.LastName + " (" + li.Accounts.UserName + ")",
                    DeliveryCost = li.DeliveryCost.HasValue ? li.DeliveryCost.Value.ToString() : string.Empty,
                    DeliveryDate = li.DeliveryDate,
                    ShipmentStatus = li.ShipmentStatusId,
                    Insurance = li.Insurance,
                    Price = li.Price.HasValue ? li.Price.Value.ToString() : string.Empty,
                    ReceiptDate = li.ReceiptDate,
                    ShippingNumber = li.ShippingNumber,
                    ShipmentStatusList = db.ShipmentStatuses.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString()}).ToList(),
                    DeliveryList = db.Deliveries.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList(),
                    Weight = li.Weight.ToString(),
                    DeliveryName = li.DeliveryId.HasValue ? li.Deliveries.Name : "",
                    ShipmentTrackingLink = li.DeliveryId.HasValue && !string.IsNullOrEmpty(li.Deliveries.TrackUrl) ? li.Deliveries.TrackUrl + li.ShippingNumber : "",
                    AllProductsInShipment =  li.ShipmentProducts.Count(x => !x.Deleted),
                    PackedProductsInShipment =  li.ShipmentProducts.Count(x => !x.Deleted && x.InShipment),

                    Customer = new CustomerModel()
                    {
                        Id = li.Id,
                        Name = li.Customers.Name,
                        Surname = li.Customers.Surname,
                        CompanyName = li.Customers.CompanyName,
                        Email = li.Customers.Email,
                        Phone = li.Customers.Phone,
                        NIP = li.Customers.NIP,
                        PESEL = li.Customers.PESEL,
                        REGON = li.Customers.REGON,
                        Address = new AddressModel()
                        {
                            AddressId = li.Customers.Address.Id,
                            FlatNo = li.Customers.Address.FlatNo,
                            HouseNo = li.Customers.Address.HouseNo,
                            Place = li.Customers.Address.Place,
                            PostCode = li.Customers.Address.PostCode,
                            Street = li.Customers.Address.Street
                        }
                    }
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ChangeShipmentStatus(int id, int newStatus, ShipmentModel shipmentModel = null)
        {
            try
            {
                using (var db = new CarPartsEntities())
                {
                    var shipment = db.Shipments.First(x => x.Id == id);

                    if (shipment != null)
                    {
                        if (newStatus == (int)ShipmentStatus.PreparedForShipment)
                        {
                            shipment.Price = shipment.ShipmentProducts.Where(x => !x.Deleted).Sum(x => x.Count * x.Products.Price);
                            shipment.Weight = shipment.ShipmentProducts.Sum(x => x.Products.Weight * x.Count);
                        }

                        if (newStatus == (int)ShipmentStatus.WaitingForCourier)
                        {
                            shipment.DeliveryId = shipmentModel.DeliveryId;
                            shipment.ReceiptDate = shipmentModel.ReceiptDate;
                            shipment.DeliveryDate = shipmentModel.DeliveryDate;
                            shipment.ShippingNumber = shipmentModel.ShippingNumber;
                            shipment.Insurance = shipmentModel.Insurance;

                            var delivery = db.Deliveries.FirstOrDefault(x => x.Id == shipmentModel.DeliveryId);

                            shipment.DeliveryCost = shipment.Weight * delivery.PriceForWeight;

                            if (shipmentModel.Insurance)
                            {
                                shipment.DeliveryCost += (decimal)(shipment.Price * ((decimal)delivery.InsurancePercentCost / 100));
                            }
                        }

                        shipment.ShipmentStatusId = newStatus;

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
    }
}
