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
    public partial class ProductController : Controller
    {
        [AuthRole(Roles = new Role[] { Role.Admin, Role.Warehouseman, Role.Seller })]
        public ActionResult Index(bool l = true)
        {
            return View(l);
        }

        [HttpPost]
        public JsonResult ProductsJSON(ProductFilterModel filterModel, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            IList<ProductModel> retList = new List<ProductModel>();
            int totalRecordCount = 0;

            var list = GetProductsFiltered(filterModel, jtSorting, jtStartIndex, jtPageSize, out totalRecordCount);

            foreach (var li in list)
            {
                var product = new ProductModel()
                {
                    Id = li.Id,
                    Name = li.Name,
                    Details = li.Details,
                    Price = li.Price.ToString(),
                    QuantityInWarehouse = li.QuantityInWarehouse,
                    Weight = li.Weight.ToString()
                };

                retList.Add(product);
            }

            var tableResult = new TableResult<ProductModel>() { Records = retList, TotalRecordCount = totalRecordCount };

            return Json(tableResult, JsonRequestBehavior.AllowGet);
        }

        private List<Products> GetProductsFiltered(ProductFilterModel filterModel, string sortOrder, int start, int length, out int totalRecordCount)
        {
            totalRecordCount = 0;

            using (var db = new CarPartsEntities())
            {
                var result = db.Products.Where(x => !x.Deleted &&
                    (string.IsNullOrEmpty(filterModel.FilterName) || x.Name.Contains(filterModel.FilterName))
                    && (string.IsNullOrEmpty(filterModel.FilterDetails) || x.Details.Contains(filterModel.FilterDetails)))
                .ToList();

                totalRecordCount = result.Count;

                result = result.Skip(start).Take(length).ToList();

                switch (sortOrder)
                {
                    case "Name ASC":
                        result = result.OrderBy(a => a.Name).ToList();
                        break;
                    case "Name DESC":
                        result = result.OrderByDescending(a => a.Name).ToList();
                        break;

                    case "Details ASC":
                        result = result.OrderBy(a => a.Details).ToList();
                        break;
                    case "Details DESC":
                        result = result.OrderByDescending(a => a.Details).ToList();
                        break;

                    default:
                        result = result.AsQueryable().ToList();
                        break;
                }
                return result.ToList();
            }
        }

        [HttpPost]
        public JsonResult UpdateProductJSON(ProductModel model)
        {
            try
            {
                using (var db = new CarPartsEntities())
                {
                    var product = db.Products.First(x => x.Id == model.Id);

                    if (product != null)
                    {
                        product.Name = model.Name;
                        product.Details = model.Details;
                        product.Price = decimal.Parse(model.Price);
                        product.QuantityInWarehouse = model.QuantityInWarehouse;
                        product.Weight = decimal.Parse(model.Weight);

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
        public JsonResult DeleteProductJSON(int id)
        {
            try
            {
                using (var db = new CarPartsEntities())
                {
                    var product = db.Products.First(x => x.Id == id);

                    if (product != null)
                    {
                        product.Deleted = true;

                        db.SaveChanges();
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "Nie znaleziono konta" });
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
        public JsonResult CreateProductJSON(ProductModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (CarPartsEntities db = new CarPartsEntities())
                    {
                        db.Products.Add(
                            new Products()
                            {
                                Name = model.Name,
                                Details = model.Details,
                                Price = decimal.Parse(model.Price),
                                QuantityInWarehouse = model.QuantityInWarehouse,
                                Weight = decimal.Parse(model.Weight),
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
