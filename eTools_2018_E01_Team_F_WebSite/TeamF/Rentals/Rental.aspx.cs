using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using eTools.Data.Entities;
using eTools.Data.POCOs;
using eToolsSystem.DAL;
using System.ComponentModel;
using DMIT2018Common.UserControls;
using eToolsSystem.BLL;
using System.Globalization;
using AppSecurity.BLL;
using AppSecurity.DAL;
using AppSecurity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace eTools_2018_E01_Team_F_WebSite.TeamF.Rentals
{
    public partial class Rental : System.Web.UI.Page
    {
        List<string> logger = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //if (!IsPostBack)
                //{
                //    if (!Request.IsAuthenticated)
                //    {
                //        Response.Redirect("~/Account/Login.aspx");
                //    }
                //    else
                //    {
                //        if (!User.IsInRole(SecurityRoles.Rentting))
                //        {
                //            Response.Redirect("~/Account/Login.aspx");
                //        }
                //        GetUserName();
                //    }
                //}
                clearUserControls();
            }
            //selectedCustomerRental.Text = 234.ToString();  //Testing
        }

        protected void selectCustomer_Click(object sender, ListViewCommandEventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                if (!string.IsNullOrEmpty(HIDDEN_LABEL_selectedCustomerID.Text))
                {
                    throw new Exception("A customer profile is loaded! Must cancel to re-select");
                }
                else
                {
                    RentalCustomerController mgr = new RentalCustomerController();
                    /*
                    selectedCustomerName.Text = mgr.selectSingleCustomer(int.Parse(selectedCustomerRental.Text)).ToString();
                    selectedCustomerAddress.Text = mgr.selectSingleCustomer(int.Parse(selectedCustomerRental.Text)).ToString();
                    selectedCustomerCity.Text = mgr.selectSingleCustomer(int.Parse(selectedCustomerRental.Text)).ToString();

                    DOESN'T WORK!!!!
                    Empty string is default for selectedCustomerRental
                    Customer customer = mgr.selectSingleCustomer(int.Parse(selectedCustomerRental.Text));
                    */
                    Customer customer = mgr.selectSingleCustomer(int.Parse(e.CommandArgument.ToString()));

                    HIDDEN_LABEL_selectedCustomerID.Text = customer.CustomerID.ToString();
                    selectedCustomerName.Text = customer.LastName + ", " + customer.FirstName;
                    selectedCustomerAddress.Text = customer.Address;
                    selectedCustomerCity.Text = customer.City;

                    PhoneNumberInput.Enabled = false;
                    PhoneNumberInput.ForeColor = System.Drawing.Color.LightGray;

                    phoneNumberSubmitBtn.Enabled = false;
                    phoneNumberSubmitBtn.ForeColor = System.Drawing.Color.LightGray;
                }
            }, "Customer Found", "Profile has been successfully retrived.");
        }

        protected void newRental_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                clearUserControls();
            }, "Create New Rental", "New Rental Can Be Created Now.");
        }

        //Possible Validation
        protected void searchPhoneNumber_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                if (string.IsNullOrEmpty(PhoneNumberInput.Text))
                {
                    throw new Exception("Enter customers phone number please");
                }
                //else if() regex format validation
            }, "Select a customer", "select the correspoding customer from list please");
        }

        protected void deleteRental_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                if (string.IsNullOrEmpty(selectedCustomerRental.Text))
                {
                    throw new Exception("No form to delete");
                }
                else
                {

                    RentalController mgr = new RentalController();
                    mgr.deleteRental(int.Parse(selectedCustomerRental.Text));
                    RentalEquipmentListview.DataBind();
                    clearUserControls();
                }
            }, "Deleted Rental Order", "Order has beem successfully deleted.");
        }

        protected void addRentalEquipment_Click(object sender, ListViewCommandEventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                if (string.IsNullOrEmpty(HIDDEN_LABEL_selectedCustomerID.Text))
                {
                    throw new Exception("Must have customer selected");
                }
                else if (string.IsNullOrEmpty(creditcardinput.Text))
                {
                    throw new Exception("Must have credit card");
                }
                else if(PendingRentalListView.Items.Count() == 0)
                {
                    //Cont.
                    RentalController Rmgr = new RentalController();

                    //Marshall Methods
                    CouponController Cmgr = new CouponController();
                    //Test data
                    //Chip Andale EmployeeID: 10	
                    //Fuelling,Adolph 780.600.2840 CustomerID: 47	
                    //int createAndReturnEmptyRentalID
                    //selectedCustomerRental.Text = mgr.createAndReturnEmptyRentalID(47, 10, null, " ").ToString();
                    //public int createAndReturnEmptyRentalID(int customerid, int employeeid, int? couponid, string creditcard)
                    selectedCustomerRental.Text = Rmgr.createAndReturnEmptyRentalID(int.Parse(HIDDEN_LABEL_selectedCustomerID.Text),
                                                                                   10,
                                                                                   null,
                                                                                   creditcardinput.Text).ToString();
                    //Cmgr.ValidateCoupon(string.IsNullOrEmpty(couponinput.Text) ? (int?)null : couponinput.Text).CouponID


                    //Add selected equipment
                    RentalDetailController __addmgr = new RentalDetailController();
                    __addmgr.addRentalEquipment(int.Parse(selectedCustomerRental.Text), int.Parse(e.CommandArgument.ToString()));
                    RentalEquipmentListview.DataBind();
                    PendingRentalListView.DataBind();

                    if (string.IsNullOrEmpty(selectedCustomerRental.Text))
                    {
                        throw new Exception("Equipment has not been added!!");
                    }

                    MessageUserControl.ShowInfo("Form Created", "Item added to newly created form");
                    newRental.Visible = true;
                }
                else
                {
                    RentalDetailController mgr = new RentalDetailController();
                    mgr.addRentalEquipment(int.Parse(selectedCustomerRental.Text), int.Parse(e.CommandArgument.ToString()));
                    RentalEquipmentListview.DataBind();
                    PendingRentalListView.DataBind();

                    MessageUserControl.ShowInfo("Equipment has been added.");
                }
            });
        }

        protected void removeRentalEquipment_Click(object sender, ListViewCommandEventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                if (string.IsNullOrEmpty(HIDDEN_LABEL_selectedCustomerID.Text))
                {
                    throw new Exception("Must have customer selected");
                }
                else if (string.IsNullOrEmpty(creditcardinput.Text))
                {
                    throw new Exception("Must have credit card");
                }
                else
                {
                    RentalDetailController mgr = new RentalDetailController();
                    mgr.removeRentalEquipment(int.Parse(selectedCustomerRental.Text), int.Parse(e.CommandArgument.ToString()));
                    RentalEquipmentListview.DataBind();
                    PendingRentalListView.DataBind();
                }
            }, "Equipment Removed", "Equipment has been Removed.");
        }
        protected void clearUserControls()
        {
            HIDDEN_LABEL_selectedCustomerID.Text = "";
            selectedCustomerRental.Text = "";

            PhoneNumberInput.Text = "";
            selectedCustomerName.Text = "";
            selectedCustomerAddress.Text = "";
            selectedCustomerCity.Text = "";
            creditcardinput.Text = "";

            PhoneNumberInput.Enabled = true;
            PhoneNumberInput.ForeColor = System.Drawing.Color.Empty;

            phoneNumberSubmitBtn.Enabled = true;
            phoneNumberSubmitBtn.ForeColor = System.Drawing.Color.Empty;

            //no real need
            //RentalEquipmentListview.DataBind();
            CustomerListView.DataBind();
            PendingRentalListView.DataBind();
            newRental.Visible = false;
        }
        protected void GetUserName()
        {
            //grab the username from security (User)
            string username = User.Identity.Name;  //I need to use this name to find the appllication user record. 
            UserDisplayNameLB.Text = username;

            //obtain the employee information for this username
            MessageUserControl.TryRun(() =>
            {
                //connect to the ApplicationUserManager    //type casting 
                //connection string to applicationusermanager
                ApplicationUserManager secmgr = new ApplicationUserManager(
                    new UserStore<ApplicationUser>(new ApplicationDbContext()));
                EmployeeInfo info = secmgr.User_GetEmployee(username);
                EmployeeIDLB.Text = info.EmployeeID.ToString();
                EmployeeNameLB.Text = info.FullName;
            });
        }//eom

        protected void RentalEquipmentListview_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}