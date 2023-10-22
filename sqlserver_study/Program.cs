using Dapper;
using Microsoft.Data.SqlClient;

internal class Program
{
    private static void Main(string[] args)
    {
        void Fun1()
        {
            string connectionString = "YourConnectionStringHere";
            int studentId = 1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = connection.Query<Student>("SELECT * FROM Student WHERE Id = @Id", new { Id = studentId }).FirstOrDefault();

                if (result != null)
                {
                    // You can now work with the 'result' object, which is an instance of the 'Student' class.
                }
            }

        }
    }
}

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    // Other properties
}