using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vehicules.Models;

namespace Vehicules.Services
{
    public class AdoNetVehiculeRepository : IVehiculeRepository
    {
        static string connectionString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = EFVehicules; Integrated Security = SSPI";


        public void add(Vehicule vehicule)
        {
            string cmdText = "insert into Vehicules(Make, Model, VIN, Type, Color) values(@Make, @Model, @VIN, @Type, @Color) ";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@Make", vehicule.Make);
            command.Parameters.AddWithValue("@Model", vehicule.Model);
            command.Parameters.AddWithValue("@VIN", vehicule.VIN);
            command.Parameters.AddWithValue("@Type", vehicule.Type);
            command.Parameters.AddWithValue("@Color", vehicule.Color);

            using (connection)
            {
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {

                    throw e;
                }
            }
        }

        public void Delete(Vehicule vehicule)
        {
         // i'm aware of the security issues, fixing it when i have time.  
            string cmdText = $"Delete from Vehicules where id = {vehicule.Id}";
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(cmdText, connection);
            using (connection)
            {
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {

                    throw e;
                }
            }
        }

        public Vehicule Get(int id)
        {
            string cmdText = $"select * from Vehicules where id ={id}";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(cmdText, connection);
            Vehicule vehicule = new Vehicule();
            vehicule.Id = id;
            using (connection)
            {
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        { 
                            vehicule.Make = (string)reader["Make"];
                            vehicule.Model = (string)reader["Model"];
                            vehicule.VIN = (string)reader["VIN"];
                            vehicule.Type = (VehicleTypeEnum)reader["Type"];
                            vehicule.Color = (ColorEnum)reader["Color"];
                        }
                    }
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
            return vehicule;
        }

        public IEnumerable<Vehicule> GetAll()
        {
            string cmdText = "select * from Vehicules";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(cmdText, connection);
            List<Vehicule> vehiculeLijst = new List<Vehicule>();

            using (connection)
            {
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Vehicule vehicule = new Vehicule();
                            vehicule.Id = (int)reader["id"];
                            vehicule.Make = (string)reader["Make"];
                            vehicule.Model = (string)reader["Model"];
                            vehicule.VIN = (string)reader["VIN"];
                            vehicule.Type = (VehicleTypeEnum)reader["Type"];
                            vehicule.Color = (ColorEnum)reader["Color"];

                            vehiculeLijst.Add(vehicule);
                        }
                    }

                }
                catch (SqlException e)
                {
                    throw e;
                }
                return vehiculeLijst;
            }
        }

        public void Update(Vehicule vehicule)
        {
            string cmdText = $"update Vehicules set Make = '{vehicule.Make}', Model = '{vehicule.Model}', VIN = '{vehicule.VIN}', Type = '{(int)vehicule.Type}', Color = '{(int)vehicule.Color}'" +
                $" where id = {vehicule.Id}";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(cmdText, connection);
            using (connection)
            {
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {

                    throw e;
                }
            }
        }
    }
}
