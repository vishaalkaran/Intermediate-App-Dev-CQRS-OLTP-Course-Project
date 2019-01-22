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
    public class PurchaseOrderDetailsController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<OpenPurchaseOrderDetails> List_OpenPurchaseOrderDetails (int PurchaseOrderID)
        {
            using (var context = new eToolsContext())
            {
                List<OpenPurchaseOrderDetails> openPODetails = (from x in context.PurchaseOrderDetails
                                                                where x.PurchaseOrder.PurchaseOrderID == PurchaseOrderID
                                                                select new OpenPurchaseOrderDetails
                                                                {
                                                                    PurchaseOrderDetailID = x.PurchaseOrderDetailID,
                                                                    VendorStockNumber = context.StockItems.Where(s => s.StockItemID == x.StockItemID).FirstOrDefault().VendorStockNumber,
                                                                    StockItemID = x.StockItemID,
                                                                    StockItemDescription = x.StockItem.Description,
                                                                    QuantityOnOrder = x.Quantity,
                                                                    QuantityOutstanding = x.Quantity - ((x.ReceiveOrderDetails.Sum(q => q.QuantityReceived)) == null ? 0 : x.ReceiveOrderDetails.Sum(q => q.QuantityReceived)),
                                                                    ReceivedQuantity = 0,
                                                                    ReturnedQuantity = 0,
                                                                    ReturnReason = ""
                                                               }).ToList();
                return openPODetails;
            }
        }

        public List<CurrentActiveOrderView> List_OrderDetailPOCO(int employeeID,int vendorID)
        {

            using (var context = new eToolsContext())
            {
                //0. check if OrderExist
                //1. if order DNE, create order
                //1.1 create detail (suggested view)
                //2. if order exist, 
                //2.1 if detail DNE, poco is null, return null
                //2.2 else poco is existing orderdetails

                //0
                decimal subTotal = 0;
                decimal tax = 0;
                PurchaseOrder existOrder = (from x in context.PurchaseOrders
                                            where x.VendorID == vendorID
                                            where x.PurchaseOrderNumber == null && x.OrderDate == null
                                            select x).FirstOrDefault();
                //1
                if (existOrder == null)
                {
                    existOrder = new PurchaseOrder();
                    existOrder.VendorID = vendorID;
                    existOrder.EmployeeID = employeeID;  //Change this latter!!!!!!!
                    existOrder.TaxAmount = 0;
                    existOrder.SubTotal = 0;
                    existOrder.Closed = false;
                    context.PurchaseOrders.Add(existOrder);
                    //1.1
                    //suggested order view 
                    var theNewDetailListItem = (from x in context.StockItems
                                                where x.VendorID == vendorID
                                                where x.ReOrderLevel - x.QuantityOnHand - x.QuantityOnOrder > 0
                                                select new CurrentActiveOrderView
                                                {
                                                    SID = x.StockItemID,
                                                    Description = x.Description,
                                                    QOH = x.QuantityOnHand,
                                                    QOO = x.QuantityOnOrder,
                                                    ROL = x.ReOrderLevel,
                                                    QTO = x.ReOrderLevel - x.QuantityOnHand - x.QuantityOnOrder,
                                                    //Buffer = a.Key.QuantityOnHand + a.Key.QuantityOnOrder - a.Key.ReOrderLevel,
                                                    Price = x.PurchasePrice
                                                }).ToList();
                    //add order details
                    foreach (var item in theNewDetailListItem)
                    {
                        PurchaseOrderDetail additem = new PurchaseOrderDetail();
                        additem.StockItemID = item.SID;
                        additem.PurchasePrice = item.Price;
                        additem.Quantity = item.QTO;
                        existOrder.PurchaseOrderDetails.Add(additem);

                        subTotal += (item.Price * item.QTO);
                        tax += ((item.Price * item.QTO) * (decimal)0.05);
                    }

                    existOrder.SubTotal = subTotal;
                    existOrder.TaxAmount = tax;
                    context.SaveChanges();
                    return theNewDetailListItem;
                }//end of step 1
                else
                {
                    var theExistDetailListItem = (from x in context.PurchaseOrderDetails
                                                  where x.PurchaseOrder.VendorID == vendorID
                                                  where x.PurchaseOrder.PurchaseOrderNumber == null && x.PurchaseOrder.OrderDate == null
                                                  select new CurrentActiveOrderView
                                                  {
                                                      SID = x.StockItem.StockItemID,
                                                      Description = x.StockItem.Description,
                                                      QOH = x.StockItem.QuantityOnHand,
                                                      QOO = x.StockItem.QuantityOnOrder,
                                                      ROL = x.StockItem.ReOrderLevel,
                                                      QTO = x.Quantity,
                                                      Price = x.PurchasePrice
                                                  }).ToList();
                    if (theExistDetailListItem.Count==0)
                    {
                        return null;
                    }
                    else
                    {
                        return theExistDetailListItem;
                    }
                }//end of step 2  
            }//eou
        }//eom

        public List<VendorStockItemsView> List_VendorStockItemsPOCO(int vendorID, List<CurrentActiveOrderView> orderDetailPOCOList)
        {
            //List<string> businessEXResons = new List<string>();
            using (var context = new eToolsContext())
            {
                StockItem theStockItems = (from x in context.StockItems
                                           where x.VendorID == vendorID
                                           select x).FirstOrDefault();
                if (theStockItems == null)
                {
                    return null;
                }
                else
                {
                    var theStockItemList = (from x in context.StockItems
                                            where x.VendorID == vendorID
                                            where x.Discontinued == false
                                            select new VendorStockItemsView
                                            {
                                                SID = x.StockItemID,
                                                ItemDescription = x.Description,
                                                QOH = x.QuantityOnHand,
                                                QOO = x.QuantityOnOrder,
                                                ROL = x.ReOrderLevel,
                                                Buffer = x.QuantityOnHand + x.QuantityOnOrder - x.ReOrderLevel,
                                                Price = x.PurchasePrice
                                            }).ToList();
                    if (orderDetailPOCOList == null)
                    {
                        return theStockItemList;
                    }
                    else
                    {
                        var theVendorStockItemPOCOView = theStockItemList.Where(xSList => !orderDetailPOCOList.Any(xOList => xOList.SID == xSList.SID)).ToList();
                        return theVendorStockItemPOCOView;
                    }//eol                    
                }//eol
            }//eou
        }//eom

        public string AddDBPOPODetails(int employeeID, int vendorID, List<CurrentActiveOrderView> currentOrderListPOCOs)
        {
            using (var context = new eToolsContext())
            {
                decimal subTotal = 0;
                decimal tax = 0;
               
                PurchaseOrder existOrder = (from x in context.PurchaseOrders
                                  where x.VendorID == vendorID
                                  where x.PurchaseOrderNumber == null && x.OrderDate == null
                                  select x).FirstOrDefault();
                if (existOrder == null)
                {
                    existOrder = new PurchaseOrder();
                    existOrder.VendorID = vendorID;
                    existOrder.EmployeeID = employeeID;  //Change this latter!!!!!!!
                    existOrder.TaxAmount = 0;
                    existOrder.SubTotal = 0;
                    existOrder.Closed = false;
                    context.PurchaseOrders.Add(existOrder);
                }                
                
                    //add list, poco list with sid not in existing list
                var toAddList = currentOrderListPOCOs.Where(poco => !existOrder.PurchaseOrderDetails.Any(detail => detail.StockItemID == poco.SID));
                if (toAddList != null)
                {
                    foreach (var item in toAddList)
                    {
                        //add new details
                        PurchaseOrderDetail newPODetail = new PurchaseOrderDetail();
                        newPODetail.StockItemID = item.SID;
                        newPODetail.PurchasePrice = item.Price;
                        newPODetail.Quantity = item.QTO;
                        existOrder.PurchaseOrderDetails.Add(newPODetail);

                        //subTotal += (item.Price * item.QTO);
                        //tax += ((item.Price * item.QTO) * (decimal)0.05);
                    }
                    //existOrder.SubTotal += subTotal;
                    //existOrder.TaxAmount += tax;
                }

                if (existOrder.PurchaseOrderDetails.Count >1)
                {
                    string updateMsg = UpdateDBPOPODetails(vendorID, currentOrderListPOCOs);
                }

                //existOrder = context.PurchaseOrders.Where(x => x.VendorID.Equals(vendorID) && x.OrderDate.Equals(null) && x.PurchaseOrderNumber.Equals(null)).FirstOrDefault();
                //var priceList = context.PurchaseOrders.Where(x=>x.VendorID.Equals(vendorID)&&x.OrderDate.Equals(null)&&x.PurchaseOrderNumber.Equals(null)).FirstOrDefault().PurchaseOrderDetails.ToList();
                var priceList = currentOrderListPOCOs;
                foreach (var item in priceList)
                {
                    subTotal += item.Price * item.QTO;
                    tax += (item.Price * item.QTO) * (decimal)0.05;
                }
                existOrder.SubTotal = subTotal;
                existOrder.TaxAmount = tax;
                
                context.SaveChanges();
                
                return toAddList.Count().ToString() + " item has been added";
            }//eou
               
         }//eom

        //public string RemoveDBPOPODetails(int vendorID, List<CurrentActiveOrderView> currentOrderListPOCOs)
        //{
        //    using (var context = new eToolsContext())
        //    {
        //        decimal subTotal = 0;
        //        decimal tax = 0;
        //        string returnString = "no existing order";                
        //        PurchaseOrder existOrder = (from x in context.PurchaseOrders
        //                                    where x.VendorID == vendorID
        //                                    where x.PurchaseOrderNumber == null && x.OrderDate == null
        //                                    select x).FirstOrDefault();

        //        //PurchaseOrderDetail checktoRemoveItem = existOrder.PurchaseOrderDetails.Where(detail => !currentOrderListPOCOs.Any(poco => poco.SID == detail.StockItemID)).FirstOrDefault();

        //        if (existOrder != null)
        //        {
        //            List<PurchaseOrderDetail> toRemovePODetailList = existOrder.PurchaseOrderDetails
        //                                                            .Where(detail => !currentOrderListPOCOs.Any(poco => poco.SID == detail.StockItemID)).ToList();
        //            foreach (var item in toRemovePODetailList)
        //            {
        //                subTotal += item.PurchasePrice * item.Quantity;
        //                tax += (item.PurchasePrice * item.Quantity) * (decimal)0.05;
        //                context.PurchaseOrderDetails.Remove(item);
        //            }
        //            existOrder.SubTotal -= subTotal;
        //            existOrder.TaxAmount -= tax;

        //            returnString = "items removed" + toRemovePODetailList.Count; 
        //            //in case the data base is ever messed up, and you have a negative purchase order, purge the Order details
        //            if (existOrder.SubTotal < 0 )
        //            {
        //                existOrder.SubTotal = 0;
        //                existOrder.TaxAmount = 0;
        //                List<PurchaseOrderDetail> deleteAllDetals = (from x in context.PurchaseOrderDetails
        //                                                             where x.PurchaseOrderID == existOrder.PurchaseOrderID
        //                                                             select x).ToList();
        //                foreach (var item in deleteAllDetals)
        //                {
        //                    context.PurchaseOrderDetails.Remove(item);
        //                }
        //                returnString = "subtotal <0, therefore all items in the orderdetails are removed, hit fetch for new suggested order" + toRemovePODetailList.Count;
        //            }//end of if--the extreme condition
                    
        //        }                           
        //        context.SaveChanges();
        //        return returnString;
        //    }//eou

        //}//eom

        public string DeletePurchaseOrder (int vendorID)
        {
            using (var context = new eToolsContext())
            {
                string msg;
                PurchaseOrder existOrder = (from x in context.PurchaseOrders
                                            where x.VendorID == vendorID
                                            where x.PurchaseOrderNumber == null && x.OrderDate == null
                                            select x).FirstOrDefault();

                if (existOrder == null)
                {
                    throw new Exception("Order does not exist");
                }
                else
                {
                    if (existOrder.PurchaseOrderDetails.Count > 0)
                    {
                        List<PurchaseOrderDetail> deleteAllDetals = (from x in context.PurchaseOrderDetails
                                                                     where x.PurchaseOrderID == existOrder.PurchaseOrderID
                                                                     select x).ToList();
                        foreach (var item in deleteAllDetals)
                        {
                            context.PurchaseOrderDetails.Remove(item);
                        }

                        context.PurchaseOrders.Remove(existOrder);
                    }

                    else
                    {
                        context.PurchaseOrders.Remove(existOrder);
                    }
                }
                msg = "The active orderID " + existOrder.PurchaseOrderID + " which assoicated with Vendor " + existOrder.VendorID + "has been deleted at " + DateTime.Now.ToString(); 
                context.SaveChanges();
                return msg;

            }
        }

        public string UpdateDBPOPODetails (int vendorID, List<CurrentActiveOrderView> currentOrderListPOCOs)
        {
            using (var context = new eToolsContext())
            { 
                decimal subtotal = 0;
                //decimal subtotal = 0;
                decimal tax = 0;
                //1. check existing order
                var existOrder = (from x in context.PurchaseOrders
                                  where x.VendorID == vendorID
                                  where x.PurchaseOrderNumber == null && x.OrderDate == null
                                  select x).FirstOrDefault();
                if (existOrder == null)
                {
                    throw new Exception("error, order does not exist");
                }
                else
                {
                    //change list: poco.sid == detail.sid
                    List<PurchaseOrderDetail> changeList = existOrder.PurchaseOrderDetails.ToList();                  


                    foreach (var item in changeList)
                    {
                        var pocoItem = currentOrderListPOCOs.Where(poco => poco.SID == item.StockItemID).FirstOrDefault();
                        if (pocoItem != null)
                        {
                            item.PurchasePrice = pocoItem.Price;                            
                            item.Quantity = pocoItem.QTO;
                            subtotal += item.PurchasePrice * item.Quantity;
                            tax += (item.PurchasePrice * item.Quantity) * (decimal)0.05;
                        }
                        
                        
                        context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }
                    
                    existOrder.SubTotal = subtotal;
                    existOrder.TaxAmount = tax;
                    context.Entry(existOrder).State = System.Data.Entity.EntityState.Modified;
                }//endofelse
                string msg = "Order " + existOrder.PurchaseOrderID.ToString() + "with " + existOrder.PurchaseOrderDetails.Count + " items was updated at " + DateTime.Now.ToString();
                context.SaveChanges();
                return msg;
            }//eou
        }//eom


        public int Place_Order(int venderID, List<CurrentActiveOrderView> currentOrderListPOCOs)
        {
            using (var context = new eToolsContext())
            {
                PurchaseOrder placeOrder = context.PurchaseOrders.Where(x => x.VendorID.Equals(venderID) && (x.PurchaseOrderNumber == null && x.OrderDate == null)).FirstOrDefault();

                string updateMsg = UpdateDBPOPODetails(venderID, currentOrderListPOCOs);

                placeOrder.OrderDate = DateTime.Now;
                placeOrder.PurchaseOrderNumber = context.PurchaseOrders.Max(x => x.PurchaseOrderNumber)==null? 1 : context.PurchaseOrders.Max(x => x.PurchaseOrderNumber)+1;
                foreach (var item in placeOrder.PurchaseOrderDetails)
                {
                    item.StockItem.QuantityOnOrder += item.Quantity;
                    context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
                context.SaveChanges();
                return placeOrder.PurchaseOrderID;
            }
        }//eom

        public string RemovingASingleRowFromActiveOrder(int vendorID, int sID)
        {
            using (var context = new eToolsContext())
            {

                decimal subtotal = 0;
                decimal tax = 0;
                //1. remove the item from DB
                PurchaseOrderDetail toRemove = context.PurchaseOrderDetails
                    .Where(x => x.PurchaseOrder.VendorID.Equals(vendorID) && x.PurchaseOrder.PurchaseOrderNumber.Equals(null) && x.PurchaseOrder.OrderDate.Equals(null) && x.StockItemID.Equals(sID))
                    .FirstOrDefault();

                
                context.PurchaseOrderDetails.Remove(toRemove);
                //2. Calculate the subtotal and tax from the orderDetail
                var detailList = context.PurchaseOrders.Where(x => x.VendorID.Equals(vendorID) && x.OrderDate.Equals(null) && x.PurchaseOrderNumber.Equals(null)).FirstOrDefault().PurchaseOrderDetails.ToList();
                if (detailList.Count>0)
                {
                    foreach (var item in detailList)
                    {
                        subtotal += item.Quantity * item.PurchasePrice;
                        tax += (item.Quantity * item.PurchasePrice) * (decimal)0.05;
                    }
                }
                //3. update the order
                PurchaseOrder changedPO = context.PurchaseOrders.Where(x => x.VendorID.Equals(vendorID) && x.OrderDate.Equals(null) && x.PurchaseOrderNumber.Equals(null)).FirstOrDefault();
                changedPO.SubTotal = subtotal;
                changedPO.TaxAmount = tax;
                //4. save change.
                context.SaveChanges();
                return toRemove.StockItemID.ToString();
            }
            
        }//eom

        public List<CurrentActiveOrderView> RemovingWhileUpdating(int employeeID, int vendorID, List<CurrentActiveOrderView> currentOrderListPOCOs)
        {
            using (var context = new eToolsContext())
            {
                //decimal subtotal = 0;
                //decimal tax = 0;
                var existOrder = (from x in context.PurchaseOrders
                                  where x.VendorID == vendorID
                                  where x.PurchaseOrderNumber == null && x.OrderDate == null
                                  select x).FirstOrDefault();


                if (existOrder == null)
                {
                    throw new Exception("no exsiting order");
                }
                else
                {
                   
                    List<PurchaseOrderDetail> toRemovePODetailList = existOrder.PurchaseOrderDetails
                                                                        .Where(detail => !currentOrderListPOCOs.Any(poco => poco.SID == detail.StockItemID)).ToList();
                    foreach (var item in toRemovePODetailList)
                    {
                        context.PurchaseOrderDetails.Remove(item);                           
                    }
                    if (existOrder.PurchaseOrderDetails.Count() == 0)
                    {
                        existOrder.SubTotal = 0;
                        existOrder.TaxAmount = 0;
                        context.SaveChanges();
                        return null;
                    }
                    context.SaveChanges();                                  
                    string updateMsg = UpdateDBPOPODetails(vendorID, currentOrderListPOCOs);                            
                    return List_OrderDetailPOCO(employeeID,vendorID);                      
                   
                }//end of else

                              

             }//eou
        }//eom

        
    }


}