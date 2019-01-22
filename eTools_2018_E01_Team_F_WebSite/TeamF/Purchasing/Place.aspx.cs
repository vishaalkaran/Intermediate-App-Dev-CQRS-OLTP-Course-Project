using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additonal Namespaces
using eToolsSystem.BLL;
using eTools.Data.POCOs;
using eTools.Data.Entities;
using DMIT2018Common.UserControls;
using System.Globalization;
using AppSecurity.BLL;
using AppSecurity.DAL;
using AppSecurity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace eTools_2018_E01_Team_F_WebSite.TeamF.Purchasing
{
    public partial class Place : System.Web.UI.Page
    {
        //List<CurrentActiveOrderView> currentOrderListPOCOs = new List<CurrentActiveOrderView>();

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
                    if (!User.IsInRole(SecurityRoles.Purchasing))
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }

                }
            }


                TestLB.Text = "";
            DeleteButton.Attributes.Add("onclick", "return window.confirm('Confirm Delete Current Active Order?');");
            PlaceButton.Attributes.Add("onclick", "return window.confirm('Confirm Place Order?');");
            UpdateButton.Attributes.Add("onclick", "return window.confirm('Confirm Update Current Active Order?');");
            ResetButton.Attributes.Add("onclick", "return window.confirm('Confirm reseting the current window(will not change any database)?');");
            //ResetLinkButton.Attributes.Add("onclick", "return window.confirm('Confirm reseting the current window(will not change any database)?');");
            GetUserName();
        }

        protected void FetchButton_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                int count = 0;
                int tempint = int.Parse(VendorDDL.SelectedValue);
                if (tempint == 0)
                {
                    //Response.Redirect(Request.RawUrl);
                    throw new Exception("Please select a vendor");                    
                }
                int employeeID = int.Parse(EmployeeIDLB.Text);
                VendorIDLB.Text = tempint.ToString();
                int vendorID = int.Parse(VendorIDLB.Text);
                PurchaseOrderDetailsController sysmgr = new PurchaseOrderDetailsController();
                List<CurrentActiveOrderView> currentOrderListPOCOs = new List<CurrentActiveOrderView>();
                currentOrderListPOCOs = sysmgr.List_OrderDetailPOCO(employeeID, vendorID);
                List<VendorStockItemsView> currentStockItemListPOCOs = sysmgr.List_VendorStockItemsPOCO(vendorID, currentOrderListPOCOs);

                CurrentActiveOrderGDView.DataSource = currentOrderListPOCOs;
                CurrentActiveOrderGDView.DataBind();
                VendorStockItemsGDView.DataSource = currentStockItemListPOCOs;
                VendorStockItemsGDView.DataBind();

                OrderSummaryDisplay(vendorID);
                count = CurrentActiveOrderGDView.Rows.Count;
                MessageUserControl.ShowInfo("Fetched data", "Displaying " + count + " rows");
                //TestLB.Text = "POID " + currentStockItemListPOCO.ToString() + " VID "+ currentPurchaseOrder.VendorID.ToString();
            });
            //MessageUserControl.TryRun(() =>
            //{                
            //}, "Fetched data", "Displaying " + count + " rows");
        }

        protected void CurrentActiveOrderGridView_RowCommand_RemoveItem(object sender, CommandEventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                int orderDetailSID = int.Parse(e.CommandArgument.ToString());
                int vendorID = int.Parse(VendorIDLB.Text);
                int employeeID = int.Parse(EmployeeIDLB.Text);
                PurchaseOrderDetailsController sysmgr = new PurchaseOrderDetailsController();
                //TestLB.Text = "Stock item number " + sysmgr.RemovingASingleRowFromActiveOrder(vendorID, orderDetailSID) + " was removed from the list, " + (CurrentActiveOrderGDView.Rows.Count-1).ToString() +" items left on view";
                List<CurrentActiveOrderView> currentOrderListPOCOs = new List<CurrentActiveOrderView>();
                currentOrderListPOCOs = GetCurrentActiveOrderPOCOList();
                CurrentActiveOrderView toRemoveItem = currentOrderListPOCOs.Where(x => x.SID.Equals(orderDetailSID)).FirstOrDefault();

                currentOrderListPOCOs.Remove(toRemoveItem);

                currentOrderListPOCOs = sysmgr.RemovingWhileUpdating(employeeID,vendorID, currentOrderListPOCOs);
                int tempCount = 0;
                if (currentOrderListPOCOs ==null)
                {
                    tempCount = 0;
                }
                else
                {
                    tempCount = currentOrderListPOCOs.Count;
                }                

                TestLB.Text = "Item " + orderDetailSID + " was removed from the list, " + tempCount.ToString() + " items left on view";                
                MessageUserControl.ShowInfo("Item Removed",TestLB.Text);
                
                             
                List<VendorStockItemsView> vendorStockItemListPOCOs = sysmgr.List_VendorStockItemsPOCO(vendorID, currentOrderListPOCOs);

                //display
                
                CurrentActiveOrderGDView.DataSource = currentOrderListPOCOs;               
                
                CurrentActiveOrderGDView.DataBind();
                VendorStockItemsGDView.DataSource = vendorStockItemListPOCOs;
                VendorStockItemsGDView.DataBind();

                OrderSummaryDisplay(vendorID);
                VendorDDL.SelectedValue = int.Parse(VendorIDLB.Text).ToString();

            });
            

        } //eom;

        protected void CurrentActiveOrderGridView_RowCommand_AddItem(object sender, CommandEventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                int orderDetailSID = int.Parse(e.CommandArgument.ToString());
                int employeeID = int.Parse(EmployeeIDLB.Text);
                List<CurrentActiveOrderView> currentOrderListPOCOs = GetCurrentActiveOrderPOCOList();                
                PurchaseOrderDetailsController sysmgr = new PurchaseOrderDetailsController();
                int vendorID = int.Parse(VendorIDLB.Text);
                List<VendorStockItemsView> vendorStockItemListPOCOs = sysmgr.List_VendorStockItemsPOCO(vendorID, currentOrderListPOCOs);

                VendorStockItemsView stockItemToRemove = vendorStockItemListPOCOs.SingleOrDefault(x => x.SID == orderDetailSID);
                if (stockItemToRemove == null)
                {
                    throw new Exception();                
                }
                else
                {
                    CurrentActiveOrderView detailItemToAdd = new CurrentActiveOrderView();
                    detailItemToAdd.SID = stockItemToRemove.SID;
                    detailItemToAdd.Description = stockItemToRemove.ItemDescription;
                    detailItemToAdd.QOH = stockItemToRemove.QOH;
                    detailItemToAdd.QOO = stockItemToRemove.QOO;
                    detailItemToAdd.ROL = stockItemToRemove.ROL;
                    detailItemToAdd.QTO = 1;
                    detailItemToAdd.Price = stockItemToRemove.Price;
                    //add the item to top view
                    currentOrderListPOCOs.Add(detailItemToAdd);
                    //pass the top view into the controller for the bottom view, this will remove the stockItemToRemove, because SID exist in the top list.
                    TestLB.Text = "the add stockitem id is " + orderDetailSID.ToString() + sysmgr.AddDBPOPODetails(employeeID,vendorID, currentOrderListPOCOs);
                    vendorStockItemListPOCOs = sysmgr.List_VendorStockItemsPOCO(vendorID, currentOrderListPOCOs);
                    
                    CurrentActiveOrderGDView.DataSource = currentOrderListPOCOs;                    
                    CurrentActiveOrderGDView.DataBind();
                    VendorStockItemsGDView.DataSource = vendorStockItemListPOCOs;
                    VendorStockItemsGDView.DataBind();
                    //for development, change this part latter!
                    
                    OrderSummaryDisplay(vendorID);
                    VendorDDL.SelectedValue = int.Parse(VendorIDLB.Text).ToString();
                    MessageUserControl.ShowInfo("Item added", TestLB.Text);

                }//endofelse;               
            });            
           
        }//eom;

        protected List<CurrentActiveOrderView> GetCurrentActiveOrderPOCOList()
        {
            List<CurrentActiveOrderView> currentOrderListPOCO = new List<CurrentActiveOrderView>();
            List<string> reasons = new List<string>();            
            for (int i = 0; i < CurrentActiveOrderGDView.Rows.Count; i++)
            {
                CurrentActiveOrderView orderDetailPOCO = new CurrentActiveOrderView();
                int parseNum;
                decimal parseDecimal;
                bool result;

                orderDetailPOCO.SID = int.Parse((CurrentActiveOrderGDView.Rows[i].FindControl("SIDLB") as Label).Text);
                orderDetailPOCO.Description = (CurrentActiveOrderGDView.Rows[i].FindControl("DescriptionLB") as Label).Text;
                orderDetailPOCO.QOH = int.Parse((CurrentActiveOrderGDView.Rows[i].FindControl("QOHLB") as Label).Text);
                orderDetailPOCO.QOO = int.Parse((CurrentActiveOrderGDView.Rows[i].FindControl("QOOLB") as Label).Text);
                orderDetailPOCO.ROL = int.Parse((CurrentActiveOrderGDView.Rows[i].FindControl("ROLLB") as Label).Text);
                //orderDetailPOCO.QTO = int.Parse((CurrentActiveOrderGDView.Rows[i].FindControl("QTOTB") as TextBox).Text);
                //orderDetailPOCO.Price = decimal.Parse((CurrentActiveOrderGDView.Rows[i].FindControl("PriceTB") as TextBox).Text);
                //check for QTO input...
                result = int.TryParse(((CurrentActiveOrderGDView.Rows[i].FindControl("QTOTB") as TextBox).Text), out parseNum);
                if (result)
                {
                    if (parseNum > 0)
                    {
                        orderDetailPOCO.QTO = parseNum;
                    }
                    else
                    {
                        reasons.Add("First Error at " + orderDetailPOCO.SID.ToString() + "Quantity must be greater than zero");
                    }
                        
                }
                else
                {
                    reasons.Add("First Error at " + orderDetailPOCO.SID.ToString() + " Invalid Input quantity, quantity is a positive integer");
                }

                //check fro price input..
                result = decimal.TryParse(((CurrentActiveOrderGDView.Rows[i].FindControl("PriceTB") as TextBox).Text)
                    ,NumberStyles.Currency
                    ,CultureInfo.CurrentCulture
                    ,out parseDecimal);
                if (result)
                {
                    if (parseDecimal > 0)
                    {
                        orderDetailPOCO.Price = parseDecimal;
                    }
                    else
                    {
                        reasons.Add("First Error at " + orderDetailPOCO.SID.ToString() + " Price must be greater than zero");
                    }
                }
                else
                {
                    reasons.Add("First Error at " + orderDetailPOCO.SID.ToString() + " Invalid Input Price, Price is a positive decimal value");
                }




                if (reasons.Count>0)
                {
                    throw new BusinessRuleException("Invalid input", reasons);
                }

                currentOrderListPOCO.Add(orderDetailPOCO);
            }// end of for
           

                return currentOrderListPOCO;
        }//eom;

        protected List<VendorStockItemsView> GetVendorStockItemPOCOList()
        {
            
            List<VendorStockItemsView> vendorStockItemListPOCO = new List<VendorStockItemsView>();
            
            for (int i = 0; i < VendorStockItemsGDView.Rows.Count; i++)
            {
                VendorStockItemsView stockItemPOCO = new VendorStockItemsView();
                stockItemPOCO.SID = int.Parse((VendorStockItemsGDView.Rows[i].FindControl("SIDLB") as Label).Text);
                stockItemPOCO.ItemDescription = (CurrentActiveOrderGDView.Rows[i].FindControl("ItemDescriptionLB") as Label).Text;
                stockItemPOCO.QOH = int.Parse((CurrentActiveOrderGDView.Rows[i].FindControl("QOHLB") as Label).Text);
                stockItemPOCO.QOO = int.Parse((CurrentActiveOrderGDView.Rows[i].FindControl("QOOLB") as Label).Text);
                stockItemPOCO.ROL = int.Parse((CurrentActiveOrderGDView.Rows[i].FindControl("ROLLB") as Label).Text);
                stockItemPOCO.Buffer = int.Parse((CurrentActiveOrderGDView.Rows[i].FindControl("BufferLB") as Label).Text);
                stockItemPOCO.Price = decimal.Parse((CurrentActiveOrderGDView.Rows[i].FindControl("PriceLB") as Label).Text, NumberStyles.Currency);

                vendorStockItemListPOCO.Add(stockItemPOCO);
            }            

            return vendorStockItemListPOCO;
        }//eom;

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                PurchaseOrderDetailsController sysmgr = new PurchaseOrderDetailsController();
                List<CurrentActiveOrderView> currentOrderListPOCOs = GetCurrentActiveOrderPOCOList();
                if (currentOrderListPOCOs == null || String.IsNullOrEmpty(VendorIDLB.Text)|| VendorIDLB.Text == "0")
                {
                    throw new Exception("empty active order, nothing to update");
                }                
                int vendorID = int.Parse(VendorIDLB.Text);
                TestLB.Text = sysmgr.UpdateDBPOPODetails(vendorID, currentOrderListPOCOs);
                CurrentActiveOrderGDView.DataSource = currentOrderListPOCOs;
                CurrentActiveOrderGDView.DataBind();
                
                OrderSummaryDisplay(vendorID);
                VendorDDL.SelectedValue = int.Parse(VendorIDLB.Text).ToString();
                MessageUserControl.ShowInfo("Order updated", TestLB.Text);
            });
            
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {

            MessageUserControl.TryRun(() =>
            {
                
                PurchaseOrderDetailsController sysmgr = new PurchaseOrderDetailsController();
                int vendorID = int.Parse(VendorIDLB.Text);
                TestLB.Text = sysmgr.DeletePurchaseOrder(vendorID);

                List<CurrentActiveOrderView> currentOrderListPOCOs = new List<CurrentActiveOrderView>();
                List<VendorStockItemsView> vendorStockItemListPOCOs = sysmgr.List_VendorStockItemsPOCO(vendorID, currentOrderListPOCOs);

                CurrentActiveOrderGDView.DataSource = currentOrderListPOCOs;
                CurrentActiveOrderGDView.DataBind();
                VendorStockItemsGDView.DataSource = vendorStockItemListPOCOs;
                VendorStockItemsGDView.DataBind();
                OrderSummaryDisplay(vendorID);
                VendorDDL.SelectedValue = 0.ToString();
                //Response.Redirect(Request.RawUrl);
                MessageUserControl.ShowInfo("Order deleted", TestLB.Text);
            });
            
        }

        protected void OrderSummaryDisplay(int vendorID)
        {
            purchaseOrderController sysDebugger = new purchaseOrderController();
            List<CurrentOrderSummary> VendorOrderSummary = new List<CurrentOrderSummary>();
            VendorOrderSummary = sysDebugger.GetCurrentOrderSummary(vendorID);
            OrderSummaryGDView.DataSource = VendorOrderSummary;
            OrderSummaryGDView.DataBind();
        }

        protected void PlaceButton_Click(object sender, EventArgs e)
        {
            int vendorID = 0;
            MessageUserControl.TryRun(() =>
            {

                PurchaseOrderDetailsController sysmgr = new PurchaseOrderDetailsController();



                vendorID = int.Parse(VendorIDLB.Text);
                List<CurrentActiveOrderView> currentOrderListPOCOs = new List<CurrentActiveOrderView>();
                currentOrderListPOCOs = GetCurrentActiveOrderPOCOList();
                if (currentOrderListPOCOs == null)
                {
                    throw new Exception("Error, no current active order, nothing to place.");
                }

                TestLB.Text = sysmgr.Place_Order(vendorID, currentOrderListPOCOs).ToString();             
                currentOrderListPOCOs = new List<CurrentActiveOrderView>();
                List<VendorStockItemsView> vendorStockItemListPOCOs = sysmgr.List_VendorStockItemsPOCO(vendorID, currentOrderListPOCOs);

                CurrentActiveOrderGDView.DataSource = currentOrderListPOCOs;
                CurrentActiveOrderGDView.DataBind();
                VendorStockItemsGDView.DataSource = vendorStockItemListPOCOs;
                VendorStockItemsGDView.DataBind();
                OrderSummaryDisplay(vendorID);
                VendorDDL.SelectedValue = int.Parse(VendorIDLB.Text).ToString();

                MessageUserControl.ShowInfo("Order Placed", "Order number of " + TestLB.Text + " for Vendor number " + VendorIDLB.Text + " has been placed.");              

            });
            

        }//eom

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

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}