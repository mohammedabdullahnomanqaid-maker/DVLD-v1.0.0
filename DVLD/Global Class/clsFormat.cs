using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course19
{
    public class clsFormat
    {
        static public string DateToShort(DateTime dateTime)
        {
            return dateTime.ToString("dd/MMM/yyyy");
        }
    }
}
