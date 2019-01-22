using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using eTools.Data.Entities;
using eToolsSystem.DAL;
using System.ComponentModel;
using eTools.Data.DTOs;
using eTools.Data.POCOs;

namespace eToolsSystem.BLL
{
    [DataObject]
    public class purchaseOrderController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<OpenPurchaseOrders> List_OpenPurchaseOrders()
        {
            using (var context = new eToolsContext())
            {
                List<OpenPurchaseOrders> openPOs = (from x in context.PurchaseOrders
                                                    where (x.Closed != true && x.OrderDate != null && x.PurchaseOrderNumber != null)
                                                    select new OpenPurchaseOrders
                                                    {
                                                        PurchaseOrderID = x.PurchaseOrderID,
                                                        PurchaseOrderNumber = x.PurchaseOrderNumber,
                                                        OrderDate = x.OrderDate,
                                                        VendorName = x.Vendor.VendorName,
                                                        VendorPhone = x.Vendor.Phone
                                                    }).ToList();
                return openPOs;
            }
        }
        public void ForceClosePurchaseOrder(int purchaseOrderID, string reasonClosed)
        {
            using (var context = new eToolsContext())
            {
                PurchaseOrder pOrder = context.PurchaseOrders.Find(purchaseOrderID);
                pOrder.Closed = true;
                pOrder.Notes = reasonClosed;
                context.Entry(pOrder).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public PurchaseOrder PurchaseOrder_Get(int purchaseorderid)
        {
            using (var context = new eToolsContext())
            {
                return context.PurchaseOrders.Find(purchaseorderid);
            }
        }

        public List<CurrentOrderSummary> GetCurrentOrderSummary(int vendorID)
        {
            using (var context = new eToolsContext())
            {
                DateTime date = DateTime.Now.AddMonths(-1);
                var poco = (from x in context.PurchaseOrders
                            where x.VendorID == vendorID
                            where ((x.PurchaseOrderNumber == null && x.OrderDate == null) || (x.OrderDate > date))
                            select new CurrentOrderSummary
                            {
                                PurchaseOrderID = x.PurchaseOrderID,
                                PurchaseOrderNumber = x.PurchaseOrderNumber,
                                OrderDate = x.OrderDate,
                                VendorID = x.VendorID,
                                VendorName = context.Vendors.Where(v => v.VendorID == x.VendorID).ToList().Select(v => v.VendorName).FirstOrDefault(),
                                //VendorName = (from y in context.Vendors
                                //             where y.VendorID == x.VendorID
                                //             select y.VendorName).FirstOrDefault(),
                                
                                EmployeeID = x.EmployeeID,
                                EmployeeName = x.Employee.LastName + ", " + x.Employee.FirstName,                                
                                Subtotal = x.SubTotal,
                                Tax = x.TaxAmount,
                                OrderDetailCount = x.PurchaseOrderDetails.Count()                        
                            }).ToList();

                return poco;
            }
        }
    }
}
