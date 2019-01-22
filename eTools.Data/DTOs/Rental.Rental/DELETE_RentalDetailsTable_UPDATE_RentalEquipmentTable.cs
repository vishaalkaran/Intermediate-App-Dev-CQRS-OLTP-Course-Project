using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eTools.Data.POCOs;
#endregion

namespace eTools.Data.DTOs
{
    public class DELETE_RentalDetailsTable_UPDATE_RentalEquipmentTable
    {
        public IEnumerable<SingleEquipmentPairWithRentalDetailDelete> deleteSet { get; set; }
    }
}
