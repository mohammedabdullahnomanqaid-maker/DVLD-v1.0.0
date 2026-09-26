using System;
using System.Configuration;

namespace DataAccessLayer
{
   static class clsConnection
    {
       static public string ConnectionString =ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString ;
    }

}

