using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClubDataAccessLayer;

namespace KarateClubDataAccessLayer
{
    public class AddMemberDTO {
        public int MemberID { get; set; }
        public int PersonID { set; get; }
        public int LastBeltRankID { set; get; }

        public bool IsActive { set; get; }

        public AddMemberDTO() { }
        public AddMemberDTO(int MemberID, int PersonID, int LastBeltRankID, bool IsActive)
        {

            this.MemberID = MemberID;
            this.PersonID = PersonID;
            this.LastBeltRankID = LastBeltRankID;
            this.IsActive = IsActive;


        }

      


    }
    public class MemberDTO
    {
        public int MemberID { get; set; }
        public string Name { set; get; }
     
        public string RankName { set; get; }
        public string Gender { set; get; }
        public string Phone { set; get; }
        public DateTime BirthOfDate { set; get; }
        public bool IsActive { set; get; }

        public MemberDTO() { }
        //public MemberDTO(int MemberID, int PersonID, int LastBeltRankID, bool IsActive)
        //{

        //    this.MemberID = MemberID;
        //    this.PersonID = PersonID;
        //    this.LastBeltRankID = LastBeltRankID;
        //    this.IsActive = IsActive;


        //}
        public MemberDTO(int MemberID, string Name, string RankName,
    string Gender, DateTime BirthOfDate, string Phone, bool IsActive)
        {

            this.MemberID = MemberID;
            this.Name = Name;
            this.RankName = RankName;
            this.Gender = Gender;
            this.BirthOfDate = BirthOfDate;
            this.Phone = Phone;
            this.IsActive = IsActive;


        }
    }

    public class clsMembersData
    {

        public static List<MemberDTO> GetAllMembers()
        {
            var MemberList = new List<MemberDTO>();
            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllMembers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            MemberList.Add(new MemberDTO(

                                reader.GetInt32(reader.GetOrdinal("MemberID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("RankName")),
                                reader.GetString(reader.GetOrdinal("Gender")),
                                reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                reader.GetString(reader.GetOrdinal("Phone")),
                                reader.GetBoolean(reader.GetOrdinal("IsActive"))


                                ));

                        }

                    }
                }
                return MemberList;
            }
        }




        public static MemberDTO GetMemberByMemberID(int MemberID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_GetMemberInfoByID", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MemberID", MemberID);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            return new MemberDTO(

                                reader.GetInt32(reader.GetOrdinal("MemberID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("RankName")),
                                reader.GetString(reader.GetOrdinal("Gender")),
                                reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                reader.GetString(reader.GetOrdinal("Phone")),
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
        public static AddMemberDTO GetMemberByMemberIDToAdd(int MemberID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_GetMemberInfoByIDToAdd", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MemberID", MemberID);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            return new AddMemberDTO(

                                reader.GetInt32(reader.GetOrdinal("MemberID")),
                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                reader.GetInt32(reader.GetOrdinal("LastBeltRankID")),
                                
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


        public static bool DoesMemberExist(int MemberID)
        {

            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_DoesMemberExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    command.Parameters.AddWithValue("@MemberID", (object)MemberID ?? DBNull.Value);
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

        public static int CountMember()
        {

            int Count = 0;
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_GetMembersCount", connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int Value))
                        {
                            Count = Value;
                        }



                    }

                }

            }
            catch (Exception ex) { }
            return Count;
        }

        public static bool DeleteMember(int MemberID)
        {

            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteMember", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MemberID", MemberID);

                    connection.Open();
                    int ReowsEffected = (int)command.ExecuteNonQuery();

                    return (ReowsEffected == 1);
                }
            }

        }


        public static int AddNewMember(AddMemberDTO AddMDTO)
        {

            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {

                using (SqlCommand command = new SqlCommand("SP_AddNewMember", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", AddMDTO.PersonID);
                    command.Parameters.AddWithValue("@LastBeltRankID", AddMDTO.LastBeltRankID);
                    command.Parameters.AddWithValue("@IsActive ", AddMDTO.IsActive);
                   

                    var OutPutIdPram = new SqlParameter("@NewMemberID", SqlDbType.Int)
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



       
        public static bool UpdateMember(AddMemberDTO UpdateMDTO)
        {
            using (SqlConnection connection = new SqlConnection(clsDataSettings._connectionString1))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateMember", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MemberID", UpdateMDTO.MemberID);
                    command.Parameters.AddWithValue("@PersonID", UpdateMDTO.PersonID);
                    
                    command.Parameters.AddWithValue("@LastBeltRankID", UpdateMDTO.LastBeltRankID);
                    command.Parameters.AddWithValue("@IsActive", UpdateMDTO.IsActive);
                   
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;

                }

            }
        }
    }
}