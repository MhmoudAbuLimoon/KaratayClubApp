using KarateClubDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClubBisniceLayer
{
    public class clsMembers
    {
        public enum enMode { AddNew = 0, Update = 1 };
        enMode Mode = enMode.AddNew;
        public int MemberID { get; set; }
        public string Name { set; get; }
        public int PersonID { set; get; }
        public int LastBeltRankID { set; get; }
        public string RankName { set; get; }
        public string Gender { set; get; }
        public string Phone { set; get; }
        public DateTime BirthOfDate { set; get; }
        public bool IsActive { set; get; }
        public MemberDTO MDTO
        {
            get
            {

                return (new MemberDTO(this.MemberID, this.Name, this.RankName,
            this.Gender, this.BirthOfDate, this.Phone, this.IsActive));
            }
        }
       
        public AddMemberDTO AddMDTO
        {
            get
            {

                return (new AddMemberDTO(this.MemberID, this.PersonID,this.LastBeltRankID,this.IsActive));
            }
        }
        public clsMembers(MemberDTO MDTO, enMode MMode = enMode.AddNew)
        {

            this.MemberID = MDTO.MemberID;
            this.Name = MDTO.Name;
          
            this.RankName = MDTO.RankName;
            this.Gender = MDTO.Gender;
            this.BirthOfDate = MDTO.BirthOfDate;
            this.Phone = MDTO.Phone;
            this.IsActive = MDTO.IsActive;
            Mode = MMode;

        }
        public clsMembers(AddMemberDTO AddMDTO, enMode MMode = enMode.AddNew)
        {

            this.MemberID = AddMDTO.MemberID;
            this.PersonID = AddMDTO.PersonID;
            this.LastBeltRankID = AddMDTO.LastBeltRankID;
            this.IsActive = MDTO.IsActive;
            Mode = MMode;
        }
        public clsMembers(AddMemberDTO ActivityDTO)
        {
            this.MemberID = ActivityDTO.MemberID;
            this.IsActive = ActivityDTO.IsActive;

        }

        public static List<MemberDTO> GetAllMember()
        {
            return clsMembersData.GetAllMembers();
        }

        public static clsMembers Find(int MemberID)
        {

            MemberDTO MDTO = clsMembersData.GetMemberByMemberID(MemberID);

            if (MDTO != null)
            {
                return new clsMembers(MDTO, enMode.Update );
            }
            else
            {
                return null;
            }

        }

        public static clsMembers FindAddMember(int MemberID)
        {

            AddMemberDTO AddMDTO = clsMembersData.GetMemberByMemberIDToAdd(MemberID);
            

            if (AddMDTO != null)
            {
                return new clsMembers(AddMDTO, enMode.Update);
            }
            else
            {
                return null;
            }

        }

        public static bool DoesMemberExist(int MemberID) {
            return clsMembersData.DoesMemberExist(MemberID);
        }
        private async Task<bool> _AddNewMember()
        {

            //  this.ID = stu.AddNewProduct(this.PDTO);
            this.MemberID = clsMembersData.AddNewMember(this.AddMDTO);

            return MemberID != -1;
        }
        private async Task<bool> _UpdateMember()
        {

            KarateClubDataAccessLayer.clsMembersData.UpdateMember(this.AddMDTO);
           
            return PersonID != -1;
        }
        
        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewMember())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return await _UpdateMember();

            }

            return false;
        }
        public static int CountMember()
        {
            return clsMembersData.CountMember();
        }

       
        public static bool DeleteMember(int MemberID) {
            return clsMembersData.DeleteMember(MemberID);
        }


        }
    }

