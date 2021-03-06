using System;
using System.Data;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = GetConnectionString();
        AdapterUpdate(connectionString);
        Console.ReadLine();
    }
    // <Snippet1>
    private static void AdapterUpdate(string connectionString)
    {
        using (SqlConnection connection =
                   new SqlConnection(connectionString))
        {
            SqlDataAdapter dataAdpater = new SqlDataAdapter(
              "SELECT CategoryID, CategoryName FROM Categories",
              connection);

            dataAdpater.UpdateCommand = new SqlCommand(
               "UPDATE Categories SET CategoryName = @CategoryName " +
               "WHERE CategoryID = @CategoryID", connection);

            dataAdpater.UpdateCommand.Parameters.Add(
               "@CategoryName", SqlDbType.NVarChar, 15, "CategoryName");

            SqlParameter parameter = dataAdpater.UpdateCommand.Parameters.Add(
              "@CategoryID", SqlDbType.Int);
            parameter.SourceColumn = "CategoryID";
            parameter.SourceVersion = DataRowVersion.Original;

            DataTable categoryTable = new DataTable();
            dataAdpater.Fill(categoryTable);

            DataRow categoryRow = categoryTable.Rows[0];
            categoryRow["CategoryName"] = "New Beverages";

            dataAdpater.Update(categoryTable);

            Console.WriteLine("Rows after update.");
            foreach (DataRow row in categoryTable.Rows)
            {
                {
                    Console.WriteLine("{0}: {1}", row[0], row[1]);
                }
            }
        }
    }
    // </Snippet1>

    static void UpdateCustomers(DataSet dataSet, SqlDataAdapter adapter)
    {
        // <Snippet2>
        // Assumes that dataSet and adapter are valid objects.
        DataTable table = dataSet.Tables["Customers"];

        // First process deletes.
        adapter.Update(table.Select(null, null, DataViewRowState.Deleted));

        // Next process updates.
        adapter.Update(table.Select(null, null,
            DataViewRowState.ModifiedCurrent));

        // Finally, process inserts.
        adapter.Update(table.Select(null, null, DataViewRowState.Added));
        // </Snippet2>
    }

    static private string GetConnectionString()
    {
        // To avoid storing the connection string in your code,
        // you can retrieve it from a configuration file.
        return "Data Source=(local);Initial Catalog=Northwind;"
            + "Integrated Security=true";
    }
}
