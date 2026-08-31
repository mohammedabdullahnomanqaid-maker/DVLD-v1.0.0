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
    public partial class frmTestAppointment : Form
    {
        private clsTestAppointment _TestAppointment;

        clsTestTypes.enTestType _TestTypeMode = clsTestTypes.enTestType.VisionTest;
        int _LocalDrivingLicenseApplicationID=-1;
        public frmTestAppointment(int LocalDrivingLicenseApplicationID,clsTestTypes.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestTypeMode = TestTypeID;
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
        }

        //done
        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = uctShowLocalDrivingLicenseApplicationInfo1.LocalDrivingLicenseApplicationID;
            bool IsPassed =clsTest.IsPassed (LocalDrivingLicenseApplicationID,(int)_TestTypeMode);
            if (IsPassed)
            {
                MessageBox.Show("This person already passed the test before, you can only retake failed test.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //check if he has an active appointment if yes prevent him if no allow.
            bool IsNotLoked = clsTestAppointment.IsThereAnActiveAppointment(LocalDrivingLicenseApplicationID, (int)_TestTypeMode);
            if(IsNotLoked)
            {
                MessageBox.Show("Person already have an active appointment for this test,you can not add new appointment", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Form frm = new frmAddEditAppointment(LocalDrivingLicenseApplicationID,_TestTypeMode);
            frm.ShowDialog();
            frmVisionTest_Load(null, null);
        }
        //done
        private void _PerformTypeOfTest()
        {
            switch (_TestTypeMode)
            {
                case clsTestTypes.enTestType.VisionTest:
                    picTypeOfTest.Image = Properties.Resources.Vision_512;
                    lbTypeOFTest.Text = "Vision Test Appointment";
                    this.Text = lbTypeOFTest.Text;
                    break;

                case clsTestTypes.enTestType.WrittenTest:
                    picTypeOfTest.Image = Properties.Resources.Written_Test_512;
                    lbTypeOFTest.Text = "Written Test Appointment";
                    this.Text = lbTypeOFTest.Text;
                    break;

                case clsTestTypes.enTestType.StreetTest:
                    picTypeOfTest.Image = Properties.Resources.driving_test_512;
                    lbTypeOFTest.Text = "Street Test Appointment";
                    this.Text = lbTypeOFTest.Text;
                    break;
            }
        }
        //done
        private void frmVisionTest_Load(object sender, EventArgs e)
        {
           
          
            dgvAppointment.DataSource = clsTestAppointment.GetTestAppointmentPerTest(_LocalDrivingLicenseApplicationID,(int)_TestTypeMode) ;
            lbRecord.Text = dgvAppointment.Rows.Count.ToString();

            if (dgvAppointment.Rows.Count > 0)
            {
                dgvAppointment.Columns[0].HeaderText = "Appointment ID";
                dgvAppointment.Columns[0].Width = 190;

                dgvAppointment.Columns[1].HeaderText = "Appointment Date";
                dgvAppointment.Columns[1].Width = 250;

                dgvAppointment.Columns[2].HeaderText = "Paid Fees";
                dgvAppointment.Columns[2].Width = 150;

                dgvAppointment.Columns[3].HeaderText = "Is Locked";
                dgvAppointment.Columns[3].Width = 120;

            }

            uctShowLocalDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
            _PerformTypeOfTest();
        }
        //done
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditAppointment(_LocalDrivingLicenseApplicationID,_TestTypeMode,(int)dgvAppointment.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmVisionTest_Load(null, null);
        }
        //done
        private void reTakeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmMakeTest((int)dgvAppointment.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmVisionTest_Load(null, null);
        }
    }
}
