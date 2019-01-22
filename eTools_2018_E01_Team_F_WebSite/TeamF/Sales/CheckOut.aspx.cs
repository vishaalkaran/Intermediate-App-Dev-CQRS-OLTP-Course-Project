using AppSecurity.BLL;
using AppSecurity.DAL;
using AppSecurity.Entities;
using eTools.Data.Entities;
using eTools.Data.POCOs;
using eToolsSystem.BLL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eTools_2018_E01_Team_F_WebSite.TeamF.Sales
{
    public partial class CheckOut : System.Web.UI.Page
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
                    if (!User.IsInRole(SecurityRoles.Saleing))
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                }
            }

            MainView.ActiveViewIndex = 0;
            ApplicationUserManager userManager = new ApplicationUserManager(new
                   UserStore<ApplicationUser>(new ApplicationDbContext()));
            string userName = Context.User.Identity.GetUserName();
            int employeeid = 0;
            if (!string.IsNullOrEmpty(userName))
            {
                employeeid = userManager.Get_CurrentEmployeeIDFromUserName(userName);
                UserName1.Text = userName;
                UserID1.Text = employeeid.ToString();
            }
        }
        protected void AddToCart_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            int stockitemid = int.Parse(e.CommandArgument.ToString());
            ApplicationUserManager userManager = new ApplicationUserManager(new
                   UserStore<ApplicationUser>(new ApplicationDbContext()));
            string userName = Context.User.Identity.GetUserName();
            int employeeid = 0 ;
            if (!string.IsNullOrEmpty(userName))
                employeeid = userManager.Get_CurrentEmployeeIDFromUserName(userName);
            int qty = 0;
            int.TryParse((e.Item.FindControl("QuantitySelected") as TextBox).Text, out qty);
            if(qty < 1)
            {
                MessageUserControl.ShowInfo("Could not add item to shopping cart. Quantity must be greater than zero.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    ShoppingCartController sysmgr = new ShoppingCartController();
                    sysmgr.Add_ProductToShoppingCart(employeeid, stockitemid, qty);
                }, "Product Added", "The product has been added to your cart.");
            }
        }
        protected void UpdateCartItem_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            int shoppingCartItemID = int.Parse(e.CommandArgument.ToString());
            string itemDescription = (e.Item.FindControl("DescriptionLabel") as Label).Text;

            if ((e.Item.FindControl("DeleteCheckBox") as CheckBox).Checked)
            {
                MessageUserControl.TryRun(() =>
                {
                    ShoppingCartItemController sysmgr = new ShoppingCartItemController();
                    sysmgr.Delete_ShoppingCartItem(shoppingCartItemID);
                    CartListView.DataBind();
                }, "Item Deleted", itemDescription + " has been removed from your shopping cart.");
            }
            else
            {
                int qty = 0;
                bool quantityInputFail = false;
                string inputQty = (e.Item.FindControl("QuantitySelectedLabel") as TextBox).Text;
                if (!(int.TryParse(inputQty, out qty)))
                {
                    quantityInputFail = true;
                }
                if (quantityInputFail)
                {
                    MessageUserControl.ShowInfo("Could not update item quantity. Quantity must be a valid integer.");
                }
                else
                {
                    MessageUserControl.TryRun(() =>
                    {

                        ShoppingCartItemController sysmgr = new ShoppingCartItemController();
                        ShoppingCartItem item = sysmgr.Get_ShoppingCartItem(shoppingCartItemID);
                        item.Quantity = qty;
                        sysmgr.Update_ShoppingCartItem(item);
                        CartListView.DataBind();
                    }, "Item Quantity Updated", itemDescription + " has had its quantity updated.");
                }
            }
            MainView.ActiveViewIndex = 1;
        }

        protected void ViewCartButton_Click(object sender, EventArgs e)
        {
            MainView.ActiveViewIndex = 1;
            ApplicationUserManager userManager = new ApplicationUserManager(new
                   UserStore<ApplicationUser>(new ApplicationDbContext()));
            string userName = Context.User.Identity.GetUserName();
            int employeeid = 0;
            if (!string.IsNullOrEmpty(userName))
            {
                employeeid = userManager.Get_CurrentEmployeeIDFromUserName(userName);
                UserName2.Text = userName;
                UserID2.Text = employeeid.ToString();
            }
            CartListView.DataBind();
        }
        protected void CheckoutButton_Click(object sender, EventArgs e)
        {
            MainView.ActiveViewIndex = 2;
            ApplicationUserManager userManager = new ApplicationUserManager(new
                   UserStore<ApplicationUser>(new ApplicationDbContext()));
            string userName = Context.User.Identity.GetUserName();
            int employeeid = 0;
            if (!string.IsNullOrEmpty(userName))
            {
                employeeid = userManager.Get_CurrentEmployeeIDFromUserName(userName);
                UserName3.Text = userName;
                UserID3.Text = employeeid.ToString();
            }
            CheckoutGridView.DataBind();

            ShoppingCartController sysmgr = new ShoppingCartController();
            List<CartSelection> cartItems = sysmgr.Get_CartItemsByEmployeeID(employeeid);

            decimal subTotal = cartItems.Sum(x => x.QuantitySelected * x.SellingPrice);
            SubTotalLabel.Text = subTotal.ToString();

            decimal tax = subTotal / 20;
            TaxLabel.Text = tax.ToString();

            decimal discount = Label3.Visible ? decimal.Parse(DiscountLabel.Text) : 0;

            decimal total = subTotal + tax - discount;

            TotalLabel.Text = total.ToString();
        }

        protected void OrderButton_Click(object sender, EventArgs e)
        {
            ApplicationUserManager userManager = new ApplicationUserManager(new
                   UserStore<ApplicationUser>(new ApplicationDbContext()));
            string userName = Context.User.Identity.GetUserName();
            int employeeid = 0;
            if (!string.IsNullOrEmpty(userName))
                employeeid = userManager.Get_CurrentEmployeeIDFromUserName(userName);
            MessageUserControl.TryRun(() =>
            {
                Sale sale = new Sale();
                sale.SaleDate = DateTime.Now;
                sale.PaymentType = PaymentDDL.SelectedValue;
                sale.EmployeeID = employeeid;
                sale.TaxAmount = decimal.Parse(TaxLabel.Text);
                sale.SubTotal = decimal.Parse(SubTotalLabel.Text);
                if(!string.IsNullOrEmpty(CouponIDLabel.Text))
                    sale.CouponID = int.Parse(CouponIDLabel.Text);

                ShoppingCartController sysmgr = new ShoppingCartController();
                ShoppingCart cart = sysmgr.Get_ShoppingCartByEmployeeID(employeeid);
                SalesDetailController sysmg2 = new SalesDetailController();
                sysmg2.PlaceOrder(sale, cart);
            }, "Place Order", "Order successfully placed.");
            ProductSelectionListView.DataBind();
            CartListView.DataBind();
            CheckoutGridView.DataBind();
            SubTotalLabel.Text = "";
            TaxLabel.Text = "";
            Label3.Text = "";
            DiscountLabel.Text = "";
            TotalLabel.Text = "";
            MainView.ActiveViewIndex = 2;
        }

        protected void CouponButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CouponTextBox.Text))
            {
                MessageUserControl.ShowInfo("Please enter a valid coupon.");
            }
            else
            {
                CouponController sysmgr = new CouponController();
                Coupon coupon = sysmgr.ValidateCoupon(CouponTextBox.Text);
                if (coupon == null)
                {
                    MessageUserControl.ShowInfo("Please enter a valid coupon.");
                }
                else
                {
                    MessageUserControl.TryRun(() =>
                    {
                        decimal couponDiscount = decimal.Parse(coupon.CouponDiscount.ToString());
                        decimal discount = couponDiscount / 100;
                        CouponIDLabel.Text = coupon.CouponID.ToString();
                        Label3.Visible = true;
                        decimal discountValue = decimal.Parse(SubTotalLabel.Text) * discount;
                        DiscountLabel.Text = discountValue.ToString();
                        decimal total = decimal.Parse(TotalLabel.Text);
                        total -= discountValue;
                        TotalLabel.Text = total.ToString();
                    }, "Coupon", "Coupon successfully applied.");
                    MainView.ActiveViewIndex = 2;
                }
            }
        }

        protected void ViewShoppingButton_Click(object sender, EventArgs e)
        {
            MainView.ActiveViewIndex = 0;
            ApplicationUserManager userManager = new ApplicationUserManager(new
                   UserStore<ApplicationUser>(new ApplicationDbContext()));
            string userName = Context.User.Identity.GetUserName();
            int employeeid = 0;
            if (!string.IsNullOrEmpty(userName))
            {
                employeeid = userManager.Get_CurrentEmployeeIDFromUserName(userName);
                UserName1.Text = userName;
                UserID1.Text = employeeid.ToString();
            }
        }
    }
}