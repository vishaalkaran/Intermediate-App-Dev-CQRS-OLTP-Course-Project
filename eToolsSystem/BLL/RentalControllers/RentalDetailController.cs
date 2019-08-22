using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eTools.Data.Entities;
using eTools.Data.POCOs;
using eToolsSystem.DAL;
using System.ComponentModel;
using DMIT2018Common.UserControls;
using System.Data.Entity;
#endregion

namespace eToolsSystem.BLL
{
    [DataObject]
    public class RentalDetailController
    {
        List<string> logger = new List<string>();

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void addRentalEquipment(int rentalId, int equitmentid)
        {
            using (var context = new eToolsContext())
            {
                //Checkout equipment 
                RentalEquipment checkoutEquipment = context.RentalEquipments
                                                            .Where(x => (x.RentalEquipmentID == equitmentid)).FirstOrDefault();
                //Update
                checkoutEquipment.Available = false;

                //COMMIT UPDATE***
                context.Entry(checkoutEquipment).State = EntityState.Modified;
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                //create Rental Details
                RentalDetail newRentalEquipment = new RentalDetail();
                //Recived values
                newRentalEquipment.RentalID = rentalId;
                newRentalEquipment.RentalEquipmentID = equitmentid;


                //unmutatble Values
                newRentalEquipment.DailyRate = context.RentalEquipments
                                                       .Where(x => (x.RentalEquipmentID == equitmentid))
                                                       .Select(x => x.DailyRate).FirstOrDefault();

                newRentalEquipment.ConditionOut = context.RentalEquipments
                                                           .Where(x => (x.RentalEquipmentID == equitmentid))
                                                           .Select(x => x.Condition).FirstOrDefault();


                //Pending return 
                //Set to default values
                newRentalEquipment.Days = 1; //bussiness rule!
                newRentalEquipment.ConditionIn = "Has not been returned";
                newRentalEquipment.DamageRepairCost = 0.0m;
                newRentalEquipment.Comments = "";

                //COMMIT ADD***
                context.Entry(newRentalEquipment).State = EntityState.Added;
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

          
                //Finalize Transaction!
                context.SaveChanges();
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void removeRentalEquipment(int rentalId, int equitmentid)
        {
            using (var context = new eToolsContext())
            {
                //Return equipment 
                RentalEquipment checkoutEquipment = context.RentalEquipments
                                                            .Where(x => (x.RentalEquipmentID == equitmentid)).FirstOrDefault();
                //Update availability
                checkoutEquipment.Available = true;

                //COMMIT UPDATE***
                context.Entry(checkoutEquipment).State = EntityState.Modified;
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////



                //retrive rentaldetails table that will destoryed
                RentalDetail existingRentalEquipment = context.RentalDetails
                                                        .Where(x => ((x.RentalID == rentalId) 
                                                           && (x.RentalEquipmentID == equitmentid))).FirstOrDefault();

                //COMMIT DELETE***
                context.Entry(existingRentalEquipment).State = EntityState.Deleted;

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////



               


                //Finalize Transaction!
                context.SaveChanges();
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public List<CurrentEquipmentSelection> currentCustomerRental(int rentalId)
        {
            using (var context = new eToolsContext())
            {
                List<CurrentEquipmentSelection> current = context.RentalDetails
                                                                 .Where(x => (x.RentalID == rentalId))
                                                                 .Select(x =>
                                                                         new CurrentEquipmentSelection()
                                                                         {
                                                                             eqiupmentID = x.RentalEquipment.RentalEquipmentID,
                                                                             Description = x.RentalEquipment.Description,
                                                                             SerailNumber = x.RentalEquipment.SerialNumber,
                                                                             Rate = x.RentalEquipment.DailyRate,
                                                                             outDate = x.Rental.RentalDate
                                                                         }).ToList();

                return current;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public auxReturnInfo getauxeturnInfo(int rentalid, double daysOut)
        {
            using (var context = new eToolsContext())
            {
                //auxReturnInfo info = context.Rentals
                //                               .Where(x => (x.RentalID == rentalid))
                //                               .Select(x =>
                //                                     new auxReturnInfo()
                //                                     {
                //                                         rentalid = x.RentalID,
                //                                         creditcard = x.CreditCard,
                //                                         dateout = x.RentalDate,
                //                                         subtotal = context.RentalDetails
                //                                           .Where(xx => (xx.RentalID == x.RentalID))
                //                                           .Select(xx => (xx.DailyRate * (Decimal)(xx.Days)))
                //                                           .Sum(),


                //                                         discount = (x.CouponID == null) ? (Decimal)0.0 : (Decimal)(x.Coupon.CouponDiscount),

                //                                         total = ((context.RentalDetails
                //                                                             .Where(xx => (xx.RentalID == x.RentalID))
                //                                                             .Select(xx => (xx.DailyRate * (Decimal)(xx.Days)))
                //                                                             .Sum() + (context.RentalDetails
                //                                                                                   .Where(xx => (xx.RentalID == x.RentalID))
                //                                                                                   .Select(xx => (xx.DailyRate * (Decimal)(xx.Days)))
                //                                                                                   .Sum() * (Decimal)0.05 )) - ((x.CouponID == null) ? (Decimal)0.0 : (Decimal)(x.Coupon.CouponDiscount)))
                //                                                                                             }).FirstOrDefault(); 


                auxReturnInfo info = context.Rentals
                                            .Where(x => (x.RentalID == rentalid))
                                            .Select(x =>
                                                    new auxReturnInfo()
                                                    {
                                                        rentalid = x.RentalID,
                                                        creditcard = x.CreditCard,
                                                        dateout = x.RentalDate,

                                                        subtotal = context.RentalDetails
                                                        .Where(xx => (xx.RentalID == x.RentalID))
                                                        .Select(xx => ((Double)(xx.DailyRate) * daysOut)).Sum(),

                                                        gst = (context.RentalDetails
                                                            .Where(xx => (xx.RentalID == x.RentalID))
                                                            .Select(xx => ((Double)(xx.DailyRate) * daysOut)).Sum() * 0.05),

                                                        discount = (x.CouponID == null) ? 0 : (Double)(x.Coupon.CouponDiscount),

                                                        total = ((context.RentalDetails
                                                         .Where(xx => (xx.RentalID == x.RentalID))
                                                         .Select(xx => ((Double)(xx.DailyRate) * daysOut))
                                                         .Sum() + (context.RentalDetails
                                                                       .Where(xx => (xx.RentalID == x.RentalID))
                                                                       .Select(xx => (Double)xx.DailyRate * daysOut)
                                                                       .Sum() * 0.05)) - ((x.CouponID == null) ? 0 : (Double)(x.Coupon.CouponDiscount)))
                                                    }).FirstOrDefault();
                return info;
            }
        }
    }
}
