<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckOut.aspx.cs" Inherits="eTools_2018_E01_Team_F_WebSite.TeamF.Sales.CheckOut" %>
<%@ Register Src="~/UserControls/MessageUserControl.ascx" 
    TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Sales</h1>
    <div class="row">
       <div class="col-md-12">
           <uc1:MessageUserControl runat="server" id="MessageUserControl" />
       </div>
    </div>
    <asp:MultiView ID="MainView" runat="server" ActiveViewIndex="0">
        <asp:View ID="Shopping" runat="server">
            <!--Shopping-->
            <asp:Label ID="Label5" runat="server" Text="UserName: " Font-Bold="true" /><asp:Label ID="UserName1" runat="server"/><br />
            <asp:Label ID="Label8" runat="server" Text="ID: " Font-Bold="true" /><asp:Label ID="UserID1" runat="server"/>
            <h3>Shopping</h3>
            <div class="row">
                <div class="col-md-2">
                    <!--category list-->
                    <asp:DropDownList ID="CategoryDDL" runat="server" DataSourceID="CategoryDDLODS" DataTextField="Description" DataValueField="CategoryID" AutoPostBack="true" ></asp:DropDownList>
                    <asp:ObjectDataSource ID="CategoryDDLODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Get_CategoryList" TypeName="eToolsSystem.BLL.CategoryController"></asp:ObjectDataSource>
                </div>
                <div class="col-md-10">
                    <asp:ListView ID="ProductSelectionListView" runat="server" OnItemCommand="AddToCart_ItemCommand" DataSourceID="ProductSelectionListViewODS">
                        <AlternatingItemTemplate>
                            <tr style="background-color: #FFFFFF; color: #284775;">
                                <td>
                                    <asp:LinkButton ID="AddToCart" runat="server"
                                         CssClass="btn" CommandArgument='<%# Eval("StockItemID") %>'>
                                        <span aria-hidden="true" class="glyphicon glyphicon-plus">&nbsp;</span>
                                    </asp:LinkButton></td>
                                <td>
                                    <asp:TextBox Text='<%# Eval("QuantitySelected") %>' runat="server" ID="QuantitySelected" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("SellingPrice") %>' runat="server" ID="SellingPriceLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("QuantityOnHand") %>' runat="server" ID="QuantityOnHandLabel" /></td>
                            </tr>
                        </AlternatingItemTemplate>
                        <EmptyDataTemplate>
                            <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                                <tr>
                                    <td>No data was returned.</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <tr style="background-color: #E0FFFF; color: #333333;">
                                <td>
                                    <asp:LinkButton ID="AddToCart" runat="server"
                                         CssClass="btn" CommandArgument='<%# Eval("StockItemID") %>'>
                                        <span aria-hidden="true" class="glyphicon glyphicon-plus">&nbsp;</span>
                                    </asp:LinkButton></td>
                                <td>
                                    <asp:TextBox Text='<%# Eval("QuantitySelected") %>' runat="server" ID="QuantitySelected" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("SellingPrice") %>' runat="server" ID="SellingPriceLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("QuantityOnHand") %>' runat="server" ID="QuantityOnHandLabel" /></td>
                            </tr>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <table runat="server">
                                <tr runat="server">
                                    <td runat="server">
                                        <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                            <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                                <th runat="server">AddToCart</th>
                                                <th runat="server">QuantitySelected</th>
                                                <th runat="server">SellingPrice</th>
                                                <th runat="server">Description</th>
                                                <th runat="server">QuantityOnHand</th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder"></tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server">
                                    <td runat="server" style="text-align: center; background-color: #5D7B9D; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF"></td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                    </asp:ListView>
                    <asp:ObjectDataSource ID="ProductSelectionListViewODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Get_StockItemsByCategory" TypeName="eToolsSystem.BLL.StockItemController">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="CategoryDDL" PropertyName="SelectedValue" Name="categoryid" Type="Int32"></asp:ControlParameter>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <div style="text-align:right">
                        <br/><asp:LinkButton ID="ViewCartButton" runat="server" OnClick="ViewCartButton_Click" >View Cart</asp:LinkButton>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View ID="ViewCart" runat="server">
            <!--Cart-->
            <asp:Label ID="Label6" runat="server" Text="UserName: " Font-Bold="true" /><asp:Label ID="UserName2" runat="server"/><br />
            <asp:Label ID="Label10" runat="server" Text="ID: " Font-Bold="true" /><asp:Label ID="UserID2" runat="server"/>
            <h3>Cart</h3>
            <div class="row">
                <div class="col-md-12">
                    <!--cart listview, edit quantities and automatically update item extended cost need command button and checkbox to delete row -- in event method can check for checkbox and delete or update total, need empty data template-->
                    <asp:ListView ID="CartListView" runat="server" OnItemCommand="UpdateCartItem_ItemCommand" DataSourceID="CartListViewODS">
                        <AlternatingItemTemplate>
                            <tr style="background-color: #FFFFFF; color: #284775;">
                        
                                <td>
                                    <asp:Label Text='<%# Eval("SellingPrice") %>' runat="server" ID="SellingPriceLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Total") %>' runat="server" ID="TotalLabel" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Eval("QuantitySelected") %>' runat="server" ID="QuantitySelectedLabel" /></td>
                                <td>
                                    <asp:CheckBox Text="Delete?" runat="server" ID="DeleteCheckBox" /></td>
                                <td>
                                    <asp:LinkButton ID="UpdateCartItem" runat="server"
                                         CssClass="btn" CommandArgument='<%# Eval("ShoppingCartitemID") %>'>
                                        <span aria-hidden="true" class="glyphicon">Update</span>
                                    </asp:LinkButton></td>
                            </tr>
                        </AlternatingItemTemplate>
                        <EmptyDataTemplate>
                            <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                                <tr>
                                    <td>No data was returned.</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <tr style="background-color: #E0FFFF; color: #333333;">
                        
                                <td>
                                    <asp:Label Text='<%# Eval("SellingPrice") %>' runat="server" ID="SellingPriceLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Total") %>' runat="server" ID="TotalLabel" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Eval("QuantitySelected") %>' runat="server" ID="QuantitySelectedLabel" /></td>
                                <td>
                                    <asp:CheckBox Text="Delete?" runat="server" ID="DeleteCheckBox" /></td>
                                <td>
                                    <asp:LinkButton ID="UpdateCartItem" runat="server"
                                         CssClass="btn" CommandArgument='<%# Eval("ShoppingCartitemID") %>'>
                                        <span aria-hidden="true" class="glyphicon">Update</span>
                                    </asp:LinkButton></td>
                            </tr>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <table runat="server">
                                <tr runat="server">
                                    <td runat="server">
                                        <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                            <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                                <th runat="server">SellingPrice</th>
                                                <th runat="server">Description</th>
                                                <th runat="server">Total</th>
                                                <th runat="server">QuantitySelected</th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder"></tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server">
                                    <td runat="server" style="text-align: center; background-color: #5D7B9D; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF"></td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                    </asp:ListView>
                    <asp:ObjectDataSource ID="CartListViewODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Get_CartItemsByEmployeeID" TypeName="eToolsSystem.BLL.ShoppingCartController">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="UserID2" PropertyName="Text" DefaultValue="" Name="employeeid" Type="Int32"></asp:ControlParameter>

                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <br />
                    <div style="text-align:left">
                        <asp:LinkButton ID="ViewShoppingButton" runat="server" OnClick="ViewShoppingButton_Click" align="left">Shopping</asp:LinkButton>
                    </div>
                    <div style="text-align:right">
                        <asp:LinkButton ID="CheckoutButton" runat="server" OnClick="CheckoutButton_Click" align="right">Checkout</asp:LinkButton>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View ID="Checkout" runat="server">
            <!--Checkout-->
            <asp:Label ID="Label7" runat="server" Text="UserName: " Font-Bold="true" /><asp:Label ID="UserName3" runat="server"/><br />
            <asp:Label ID="Label13" runat="server" Text="ID: " Font-Bold="true" /><asp:Label ID="UserID3" runat="server"/>
            <h3>Checkout</h3>
            <div class="row">
                <div class="col-md-12">
                    <asp:GridView ID="CheckoutGridView" runat="server" AutoGenerateColumns="False" DataSourceID="CheckoutGridViewODS" EmptyDataText="Nothing in the shopping cart.">
                        <Columns>
                            <asp:BoundField DataField="ShoppingCartitemID" HeaderText="ShoppingCartitemID" SortExpression="ShoppingCartitemID"></asp:BoundField>
                            <asp:BoundField DataField="SellingPrice" HeaderText="SellingPrice" SortExpression="SellingPrice"></asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"></asp:BoundField>
                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total"></asp:BoundField>
                            <asp:BoundField DataField="QuantitySelected" HeaderText="QuantitySelected" SortExpression="QuantitySelected"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="CheckoutGridViewODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Get_CartItemsByEmployeeID" TypeName="eToolsSystem.BLL.ShoppingCartController">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="UserID3" PropertyName="Text" DefaultValue="" Name="employeeid" Type="Int32"></asp:ControlParameter>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <br />
                    <asp:Label ID="CouponLabel" runat="server" Text="Coupon"></asp:Label>
                    <asp:TextBox ID="CouponTextBox" runat="server"></asp:TextBox>
                    <asp:LinkButton ID="CouponButton" runat="server" OnClick="CouponButton_Click" Text="Apply Coupon"></asp:LinkButton><br />
                    <br />
                    <asp:DropDownList ID="PaymentDDL" runat="server">
                        <asp:ListItem Value="M">Money</asp:ListItem>
                        <asp:ListItem Value="C">Credit</asp:ListItem>
                        <asp:ListItem Value="D">Debit</asp:ListItem>
                    </asp:DropDownList>
                    <br /><br />
                    <div style="text-align:left">
                        <asp:LinkButton ID="ViewCartButton2" runat="server" OnClick="ViewCartButton_Click" align="left">View Cart</asp:LinkButton>
                    </div>
                </div>
                <div class="col-md-6">
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="SubTotal:"></asp:Label>&nbsp;<asp:Label ID="SubTotalLabel" runat="server" ></asp:Label><br />
                    <asp:Label ID="Label2" runat="server" Text="Tax:"></asp:Label>&nbsp;<asp:Label ID="TaxLabel" runat="server" ></asp:Label><br />
                    <asp:Label ID="CouponIDLabel" runat="server" Visible="false"/><asp:Label ID="Label3" runat="server" Text="Discount:" Visible="false"></asp:Label>&nbsp;<asp:Label ID="DiscountLabel" runat="server" ></asp:Label><br />
                    <asp:Label ID="Label4" runat="server" Text="Total:"></asp:Label>&nbsp;<asp:Label ID="TotalLabel" runat="server" ></asp:Label><br /><br />
                    <asp:Button ID="OrderButton" runat="server" Text="Place Order" OnClick="OrderButton_Click" />
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
        
</asp:Content>
