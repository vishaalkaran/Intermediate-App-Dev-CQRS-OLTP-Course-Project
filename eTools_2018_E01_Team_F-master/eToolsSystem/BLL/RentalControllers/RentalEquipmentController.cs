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
    public class RentalEquipmentController
    {
        List<string> logger = new List<string>();

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<EquipmentSelection> allAvailableEquipmentList()
        {
            using (var context = new eToolsContext())
            {
                List<EquipmentSelection> availableEquipment = context.RentalEquipments
                                                                     .Where(x => (x.Available == true))
                                                                     .Select(x =>
                                                                            new EquipmentSelection()
                                                                            {
                                                                                eqiupmentID = x.RentalEquipmentID,
                                                                                Description = x.Description,
                                                                                SerailNumber = x.SerialNumber,
                                                                                Rate = x.DailyRate
                                                                            }).ToList();
                return availableEquipment;
            }
        } 
    }
}
