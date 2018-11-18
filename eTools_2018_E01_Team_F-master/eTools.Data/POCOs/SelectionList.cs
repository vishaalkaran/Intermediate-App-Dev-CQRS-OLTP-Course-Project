using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace eTools.Data.POCOs
{

    public class SelectionList
    {
        //this poco is going to be used on all DDL, therefore needs a display text and a value field
        public int IDValueField { get; set; }
        public string DisplayText { get; set; }
    }
}
