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
    public class UnorderedPurchaseItemCartController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<UnorderedPurchaseItemCart> Get_ListUnorderedPurchaseItemCart()
        {
            using(var context = new eToolsContext())
            {
                return context.UnorderedPurchaseItemCart.ToList();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void Add_UnorderedItemToCart(UnorderedPurchaseItemCart item)
        {
            using(var context = new eToolsContext())
            {
                context.UnorderedPurchaseItemCart.Add(item);
                context.SaveChanges();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void Delete_UnorderedPurchaseItemCart(UnorderedPurchaseItemCart item)
        {
            Delete_UnorderedPurchaseItemCart(item.CartID);
        }
        public void Delete_UnorderedPurchaseItemCart(int cartid)
        {
            using (var context = new eToolsContext())
            {
                var existing = context.UnorderedPurchaseItemCart.Find(cartid);
                if (existing == null)
                {
                    throw new Exception("Unordered item does not exist on file.");
                }
                context.UnorderedPurchaseItemCart.Remove(existing);
                context.SaveChanges();
            }
        }
    }
}
