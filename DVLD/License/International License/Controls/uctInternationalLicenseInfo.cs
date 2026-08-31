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

namespace Course19
{
    public partial class uctInternationalLicenseInfo : UserControl
    {
        private clsInternationalLicense _InternationalLicense;
        private clsBPeople _Person;
        public uctInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        private void _ResetDefaultValue()
        {
            lbApplicationID.Text = "[???}";
            lbDateOfBirth.Text = "[???}";
            lbDriverID.Text = "[???}";
            lbExpirationDate.Text = "[???}";
            lbGendor.Text = "[???}";
            lbLicenseID.Text = "[???}";
            lbIntLicenseID.Text = "[???}";
            lbIsActive.Text = "[???}";
            lbIssueDate.Text = "[???}";
            lbName.Text = "[???}";
            lbNationalNo.Text = "[???}";
            picPerson.Image = Properties.Resources.Male_512;
        }

        public void LoadInternationalLicenseInfo(int InternationalLicenseID)
        {
            _InternationalLicense = clsInternationalLicense.Find(InternationalLicenseID);
            if (_InternationalLicense == null)
            {
                _ResetDefaultValue();
                MessageBox.Show("International License ID =" + InternationalLicenseID.ToString() + " could not found", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int PersonID = clsBPeople.GetPersonIDByLicenseID(_InternationalLicense.IssuedUsingLocalLicense);
            _Person = clsBPeople.Find(PersonID);

            if (_Person == null)
            {
                _ResetDefaultValue();
                MessageBox.Show("Person ID =" + PersonID.ToString() + " could not found", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lbApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
            lbDriverID.Text = _InternationalLicense.DriverID.ToString();
            lbExpirationDate.Text = _InternationalLicense.ExpirationDate.ToString();
            lbIntLicenseID.Text = InternationalLicenseID.ToString();
            lbIssueDate.Text = _InternationalLicense.IssuedDate.ToString();
            lbLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicense.ToString();
            if (_InternationalLicense.IsActive)
                lbIsActive.Text = "Yes";
            else
                lbIsActive.Text = "No";
            lbName.Text = _Person.FullName;
            lbNationalNo.Text = _Person.NationalNo;
            lbDateOfBirth.Text = _Person.DateOfBirth.ToString();
            if (_Person.Gendor == 0)
                lbGendor.Text = "Male";
            else
                lbGendor.Text = "Femal";
            if (_Person.ImagePath != null || _Person.ImagePath != "")
                picPerson.ImageLocation = _Person.ImagePath;
            else
            {
                if (_Person.Gendor == 0)
                    picPerson.Image = Properties.Resources.Male_512;
                else
                    picPerson.Image = Properties.Resources.Female_512;
            }
        }
    }

}
