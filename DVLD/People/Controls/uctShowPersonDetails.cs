using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussinseLayer;
using System.IO;

namespace Course19
{
    public partial class uctShowPersonDetails : UserControl
    {
        private int _PersonID=-1;

        private clsBPeople _Person;
        public clsBPeople SelectedPersonInfo
        {
            get { return _Person; }
        }
        public uctShowPersonDetails()
        {
            InitializeComponent();
        }
        public int PersonID
        {
            get { return _PersonID; }
        }
        private void _LoadPersonalImage()
        {
            if (_Person.Gendor == 0)
                picPersonalPhoto.Image = Properties.Resources.Male_512;
            else
                picPersonalPhoto.Image = Properties.Resources.Female_512;

            string ImagePaht=  _Person.ImagePath;
            if (ImagePaht != "")
                if (File.Exists(ImagePaht))
                    picPersonalPhoto.ImageLocation = ImagePaht;
            else
                    MessageBox.Show("Could not find this image: = " + ImagePaht, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        public void ResetPersonInfo()
        {
            llEditPersonInfo.Enabled = false;
            _PersonID = -1;
            lbName.Text = "[????]";
            lbPersonID.Text = "[????]";
            lbAddress.Text = "[????]";
            lbNationalNo.Text = "[????]";
            lbPhone.Text = "[????]";
            lbEmail.Text = "[????]";
            lbGendor.Text = "[????]";
            lbDateOfBirth.Text = "[????]";
            lbCountry.Text = "[????]";
            picGendor.Image = Properties.Resources.Man_32;
            picPersonalPhoto.Image = Properties.Resources.Male_512;
          
        }
        private void _FillPersonInfo()
        {
            llEditPersonInfo.Visible = true;

            lbPersonID.Text = _Person.PersonID.ToString();


                    lbName.Text = _Person.FirstName+" " +_Person.SecondName +" " +_Person.ThirdName +" " +_Person.LastName ;
            lbEmail.Text = _Person.Email;
            lbPhone.Text = _Person.Phone;
            _PersonID = _Person.PersonID;
            lbAddress.Text = _Person.Address;
            lbNationalNo.Text = _Person.NationalNo;
            lbDateOfBirth.Text = _Person.DateOfBirth.ToString();
            lbGendor.Text=(_Person.Gendor == 1)?"Femal": "Male";

            lbCountry.Text = clsCountry.Find(_Person.CountryID).CountryName;
            _LoadPersonalImage();
        }
        public void LoadData(int PersonID)
        {
          
            _Person = clsBPeople.Find(PersonID);
           
            if (_Person == null) 
            {
                ResetPersonInfo();
                MessageBox.Show($"No Person with PersonID= {PersonID} " , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
          
            _FillPersonInfo();
        }
        public void LoadData(string NationalNo)
        {

            _Person = clsBPeople.Find(NationalNo);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show($"No Person with National No {NationalNo} ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
                return;
            }

            _FillPersonInfo();
        }
        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmAddEditPerson(PersonID);
             frm.ShowDialog();

            //refresh 
            LoadData(PersonID);
        }
    }
}
