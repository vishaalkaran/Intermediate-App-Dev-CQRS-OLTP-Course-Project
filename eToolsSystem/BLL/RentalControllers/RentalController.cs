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

                    //Heavy Query!
                    DELETE_RentalDetailsTable_UPDATE_RentalEquipmentTable removeList = context.Rentals.Where(x => (x.RentalID == rentalid))
                                                                                                        .Select(x =>
                                                                                                                new DELETE_RentalDetailsTable_UPDATE_RentalEquipmentTable()
                                                                                                                {
                                                                                                                    deleteSet = x.RentalDetails.Select(y =>
                                                                                                                                                        new SingleEquipmentPairWithRentalDetailDelete()
                                                                                                                                                        {
                                                                                                                                                            _RentalDetailTable = y,
                                                                                                                                                            _RentalEquipmentTable = y.RentalEquipment
                                                                                                                                                        })
                                                                                                                }).FirstOrDefault();

                    //The.find(...) returns an entity based on...ID

                    //DTO
                    foreach (var remove in removeList.deleteSet)
                    {
                        //I not sure if this returns or mods anything
                        //ii._RentalEquipmentTable.Available = true;

                        //better way... not this, but use above query
                        // ...== ii._RentalEquipmentTable.RentalEquipmentID)... id the equipment
                        //RentalEquipment updatedEquipment = context.RentalEquipments
                        //                                            .Where(x => (x.RentalEquipmentID == ii._RentalEquipmentTable.RentalEquipmentID)).FirstOrDefault();

                        RentalEquipment updatedEquipment = remove._RentalEquipmentTable;
                        updatedEquipment.Available = true;
                        context.Entry(updatedEquipment).State = EntityState.Modified;     //Update Equipment ....Available = true; aka returned


                        RentalDetail deletedRentalDetail = remove._RentalDetailTable;
                        context.Entry(deletedRentalDetail).State = EntityState.Deleted;  //Delete RentalDetails
                    }

                    //Delete Rental   
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
                                       .Where(x => ((x.RentalID == rentalid)))
                                       .Select(
                                          x =>
                                             new ReturnForm()
                                             {
                                                 eqiupmentID = x.RentalEquipment.RentalEquipmentID,
                                                 Description = x.RentalEquipment.Description,
                                                 SerialNumber = x.RentalEquipment.SerialNumber,
                                                 Rate = x.RentalEquipment.DailyRate,
                                                 outDate = x.Rental.RentalDate,
                                                 coditionCommets = "",
                                                 customerCommets = "",
                                                 Av = false
                                             }).ToList();
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