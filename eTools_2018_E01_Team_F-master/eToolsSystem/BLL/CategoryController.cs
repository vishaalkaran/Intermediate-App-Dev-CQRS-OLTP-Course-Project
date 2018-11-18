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
    public class CategoryController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Category> Get_CategoryList()
        {
            using(var context = new eToolsContext())
            {
                return context.Categories.ToList();
            }
        }
    }
}
