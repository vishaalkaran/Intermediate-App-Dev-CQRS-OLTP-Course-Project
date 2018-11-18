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
#endregion


namespace eToolsSystem.BLL
{
    [DataObject]
    public class VendorController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> List_VendorNames()
        {
            using (var context = new eToolsContext())
            {
                var results = from x in context.Vendors
                              orderby x.VendorID
                              select new SelectionList
                              {
                                  IDValueField = x.VendorID,
                                  DisplayText = x.VendorName
                              };
                return results.ToList();
            }
        }
        public Vendor Get_Vendor(int vendorID)
        {
            using (var context = new eToolsContext())
            {
                return context.Vendors.Find(vendorID);
            }
        }
    }
}
