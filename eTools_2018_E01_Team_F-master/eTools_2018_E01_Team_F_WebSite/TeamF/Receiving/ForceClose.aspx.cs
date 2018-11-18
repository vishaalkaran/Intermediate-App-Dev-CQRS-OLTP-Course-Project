using AppSecurity.BLL;
using AppSecurity.DAL;
using AppSecurity.Entities;
using DMIT2018Common.UserControls;
using eTools.Data.Entities;
using eTools.Data.POCOs;
using eToolsSystem.BLL;
using eToolsSystem.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eTools_2018_E01_Team_F_WebSite.TeamF.Receiving
{
    public partial class ForceClose : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {
                    if (!User.IsInRole(SecurityRoles.Recieving))
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }

                }
            }


           
            MessageUserControl.TryRun(() =>
            {
                ApplicationUserManager userManager = new ApplicationUserManager(new
                   UserStore<ApplicationUser>(new ApplicationDbContext()));
                string userName = Context.User.Identity.GetUserName();
                //int employeeid = 0;
                if (!string.IsNullOrEmpty(userName))
                {
                    //employeeid = userManager.Get_CurrentEmployeeIDFromUserName(userName);
                    EmployeeInfo info = userManager.User_GetEmployee(userName);
                    UserNameLabel.Text = userName;
                    UserIDLabel.Text = info.EmployeeID.ToString();
                    EmployeeNameLB.Text = info.FullName;
                }
            });
        }

        protected void ReceiveButton_Click(object sender, EventArgs e)
        {
            PurchaseOrderDetailsController sysmgr = new PurchaseOrderDetailsController();
            int pOrderID = int.Parse(PurchaseOrderIDLabel.Text);
            List<OpenPurchaseOrderDetails> oPODetails = sysmgr.List_OpenPurchaseOrderDetails(pOrderID);


            //int receivedQty = 0;
            int returnQtyint = 0;
            int receivedQtyint = 0;
            bool receivedQtyinputfail = false;
            bool returnedQtyinputfail = false;
            bool returnReasoninputfail = false;
            int i = 0;
            foreach (GridViewRow agvrow in OpenPODetailsGridView.Rows)
            {
                string receivedQty = ((agvrow.FindControl("ReceivedQuantity") as TextBox).Text);
                if ((!(int.TryParse(receivedQty, out receivedQtyint))) || receivedQtyint < 0)
                {
                    receivedQtyinputfail = true;
                }
                string returnedQty = ((agvrow.FindControl("ReturnedQuantity") as TextBox).Text);
                if ((!(int.TryParse(returnedQty, out returnQtyint))) || returnQtyint < 0)
                {
                    returnedQtyinputfail = true;
                }
                oPODetails[i].ReceivedQuantity = receivedQtyint;
                oPODetails[i].ReturnedQuantity = returnQtyint;
                string returnReason = (agvrow.FindControl("ReturnReason") as TextBox).Text;
                if (string.IsNullOrEmpty(returnReason) && returnQtyint > 0)
                {
                    returnReasoninputfail = true;
                }
                oPODetails[i].ReturnReason = returnReason;
                i++;
            }
            if(receivedQtyinputfail || returnedQtyinputfail || returnReasoninputfail)
            {
                if (returnReasoninputfail)
                {
                    MessageUserControl.ShowInfo("Receive Order", "Incorrect data. If returning items from the purchase order, must provide a reason.");
                }
                else
                {
                    MessageUserControl.ShowInfo("Receive Order", "Incorrect data. ReceivedQuantity and ReturnedQuantity must be an integer greater than zero.");
                }
            }
            else
            {
                if(oPODetails.Any(x => x.ReceivedQuantity > x.QuantityOutstanding))
                {
                    MessageUserControl.ShowInfo("Receive Order", "Incorrect data. ReceivedQuantity cannot be greater than QuantityOutstanding.");
                }else
                {
                    MessageUserControl.TryRun(() =>
                    {
                        ReceiveOrderDetailsController sysmgr2 = new ReceiveOrderDetailsController();
                        sysmgr2.ReceiveOrder(pOrderID, oPODetails);
                        purchaseOrderController sysmgr3 = new purchaseOrderController();
                        PurchaseOrder pOrder = sysmgr3.PurchaseOrder_Get(pOrderID);
                        if (pOrder.Closed == true)
                        {
                            PurchaseOrderIDLabel.Text = "";
                            PurchaseOrderNumberLabel.Text = "";
                            DateLabel.Text = "";
                            VendorLabel.Text = "";
                            VendorPhoneLabel.Text = "";
                            ReceiveButton.Visible = false;
                            ForceCloseButton.Visible = false;
                            ReasonLabel.Visible = false;
                            ReasonTextBox.Visible = false;
                        }
                        OpenPOListView.DataBind();
                        OpenPODetailsGridView.DataBind();
                        UnorderedPurchaseItemCartListView.DataBind();
                    }, "Receive Order", "Order successfully received.");
                }
            }
        }

        protected void ForceCloseButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ReasonTextBox.Text))
            {
                MessageUserControl.ShowInfo("Required Data",
                    "Reason is required to close order.");
            }
            else
            {
                MessageUserControl.TryRun(() => {
                    int pOrderID = int.Parse(PurchaseOrderIDLabel.Text);
                    string forceCloseReason = ReasonTextBox.Text;
                    purchaseOrderController sysmgr = new purchaseOrderController();
                    sysmgr.ForceClosePurchaseOrder(pOrderID,forceCloseReason);
                }, "Force Close Order", "Order successfuly closed.");
                PurchaseOrderIDLabel.Text = "";
                PurchaseOrderNumberLabel.Text = "";
                DateLabel.Text = "";
                VendorLabel.Text = "";
                VendorPhoneLabel.Text = "";
                OpenPOListView.DataBind();
                OpenPODetailsGridView.DataBind();
                ReceiveButton.Visible = false;
                ForceCloseButton.Visible = false;
                ReasonLabel.Visible = false;
                ReasonTextBox.Visible = false;
            }
        }

        protected void PurchaseOrderSelectionList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            ReceiveButton.Visible = true;
            ForceCloseButton.Visible = true;
            ReasonLabel.Visible = true;
            ReasonTextBox.Visible = true;

            int pOrderID = int.Parse(e.CommandArgument.ToString());
            PurchaseOrderIDLabel.Text = pOrderID.ToString();
            UnorderedPurchaseItemCartListView.DataBind();

            using (var context = new eToolsContext())
            {
                PurchaseOrder pOrder = context.PurchaseOrders.Find(pOrderID);
                PurchaseOrderNumberLabel.Text = "Purchase Order #: " + pOrder.PurchaseOrderNumber.ToString();
                DateLabel.Text = "Date: " + pOrder.OrderDate == null ? "" : pOrder.OrderDate.Value.ToShortDateString();
                VendorLabel.Text = pOrder.Vendor.VendorName;
                VendorPhoneLabel.Text = pOrder.Vendor.Phone;
            }
        }
    }
}