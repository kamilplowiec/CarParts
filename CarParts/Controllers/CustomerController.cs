using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using CarParts.Models;
using System.Linq.Expressions;
using CarParts.Attributes;
using CarParts.Models.Customer;
using CarParts.Enums;

namespace CarParts.Controllers
{  
    public partial class CustomerController : Controller
    {
        [AuthRole(Roles = new Role[] { Role.Admin, Role.Seller })]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CustomersJSON(CustomerFilterModel filterModel, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            IList<CustomerModel> retList = new List<CustomerModel>();
            int totalRecordCount = 0;

            using (var db = new CarPartsEntities())
            {
                var result = db.Customers.Where(x => !x.Deleted &&
                    (string.IsNullOrEmpty(filterModel.FilterName) || (x.Name + " " + x.Surname + " " + x.CompanyName).Contains(filterModel.FilterName))
                    && (string.IsNullOrEmpty(filterModel.FilterNIP) || x.NIP.Contains(filterModel.FilterNIP))
                    && (string.IsNullOrEmpty(filterModel.FilterREGON) || x.REGON.Contains(filterModel.FilterREGON))
                    && (string.IsNullOrEmpty(filterModel.FilterPESEL) || x.PESEL.Contains(filterModel.FilterPESEL))
                    && (string.IsNullOrEmpty(filterModel.FilterEmail) || x.Email.Contains(filterModel.FilterEmail))
                    && (string.IsNullOrEmpty(filterModel.FilterPhone) || x.Phone.Contains(filterModel.FilterPhone))
                    && (string.IsNullOrEmpty(filterModel.FilterAddress) || (x.Address.Street + " " + x.Address.HouseNo + " " + x.Address.Place + " " + x.Address.PostCode).Contains(filterModel.FilterAddress))
                    ).ToList();

                totalRecordCount = result.Count;

                result = result.Skip(jtStartIndex).Take(jtPageSize).ToList();

                switch (jtSorting)
                {
                    case "FullName ASC":
                        result = result.OrderBy(a => a.CompanyName).ThenBy(x => x.Name).ThenBy(x => x.Surname).ToList();
                        break;
                    case "FullName DESC":
                        result = result.OrderByDescending(a => a.CompanyName).ThenByDescending(x => x.Name).ThenByDescending(x => x.Surname).ToList();
                        break;

                    default:
                        result = result.AsQueryable().ToList();
                        break;
                }

                foreach (var li in result)
                {
                    var customer = new CustomerModel()
                    {
                        Id = li.Id,
                        Name = li.Name,
                        Surname = li.Surname,
                        CompanyName = li.CompanyName,
                        Email = li.Email,
                        Phone = li.Phone,
                        NIP = li.NIP,
                        PESEL = li.PESEL,
                        REGON = li.REGON,
                        Address = new AddressModel()
                        {
                            AddressId = li.Address.Id,
                            FlatNo = li.Address.FlatNo,
                            HouseNo = li.Address.HouseNo,
                            Place = li.Address.Place,
                            PostCode = li.Address.PostCode,
                            Street = li.Address.Street
                        }
                    };

                    retList.Add(customer);
                }
            }
            var tableResult = new TableResult<CustomerModel>() { Records = retList, TotalRecordCount = totalRecordCount };

            return Json(tableResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateCustomerJSON(CustomerModel model)
        {
            try
            {
                using (var db = new CarPartsEntities())
                {
                    var customer = db.Customers.First(x => x.Id == model.Id);

                    if (customer != null)
                    {
                        var address = db.Address.First(x => x.Id == customer.AddressId);

                        if (address != null)
                        {
                            address.Street = model.Address.Street;
                            address.HouseNo = model.Address.HouseNo;
                            address.FlatNo = model.Address.FlatNo;
                            address.PostCode = model.Address.PostCode;
                            address.Place = model.Address.Place;
                         }

                        customer.CompanyName = model.CompanyName;
                        customer.Email = model.Email;
                        customer.Name = model.Name;
                        customer.NIP = model.NIP;
                        customer.PESEL = model.PESEL;
                        customer.Phone = model.Phone;
                        customer.REGON = model.REGON;
                        customer.Surname = model.Surname;

                        db.SaveChanges();
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "Nie znaleziono klienta" });
                    }
                }

                return Json(new { Result = "OK"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteCustomerJSON(int id)
        {
            try 
            {
                using (var db = new CarPartsEntities())
                {
                    var customer = db.Customers.First(x => x.Id == id);

                    if (customer != null)
                    {
                        customer.Deleted = true;

                        db.SaveChanges();
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "Nie znaleziono klienta" });
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
        public JsonResult CreateCustomerJSON(CustomerModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.CompanyName))
                {
                    if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Surname))
                    {
                        ModelState.AddModelError("CompanyName", "Pole Nazwa firmy lub Imię i Nazwisko są wymagane");
                    }
                    else if (string.IsNullOrEmpty(model.PESEL))
                    {
                        ModelState.AddModelError("PESEL", "Pole PESEL jest wymagane");
                    }
                }
                else 
                {
                    if (string.IsNullOrEmpty(model.NIP))
                    {
                        ModelState.AddModelError("NIP", "Pole NIP jest wymagane");
                    }

                    if (string.IsNullOrEmpty(model.REGON))
                    {
                        ModelState.AddModelError("NIP", "Pole REGON jest wymagane");
                    }
                }

                if (ModelState.IsValid)
                {
                    using (CarPartsEntities db = new CarPartsEntities())
                    {
                        model.Address.AddressId = 
                            db.Address.Add(new Address()
                        {
                            Street = model.Address.Street,
                            HouseNo = model.Address.HouseNo,
                            FlatNo = model.Address.FlatNo,
                            PostCode = model.Address.PostCode,
                            Place = model.Address.Place
                        }).Id;

                        db.Customers.Add(new Customers()
                        {
                            AddressId = model.Address.AddressId,
                            CompanyName = model.CompanyName,
                            Email = model.Email,
                            Name = model.Name,
                            NIP = model.NIP,
                            PESEL = model.PESEL,
                            Phone = model.Phone,
                            REGON = model.REGON,
                            Surname = model.Surname
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
