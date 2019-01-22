using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using eTools.Data.Entities;
using eTools.Data.POCOs;
using eToolsSystem.DAL;
using System.ComponentModel;
using DMIT2018Common.UserControls;

namespace eToolsSystem.BLL
{
    public class CouponController
    {
        public Coupon ValidateCoupon(string couponValue)
        {
            using(var context = new eToolsContext())
            {
                Coupon coupon = context.Coupons.Where(x => x.CouponIDValue.Equals(couponValue)).FirstOrDefault();
                return coupon;
            }
        }
    }
}
