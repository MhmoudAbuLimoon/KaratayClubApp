using KarateClubDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClubBisniceLayer
{
    public class clsInstructor
    {
        public enum enMode { AddNew = 0, Update = 1 };
        enMode Mode = enMode.AddNew;
        public string Name { set; get; }
        public string Phone { set; get; }
        public string Gender { set; get; }
        public DateTime DateOfBirth { set; get; }
        public int InstructorID { set; get; }
        public int PersonID { set; get; }
        public string Qualification { set; get; }
        public InstDTO INDTO
        {
            get
            {
                return (new InstDTO(this.InstructorID, this.Name, this.Gender,
                    this.Phone, this.DateOfBirth, this.Qualification
                    ));
            }
        }
        public AddInstDTO AddInstDTO
        {
            get
            {
                return (new AddInstDTO(this.InstructorID, this.PersonID, this.Qualification
                    ));
            }
        }
        public clsInstructor(InstDTO IntDTO)
        {

            this.InstructorID = IntDTO.InstructorID;
            this.Name = IntDTO.Name;
            this.Gender = IntDTO.Gender;
            this.Phone = IntDTO.Phone;
            this.DateOfBirth = IntDTO.DateOfBirth;
            this.Qualification = IntDTO.Qualification;

           
        }
        public clsInstructor(AddInstDTO AddInstDTO, enMode IMode = enMode.AddNew)
        {

            this.InstructorID = AddInstDTO.InstructorID;
            this.PersonID = AddInstDTO.PersonID;
            this.Qualification = AddInstDTO.Qualification;

            Mode = IMode;
        }
        public static List<InstDTO> GetAllInstructor()
        {
            return clsInstructorData.GetAllInstrotrs();
        }

        public static bool DoesInstructorExist(int InstructorID)
        {
            return KarateClubDataAccessLayer.clsInstructorData.DoesInstructorExist(InstructorID);
        }

        public static clsInstructor Find(int InstructorID)
        {

            InstDTO IDTO = clsInstructorData.GetInstructorByID(InstructorID);

            if (IDTO != null)
            {
                return new clsInstructor(IDTO);
            }
            else
            {
                return null;
            }

        }


        private async Task<bool> _AddNewInstructor()
        {

            //  this.ID = stu.AddNewProduct(this.PDTO);
            this.InstructorID = clsInstructorData.AddNewInstructor(this.AddInstDTO);

            //if (clsPersonData.DoesPersonExist(AddInstDTO.PersonID)) {
            
            //}

           

            return InstructorID != -1;
        }
        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewInstructor())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

              //  case enMode.Update:

                   // return await _UpdatePerson();
                   // return await null;

            }

            return false;
        }
        public static bool DeleteInstructor(int InstructorID)
        {
            return clsInstructorData.DeleteInstructor(InstructorID);
        }
    }
}
