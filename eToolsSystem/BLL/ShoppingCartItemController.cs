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
    public class ShoppingCartItemController
    {
        public ShoppingCartItem Get_ShoppingCartItem(int itemID)
        {
            using(var context = new eToolsContext())
            {
                return context.ShoppingCartItems.Find(itemID);
            }
        }
        public void Delete_ShoppingCartItem(int itemid)
        {
            using (var context = new eToolsContext())
            {
                var existing = context.ShoppingCartItems.Find(itemid);
                if (existing == null)
                {
                    throw new Exception("Shopping cart item does not exist on file.");
                }
                context.ShoppingCartItems.Remove(existing);
                context.SaveChanges();
            }
        }
        public void Update_ShoppingCartItem(ShoppingCartItem item)
        {
            using(var context = new eToolsContext())
            {
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
