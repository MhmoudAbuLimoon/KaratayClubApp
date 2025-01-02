using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace KarateClubDataAccessLayer
{

    public class PersonAddDTO
    {


        public int PersonID { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public DateTime DateOfBirth { set; get; }
        public byte Gender { set; get; }
        public string Img { get; set; }
        public List<IFormFile> Image { get; set; }

        public PersonAddDTO() { }


        public PersonAddDTO(int PersonID, string Name, string Address, string Phone, string Email, DateTime DateOfBirth,
            byte Gender)
        {
            this.PersonID = PersonID;
            this.Name = Name;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;


        }
        public PersonAddDTO(int PersonID, string Name, string Address, string Phone, string Email, DateTime DateOfBirth,
    byte Gender,string Img)
        {
            this.PersonID = PersonID;
            this.Name = Name;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Img=Img;


        }
        public PersonAddDTO(int PersonID, string Name, string Address, string Phone, string Email, DateTime DateOfBirth,
        byte Gender, List<IFormFile> Image)
        {
            this.PersonID = PersonID;
            this.Name = Name;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Image = Image;

        }

    }




    public class clsPersonData
    {
        //  public static string ConnectionString = "Server=.;Database=KarateClub;User Id=sa;Password=sa123456;Trusted_Connection=True;";
        static string _connectionString = "Server=localhost;Database=KarateClub;User Id=sa;Password=sa123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";
        public static List<PersonAddDTO> GetAllPerson()
        {
            var PersonList = new List<PersonAddDTO>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllPeople", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            PersonList.Add(new PersonAddDTO(

                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("Address")),
                                reader.GetString(reader.GetOrdinal("Phone")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                reader.GetByte(reader.GetOrdinal("Gender")),
                                reader.GetString(reader.GetOrdinal("ImagePath"))

                                ));

                        }

                    }
                }
                return PersonList;
            }
        }

        public static PersonAddDTO GetPersonByID(int PersonID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPersonByID", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            return new PersonAddDTO(

                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("Address")),
                                reader.GetString(reader.GetOrdinal("Phone")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                reader.GetByte(reader.GetOrdinal("Gender")),
                                 reader.GetString(reader.GetOrdinal("ImagePath"))

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

        public static bool DoesPersonExist(int id)
        {

            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_DoesPersonExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    command.Parameters.AddWithValue("@PersonID", (object)id ?? DBNull.Value);
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

        public static bool UpdateImagePerson(String ImageUrl, int PersonID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateImagePath", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    
                    command.Parameters.AddWithValue("@ImagePath", ImageUrl);

                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;

                }

            }
        }

        public static bool UpdatePerson(PersonAddDTO UpdatePDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdatePerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", UpdatePDTO.PersonID);
                    command.Parameters.AddWithValue("@Name", UpdatePDTO.Name);
                    command.Parameters.AddWithValue("@Address", UpdatePDTO.Address);
                    command.Parameters.AddWithValue("@Phone", UpdatePDTO.Phone);
                    command.Parameters.AddWithValue("@Email", UpdatePDTO.Email);
                    command.Parameters.AddWithValue("@DateOfBirth", UpdatePDTO.DateOfBirth);
                    command.Parameters.AddWithValue("@Gender", UpdatePDTO.Gender);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;

                }

            }
        }
        public static int AddNewPerson(PersonAddDTO AddPDTO)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                using (SqlCommand command = new SqlCommand("SP_AddNewPerson", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", AddPDTO.Name);
                    command.Parameters.AddWithValue("@Address", AddPDTO.Address);
                    command.Parameters.AddWithValue("@Phone", AddPDTO.Phone);
                    command.Parameters.AddWithValue("@Email", AddPDTO.Email);
                    command.Parameters.AddWithValue("@DateOfBirth", AddPDTO.DateOfBirth);
                    command.Parameters.AddWithValue("@Gender", AddPDTO.Gender);
              
                    var OutPutIdPram = new SqlParameter("@NewPersonID", SqlDbType.Int)
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

        public static int AddNewStudentImage(String ImageUrl, int PersonID)
        {
            int ID = -1;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_AddNewPersonImage", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ImagePath", ImageUrl);

                    cmd.Parameters.AddWithValue("@PersonID", PersonID);

                    var OutPutIdPram = new SqlParameter("@NewImageID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(OutPutIdPram);
                    con.Open();
                    cmd.ExecuteNonQuery();


                    return (int)OutPutIdPram.Value;
                }
            }

            return ID;
        }

        public static bool DeletePerson(int personID)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_DeletePerson", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", personID);

                    connection.Open();
                    int ReowsEffected = (int)command.ExecuteNonQuery();

                    return (ReowsEffected == 1);
                }
            }




        }


    }
}

