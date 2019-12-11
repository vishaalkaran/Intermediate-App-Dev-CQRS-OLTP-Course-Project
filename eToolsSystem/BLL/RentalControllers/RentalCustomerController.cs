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
#endregion

namespace eToolsSystem.BLL
{
    [DataObject]
    public class RentalCustomerController
    {
        List<string> logger = new List<string>();

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CustomerSelection> customerList()
        {
            using (var context = new eToolsContext())
            {
                List<CustomerSelection> customers = context.Customers               
                                                           .Select(x => 
                                                                   new CustomerSelection()
                                                                   {
                                                                       ID = x.CustomerID,
                                                                       Fullname = ((x.LastName + ", ") + x.FirstName),
                                                                       Address = x.Address,
                                                                       PhoneNumber = x.ContactPhone
                                                                   }).ToList();
                    return customers;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CustomerSelection> customersByPhoneNumber(string pNumber)
        {
            using (var context = new eToolsContext())
            {
                List<CustomerSelection> customers = context.Customers.Where(x => (x.ContactPhone == pNumber))
                                                                     .Select(x =>
                                                                             new CustomerSelection()
                                                                             {
                                                                                 ID = x.CustomerID,
                                                                                 Fullname = ((x.LastName + ", ") + x.FirstName),
                                                                                 Address = x.Address,
                                                                                 PhoneNumber = x.ContactPhone
                                                                             }).ToList();
                return customers;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Customer selectSingleCustomer(int rentalid)
        {
            using (var context = new eToolsContext())
            {
                    return context.Rentals.Where(x => (x.RentalID == rentalid)) .Select(x => x.Customer).FirstOrDefault();

            }
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<returnCustomer> customersReturnLookUp(string clientPhoneNumberOrRentalID)
        {
            using (var context = new eToolsContext())
            {
                //This method checks id and phone# 
                List<returnCustomer> lookup = context.Rentals.Where(x => (((x.Customer.ContactPhone == clientPhoneNumberOrRentalID) || (x.RentalID.ToString() == clientPhoneNumberOrRentalID)) 
                                                                           && (x.SubTotal == 0))).Select(x =>
                                                                                                         new returnCustomer()
                                                                                                         {
                                                                                                             rentalid = x.RentalID,
                                                                                                             customerid = x.Customer.CustomerID,
                                                                                                             fullname = ((x.Customer.LastName + ", ") + x.Customer.FirstName),
                                                                                                             address = x.Customer.Address,
                                                                                                             mmddyy = x.RentalDate
                                                                                                         }).ToList();
                return lookup;

            }
        }  
    }
}
