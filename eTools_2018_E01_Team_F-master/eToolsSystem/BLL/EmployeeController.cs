using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eTools.Data.Entities;
using eToolsSystem.DAL;
using System.ComponentModel;
using eTools.Data.DTOs;
using eTools.Data.POCOs;
#endregion


namespace eToolsSystem.BLL
{
    [DataObject]
    public class EmployeeController
    {
        public Employee Employee_Get(int employeeid)
        {
            //!!!THIS METHOD IS FOR AppSecurity/BLL/ApplicationUserManager!!!
            using (var context = new eToolsContext())
            {
                return context.Employees.Find(employeeid);
            }
        }
    }
}
