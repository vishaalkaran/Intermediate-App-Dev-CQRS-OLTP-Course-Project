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
    public class SalesDetailController
    {
        public void PlaceOrder(Sale sale, ShoppingCart cart)
        {
            using(var context = new eToolsContext())
            {
                context.Sales.Add(sale);
                List<ShoppingCartItem> cartItems = context.ShoppingCartItems.Where(x => x.ShoppingCartID.Equals(cart.ShoppingCartID)).Select(x => x).ToList();
                foreach (ShoppingCartItem cartItem in cartItems)
                {
                    StockItem stockItem = context.StockItems.Where(x => x.StockItemID.Equals(cartItem.StockItemID)).FirstOrDefault();
                    SaleDetail saleDetail = new SaleDetail();
                    saleDetail.StockItemID = cartItem.StockItemID;
                    saleDetail.SellingPrice = stockItem.SellingPrice;
                    saleDetail.Quantity = cartItem.Quantity;
                    if (saleDetail.Quantity > (stockItem.QuantityOnHand + stockItem.QuantityOnOrder))
                        throw new Exception("Sale cannot proceed. " + stockItem.Description + " is out of stock.");
                    saleDetail.Backordered = saleDetail.Quantity > stockItem.QuantityOnHand ? true : false;
                    sale.SaleDetails.Add(saleDetail);
                    stockItem.QuantityOnHand = saleDetail.Quantity > stockItem.QuantityOnHand ? 0 : stockItem.QuantityOnHand - saleDetail.Quantity;
                    context.Entry(stockItem).State = System.Data.Entity.EntityState.Modified;
                    context.ShoppingCartItems.Remove(cartItem);
                }
                ShoppingCart theCart = context.ShoppingCarts.Where(x => x.ShoppingCartID.Equals(cart.ShoppingCartID)).FirstOrDefault();
                context.ShoppingCarts.Remove(theCart);
                context.SaveChanges();
            }
        }
    }
}
