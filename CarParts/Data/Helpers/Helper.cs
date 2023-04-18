using CarParts.Attributes;
using CarParts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace CarParts.Helpers
{
    public class CurrentUser
    {
        /// <summary>
        /// Lista id statusów paczek, które użytkownik może widzieć
        /// </summary>
        /// <returns></returns>
        public static int[] AllowedShipmentStatuses()
        {
            if (Role == Role.Admin)
            {
                return new int[]
                {
                    (int)ShipmentStatus.None,
                    (int)ShipmentStatus.New,
                    (int)ShipmentStatus.PackageCompletion,
                    (int)ShipmentStatus.WaitingForCourier,
                    (int)ShipmentStatus.ReceivedByuCourier,
                    (int)ShipmentStatus.PreparedForShipment,
                    (int)ShipmentStatus.PartsOrdered,
                    (int)ShipmentStatus.Stopped,
                };
            }
            else if (Role == Role.Warehouseman)
            {
                return new int[]
                {
                    (int)ShipmentStatus.PackageCompletion,
                    (int)ShipmentStatus.WaitingForCourier,
                    (int)ShipmentStatus.ReceivedByuCourier,
                    (int)ShipmentStatus.PreparedForShipment,
                    (int)ShipmentStatus.PartsOrdered,
                    (int)ShipmentStatus.Stopped,
                };
            }
            else if (Role == Role.Seller)
            {
                return new int[]
                {
                    (int)ShipmentStatus.New,
                    (int)ShipmentStatus.PackageCompletion,
                    (int)ShipmentStatus.WaitingForCourier,
                    (int)ShipmentStatus.ReceivedByuCourier,
                    (int)ShipmentStatus.PreparedForShipment,
                    (int)ShipmentStatus.PartsOrdered,
                    (int)ShipmentStatus.Stopped,
                };
            }
            else
                return new int[0];
        }

        public static Role Role
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;

                        string userRole = identity.Ticket.UserData;

                        int role;
                        if (int.TryParse(userRole, out role))
                        {
                            return (Role)role;
                        }
                    }
                }

                return Role.None;
            }
        }

        public static bool Seller
        {
            get
            {
                return Role == Role.Seller || Role == Role.Admin;
            }
        }

        public static bool Warehouseman
        {
            get
            {
                return Role == Role.Warehouseman || Role == Role.Admin;
            }
        }

        public static bool Administrator
        {
            get
            {
                return Role == Role.Admin;
            }
        }

        public static string Name
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return HttpContext.Current.User.Identity.Name;
                }

                return string.Empty;
            }
        }

        public static int Id
        {
            get
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    return 0;

                var sessionUserId = HttpContext.Current.Session["UserId"];

                if (sessionUserId == null)
                {
                    using (CarPartsEntities db = new CarPartsEntities())
                    {
                        var account = db.Accounts.FirstOrDefault(x => x.UserName == Name);

                        if (account != null)
                        {
                            int userId = account.Id;

                            HttpContext.Current.Session["UserId"] = userId;

                            return userId;
                        }
                    }
                }
                else
                {
                    int userId;
                    if (int.TryParse(sessionUserId.ToString(), out userId))
                    {
                        return userId;
                    }
                }

                return 0;
            }
        }
    }
}