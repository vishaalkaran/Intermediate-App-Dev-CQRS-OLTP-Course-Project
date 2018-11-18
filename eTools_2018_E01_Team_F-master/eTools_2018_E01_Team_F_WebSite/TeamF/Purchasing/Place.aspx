<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Place.aspx.cs" Inherits="eTools_2018_E01_Team_F_WebSite.TeamF.Purchasing.Place" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h1>Purchasing</h1>
    </div>    

    <div class="row">
        <div class="col-sm-12">
            <uc1:MessageUserControl runat="server" id="MessageUserControl" />
        </div>
    </div>
    
    <div>
        <asp:Label ID="UserNameLB" runat="server" Text ="User Name">
        </asp:Label> &nbsp;&nbsp;&nbsp;

        <asp:Label ID="UserDisplayNameLB" runat="server" Text ="">
        </asp:Label> &nbsp;&nbsp;&nbsp;

        <asp:Label ID="EmployeeIDLB" runat="server" Text ="">
        </asp:Label> &nbsp;&nbsp;&nbsp;

        <asp:Label ID="EmployeeNameLB" runat="server" Text ="">
        </asp:Label> &nbsp;&nbsp;&nbsp;
    </div>

    <div class="row">
        <div class="col-sm-6">
            <asp:Button ID="UpdateButton" runat="server" Text="Update" OnClick="UpdateButton_Click" />
            <asp:Button ID="PlaceButton" runat="server" Text="Place" OnClick="PlaceButton_Click"  />
            <asp:Button ID="DeleteButton" runat="server" Text="Delete" OnClick="DeleteButton_Click" />
            <asp:Button ID="ResetButton" runat="server" Text="ResetButton" OnClick="ResetButton_Click" />
            <%--<asp:LinkButton runat="server" id="ResetLinkButton" href="Place" CssClass="btn btn-primary btn-sm">Rest</asp:LinkButton> <br /><br />--%>

            
        </div>
    </div>



    <div class ="row">
        <asp:GridView ID="OrderSummaryGDView" runat="server" AutoGenerateColumns="False"
             Caption="Orders Summaries" GridLines="Horizontal" BorderStyle="None" >
            <Columns>                
                <asp:TemplateField HeaderText="OrderID">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="PurchaseOrderIDLB" Width="60px"
                            Text='<%# Eval("PurchaseOrderID") %>'></asp:Label>                       
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OrderNumber">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="PurchaseOrderNumberLB" Width="100px"
                            Text='<%# Eval("PurchaseOrderNumber") %>'></asp:Label>                       
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OrderDate">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="OrderDateLB" Width="150px"
                            Text='<%# Eval("OrderDate") %>'></asp:Label>                       
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="VendorName">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="VendorNameLB" Width="150px"
                            Text='<%# Eval("VendorName") %>'></asp:Label>                       
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="EmployeeID">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="EmployeeIDLB" Width="80px"
                            Text='<%# Eval("EmployeeID") %>'></asp:Label>                       
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="EmployeeName">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="EmployeeNameLB" Width="110px"
                            Text='<%# Eval("EmployeeName") %>'></asp:Label>                       
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subtotal">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="SubtotalLB" Width="100px"
                            Text='<%# Eval("Subtotal","{0:C2}") %>'></asp:Label>                       
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tax">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="TaxLB" Width="60px"
                            Text='<%# Eval("Tax","{0:C2}") %>'></asp:Label>                       
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OrderDetailCount">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="OrderDetailCountLB" Width="60px"
                            Text='<%# Eval("OrderDetailCount") %>'></asp:Label>                       
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>                                
            </Columns>
            <EmptyDataTemplate>
            No data to view for the playlist.
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

    <div class="row">
        <div class="col-sm-2">
            <asp:Label ID="Label1" runat="server" Text="Vendors" ></asp:Label><br />
            <asp:DropDownList ID="VendorDDL" runat="server"
                Width="150px" DataSourceID="VendorListODS" DataTextField="DisplayText" DataValueField="IDValueField" AppendDataBoundItems="true">
                <asp:ListItem Text="Select_A_Vendor" Value="0" Selected="True"></asp:ListItem>
            </asp:DropDownList><br />
            <asp:Button ID="FetchButton" runat="server" Text="Fetch" OnClick="FetchButton_Click" />
        </div>       
    </div>

    <div class="row">
        <asp:Label ID="TestLB" runat="server" Text="" Visible="False"></asp:Label>
        <asp:Label ID="VendorIDLB" runat="server" Visible="False" Text=""></asp:Label>
        <asp:GridView ID="CurrentActiveOrderGDView" runat="server" AutoGenerateColumns="False"
             Caption="Current Active Orders" GridLines="Horizontal" BorderStyle="None" Font-Bold="True" Font-Size="Medium" Font-Underline="False" ForeColor="#000066">
            <Columns>                
                <asp:TemplateField HeaderText="SID">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="SIDLB" Width="40px"
                            Text='<%# Eval("SID") %>'></asp:Label>                       
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="DescriptionLB" Width="300px"
                            Text='<%# Eval("Description") %>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="QOH">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="QOHLB" Width="60px"
                            Text='<%# Eval("QOH") %>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="QOO">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="QOOLB" Width="60px"
                            Text='<%# Eval("QOO") %>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ROL">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="ROLLB" Width="60px"
                            Text='<%# Eval("ROL") %>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="QTO">
                    <ItemTemplate>                        
                        <asp:TextBox runat="server" ID="QTOTB" Width="100px"
                            Text='<%# Bind("QTO") %>' ></asp:TextBox>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Price">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="PriceTB" Width="120px"
                            Text='<%# Bind("Price","{0:C2}") %>'></asp:TextBox>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>  
               <asp:TemplateField HeaderText="Button">
                    <ItemTemplate>
                        <%--<asp:Label runat="server" ID="SIDLB" Width="40px"
                            Text='<%# Eval("SID") %>'></asp:Label>--%>
                        <asp:LinkButton ID="RemoveButton" runat="server"
                            CssClass="btn" CommandArgument='<%# Eval("SID") %>' OnCommand="CurrentActiveOrderGridView_RowCommand_RemoveItem">RemoveButton
                        </asp:LinkButton>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                                  
            </Columns>
            <EmptyDataTemplate>
            No data to view for the playlist.
            </EmptyDataTemplate>
        </asp:GridView>

    </div>
    <div class="row">
        <asp:GridView ID="VendorStockItemsGDView" runat="server" AutoGenerateColumns="False"
             Caption="Vendor StockItems" GridLines="Horizontal" BorderStyle="None" >
            <Columns>                
                <asp:TemplateField HeaderText="SID">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="SIDLB" Width="40px"
                            Text='<%# Eval("SID") %>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ItemDescription">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="ItemDescriptionLB"  Width="300px"
                            Text='<%# Eval("ItemDescription") %>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="QOH">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="QOHLB"  Width="60px"
                            Text='<%# Eval("QOH") %>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="QOO">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="QOOLB" Width="60px"
                            Text='<%# Eval("QOO") %>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ROL">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="ROLLB" Width="60px"
                            Text='<%# Eval("ROL") %>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Buffer">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="BufferLB" Width="100px"
                            Text='<%# Eval("Buffer") %>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Price">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="PriceLB" Width="120px"
                            Text='<%# Eval("Price","{0:C2}") %>' DataFormatString="{0:c}"></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Button">
                    <ItemTemplate>                        
                        <asp:LinkButton ID="AddButton" runat="server"
                            CssClass="btn" CommandArgument='<%# Eval("SID") %>' 
                            OnCommand="CurrentActiveOrderGridView_RowCommand_AddItem">AddButton
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>                
            </Columns>
            <EmptyDataTemplate>
            No data to view for the playlist.
            </EmptyDataTemplate>

        </asp:GridView>
    </div>


    
    <asp:ObjectDataSource ID="VendorListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="List_VendorNames" TypeName="eToolsSystem.BLL.VendorController"></asp:ObjectDataSource>


</asp:Content>
