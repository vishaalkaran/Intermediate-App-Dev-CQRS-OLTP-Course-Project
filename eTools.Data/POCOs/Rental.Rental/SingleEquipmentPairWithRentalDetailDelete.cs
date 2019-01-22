using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eTools.Data.Entities;
#endregion

namespace eTools.Data.POCOs
{
    public class SingleEquipmentPairWithRentalDetailDelete
    {
        public RentalDetail _RentalDetailTable { get; set; }
        public RentalEquipment _RentalEquipmentTable { get; set; }
    }
}
