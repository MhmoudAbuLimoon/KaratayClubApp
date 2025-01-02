using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using KarateClubDataAccessLayer;
using System.Data;
using System.IO;

namespace KarateClubDataAccessLayer
{

    public class UserDTO
    {
       public PersonAddDTO PersonDTO { get; set; }
        public int UserID { set; get; }
        public string UserName { set; get; }
        public int PersonID { set; get; }
        public string PassWord { set; get; }
        public int Permissions { set; get; }
        public bool IsActive { set; get; }
        public UserDTO() { }

        public UserDTO(PersonAddDTO PersonDTO, int UserID, String UserName, string Password, int Permissions, bool IsActive)
        {

            this.PersonDTO = PersonDTO;
            this.UserID = UserID;
            this.UserName = UserName;
            this.PassWord = Password;
            this.Permissions = Permissions;
            this.IsActive = IsActive;
        }

        public UserDTO( int UserID, String UserName,  int Permissions, bool IsActive)
        {

            
            this.UserID = UserID;
            this.UserName = UserName;
          
            this.Permissions = Permissions;
            this.IsActive = IsActive;
        }

        public UserDTO(int UserID, int PersonID, String UserName, string PassWord,int Permissions, bool IsActive)
        {


            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.PassWord = PassWord;
            this.Permissions = Permissions;
            this.IsActive = IsActive;
        }
    }

    public  class AddUpdateUser {
        public int UserID { set; get; }
        public string UserName { set; get; }
        public int PersonID { set; get; }
        public string PassWord { set; get; }
        public int Permissions { set; get; }
        public bool IsActive { set; get; }

        public AddUpdateUser() { }
        public AddUpdateUser(int UserID, int PersonID, String UserName, string PassWord,
            int Permissions, bool IsActive)
        {


            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.PassWord = PassWord;
            this.Permissions = Permissions;
            this.IsActive = IsActive;
        }

        public AddUpdateUser(int UserID, string PassWord)
        {

            this.UserID = UserID;
            this.PassWord = PassWord;
        
        }

    }

    public class ChangePassWord
    {
        public int UserID { set; get; }
        public string PassWord { set; get; }

        public ChangePassWord() { }
        public ChangePassWord(int UserID,string PassWord) {
            this.UserID = UserID;
            this.PassWord = PassWord;
        }

    }
    public class UserDTOGetAll
    {
        public int UserID { set; get; }
        
        public string Name { set; get; }
        public string UserName { set; get; }
        public string Address { set; get; }
        public string Email { set; get; }
        public byte Gender { set; get; }
        public DateTime DateOfBirth { set; get; }
        public string Phone { set; get; }
        public bool IsActive { set; get; }
        public UserDTOGetAll(int UserID, string Name, String UserName,string Address, string Phone, string Email, byte Gender,DateTime DateOfBirth, bool IsActive)
        {


            this.UserID = UserID;
            this.Name = Name;
            this.UserName = UserName;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.Gender = Gender;
            this.DateOfBirth = DateOfBirth;
            this.IsActive = IsActive;
        }
        public UserDTOGetAll(int UserID, string Name, String UserName,byte Gender ,DateTime DateOfBirth, string Phone,bool IsActive)
        {


            this.UserID = UserID;
            this.Name = Name;
            this.UserName = UserName;
            this.Gender = Gender;
            this.DateOfBirth = DateOfBirth;
            this.Phone = Phone;

            this.IsActive = IsActive;
        }
        
    }
    public class clsUserData
    {
        public static List<UserDTOGetAll> GetAllUser()
        {
            var ListUser = new List<UserDTOGetAll>();
            using (SqlConnection connection =new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command =new SqlCommand("SP_GetAllUser_One", connection)) {

                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader= command.ExecuteReader()) {

                        while (reader.Read()) {

                            ListUser.Add(new UserDTOGetAll(

                                  reader.GetInt32(reader.GetOrdinal("UserID")),
                                  reader.GetString(reader.GetOrdinal("Name")),
                                  reader.GetString(reader.GetOrdinal("UserName")),
                                  reader.GetString(reader.GetOrdinal("Address")),
                                  reader.GetString(reader.GetOrdinal("Phone")),
                                  reader.GetString(reader.GetOrdinal("Email")),
                                  reader.GetByte(reader.GetOrdinal("Gender")),
                                  reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                  reader.GetBoolean(reader.GetOrdinal("IsActive"))


                                )) ;
                            

                        
                        
                        }
                    
                    
                    }
                    return ListUser;
                
                }

            }


        }
        public static bool DoesUserExist(int UserID)
        {

            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_DoesUserExistByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    command.Parameters.AddWithValue("@UserID", (object)UserID ?? DBNull.Value);
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
        //
        public static bool DoesUserExistByUserNameAndPass(string UserName,string Pass)
        {

            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_DoesUserExistByUsernameAndPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    command.Parameters.AddWithValue("@Username", UserName);
                    command.Parameters.AddWithValue("@Password", Pass);
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
        //
        public static UserDTOGetAll GetUserByID(int UserID) {

            using(SqlConnection connection =new SqlConnection(clsDataSettings._connectionString1)){

                using (SqlCommand command =new SqlCommand("SP_GetUserInfoByID", connection)) {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID",UserID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader()) {

                        if (reader.Read()) {

                            return new UserDTOGetAll(

                                 reader.GetInt32(reader.GetOrdinal("UserID")),
                                 reader.GetString(reader.GetOrdinal("Name")),
                                 reader.GetString(reader.GetOrdinal("UserName")),
                                 reader.GetString(reader.GetOrdinal("Address")),
                                 reader.GetString(reader.GetOrdinal("Phone")),
                                 reader.GetString(reader.GetOrdinal("Email")),
                                 reader.GetByte(reader.GetOrdinal("Gender")),
                                 reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                 reader.GetBoolean(reader.GetOrdinal("IsActive"))


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
        //
        public static AddUpdateUser GetJustUserByID(int UserID)
        {

            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {

                using (SqlCommand command = new SqlCommand("SP_GetUserInfoByID_One", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            return new AddUpdateUser(

                                 reader.GetInt32(reader.GetOrdinal("UserID")),
                                 reader.GetInt32(reader.GetOrdinal("PersonID")),
                                 reader.GetString(reader.GetOrdinal("UserName")),
                                 reader.GetString(reader.GetOrdinal("PassWord")),
                                 reader.GetInt32(reader.GetOrdinal("Permissions")),
                                 reader.GetBoolean(reader.GetOrdinal("IsActive"))


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
        //

        public static ChangePassWord GetUserByIDToChangePass(int UserID)
        {

            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {

                using (SqlCommand command = new SqlCommand("SP_GetUserInfoByID_One", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            return new ChangePassWord(

                                 reader.GetInt32(reader.GetOrdinal("UserID")),
                                 reader.GetString(reader.GetOrdinal("PassWord"))
                                


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
        public static UserDTO GetUserByUserName(string UserName)
        {

            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {

                using (SqlCommand command = new SqlCommand("SP_GetUserInfoByUsername", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", UserName);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new UserDTO(

                                  reader.GetInt32(reader.GetOrdinal("UserID")),

                                  reader.GetString(reader.GetOrdinal("UserName")),
                                   reader.GetInt32(reader.GetOrdinal("Permissions")),


                                   reader.GetBoolean(reader.GetOrdinal("IsActive"))

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



        public static int AddNewUser(AddUpdateUser AddUDTO)
        {

            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {

                using (SqlCommand command = new SqlCommand("SP_AddNewUser", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", AddUDTO.PersonID);
                    command.Parameters.AddWithValue("@Username", AddUDTO.UserName);
                    command.Parameters.AddWithValue("@Password", AddUDTO.PassWord);
                    command.Parameters.AddWithValue("@Permissions", AddUDTO.Permissions);
                    command.Parameters.AddWithValue("@IsActive", AddUDTO.IsActive);
                  

                    var OutPutIdPram = new SqlParameter("@NewUserID", SqlDbType.Int)
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

        public static bool UpdateUser(AddUpdateUser UpdateUDTO)
        {
            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UpdateUDTO.UserID);
                    command.Parameters.AddWithValue("@PersonID", UpdateUDTO.PersonID);
                    command.Parameters.AddWithValue("@Username", UpdateUDTO.UserName);
                    command.Parameters.AddWithValue("@Password", UpdateUDTO.PassWord);
                    command.Parameters.AddWithValue("@Permissions", UpdateUDTO.Permissions);
                    command.Parameters.AddWithValue("@IsActive", UpdateUDTO.IsActive);
                  
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;

                }

            }
        }


        public static bool DeletePerson(int UserID)
        {

            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteUser", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);

                    connection.Open();
                    int ReowsEffected = (int)command.ExecuteNonQuery();

                    return (ReowsEffected == 1);
                }
            }



        }

        public static bool ChangePassWored(ChangePassWord changePass ) {

            int RowAffected = 0;
            try
            {
                using (SqlConnection connection=new SqlConnection(clsDataSettings._connectionString1)) {
                    using (SqlCommand command=new SqlCommand("SP_ChangePassword", connection)) {

                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open(); 
                        command.Parameters.AddWithValue("@UserID", (object)changePass.UserID ?? DBNull.Value);
                        command.Parameters.AddWithValue("@NewPassword", changePass.PassWord);

                        RowAffected = command.ExecuteNonQuery();


                    }
                
                }
            }
            catch (Exception ex) { }






            return (RowAffected>0);
        
        }

        public static int CountUser()
        {

            int Count = 0;
            try {

                using (SqlConnection connection =new SqlConnection(clsDataSettings._connectionString1)) {
                    connection.Open();

                    using (SqlCommand command=new SqlCommand("SP_GetUsersCount", connection)) {

                        command.CommandType = CommandType.StoredProcedure;

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int Value))
                        {
                            Count = Value;
                        }



                    }
                
                
                
                
                
                } 
            
            
            
            
            
            
            
            
            } catch (Exception ex) { }


            return Count;





        }


















    }
}
