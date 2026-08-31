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
    public partial class uctSearchAndShowPersonCard : UserControl
    {
        //make define for event
        public event Action<int> OnPersonSelected;
        //done
        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
                handler(PersonID);//Raise the event with the pramater.
        }
        public int PersonID
        {
            get { return uctShowPersonDetails1.PersonID; }
        }

        // to make the header of filter visible or none.
        private bool _FilterEnable=true;
        public bool FilterEnable
        {
            get
            {
                return _FilterEnable;
            }
            set
            {
                _FilterEnable = value;
                gpFilter.Enabled = _FilterEnable;
            }
        }

        //to make the btnAddNewPerson visible or none visible.
        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get 
            {
                return _ShowAddPerson;
            }
            set
            {
                _ShowAddPerson = value;
                picAdd.Visible = _ShowAddPerson;
            }
        }

        // to expose the Person info to out of control.
        public clsBPeople SelectedPersonInfo
        {
            get { return uctShowPersonDetails1.SelectedPersonInfo; }
        }

        public uctSearchAndShowPersonCard()
        {
            InitializeComponent();
        }
        //done
        private void uctSearchAndShowPersonCard_Load(object sender, EventArgs e)
        {
            cbFilter.SelectedIndex = 0;
            tbFilter.Focus();
        }
        //done
        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            //(char)13 is Enter control if user click
            //on enter button it will perform btnSearch
            if (e.KeyChar == (char)13)
            {
                btnSearch.PerformClick();
            }

            //not allow charcter
            if (cbFilter.Text=="Person ID")
             e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void tbFilterKeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

        }
        //done
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Clear();
            tbFilter.Focus();
        }
        //done
        private void picAdd_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson();
            frm.DataBack += DataBackEventHandler;//subscribe to the event
            frm.ShowDialog();
        }

        public void DataBackEventHandler(object sender,int PersonID)
        {
            cbFilter.SelectedIndex = 1;
            tbFilter.Text = PersonID.ToString();
            uctShowPersonDetails1.LoadData(PersonID);
        }
        //done
        public void FindNow()
        {
            //int.parse() convert string to intger.
            switch (cbFilter.Text.Trim())
            {
                case "Person ID":
                    uctShowPersonDetails1.LoadData(int.Parse(tbFilter.Text.Trim()));
                    break;

                case "National No":
                    uctShowPersonDetails1.LoadData(tbFilter.Text.Trim());
                    break;

                default:
                    break;
            }             
            
            //we still have condition 
            if (OnPersonSelected != null)
                //to give the user the PersonID that call event OnPersonSelected and notify him
                OnPersonSelected(uctShowPersonDetails1.PersonID);
        }
        //done
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FindNow();
        }
        //done
        private void tbFilter_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbFilter.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbFilter, "This field is required !");
            }
            else
            {
                errorProvider1.SetError(tbFilter, "");
            }
        }
       //done
        public void FilterFocus()
        {
            tbFilter.Focus();
        }
        //done
        public void LoadPersonInfo(int PersonID)
        {
            cbFilter.SelectedIndex = 1;
            tbFilter.Text = PersonID.ToString();
            FindNow();
        }
    }
}
