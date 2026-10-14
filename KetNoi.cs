using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace baikiemtragiuaky
{
    public class KetNoi
    {
        private readonly string connectionString;

        public KetNoi()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["SqlServerConnection"];
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new ConfigurationErrorsException("Chưa cấu hình chuỗi kết nối SqlServerConnection.");
            }

            connectionString = settings.ConnectionString;
        }

        public DataTable LayDuLieu(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Câu truy vấn không được để trống.", "query");
            }

            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                adapter.Fill(data);
            }

            return data;
        }

        public DataTable LayDuLieu(string query, IEnumerable<SqlParameter> parameters)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(new List<SqlParameter>(parameters).ToArray());
                }
                adapter.Fill(data);
            }
            return data;
        }

        public int ThucThi(string query, IEnumerable<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(new List<SqlParameter>(parameters).ToArray());
                }
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
