using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using eTools.Data.Entities;
using eTools.Data.POCOs;
using eToolsSystem.DAL;
using System.ComponentModel;

namespace eToolsSystem.BLL
{
    [DataObject]
    public class StockItemController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ProductSelection> Get_StockItemsByCategory(int categoryid)
        {
            using (var context = new eToolsContext())
            {
                List<ProductSelection> products = (from x in context.StockItems
                                                    where (x.CategoryID.Equals(categoryid) && x.Discontinued.Equals(false))
                                                    select new ProductSelection
                                                    {
                                                        StockItemID = x.StockItemID,
                                                        QuantitySelected = 1,
                                                        SellingPrice = x.SellingPrice,
                                                        Description = x.Description,
                                                        QuantityOnHand = x.QuantityOnHand
                                                    }).ToList();
                return products;
            }
        }
    }
}
