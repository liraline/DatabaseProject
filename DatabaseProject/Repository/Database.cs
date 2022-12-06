using System.Data.SqlClient;
using DatabaseProject.Model;
using DatabaseProject.Utils;

namespace DatabaseProject.Repository
{
    public static class Database
    {
        const string ConnectionString = @"Data Source=127.0.0.1,1433;Database=Northwind;User Id=sa;Password=Aluno@123;";

        public static void InitializeDatabase()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand createTriggerCommand = new SqlCommand("CREATE OR ALTER TRIGGER applyDiscountWhenFiveOrMoreProducts ON [Order Details] " +
                        "AFTER INSERT AS BEGIN " +
                        "UPDATE [Order Details] SET Discount = 0.05 WHERE [Order Details].OrderID = (" +
                            "SELECT [Order Details].OrderID FROM [Order Details] " +
                            "JOIN Inserted ON Inserted.OrderID = [Order Details].OrderID " + 
                            "GROUP BY [Order Details].OrderID HAVING COUNT(*) >= 5); END", connection);
                    createTriggerCommand.ExecuteNonQuery();

                    SqlCommand createProcedureCommand = new SqlCommand("CREATE OR ALTER PROCEDURE checkEmployeeTotalSales " +
                        "@employeeID INT AS " +
                        "SELECT Employees.EmployeeID, Employees.FirstName, Employees.LastName, COUNT(Orders.EmployeeID) " +
                        "FROM Employees JOIN Orders ON Employees.EmployeeID = Orders.EmployeeID " +
                        "WHERE Employees.EmployeeID = @employeeID GROUP BY Employees.EmployeeID, Employees.FirstName, Employees.LastName;", connection);
                    createProcedureCommand.ExecuteNonQuery();
                }
            }
        }

        public static EmployeeReport ExecProcedure(int employeeID)
        {
            EmployeeReport employeeReport = null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand execProcedureCommand = new SqlCommand($"EXEC checkEmployeeTotalSales {employeeID}", connection);
                    SqlDataReader procedureResponse = execProcedureCommand.ExecuteReader();

                    if (procedureResponse.HasRows)
                    {
                        procedureResponse.Read();
                        employeeReport = new EmployeeReport
                        {
                            EmployeeID = NullChecker.CheckIntField(procedureResponse, 0),
                            FirstName = NullChecker.CheckStringField(procedureResponse, 1),
                            LastName = NullChecker.CheckStringField(procedureResponse, 2),
                            TotalSales = NullChecker.CheckIntField(procedureResponse, 3)
                        };
                    }
                }
            }
            return employeeReport;
        }
    }
}
