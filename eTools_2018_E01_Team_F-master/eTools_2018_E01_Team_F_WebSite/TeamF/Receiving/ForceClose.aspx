<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForceClose.aspx.cs" Inherits="eTools_2018_E01_Team_F_WebSite.TeamF.Receiving.ForceClose" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Receiving</h1>
    <%@ Register Src="~/UserControls/MessageUserControl.ascx" 
    TagPrefix="uc1" TagName="MessageUserControl" %>
    <div class="row">
       <div class="col-sm-12">
           <uc1:MessageUserControl runat="server" id="MessageUserControl" />
       </div>
   </div>
    <asp:Label ID="Label1" runat="server" Text="UserName: " Font-Bold="true" ></asp:Label><asp:Label ID="UserNameLabel" runat="server" ></asp:Label><br />
    <asp:Label ID="Label3" runat="server" Text="UserID: " Font-Bold="true" ></asp:Label><asp:Label ID="UserIDLabel" runat="server" ></asp:Label><br />
    <asp:Label ID="Label4" runat="server" Text="EmployeeName: " Font-Bold="true" ></asp:Label>
    <asp:Label ID="EmployeeNameLB" runat="server" Text =""></asp:Label> &nbsp;&nbsp;&nbsp;

    <h3>Purchase Orders</h3>
    <asp:ListView ID="OpenPOListView" runat="server" DataSourceID="OpenPOListViewODS" OnItemCommand="PurchaseOrderSelectionList_ItemCommand" >
        <AlternatingItemTemplate>
            <tr style="background-color: #FFFFFF; color: #284775;">
                <td>
                    <asp:LinkButton ID="SelectPurchaseOrder" runat="server"
                        CssClass="btn" CommandArgument='<%# Eval("PurchaseOrderID") %>' Text="View Order">
                    </asp:LinkButton>
                <td>
                    <asp:Label Text='<%# Eval("PurchaseOrderNumber") %>' runat="server" ID="PurchaseOrderNumberLabel" /></td>
                <td>
                    <!--How to get short date?-->
                    <asp:Label Text='<%# Eval("OrderDate") %>' runat="server" ID="OrderDateLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("VendorName") %>' runat="server" ID="VendorNameLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("VendorPhone") %>' runat="server" ID="VendorPhoneLabel" /></td>
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
                    <asp:LinkButton ID="SelectPurchaseOrder" runat="server"
                        CssClass="btn" CommandArgument='<%# Eval("PurchaseOrderID") %>' Text="View Order">
                    </asp:LinkButton>
                <td>
                    <asp:Label Text='<%# Eval("PurchaseOrderNumber") %>' runat="server" ID="PurchaseOrderNumberLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("OrderDate") %>' runat="server" ID="OrderDateLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("VendorName") %>' runat="server" ID="VendorNameLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("VendorPhone") %>' runat="server" ID="VendorPhoneLabel" /></td>
            </tr>
        </ItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                            <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                <th runat="server">PurchaseOrderID</th>
                                <th runat="server">PurchaseOrderNumber</th>
                                <th runat="server">OrderDate</th>
                                <th runat="server">VendorName</th>
                                <th runat="server">VendorPhone</th>
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
    <asp:ObjectDataSource ID="OpenPOListViewODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="List_OpenPurchaseOrders" TypeName="eToolsSystem.BLL.purchaseOrderController"></asp:ObjectDataSource>
    
    <asp:Label ID="PurchaseOrderIDLabel" runat="server" Visible="false" ></asp:Label><br />
    <h3>Purchase Order Details</h3>
    <asp:Label ID="PurchaseOrderNumberLabel" runat="server" ></asp:Label>&nbsp;
    <asp:Label ID="DateLabel" runat="server" ></asp:Label>&nbsp;
    <asp:Label ID="VendorLabel" runat="server" ></asp:Label>&nbsp;
    <asp:Label ID="VendorPhoneLabel" runat="server" ></asp:Label>

    <asp:GridView ID="OpenPODetailsGridView" runat="server" AutoGenerateColumns="False" DataSourceID="OpenPODetailsGridViewODS" EmptyDataText="No data was returned.">
        <Columns>
            <asp:BoundField DataField="PurchaseOrderDetailID" HeaderText="PurchaseOrderDetailID" SortExpression="PurchaseOrderDetailID" Visible="false"></asp:BoundField>
            <asp:BoundField DataField="VendorStockNumber" HeaderText="VendorStockNumber" SortExpression="VendorStockNumber" Visible="false"></asp:BoundField>
            <asp:BoundField DataField="StockItemID" HeaderText="StockItemID" SortExpression="StockItemID"></asp:BoundField>
            <asp:BoundField DataField="StockItemDescription" HeaderText="StockItemDescription" SortExpression="StockItemDescription"></asp:BoundField>
            <asp:BoundField DataField="QuantityOnOrder" HeaderText="QuantityOnOrder" SortExpression="QuantityOnOrder"></asp:BoundField>
            <asp:BoundField DataField="QuantityOutstanding" HeaderText="QuantityOutstanding" SortExpression="QuantityOutstanding"></asp:BoundField>
            <asp:TemplateField HeaderText="ReceivedQuantity" SortExpression="ReceivedQuantity">
                <ItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("ReceivedQuantity") %>' ID="ReceivedQuantity"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ReturnedQuantity" SortExpression="ReturnedQuantity">
                <ItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("ReturnedQuantity") %>' ID="ReturnedQuantity"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ReturnReason" SortExpression="ReturnReason">
                <ItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("ReturnReason") %>' ID="ReturnReason"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="OpenPODetailsGridViewODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="List_OpenPurchaseOrderDetails" TypeName="eToolsSystem.BLL.PurchaseOrderDetailsController">
        <SelectParameters>
            <asp:ControlParameter ControlID="PurchaseOrderIDLabel" PropertyName="Text" Name="PurchaseOrderID" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:Button ID="ReceiveButton" runat="server" Text="Receive" OnClick="ReceiveButton_Click" Visible="false" />&nbsp;<asp:Button ID="ForceCloseButton" runat="server" Text="Force Close" OnClick="ForceCloseButton_Click" Visible="false" />&nbsp;
    <asp:Label ID="ReasonLabel" runat="server" Text="Reason: " Visible="false" ></asp:Label>&nbsp;<asp:TextBox ID="ReasonTextBox" runat="server" Visible="false"></asp:TextBox>
    <br />
    <h3>Unordered Returns</h3>
    <asp:ListView ID="UnorderedPurchaseItemCartListView" runat="server" DataSourceID="UnorderedPurchaseItemCartListViewODS" InsertItemPosition="LastItem" DataKeyNames="CartID" >
        <EditItemTemplate>
            <tr style="">
                <td>
                    <asp:Button runat="server" CommandName="Update" Text="Update" ID="UpdateButton" />
                    <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelButton" />
                </td>
                <td>
                    <asp:Label Text='<%# Bind("CartID") %>' runat="server" ID="CartIDTextBox" /></td>
                <td>
                    <asp:TextBox Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" /></td>
                <td>
                    <asp:TextBox Text='<%# Bind("VendorStockNumber") %>' runat="server" ID="VendorStockNumberTextBox" /></td>
                <td>
                    <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="QuantityTextBox" /></td>
            </tr>
        </EditItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" style="">
                <tr>
                    <td>No data was returned.</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr style="">
                <td>
                    <asp:Button runat="server" CommandName="Insert" Text="Insert" ID="InsertButton" />
                    <asp:Button runat="server" CommandName="Cancel" Text="Clear" ID="CancelButton" />
                </td>
                <td>
                    <asp:Label Text='<%# Bind("CartID") %>' runat="server" ID="CartIDTextBox" /></td>
                <td>
                    <asp:TextBox Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" /></td>
                <td>
                    <asp:TextBox Text='<%# Bind("VendorStockNumber") %>' runat="server" ID="VendorStockNumberTextBox" /></td>
                <td>
                    <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="QuantityTextBox" /></td>
            </tr>
        </InsertItemTemplate>
        <ItemTemplate>
            <tr style="">
                <td>
                    <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" />
                </td>
                <td>
                    <asp:Label Text='<%# Eval("CartID") %>' runat="server" ID="CartIDLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("VendorStockNumber") %>' runat="server" ID="VendorStockNumberLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" /></td>
            </tr>
        </ItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table runat="server" id="itemPlaceholderContainer" style="" border="0">
                            <tr runat="server" style="">
                                <th runat="server"></th>
                                <th runat="server">CartID</th>
                                <th runat="server">Description</th>
                                <th runat="server">VendorStockNumber</th>
                                <th runat="server">Quantity</th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder"></tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" style=""></td>
                </tr>
            </table>
        </LayoutTemplate>
        <SelectedItemTemplate>
            <tr style="">
                <td>
                    <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" />
                </td>
                <td>
                    <asp:Label Text='<%# Eval("CartID") %>' runat="server" ID="CartIDLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("VendorStockNumber") %>' runat="server" ID="VendorStockNumberLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" /></td>
            </tr>
        </SelectedItemTemplate>
    </asp:ListView>
    <asp:ObjectDataSource ID="UnorderedPurchaseItemCartListViewODS" runat="server" DataObjectTypeName="eTools.Data.Entities.UnorderedPurchaseItemCart" DeleteMethod="Delete_UnorderedPurchaseItemCart" InsertMethod="Add_UnorderedItemToCart" OldValuesParameterFormatString="original_{0}" SelectMethod="Get_ListUnorderedPurchaseItemCart" TypeName="eToolsSystem.BLL.UnorderedPurchaseItemCartController"></asp:ObjectDataSource>
</asp:Content>
