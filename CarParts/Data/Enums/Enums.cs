using CarParts.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace CarParts.Enums
{
    public enum Role
    {
        None = 0,
        Admin = 1,
        Seller = 2,
        Warehouseman = 3
    }

    public enum ShipmentStatus
    {
        None = 0,
        New = 1,
        PartsOrdered = 2,
        PackageCompletion = 3,
        PreparedForShipment = 4,
        WaitingForCourier = 5,
        ReceivedByuCourier = 6,
        Stopped = 7
    }
}