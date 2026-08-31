using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace DataAccessLayer
{
    static public class clsDatabaseInitializer
    {
        private const string DatabaseName = "DVLD";

        public static void Initializer()
        {
            // 1. إنشاء اتصال مع SQL Server نفسه
            // وليس مع قاعدة بيانات DVLD
            SqlConnectionStringBuilder builder =
                new SqlConnectionStringBuilder(
                    clsConnection.ConnectionString);

            builder.InitialCatalog = "master";

            // 2. إنشاء قاعدة البيانات إذا لم تكن موجودة
            using (SqlConnection connection =
                new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string createDatabaseQuery = @"
IF DB_ID(N'DVLD') IS NULL
BEGIN
    CREATE DATABASE [DVLD]
END";

                using (SqlCommand command =
                    new SqlCommand(createDatabaseQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            // 3. تحديد مكان ملف SQL داخل المشروع
            string scriptPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Database",
                "DVLD.sql"
            );

            // 4. التأكد أن ملف SQL موجود
            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException("لم يتم العثور على ملف قاعدة البيانات: "+ scriptPath);
            }

            // 5. الاتصال بقاعدة DVLD
            builder.InitialCatalog = DatabaseName;

            using (SqlConnection connection =
                new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                // 6. التأكد هل قاعدة البيانات مهيأة مسبقًا
                // نستخدم أول جدول للتأكد أن السكربت تم تنفيذه
                string checkTableQuery = @"
SELECT COUNT(*)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = 'Applications'";

                using (SqlCommand checkCommand =
                    new SqlCommand(checkTableQuery, connection))
                {
                    int tableCount =
                        (int)checkCommand.ExecuteScalar();

                    // إذا كان جدول Applications موجودًا
                    // فهذا يعني أن قاعدة البيانات مهيأة
                    if (tableCount > 0)
                        return;
                }

                // 7. قراءة ملف SQL كاملًا
                string script = File.ReadAllText(scriptPath);

                // 8. تقسيم السكربت عند GO
                string[] batches = Regex.Split(
                    script,
                    @"^\s*GO\s*$",
                    RegexOptions.Multiline |
                    RegexOptions.IgnoreCase
                );

                // 9. تنفيذ كل جزء من السكربت
                foreach (string batch in batches)
                {
                    if (string.IsNullOrWhiteSpace(batch))
                        continue;

                    using (SqlCommand command =
                        new SqlCommand(batch, connection))
                    {
                        command.CommandTimeout = 120;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

    }
}
    





   
