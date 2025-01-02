using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClubDataAccessLayer
{

    public class AddInstDTO
    {


     
        public int InstructorID { set; get; }
        public int PersonID { set; get; }
        public string Qualification { set; get; }

        public AddInstDTO() { }

        //public InstDTO(int InstructorID, string Name, string Gender, string Phone, DateTime DateOfBirth, string Qualification)
        //{
        //    this.InstructorID = InstructorID;
        //    this.Name = Name;
        //    this.Gender = Gender;
        //    this.Phone = Phone;
        //    this.DateOfBirth = DateOfBirth;
        //    this.Qualification = Qualification;

        //}

        public AddInstDTO(int InstructorID, int PersonID, string Qualification)
        {
            this.InstructorID = InstructorID;
            this.PersonID = PersonID;
            this.Qualification = Qualification;

        }

    }
    public class InstDTO
    {

     
        public string Name { set; get; }
        public string Phone { set; get; }
        public string Gender { set; get; }
        public DateTime DateOfBirth { set; get; }
        public int InstructorID { set; get; }
        public int PersonID { set; get; }
        public string Qualification { set; get; }

        public InstDTO() { }

        public InstDTO(int InstructorID, string Name, string Gender, string Phone, DateTime DateOfBirth, string Qualification)
        {
            this.InstructorID = InstructorID;
            this.Name = Name;
            this.Gender = Gender;
            this.Phone = Phone;
            this.DateOfBirth = DateOfBirth;
            this.Qualification = Qualification;

            }

       

    }
    public class clsInstructorData
    {

        public static List<InstDTO> GetAllInstrotrs()
        {
            var InstrctorstList = new List<InstDTO>();
            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllInstructors", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            InstrctorstList.Add(new InstDTO(

                                reader.GetInt32(reader.GetOrdinal("InstructorID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("Gender")),
                                reader.GetString(reader.GetOrdinal("Phone")),
                                reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                reader.GetString(reader.GetOrdinal("Qualification"))

                                ));

                        }

                    }
                }
                return InstrctorstList;
            }
        }


        public static bool DoesInstructorExist(int InstructorID)
        {

            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_DoesInstructorExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    command.Parameters.AddWithValue("@InstructorID", (object)InstructorID ?? DBNull.Value);
                    SqlParameter returnParameter = new SqlParameter("@ReturnVal", SqlDbType.Int)
                    {

                        Direction = ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(returnParameter);

                    command.ExecuteNonQuery();

                    IsFound = (int)returnParameter.Value == 1;
                }


            }
            return IsFound;
        }

        public static InstDTO GetInstructorByID(int InstructorID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_GetInstructorInfoByID", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InstructorID", InstructorID);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            return new InstDTO(

                                reader.GetInt32(reader.GetOrdinal("InstructorID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("Gender")),
                                reader.GetString(reader.GetOrdinal("Phone")),
                                reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                reader.GetString(reader.GetOrdinal("Qualification"))

                                );
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public static int AddNewInstructor(AddInstDTO addInstDTO)
        {

            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {

                using (SqlCommand command = new SqlCommand("SP_AddNewInstructor", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", addInstDTO.PersonID);
                    command.Parameters.AddWithValue("@Qualification", addInstDTO.Qualification);
                    
                    var OutPutIdPram = new SqlParameter("@NewInstructorID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(OutPutIdPram);
                    connection.Open();
                    command.ExecuteNonQuery();


                    return (int)OutPutIdPram.Value;


                }

            }

        }

        public static bool DeleteInstructor(int InstructorID)
        {

            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_DeletePerson", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SP_DeleteInstructor", InstructorID);

                    connection.Open();
                    int ReowsEffected = (int)command.ExecuteNonQuery();

                    return (ReowsEffected == 1);
                }
            }




        }
    }
}
