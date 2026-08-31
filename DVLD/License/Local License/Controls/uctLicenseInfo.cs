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
    public partial class uctLicenseInfo : UserControl
    {
        private clsLicense _License;
        private int _LicenseID;
        public bool IsFound = false;

        public clsLicense License
        {
            get { return _License; }
        }
   
        public int LicenseID
        {
            get { return _LicenseID; }
        }
        public uctLicenseInfo()
        {
            InitializeComponent();
        }

        private void _ResetDefault()
        {
            lbClass.Text = "[???]";
            lbName.Text = "[???]";
            lbID.Text = "[???]";
            lbIsActive.Text = "[???]";
            lbIsDtaind.Text = "[???]";
            lbGendor.Text = "[???]";
            lbIssueDate.Text = "[???]";
            lbNationalNo.Text = "[???]";
            lbDateOfBirth.Text = "[???]";
            lbDriverID.Text = "[???]";
            lbExpirationDate.Text = "[???]";
            picPerson.Image = Properties.Resources.Male_512;
        }
        private void _PerformImage()
        {
            if (_License.DriverInfo.PersonInfo.Gendor == 0)
            {
                picGendor.Image = Properties.Resources.Man_32;
                picPerson.Image = Properties.Resources.Male_512;
            }
            else
            {
                picGendor.Image = Properties.Resources.Woman_32;
                picPerson.Image = Properties.Resources.Female_512;
            }


            string ImagePath = _License.DriverInfo.PersonInfo.ImagePath;
            if (ImagePath != string.Empty)
            {
                if (File.Exists(ImagePath))
                {
                    picPerson.Load(ImagePath);
                }
                else
                    MessageBox.Show("Error : Not Found Image with path : " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        public  void LoadLicenseInfo(int LicenseID)
        {
            _LicenseID = LicenseID;

            _License = clsLicense.Find(LicenseID);
            if (_License == null)
            {
                _LicenseID = -1;
                _ResetDefault();
                MessageBox.Show("License with License ID : " + LicenseID.ToString() + " not found", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        

            lbID.Text = LicenseID.ToString();
            lbDriverID.Text = _License.DriverID.ToString();
            lbClass.Text = clsLicenseClass.Find(_License.LicenseClassID).ClassName;
            lbDateOfBirth.Text =clsFormat.DateToShort(_License.DriverInfo.PersonInfo.DateOfBirth);
            lbNationalNo.Text =_License.DriverInfo.PersonInfo.NationalNo;
            lbName.Text = _License.DriverInfo.PersonInfo.FullName;
            lbIsActive.Text = _License.IsActive ? "Yes" : "No";
            lbIsDtaind.Text = (clsDetaind.IsLicenseDetaind(_LicenseID)) ? "Yes" : "No";
            lbIssueDate.Text =clsFormat.DateToShort(_License.IssueeDate);
            lbExpirationDate.Text =clsFormat.DateToShort(_License.ExpirationDate);
            lbIssueReason.Text = _License.IssueReasonTest;
            lbNotes.Text = (_License.Notes == string.Empty) ? "No Notes" : _License.Notes;
            lbGendor.Text = (_License.DriverInfo.PersonInfo.Gendor == 0)? "Male":"Femal";
           
            _PerformImage();



        }
    }
}
