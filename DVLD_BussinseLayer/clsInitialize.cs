using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BussinseLayer
{
   static public class clsInitialize
    {
        static public void InitializeDatabase()
        {
            clsDatabaseInitializer.Initializer();
        }
    }
}
