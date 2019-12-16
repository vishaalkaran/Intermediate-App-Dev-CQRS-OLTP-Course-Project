using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eTools.Data.Entities;
using eTools.Data.DTOs;
using eTools.Data.POCOs;
using eToolsSystem.DAL;
using System.ComponentModel;
using DMIT2018Common.UserControls;
using System.Data.Entity;
#endregion

namespace eToolsSystem.BLL
{
    [DataObject]
    public class RentalController
    {
        List<string> logger = new List<string>();

        //new 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public int createAndReturnEmptyRentalID(int customerid, int employeeid, int? couponid, string creditcard)
        {
            using (var context = new eToolsContext())
            {
                //Must have credit card
                if (string.IsNullOrEmpty(creditcard)) //customer can NOT PAY 
                {
                    throw new BusinessRuleException("Must supply credit!", logger);
                }
                else //CAN PAY 
                {
                    Rental rental = new Rental();
                    rental.CustomerID = customerid;
                    rental.EmployeeID = employeeid;
                    rental.CouponID = couponid;
                    rental.SubTotal = 0;
                    rental.TaxAmount = 0;
                    rental.RentalDate = DateTime.Today; //will be overriden when submitted 
                    rental.PaymentType = 'M'.ToString(); //default is space****!  N for Not paid
                    rental.CreditCard = creditcard;

                    //this will return the add rental object w/ an id set  **I hope!!
                    //also it will override the current object 
                    rental = context.Rentals.Add(rental);


                    if (rental == null)
                    {
                        throw new BusinessRuleException("Rental could not be created!", logger);
                    }
                    else
                    {
                        //Commit changes 
                        context.SaveChanges();

                        //return rentalid
                        return rental.RentalID;
                    }
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void deleteRental(int rentalid)
        {
            using (var context = new eToolsContext())
            {
                //.FirstOrDefault() is added in Visual Studio not in LINQ, as it does NOT AUTO. CONVERT!!
                Rental rental = context.Rentals.Where(x => (x.RentalID == rentalid)).FirstOrDefault();

                //A return of 1 means an EXACT MATCH
                //either there is a value or not; 1 or 0
                if (rental == null)
                {
                    throw new BusinessRuleException("No such rental exists!", logger);
                }
                else
                {
                    var removeList = context.RentalDetails
                                       .Where(x => (x.RentalID == rentalid))
                                       .Select(x =>
                                                    new
                                                    {
                                                        RentalDetailTable = x,
                                                        RentalEquipmentTable = x.RentalEquipment
                                                    }
                                               );

                    foreach (var remove in removeList)
                    {
                        //Free equipmwnt
                        remove.RentalEquipmentTable.Available = true;
                        context.Entry(remove.RentalEquipmentTable).State = EntityState.Modified;

                        //Delete rental details/ equipment's parent 
                        context.Entry(remove.RentalDetailTable).State = EntityState.Deleted;
                    }
                    //Delete Rental/ details parent
                    context.Entry(rental).State = EntityState.Deleted;
                }
                //Commit Transaction
                context.SaveChanges();
            }
        }

        //public int? validateCoupon(int couponid)
        //{
        //    using (var context = new eToolsContext())
        //    {
        //        var count = context.Coupons
        //                       .Where(x => (x.CouponID == couponid))
        //                       .Select(x => x.CouponDiscount).FirstOrDefault();

        //        return count == 0 ? null : count;  
        //    }
        //}

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ReturnForm> returnitemlist(int rentalid)
        {
            //&& (x.Rental.PaymentType == "N")
            using (var context = new eToolsContext())
            {
                var retruns = context.RentalDetails
                                .Where(x => ((x.RentalID == rentalid) && (x.Rental.SubTotal == 0)))
                                .Select(x =>
                                                new ReturnForm
                                                {
                                                    ID = x.RentalDetailID,
                                                    Description = x.RentalEquipment.Description,
                                                    SerialNumber = x.RentalEquipment.SerialNumber,
                                                    Rate = x.RentalEquipment.DailyRate,
                                                    OutDate = x.Rental.RentalDate,
                                                    CoditionCommets = "",
                                                    CustomerCommets = "",
                                                    Av = false
                                                }
                                        ).ToList();
                return retruns;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Rental returnSingleRentalForm(int rentalid)
        {
            using (var context = new eToolsContext())
            {
                Rental rental = context.Rentals.Where(x => (x.RentalID == rentalid)).FirstOrDefault();

                //rental.SubTotal = context.RentalDetails.Where(x => (x.RentalID == rentalid)).Sum(xx => xx.DailyRate);
                //rental.TaxAmount = (context.RentalDetails.Where(x => (x.RentalID == rentalid)).Sum(xx => xx.DailyRate)) * (decimal)0.05;

                //context.Entry(rental).State = EntityState.Modified;
                //Finalize Transaction!
                context.SaveChanges();

                return rental;
            }          
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int Return_Update(ReturnForm rtn)
        {
            using (var context = new eToolsContext())
            {
                //RentalDetail item = context.RentalDetails.Where(x => (x.RentalDetailID == rtn.ID)).FirstOrDefault();
                //RentalDetail item2 = new RentalDetail();
                //item2 = item;
                //item2.Comments = rtn.CustomerCommets == null ? "" : rtn.CustomerCommets; //can be null       
                //item2.ConditionIn = rtn.CoditionCommets == null ? "" : rtn.CoditionCommets;

                RentalDetail rdptr = context.RentalDetails.Where(x => (x.RentalDetailID == rtn.ID)).FirstOrDefault();
                RentalDetail item = new RentalDetail();
                if (rdptr == null)
                {
                    throw new BusinessRuleException("No such rental exists!", logger);
                }
                else
                {
                    
                    item.RentalDetailID = rtn.ID;
                    item.RentalID = rdptr.RentalID;
                    item.RentalEquipmentID = rdptr.RentalEquipmentID;
                    item.Days = rdptr.Days;
                    item.DailyRate = rdptr.DailyRate;
                    item.ConditionOut = rdptr.ConditionOut;
                    item.ConditionIn = string.IsNullOrEmpty(rtn.CoditionCommets) ? "" : rtn.CoditionCommets;
                    item.DamageRepairCost = rdptr.DamageRepairCost;
                    item.Comments = string.IsNullOrEmpty(rtn.CustomerCommets) ? "" : rtn.CustomerCommets;
  
                }
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                return context.SaveChanges();
            }
        }

        public void payRental(int rentalid)
        {
            using (var context = new eToolsContext())
            {
                List<RentalDetail> pay = context.RentalDetails.Where(x => (x.RentalID == rentalid)).ToList();
                foreach (var rd in pay)
                {
                    rd.Paid = true;
                    context.Entry(rd).State = EntityState.Modified;
                }
                //Commit Transaction
                context.SaveChanges();
            }
        }

        //public void returnRental(int rentalid)
        //{
        //    using (var context = new eToolsContext())
        //    {
        //        foreach (var in)
        //        {

        //        }
        //        //Commit Transaction
        //        context.SaveChanges();
        //    }

        //}
    }
}

//Re-written in RentalCustomersController as selectSingleCustomer

//[DataObjectMethod(DataObjectMethodType.Select, false)]
//public int outstandingRentalID(int customerid)
//{
//    using (var context = new eToolsContext())
//    {

//        return;
//    }
//}