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
    public partial class frmMakeTest : Form
    {
        clsTest _Test;
        private clsTestTypes.enTestType _TestTypeMode = clsTestTypes.enTestType.VisionTest;

        private int _TestAppointmentID = -1;
        public frmMakeTest(int TestAppointmentID)
        {
            InitializeComponent();
            _TestAppointmentID = TestAppointmentID;
        }
        //done
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //done
        private void frmMakeTest_Load(object sender, EventArgs e)
        {
            uctTestInfo1.TestType = _TestTypeMode;

            uctTestInfo1.LoadTestAppointmentInfo(_TestAppointmentID);


            rbPass.Checked = true;
            tbNote.Focus();
          
            if (uctTestInfo1.TestID != -1)
            {
                _Test = clsTest.Find(uctTestInfo1.TestID);
                if (_Test == null)
                    return;

                if (_Test.TestResault)
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;

                tbNote.Text = _Test.Note;
                rbFail.Enabled = false;
                rbPass.Enabled = false;
                btnSave.Enabled = false;

                lbWhenLocked.Visible = true;
            }
            else
                _Test = new clsTest();
        }
        //done
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save ? After that you can not change the Pass/Fail Resault after you save.", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;
            
            _Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _Test.Note = tbNote.Text.Trim();
                _Test.TestResault = rbPass.Checked;
          
            _Test.TestAppointmentID = _TestAppointmentID;
       //    int ApplicationID=clsLocalDrivingLicenseApplication.Find(clsTestAppointment.Find(_TestAppointmentID).LocalDrivingLicenseApplicationID).ApplicationID;


            if (_Test.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); 
            }
            else
                MessageBox.Show("Data did not Saved.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
