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
using BussinseLayer;

namespace Course19
{
    public partial class frmAddEditPerson : Form
    {
        enum enGendor { Male = 0, Femal = 1 };
        private enum enMode { AddMode,UpdateMode};

        private enMode _Mode;
        clsBPeople _Person;
        private int _PersonID;
        //delelget
        public delegate void DataBackEventHandler(object sender, int PersonID);
        public event DataBackEventHandler DataBack;


        public frmAddEditPerson(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
            _Mode = enMode.UpdateMode;
        }

        public frmAddEditPerson()
        {
            InitializeComponent();
            _Mode = enMode.AddMode;
        }
                     
        private void _FillCountriesInComboBox()
        {

            cbCountries.DataSource = clsCountry.GetAllCountries();
            cbCountries.DisplayMember = "CountryName";
            cbCountries.ValueMember = "CountryID";
        }
        private void _ResetDefualtValues()
        {
            ClearForm();

            _FillCountriesInComboBox();

            if (_Mode == enMode.AddMode)
            {
                lbAddEditPerson.Text = "Add New Person";
                _Person = new clsBPeople();
            }
            else
            {
                lbAddEditPerson.Text = "Update Person";
            }

            if (rbMale.Checked)
                picPersonalPhoto.Image = Properties.Resources.Male_512;
            else
                picPersonalPhoto.Image = Properties.Resources.Female_512;

            lkRemove.Visible = (picPersonalPhoto.ImageLocation != null);

            //shoud not add age less than 18
            dateTimePicker1.MaxDate = DateTime.Now.AddYears(-18);
            dateTimePicker1.Value = dateTimePicker1.MaxDate;

            //should not add age more than 100 year
            dateTimePicker1.MinDate = DateTime.Now.AddYears(-100);

            cbCountries.SelectedIndex = cbCountries.FindString("Yemen");

         
        }

        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
      
        
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnClose, 88,32);
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnClose, 86, 29);
        }

        private void btnSave_MouseEnter(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnSave, 88, 32);
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnSave, 86, 29);
        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox temp = new TextBox();
            temp = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(temp.Text.Trim()))
            {
                errorProvider1.SetError(temp, "Error this faild is required!");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(temp, "");
            }
        }

        void ClearForm()
        {
            tbFirstName.Text = "";
            tbSecondName.Text = "";
            tbThirdName.Text = "";
            tbLastName.Text = "";
            tbEmail.Text = "";
            tbAddress.Text = ""; 
            tbPhone.Text = "";
            tbNationalNo.Text = "";
            rbMale.Checked = true;
        }

        void SaveToDB()
        {
           // clsBPeople _Person = new clsBPeople();
            _Person.FirstName = tbFirstName.Text.Trim();
            _Person.SecondName = tbSecondName.Text.Trim();
            _Person.ThirdName = tbThirdName.Text.Trim();
            _Person.LastName = tbLastName.Text.Trim();
            _Person.Email = tbEmail.Text.Trim();
            _Person.Phone = tbPhone.Text.Trim();
            _Person.Address = tbAddress.Text.Trim();
            _Person.DateOfBirth = dateTimePicker1.Value;
            _Person.NationalNo = tbNationalNo.Text.Trim();
            _Person.CountryID =(int) cbCountries.SelectedValue;

            if (rbMale.Checked)
                _Person.Gendor = (int)enGendor.Male;
            else
                _Person.Gendor = (int)enGendor.Femal;

            if (picPersonalPhoto.ImageLocation != null)
                _Person.ImagePath = picPersonalPhoto.ImageLocation.ToString();
            else
                _Person.ImagePath = "";

            if (_Person.Save())
            {
                lbPersonID.Text = _Person.PersonID.ToString();
                lbAddEditPerson.Text = "Update Person";
                _Mode = enMode.UpdateMode;
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
                MessageBox.Show("Error : Data is not saved successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool _HandlePersonImage()
        {
            //this procedure will handle the person image,
            //it will take care of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and 
            // place it in the images folder.


           if(_Person.ImagePath!=picPersonalPhoto.ImageLocation)
            {
                if (_Person.ImagePath != "")
                    if ( _Person.ImagePath!=null)
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch(IOException iox)
                    {
                        using(StreamWriter writer =new StreamWriter("Log_DVLD_Errors"))
                        {
                            writer.WriteLine("\t\t\tError in Delete Old Image\n" + iox.Message);
                        }
                    }
                }
            }
            if (picPersonalPhoto.ImageLocation != null)
            {
                string sourceImageFile = picPersonalPhoto.ImageLocation.ToString();

                if(clsUtil.CopyImageToProjectImagesFolder(ref sourceImageFile))
                {
                    //when we put the path in imagelocation it show the picture
                    picPersonalPhoto.ImageLocation = sourceImageFile;
                    return true;
                }
                else
                {
                    MessageBox.Show("Error Copying File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            //why return true? because the image is nullable
            return true;

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            //ValidateChildren this function use to check if all controls in form is validate accourding to errorprovider
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_HandlePersonImage())
                return;

            SaveToDB();

        }
        private void _LoadData()
        {
                   _Person = clsBPeople.Find(_PersonID);
            if (_Person == null)
            {
                MessageBox.Show("No Person with ID = " + _PersonID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
            lbPersonID.Text = _PersonID.ToString() ;
            tbFirstName.Text = _Person.FirstName;
            tbSecondName.Text = _Person.SecondName;
            tbThirdName.Text = _Person.ThirdName;
            tbLastName.Text = _Person.LastName;
            tbEmail.Text = _Person.Email;
            tbPhone.Text = _Person.Phone ;
            tbAddress.Text = _Person.Address;
            tbNationalNo.Text = _Person.NationalNo;
            dateTimePicker1.Value =_Person.DateOfBirth;

                    if (_Person.Gendor==1)
                    rbFemal.Checked = true;
                    else
                    rbMale.Checked = true;

            cbCountries.SelectedIndex = cbCountries.FindString(_Person.CountryInfo.CountryName) ;


            if (_Person.ImagePath != null || _Person.ImagePath != "")
                picPersonalPhoto.ImageLocation= _Person.ImagePath;

            lkRemove.Visible = (_Person.ImagePath != "");

        }
        private void frmAddEditPerson_Load(object sender, EventArgs e)
         {
            _ResetDefualtValues();
            if(_Mode==enMode.UpdateMode)
                _LoadData();

        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            picPersonalPhoto.Image = Properties.Resources.Male_512;
       
        }

        private void rbFemal_CheckedChanged(object sender, EventArgs e)
        {
            picPersonalPhoto.Image = Properties.Resources.Female_512;
        }

        private void lkSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.InitialDirectory = "E:\\my Pictures";
            openFileDialog1.Title = "Add Personal Photo";
            openFileDialog1.Filter = "Image File|*.Jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;//when user open file and close it after comeback open in the current location
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                picPersonalPhoto.Load(selectedFilePath);
                lkRemove.Visible = true;
            }
        }

        private void lkRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            picPersonalPhoto.ImageLocation = null;
            if(rbFemal.Checked)
                picPersonalPhoto.Image = Properties.Resources.Female_512;
            else
                picPersonalPhoto.Image = Properties.Resources.Male_512;

            lkRemove.Visible = false;
        }

        private void tbNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbNationalNo.Text.Trim()))
            {
                errorProvider1.SetError(tbNationalNo, "Error this faild is required!");
                e.Cancel = true;
                return;
            }
           //we still condition for update
                if(tbNationalNo.Text.Trim()!=_Person.NationalNo && clsBPeople.IsPersonExist(tbNationalNo.Text.Trim()))   
                {
                        errorProvider1.SetError(tbNationalNo, "National Number is used for another person!");
                        e.Cancel = true;
                }
          else
                errorProvider1.SetError(tbNationalNo, "");
            
        }

        private void tbEmail_Validating(object sender, CancelEventArgs e)
        {
            //no need to validate the email incase it's empty.
            if (tbEmail.Text=="")
            {
                return;
            }
           if(!clsValidation.ValidateEmail(tbEmail.Text.Trim()))
            {
                errorProvider1.SetError(tbEmail, "Error this format is not valid!");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(tbEmail, "");
            }
        }

        private void frmAddEditPerson_FormClosing(object sender, FormClosingEventArgs e)
        {
           
                if(picPersonalPhoto.Tag!=null)
                {
                    string MalePath = "C:\\DVLD-people-pictures\\Male 512.png";
                    string FemalPath = "C:\\DVLD-people-pictures\\Female 512.png";
                    if (picPersonalPhoto.Tag.ToString()!=MalePath&&picPersonalPhoto.Tag.ToString()!=FemalPath)
                    File.Delete(picPersonalPhoto.Tag.ToString());

                }
          
        }
    }
}
