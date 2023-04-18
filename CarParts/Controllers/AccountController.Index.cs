using CarParts.Models.Account;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using CarParts.Models;
using System.Linq.Expressions;
using CarParts.Attributes;
using CarParts.Enums;
using CarParts.Models.User;

namespace CarParts.Controllers
{  
    public partial class AccountController : Controller
    {
        [AuthRole(Roles = new Role[] { Role.Admin })]
        public ActionResult Index()
        {
            ViewBag.RoleList = //Konwersja enum Role na listę
                Enum.GetValues(typeof(Role)).Cast<Role>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();

            return View();
        }

        [HttpPost]
        public JsonResult UsersJSON(UserFilterModel filterModel, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            IList<RegisterModel> retList = new List<RegisterModel>();
            int totalRecordsCount = 0;

            using (var db = new CarPartsEntities())
            {
                var list = GetUsersFiltered(filterModel, jtSorting, jtStartIndex, jtPageSize, out totalRecordsCount);

                foreach (var li in list)
                {
                    var account = new RegisterModel()
                    {
                        Id = li.Id,
                        FirstName = li.FirstName,
                        LastName = li.LastName,
                        UserName = li.UserName,
                        Email = li.Email,
                        AccountRole = li.AccountRole
                    };

                    retList.Add(account);
                }

                
            }
            var tableResult = new TableResult<RegisterModel>() { Records = retList, TotalRecordCount = totalRecordsCount };

            return Json(tableResult, JsonRequestBehavior.AllowGet);
        }

        private List<Account> GetUsersFiltered(UserFilterModel filterModel, string sortOrder, int start, int length, out int totalRecordCount)
        {
            using (var db = new CarPartsEntities())
            {
                var result = db.Accounts.Where(x => !x.Deleted &&
                    (string.IsNullOrEmpty(filterModel.FilterFirstName) || x.FirstName.Contains(filterModel.FilterFirstName))
                    && (string.IsNullOrEmpty(filterModel.FilterLastName) || x.LastName.Contains(filterModel.FilterLastName))
                    && (string.IsNullOrEmpty(filterModel.FilterUsername) || x.UserName.Contains(filterModel.FilterUsername))
                    && (string.IsNullOrEmpty(filterModel.FilterEmail) || x.Email.Contains(filterModel.FilterEmail))
                    && (filterModel.FilterAccountRole == 0 || x.AccountRole == filterModel.FilterAccountRole)
                    ).ToList();

                totalRecordCount = result.Count;

                result = result.Skip(start).Take(length).ToList();

                switch (sortOrder)
                {
                    case "FirstName ASC":
                        result = result.OrderBy(a => a.FirstName).ToList();
                        break;
                    case "FirstName DESC":
                        result = result.OrderByDescending(a => a.FirstName).ToList();
                        break;

                    case "LastName ASC":
                        result = result.OrderBy(a => a.LastName).ToList();
                        break;
                    case "LastName DESC":
                        result = result.OrderByDescending(a => a.LastName).ToList();
                        break;

                    case "UserName ASC":
                        result = result.OrderBy(a => a.UserName).ToList();
                        break;
                    case "UserName DESC":
                        result = result.OrderByDescending(a => a.UserName).ToList();
                        break;

                    case "Email ASC":
                        result = result.OrderBy(a => a.Email).ToList();
                        break;
                    case "Email DESC":
                        result = result.OrderByDescending(a => a.Email).ToList();
                        break;

                    case "AccountRole ASC":
                        result = result.OrderBy(a => a.AccountRole).ToList();
                        break;
                    case "AccountRole DESC":
                        result = result.OrderByDescending(a => a.AccountRole).ToList();
                        break;

                    default:
                        result = result.AsQueryable().ToList();
                        break;
                }
                return result.ToList();
            }
        }


        [HttpPost]
        public JsonResult UpdateUserJSON(RegisterModel model)
        {
            try
            {
                using (var db = new CarPartsEntities())
                {
                    var account = db.Accounts.First(x => x.Id == model.Id);

                    if (account != null)
                    {
                        account.FirstName = model.FirstName;
                        account.LastName = model.LastName;
                        account.UserName = model.UserName;
                        account.AccountRole = model.AccountRole;
                        account.Email = model.Email;

                        if(model.ChangePassword)
                        {
                            if(string.Compare(model.Password, model.ConfirmPassword) != 0)
                            {
                                return Json(new { Result = "ERROR", Message = "Hasła nie są takie same" });
                            }

                            account.Password = CreateHash(account.Password);
                        }

                        db.SaveChanges();
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "Nie znaleziono konta" });
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
        public JsonResult DeleteUserJSON(int id)
        {
            try 
            {
                using (var db = new CarPartsEntities())
                {
                    var account = db.Accounts.First(x => x.Id == id);

                    if (account != null)
                    {
                        account.Deleted = true;

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
        public JsonResult CreateUserJSON(RegisterModel model)
        {
            try
            {
                if (UserNameExsist(model.UserName))
                {
                    return Json(new { Result = "ERROR", Message = "Konto o podanym loginie już istnieje" });
                }

                if (ModelState.IsValid)
                {
                    using (CarPartsEntities db = new CarPartsEntities())
                    {
                        db.Accounts.Add(
                            new Account()
                            {
                                Email = model.Email,
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Password = CreateHash(model.Password),
                                UserName = model.UserName,
                                AccountRole = model.AccountRole
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
