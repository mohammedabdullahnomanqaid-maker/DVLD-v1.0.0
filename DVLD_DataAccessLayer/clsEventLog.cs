using System;
using System.Diagnostics;

namespace DataAccessLayer
{
   static public class clsEventLog
    {
      private static string sourceName = "DVLD";
      private static string LogName = "Application";

        static private bool CreateEventLogger()
        {

            try
            {
                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName,LogName);
                }

            }
            catch
            {
                return false;
            }
            return true;
        }

        static public void EventLogError(string message)
        {
           if( CreateEventLogger())
            EventLog.WriteEntry(sourceName, message, EventLogEntryType.Error);
        }

        static public void EventLogWarning(string message)
        {
            if(CreateEventLogger())
            EventLog.WriteEntry(sourceName, message, EventLogEntryType.Warning);
        }

        static public void EventLogInformation(string message)
        {
           if( CreateEventLogger())
            EventLog.WriteEntry(sourceName, message, EventLogEntryType.Information);
        }


    }
}
