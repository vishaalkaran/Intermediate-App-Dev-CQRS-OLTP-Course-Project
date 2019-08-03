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
    public partial class Return : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (!Request.IsAuthenticated)
                //{
                //    Response.Redirect("~/Account/Login.aspx");
                //}
                //else
                //{
                //    if (!User.IsInRole(SecurityRoles.Rentting))
                //    {
                //        Response.Redirect("~/Account/Login.aspx");
                //    }
                //    GetUserName();
                //}
            }

            //Testing
            testingOnly();

        }//eom

        protected void newReturn_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                selectedCustomerRental.Text = "";
                HIDDEN_LABEL_selectedCustomerID.Text = "";
                PhoneNumberInput.Text = "";
                selectedCustomerName.Text = "";
                selectedCustomerAddress.Text = "";
                selectedCustomerCity.Text = "";

                PhoneNumberInput.Enabled = true;
                PhoneNumberInput.ForeColor = new System.Drawing.Color();

                phoneNumberSubmitBtn.Enabled = true;
                phoneNumberSubmitBtn.ForeColor = new System.Drawing.Color();

                newReturn.Visible = true;

            }, "Create New Rental", "New Rental Can Be Created Now.");
        }

        protected void payReturn_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {


            }, "Rental invoice has been paid", "Invoice has been saved to database");
        }

        protected void processReturn_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                

            }, "Rented equipment has been return", "Returned equipment can be rented out now");
        }

        protected void selectCustomer_Click(object sender, ListViewCommandEventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                if (!phoneNumberSubmitBtn.Enabled)
                //if (!string.IsNullOrEmpty(selectedCustomerRental.Text))
                {
                    throw new Exception("A customer profile is loaded! Must cancel to re-select");
                }
                else
                {
                    RentalCustomerController mgr = new RentalCustomerController();
                    Customer customer = mgr.selectSingleCustomer(int.Parse(e.CommandArgument.ToString()));

                    //customer = mgr.customersReturnLookUp
                    selectedCustomerRental.Text          = e.CommandArgument.ToString();
                    HIDDEN_LABEL_selectedCustomerID.Text = customer.CustomerID.ToString();
                    PhoneNumberInput.Text                = customer.ContactPhone;
                    selectedCustomerName.Text            = customer.LastName + ", " + customer.FirstName;
                    selectedCustomerAddress.Text         = customer.Address;
                    selectedCustomerCity.Text            = customer.City;

                    //Actually editing database!!!
                    //RentalController RCmgr = new RentalController();
                    //Rental by its self gernerats a error, must declare entire namespace - naming coflicts
                    //eTools.Data.Entities.Rental rental = RCmgr.returnSingleRentalForm(int.Parse(e.CommandArgument.ToString()));
                    //RentalDetails
                    //DateOut.Text = rental.RentalDate.ToString();
                    //CreditCard.Text = rental.CreditCard;
                    //Subtotal.Text = rental.SubTotal.ToString();
                    //GST.Text = rental.TaxAmount.ToString();
                    //Discount.Text = rental.Coupon.CouponDiscount.ToString();
                    //Total.Text = (((rental.SubTotal) + (rental.TaxAmount)) - ((decimal)rental.Coupon.CouponDiscount)).ToString();
                    //TextBox as Input


                    RentalDetailController RDCmgr = new RentalDetailController();
                    auxReturnInfo info = RDCmgr.getauxeturnInfo(int.Parse(e.CommandArgument.ToString()));

                    //RentalDetails
                    DateOut.Text = info.dateout.ToString();
                    CreditCard.Text = info.creditcard;
                    Subtotal.Text = info.subtotal.ToString();
                    GST.Text = info.gst.ToString();
                    Discount.Text = info.discount.ToString();
                    Total.Text = info.total.ToString();

                    PhoneNumberInput.Enabled = false;
                    PhoneNumberInput.ForeColor = System.Drawing.Color.LightGray;

                    //Button as Input
                    phoneNumberSubmitBtn.Enabled = false;
                    phoneNumberSubmitBtn.ForeColor = System.Drawing.Color.LightGray;

                    //No need as the if above checks if profile is loaded 
                    //selectedCustomerBtn

                    newReturn.Visible = true;
                }
            }, "Customer Found", "Profile has been successfully retrived. Must select New Return Button to Load New Profile");
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

        protected void ReturnListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void testingOnly()
        {
            MessageUserControl.TryRun(() =>
            {
                //test case
                int testRentalId = 235;
                RentalCustomerController mgr = new RentalCustomerController();
                Customer customer = mgr.selectSingleCustomer(testRentalId);

                //customer = mgr.customersReturnLookUp

                selectedCustomerRental.Text = testRentalId.ToString();
                HIDDEN_LABEL_selectedCustomerID.Text = customer.CustomerID.ToString();
                PhoneNumberInput.Text = customer.ContactPhone;
                selectedCustomerName.Text = customer.LastName + ", " + customer.FirstName;
                selectedCustomerAddress.Text = customer.Address;
                selectedCustomerCity.Text = customer.City;


                RentalDetailController RDCmgr = new RentalDetailController();
                auxReturnInfo info = RDCmgr.getauxeturnInfo(testRentalId);

                //RentalDetails
                DateOut.Text = info.dateout.ToString();
                CreditCard.Text = info.creditcard;
                Subtotal.Text = info.subtotal.ToString();
                GST.Text = info.gst.ToString();
                Discount.Text = info.discount.ToString();
                Total.Text = info.total.ToString();

                PhoneNumberInput.Enabled = false;
                PhoneNumberInput.ForeColor = System.Drawing.Color.LightGray;

                //Button as Input
                phoneNumberSubmitBtn.Enabled = false;
                phoneNumberSubmitBtn.ForeColor = System.Drawing.Color.LightGray;

                //No need as the if above checks if profile is loaded 
                //selectedCustomerBtn

                newReturn.Visible = true;
            }, "Testing Error", "Testing related error");
        }
    }
}