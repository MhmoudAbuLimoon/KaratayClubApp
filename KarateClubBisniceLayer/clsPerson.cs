using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClubDataAccessLayer;
using System.IO;
using Microsoft.AspNetCore.Http;



namespace KarateClubBisniceLayer
{
    public  class clsPerson
    {
        public enum enMode { AddNew=0,Update=1};
        enMode Mode = enMode.AddNew;
        public int PersonID { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public DateTime DateOfBirth { set; get; }
        public byte Gender { set; get; }
        public string Img { get; set; }
        public List< IFormFile> Image { set; get; }

        public PersonAddDTO AddPDTO
        {
            get
            {

                return (new PersonAddDTO(this.PersonID, this.Name, this.Address,
            this.Phone, this.Email, this.DateOfBirth, this.Gender, this.Img));
            }
        }



        public clsPerson( PersonAddDTO AddPDTO, enMode PMode = enMode.AddNew)
        {

            this.PersonID = AddPDTO.PersonID;
            this.Name = AddPDTO.Name;
            this.Address = AddPDTO.Address;
            this.Phone = AddPDTO.Phone;
            this.Email = AddPDTO.Email;
            this.DateOfBirth = AddPDTO.DateOfBirth;
            this.Gender = AddPDTO.Gender;
             this.Img= AddPDTO.Img;
            this.Image=AddPDTO.Image;
            Mode = PMode;
        }


        public static List<PersonAddDTO> GetAllPerson()
        {
            return clsPersonData.GetAllPerson();
        }
        public static clsPerson Find(int PersonID) {

            PersonAddDTO PDTO = clsPersonData.GetPersonByID(PersonID);

            if (PDTO !=null) {
                return  new clsPerson(PDTO, enMode.Update);
    }
            else
            {
                return null;
            }

        }
       
        public static bool DoesPersonExist(int PersonID)
        {
            return KarateClubDataAccessLayer.clsPersonData.DoesPersonExist(PersonID);
        }

        private async Task<bool> _UpdatePerson()
        {
           
              KarateClubDataAccessLayer.clsPersonData.UpdatePerson(this.AddPDTO);
            foreach (var item in this.Image)
            {
                string path = await UploadImage(item);
                clsPersonData.UpdateImagePerson(path, this.PersonID);
            }
            return PersonID != -1;
        }
       
        private async Task<bool> _AddNewPerson()
        {

            //  this.ID = stu.AddNewProduct(this.PDTO);
            this.PersonID = clsPersonData.AddNewPerson(this.AddPDTO);

            foreach (var item in this.Image)
            {
                string path = await UploadImage(item);
                clsPersonData.AddNewStudentImage(path, this.PersonID);
        
            }

            return PersonID != -1;
        }
        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewPerson())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return  await _UpdatePerson();

            }

            return false;
        }
      
        public async Task<string> UploadImage(IFormFile imageFile)
        {

            // Directory where files will be uploaded
            var uploadDirectory = @"C:\MyUploads";

            // Generate a unique filename
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadDirectory, fileName);

            // Ensure the uploads directory exists, create if it doesn't
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Return the file path as a response
            return  filePath ;
        }

        public static bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }
    }
}
