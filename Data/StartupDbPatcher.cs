using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace GymPower.Data
{
    public static class StartupDbPatcher
    {
        public static void FixOrdersTableSchema(AppDbContext db)
        {
            // Open raw SQLite connection
            var conn = db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) conn.Open();

            // Read current columns in Orders
            var existingCols = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "PRAGMA table_info(Orders);";
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    // PRAGMA table_info returns: cid, name, type, notnull, dflt_value, pk
                    var name = rdr.GetString(1);
                    existingCols.Add(name);
                }
            }

            // Required columns that match your Order model
            var required = new[]
            {
                "Id",
                "CustomerName",
                "Address",
                "Email",
                "Phone",
                "PaymentMethod",
                "OrderDate",
                "TotalPrice",
                "Status",
                "UserId"
            };

            // If any are missing, rebuild Orders table safely
            if (!required.All(existingCols.Contains))
            {
                using var tx = conn.BeginTransaction();

                // Create a new table with the correct schema
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Orders_new (
    Id            INTEGER NOT NULL PRIMARY KEY,
    CustomerName  TEXT    NOT NULL,
    Address       TEXT    NOT NULL,
    Email         TEXT    NOT NULL,
    Phone         TEXT    NOT NULL,
    PaymentMethod TEXT    NOT NULL,
    OrderDate     TEXT    NOT NULL,
    TotalPrice    REAL    NOT NULL,
    Status        TEXT    NOT NULL,
    UserId        INTEGER NULL
);";
                    cmd.ExecuteNonQuery();
                }

                // Copy over any existing data that overlaps (use COALESCE defaults)
                // If old table lacks columns, provide safe defaults.
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
INSERT INTO Orders_new (Id, CustomerName, Address, Email, Phone, PaymentMethod, OrderDate, TotalPrice, Status, UserId)
SELECT
    Id,
    COALESCE(CustomerName,  'Гост'),
    COALESCE(Address,       ''),
    COALESCE(Email,         ''),
    COALESCE(Phone,         ''),
    COALESCE(PaymentMethod, 'Наложен платеж'),
    COALESCE(OrderDate,     datetime('now')),
    COALESCE(TotalPrice,    0.0),
    COALESCE(Status,        'Обработва се'),
    CASE
        WHEN EXISTS(SELECT 1 FROM pragma_table_info('Orders') WHERE name='UserId')
        THEN UserId
        ELSE NULL
    END
FROM Orders;";
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        // If the old Orders table is too different, just continue with empty data.
                    }
                }

                // Drop old and rename new
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"DROP TABLE IF EXISTS Orders;";
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"ALTER TABLE Orders_new RENAME TO Orders;";
                    cmd.ExecuteNonQuery();
                }

                tx.Commit();
            }
        }
    }
}