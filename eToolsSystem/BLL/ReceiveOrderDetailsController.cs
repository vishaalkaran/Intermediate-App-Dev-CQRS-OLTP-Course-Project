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
    public class ReceiveOrderDetailsController
    {
        public void ReceiveOrder(int purchaseOrderID, List<OpenPurchaseOrderDetails> openPODetails)
        {
            //openpodetails and unordereditems collected in code-behind from controls on page
            using (var context = new eToolsContext())
            {
                UnorderedPurchaseItemCartController unorderedItemsController = new UnorderedPurchaseItemCartController();
                List<UnorderedPurchaseItemCart> unorderedItems = unorderedItemsController.Get_ListUnorderedPurchaseItemCart();

                //transaction
                //create a receiveorder for the purchaseorderid
                ReceiveOrder receiveOrder = new ReceiveOrder();
                receiveOrder.PurchaseOrderID = purchaseOrderID;
                receiveOrder.ReceiveDate = DateTime.Now;
                context.ReceiveOrders.Add(receiveOrder);
                //create receiveorderdetails for each openPODetails if there is anything entered in receivedquantity
                foreach(OpenPurchaseOrderDetails openPO in openPODetails)
                {
                    //make a receiveorderdetail if quantityreceived > 0 and update stockitems, make a returnorderdetail if quantityreturned > 0
                    if(openPO.ReceivedQuantity > 0)
                    {
                        ReceiveOrderDetail rOD = new ReceiveOrderDetail();
                        rOD.ReceiveOrderID = receiveOrder.ReceiveOrderID;
                        rOD.PurchaseOrderDetailID = openPO.PurchaseOrderDetailID;
                        rOD.QuantityReceived = openPO.ReceivedQuantity;
                        openPO.QuantityOutstanding -= openPO.ReceivedQuantity;
                        context.ReceiveOrderDetails.Add(rOD);
                        //adjust stockitem QoH (+ReceivedQuantity) and QoO (-ReceivedQuantity) with same stockitemid
                        StockItem stockItem = context.StockItems.Find(openPO.StockItemID);
                        stockItem.QuantityOnHand += openPO.ReceivedQuantity;
                        stockItem.QuantityOnOrder -= openPO.ReceivedQuantity;
                        context.Entry(stockItem).State = System.Data.Entity.EntityState.Modified;
                    }
                    if(openPO.ReturnedQuantity > 0)
                    {
                        ReturnedOrderDetail reOD = new ReturnedOrderDetail();
                        reOD.ReceiveOrderID = receiveOrder.ReceiveOrderID;
                        reOD.PurchaseOrderDetailID = openPO.PurchaseOrderDetailID;
                        reOD.ItemDescription = openPO.StockItemDescription;
                        reOD.Quantity = openPO.ReturnedQuantity;
                        reOD.Reason = openPO.ReturnReason;
                        reOD.VendorStockNumber = openPO.VendorStockNumber;
                        context.ReturnedOrderDetails.Add(reOD);
                    }
                }
                //create returnorderdetail for each unorderedItems and delete them from database
                UnorderedPurchaseItemCartController unOsysmgr = new UnorderedPurchaseItemCartController();
                foreach(UnorderedPurchaseItemCart item in unorderedItems)
                {
                    ReturnedOrderDetail reOD = new ReturnedOrderDetail();
                    reOD.ReceiveOrderID = receiveOrder.ReceiveOrderID;
                    reOD.ItemDescription = item.Description;
                    reOD.Quantity = item.Quantity;
                    reOD.VendorStockNumber = item.VendorStockNumber;
                    context.ReturnedOrderDetails.Add(reOD);
                    unOsysmgr.Delete_UnorderedPurchaseItemCart(item.CartID);
                }
                //if no purchaseorderdetails have > 0 QOS, close the order
                if(!(openPODetails.Any(x => x.QuantityOutstanding > 0)))
                {
                    PurchaseOrder pOrder = context.PurchaseOrders.Find(purchaseOrderID);
                    pOrder.Closed = true;
                    context.Entry(pOrder).State = System.Data.Entity.EntityState.Modified;
                }
                context.SaveChanges();
            }
        }
    }
}
