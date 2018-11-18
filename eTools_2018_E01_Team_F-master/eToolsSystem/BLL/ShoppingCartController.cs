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
    [DataObject]
    public class ShoppingCartController
    {
        public ShoppingCart Get_ShoppingCartByEmployeeID(int employeeid)
        {
            using(var context = new eToolsContext())
            {
                return context.ShoppingCarts.Where(x => x.EmployeeID.Equals(employeeid)).FirstOrDefault();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CartSelection> Get_CartItemsByEmployeeID(int employeeid)
        {
            using (var context = new eToolsContext())
            {
                List<CartSelection> cartitems = (from x in context.ShoppingCartItems
                                                 where (x.ShoppingCart.EmployeeID.Equals(employeeid))
                                                 select new CartSelection
                                                 {
                                                     ShoppingCartitemID = x.ShoppingCartItemID,
                                                     SellingPrice = x.StockItem.SellingPrice,
                                                     Description = x.StockItem.Description,
                                                     Total = x.StockItem.SellingPrice * x.Quantity,
                                                     QuantitySelected = x.Quantity
                                                 }).ToList();
                return cartitems;
            }
        }
        public void Update_ShoppingCartFromDisplay(List<CartSelection> cart)
        {
            using(var context = new eToolsContext())
            {
                bool cartUpdated = false;
                int shoppingcartID = 0;
                foreach(CartSelection cartitem in cart)
                {
                    ShoppingCartItem item = context.ShoppingCartItems.Find(cartitem.ShoppingCartitemID);
                    //check to see if cart has actually been updated, if so, update ShoppingCart UpdatedOn
                    if (item.Quantity != cartitem.QuantitySelected)
                    {
                        item.Quantity = cartitem.QuantitySelected;
                        cartUpdated = true;
                        shoppingcartID = item.ShoppingCartID;
                    }
                    context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
                if (cartUpdated)
                {
                    //set updated property in associated shopping cart
                    ShoppingCart shoppingCart = context.ShoppingCarts.Find(shoppingcartID);
                    shoppingCart.UpdatedOn = DateTime.Now;
                    context.Entry(shoppingCart).State = System.Data.Entity.EntityState.Modified;
                }
                context.SaveChanges();
            }
        }
        public void Add_ProductToShoppingCart(int employeeid, int stockitemid, int qty)
        {
            List<string> reasons = new List<string>();

            using (var context = new eToolsContext())
            {
                ShoppingCart exists = context.ShoppingCarts
                    .Where(x => x.EmployeeID.Equals(employeeid)).Select(x => x).FirstOrDefault();
                ShoppingCartItem newItem = null;

                if (exists == null)
                {
                    exists = new ShoppingCart();
                    exists.EmployeeID = employeeid;
                    exists.CreatedOn = DateTime.Now;
                    exists = context.ShoppingCarts.Add(exists);
                }
                else
                {
                    newItem = exists.ShoppingCartItems.SingleOrDefault(x => x.StockItemID == stockitemid);
                    if (newItem != null)
                    {
                        reasons.Add("Item already exists in the cart.");
                    }
                }

                if (reasons.Count() > 0)
                {
                    throw new BusinessRuleException("Adding item to cart", reasons);
                }
                else
                {
                    newItem = new ShoppingCartItem();
                    newItem.StockItemID = stockitemid;
                    newItem.Quantity = qty;
                    exists.ShoppingCartItems.Add(newItem);
                    exists.UpdatedOn = DateTime.Now;

                    context.SaveChanges();
                }
            }
        }
    }
}
