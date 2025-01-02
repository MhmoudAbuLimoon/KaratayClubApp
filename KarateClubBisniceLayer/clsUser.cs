using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClubDataAccessLayer;

namespace KarateClubBisniceLayer
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1,Change=2 };
        enMode Mode = enMode.AddNew;
        public enum enGender { Male = 0, Female = 1 };

        public enum enPermissions
        {
            All = -1,
            ManageMembers = 1,
            ManageInstructors = 2,
            ManageUsers = 4,
            ManageMembersInstructors = 8,
            ManageBeltRanks = 16,
            ManageSubscriptionPeriods = 32,
            ManageBeltTests = 64,
            ManagePayments = 128
        }

        public int UserID { set; get; }
        public int PersonID { set; get; }
        public string Name { set; get; }
        public string UserName { set; get; }
        public string Address { set; get; }
        public string Email { set; get; }
        public string PassWord { set; get; }
        public byte Gender { set; get; }
        public int Permissions { set; get; }
        public DateTime DateOfBirth { set; get; }
        public string Phone { set; get; }
        public bool IsActive { set; get; }

        //public StudentDTO SDTO
        //{
        //    get { return (new StudentDTO(this.ID, this.Name, this.Age, this.Grade)); }
        //}
        public UserDTOGetAll UDTO
        {
            get
            {

                return (new UserDTOGetAll(this.UserID, this.Name, this.UserName, this.Address, this.Phone,
                    this.Email,  this.Gender, this.DateOfBirth, this.IsActive));
            }
        }
        public UserDTO InfoUDTO
        {
            get
            {

                return (new UserDTO(this.UserID, this.UserName, this.Permissions, this.IsActive));
            }
        }
        public UserDTO AddUDTO1
        {
            get
            {

                return (new UserDTO(this.UserID, this.PersonID, this.UserName, this.PassWord, this.Permissions, this.IsActive));
            }
        }
        public AddUpdateUser AddUDTO
        {
            get
            {

                return (new AddUpdateUser(this.UserID, this.PersonID, this.UserName, this.PassWord, this.Permissions, this.IsActive));
            }
        }

        public ChangePassWord changepass
        {
            get
            {
                return (new ChangePassWord(this.UserID,this.PassWord));
            }
        }
        public clsUser(UserDTOGetAll UDTO, enMode UMode = enMode.AddNew) {

            this.UserID = UDTO.UserID;
            this.UserName = UDTO.UserName;
            this.Name = UDTO.Name;
            this.Address = UDTO.Address;
            this.Phone = UDTO.Phone;
            this.Email = UDTO.Email;
            this.Gender = UDTO.Gender;
            this.DateOfBirth = UDTO.DateOfBirth;
            this.IsActive = UDTO.IsActive;
            Mode = UMode;

        }
        public clsUser(ChangePassWord changePass, enMode UMode = enMode.Change)
        {
            this.UserID = changePass.UserID;
            this.PassWord = changePass.PassWord;
            Mode = UMode;
        }
        public clsUser(UserDTO InfoUDTO)
        {

            this.UserID = InfoUDTO.UserID;
            this.UserName = InfoUDTO.UserName;
            this.Permissions = InfoUDTO.Permissions;
            this.IsActive = InfoUDTO.IsActive;
            

        }

        public clsUser(UserDTO AddUDTO, enMode UMode = enMode.AddNew)
        {

            this.UserID = AddUDTO.UserID;
            this.PersonID = AddUDTO.PersonID;
            this.UserName = AddUDTO.UserName;
            this.PassWord = AddUDTO.PassWord;
            this.Permissions = AddUDTO.Permissions;
            this.IsActive = AddUDTO.IsActive;
            Mode = UMode;

        }

        public clsUser(AddUpdateUser AddUDTO, enMode UMode = enMode.AddNew)
        {

            this.UserID = AddUDTO.UserID;
            this.PersonID = AddUDTO.PersonID;
            this.UserName = AddUDTO.UserName;
            this.PassWord = AddUDTO.PassWord;
            this.Permissions = AddUDTO.Permissions;
            this.IsActive = AddUDTO.IsActive;
            Mode = UMode;

        }

        public static List<UserDTOGetAll> GetAllUser()
        {
            return clsUserData.GetAllUser();
        }
        public static clsUser FindUser(int UserID)
        {
            UserDTOGetAll UserInfo = clsUserData.GetUserByID(UserID);
            if (UserInfo !=null) {
                return new clsUser(UserInfo, enMode.Update);

            }
            else
            {
                return null;
            }
        }
        public static bool DoesUserExist(int UserID)
        {
            return KarateClubDataAccessLayer.clsUserData.DoesUserExist(UserID);
        }

        public static bool DoesUserExistByUseNameAndPass(string UserName,string Pass)
        {
            return KarateClubDataAccessLayer.clsUserData.DoesUserExistByUserNameAndPass(UserName,Pass);
        }
        public static clsUser FindJustUser(int UserID)
        {
            AddUpdateUser UserInfo = clsUserData.GetJustUserByID(UserID);
            if (UserInfo != null)
            {
                return new clsUser(UserInfo, enMode.Update);

            }
            else
            {
                return null;
            }
        }
        public static clsUser FindUserToChangePass(int UserID)
        {
            ChangePassWord changePass = clsUserData.GetUserByIDToChangePass(UserID);
            if (changePass != null)
            {
                return new clsUser(changePass, enMode.Change);

            }
            else
            {
                return null;
            }
        }
        public static clsUser FindUser(string UserName)
        {
            UserDTO UserInfo = clsUserData.GetUserByUserName(UserName);
            if (UserInfo != null)
            {
                return new clsUser(UserInfo, enMode.Update);

            }
            else
            {
                return null;
            }
        }

        private async Task<bool> _AddNewUser()
        {

            //  this.ID = stu.AddNewProduct(this.PDTO);
            this.UserID = clsUserData.AddNewUser(this.AddUDTO);
            return UserID != -1;
        }

        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewUser())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Change:
                    if (await _ChangePassword()) {

                        Mode = enMode.Update;
                        return true;
                    } else
                    {
                        return false;
                    }

                case enMode.Update:
                    
                    return await _UpdateUser();

            }

            return false;
        }

        private async Task<bool> _UpdateUser()
        {
            
            KarateClubDataAccessLayer.clsUserData.UpdateUser(this.AddUDTO);
         //   KarateClubDataAccessLayer.clsUserData.ChangePassWored(this.changepass);
    ;           
            return PersonID != -1;
        }


        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeletePerson(UserID);
        }

        private async Task<bool> _ChangePassword()
        {
            KarateClubDataAccessLayer.clsUserData.ChangePassWored(this.changepass);
            
            return PersonID != -1;

        }

        public static int CountUser()
        {
            return clsUserData.CountUser();
        }

    }
}
