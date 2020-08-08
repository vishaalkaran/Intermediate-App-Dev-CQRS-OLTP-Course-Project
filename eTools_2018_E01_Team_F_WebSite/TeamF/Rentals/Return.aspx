<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Return.aspx.cs" Inherits="eTools_2018_E01_Team_F_WebSite.TeamF.Rentals.Return" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-header">
        <h1>Return</h1>
    </div>

    <div>
        <asp:Label ID="UserNameLB" runat="server" Text="User Name">
        </asp:Label>
        &nbsp;&nbsp;&nbsp;

        <asp:Label ID="UserDisplayNameLB" runat="server" Text="">
        </asp:Label>
        &nbsp;&nbsp;&nbsp;

        <asp:Label ID="EmployeeIDLB" runat="server" Text="">
        </asp:Label>
        &nbsp;&nbsp;&nbsp;

        <asp:Label ID="EmployeeNameLB" runat="server" Text="">
        </asp:Label>
        &nbsp;&nbsp;&nbsp;
    </div>

    <%--Just dragged MessageUserControl.ascx on to this web form--%>
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />

    <div class="container">
        <%-- OrderID--%>
        <asp:Label runat="server" ID="Label6" Text="RentalID:" />&nbsp;&nbsp;
        <asp:Label ID="selectedCustomerRental" runat="server" Style="text-align:  left;"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="newReturn" runat="server" Visible="false" OnClick="newReturn_Click">New Return</asp:LinkButton>

        <%--This is hidden, internal use--%>
        <asp:Label ID="HIDDEN_LABEL_selectedCustomerID" runat="server" Visible="false"></asp:Label>
        <br />
        <br />

        <%--Phone number/ customer slection row--%>
        <div class="col-lg-2">
            <div class="row">
                <asp:Label ID="Label3" runat="server" Text="Phone Number:" Style="text-align:  left;"></asp:Label>
            </div>
            <div class="row">
                <br />
                <asp:Label ID="Label7" runat="server" Text="Profile:" Style="text-align:  left;"></asp:Label>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="row">
                <asp:TextBox ID="PhoneNumberInput" runat="server"></asp:TextBox>
                <asp:LinkButton ID="phoneNumberSubmitBtn" runat="server">Search</asp:LinkButton>
            </div>
            <div class="row">
                <br />
                <asp:Label ID="selectedCustomerName" runat="server" Style="text-align:  left;"></asp:Label><br />
                <asp:Label ID="selectedCustomerAddress" runat="server" Style="text-align:  left;"></asp:Label><br />
                <asp:Label ID="selectedCustomerCity" runat="server" Style="text-align:  left;"></asp:Label><br />
                <br />
            </div>
        </div>

        <div class="container">
            <div class="row">
                <div class="col-lg-6">
                    <asp:ListView ID="ListView1" runat="server" DataSourceID="CustomerReturnDataSource" OnItemCommand="selectCustomer_Click" OnSelectedIndexChanged="customerLookup_SelectedIndexChanged">
                        <EmptyDataTemplate>
                            <table runat="server" style="">
                                <tr>
                                    <td>No data was returned.</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:LinkButton ID="selectedCustomerBtn" runat="server" CssClass="btn" CommandArgument='<%# Eval("rentalid") %>'>
                                        <span aria-hidden="true" class="glyphicon glyphicon-plus">&nbsp;</span>
                                    </asp:LinkButton>
                                </td>

                                <td><asp:Label Text='<%# Eval("rentalid") %>' runat="server" ID="rentalidLabel" /></td>
                                <td><asp:Label Text='<%# Eval("fullname") %>' runat="server" ID="fullnameLabel" /></td>
                                <td><asp:Label Text='<%# Eval("address") %>' runat="server" ID="addressLabel" />&nbsp;&nbsp;</td>
                                <td><asp:Label Text='<%# Eval("mmddyy") %>' runat="server" ID="mmddyyLabel" /></td>
                            </tr>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <table runat="server" width="100%" id="itemPlaceholderContainer" style="" border="0">
                                <tr runat="server" style="">
                                    <th runat="server"></th>
                                    <th runat="server">ID</th>
                                    <th runat="server">Name</th>
                                    <th runat="server">Address</th>
                                    <th runat="server">Date</th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>





        <br />
        <br />
        <div class="col-sm-2">
            <asp:Label ID="Label5" runat="server" Text="Credit Card:" Style="text-align:  left;"></asp:Label>
        </div>

        <div class="col-sm-4">
            <asp:Label ID="CreditCard" runat="server" Style="text-align:  left;"></asp:Label>
        </div>

        <div class="col-sm-1">
            <asp:Label ID="Labe1" runat="server" Text="Payment:" Style="text-align:  left;"></asp:Label>
        </div>

        <div class="col-sm-5">
            <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatLayout="Table" RepeatColumns="3"
                                 Width="70%">
                <asp:ListItem Value="C">Credit</asp:ListItem>
                <asp:ListItem Value="D">Debit</asp:ListItem>
                <asp:ListItem Value="M">Cash</asp:ListItem>
            </asp:RadioButtonList>
        </div>

        <br />
        <br />
        <div class="col-sm-2">
            <asp:Label ID="Label8" runat="server" Text="Date Out:" Style="text-align:  left;"></asp:Label>&nbsp;&nbsp;
        </div>

        <div class="col-sm-4">
            <asp:Label ID="DateOut" runat="server" Style="text-align:  left;"></asp:Label>
        </div>

        <div class="col-sm-2">
            <asp:Label ID="DaysOut" runat="server" Style="text-align:  left;">Days:</asp:Label>
            <asp:TextBox ID="DaysOutInput" TextMode="Number" runat="server" min="0.5" max="10000" step="0.5"></asp:TextBox>
        </div>

        <div class="col-sm-2">
            <asp:LinkButton ID="ReturnBtn" runat="server" style="justify-content:flex-start" OnClick="processReturn_Click">Process</asp:LinkButton>
        </div>

        <div class="col-sm-2">
            <asp:LinkButton ID="PayBtn" runat="server" style="justify-content:flex-start" OnClick="payReturn_Click">Pay</asp:LinkButton>
        </div>

        <div class="container">
            <div class="row">
                <div class="col-lg">
                    <div class="row">

                    </div>
                </div>
            </div>
        </div>

        <div class="container">
            <div class="row">
                <div class="col-lg-12">
                    <asp:ListView ID="RenturnEquipmentListView" runat="server" DataSourceID="ReturnDataSource">
                        <EditItemTemplate>
                            <tr style="background-color: #DCDCDC; color: #000000;">
                                <td>
                                    <asp:Button runat="server" CommandName="Update" Text="Update" ID="UpdateButton" />
                                    <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelButton" />
                                </td>
                                <td><asp:Label Text='<%# Bind("RentalEquipmentID") %>' runat="server" ID="RentalEquipmentIDTextBox" /></td>
                                <td><asp:Label Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" /></td>
                                <td><asp:Label Text='<%# Bind("ModelNumber") %>' runat="server" ID="ModelNumberTextBox" /></td>
                                <td><asp:Label Text='<%# Bind("SerialNumber") %>' runat="server" ID="SerialNumberTextBox" /></td>
                                <td><asp:Label Text='<%# Bind("DailyRate") %>' runat="server" ID="DailyRateTextBox" /></td>
                                <td><asp:TextBox Text='<%# Bind("Condition") %>' runat="server" ID="ConditionTextBox" /></td>
                                <td><asp:CheckBox Checked='<%# Bind("Available") %>' runat="server" ID="AvailableCheckBox" /></td>
                            </tr>
                        </EditItemTemplate>
                        <EmptyDataTemplate>
                            <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                                <tr>
                                    <td>No data was returned.</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <tr style="background-color: #DCDCDC; color: #000000;">
                                <td><asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" /></td>
                                <td><asp:Label Text='<%# Eval("RentalEquipmentID") %>' runat="server" ID="RentalEquipmentIDLabel" /></td>
                                <td><asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                <td><asp:Label Text='<%# Eval("ModelNumber") %>' runat="server" ID="ModelNumberLabel" /></td>
                                <td><asp:Label Text='<%# Eval("SerialNumber") %>' runat="server" ID="SerialNumberLabel" /></td>
                                <td><asp:Label Text='<%# Eval("DailyRate") %>' runat="server" ID="DailyRateLabel" /></td>
                                <td><asp:Label Text='<%# Eval("Condition") %>' runat="server" ID="ConditionLabel" /></td>
                                <td><asp:CheckBox Checked='<%# Eval("Available") %>' runat="server" ID="AvailableCheckBox" Enabled="false" /></td>
                            </tr>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <table runat="server" width="100%" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                <tr runat="server" style="background-color: #DCDCDC; color: #000000;">
                                    <th runat="server"></th>
                                    <th runat="server">ID</th>
                                    <th runat="server">Description</th>
                                    <th runat="server">Model#</th>
                                    <th runat="server">Serial#</th>
                                    <th runat="server">Rate</th>
                                    <th runat="server">Condition</th>
                                    <th runat="server">Av</th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>

        

        <asp:Label ID="Label9" runat="server" Text="Subtotal:" Style="text-align: left;"></asp:Label>&nbsp;
        <asp:Label ID="Subtotal" runat="server" Text="" Style="text-align: left;"></asp:Label>&nbsp;&nbsp;

        <asp:Label ID="Label10" runat="server" Text="GST:" Style="text-align: left;"></asp:Label>&nbsp;
        <asp:Label ID="GST" runat="server" Text="" Style="text-align: left;"></asp:Label>&nbsp;&nbsp;

        <asp:Label ID="Label11" runat="server" Text="Discount:" Style="text-align: left;"></asp:Label>&nbsp;
        <asp:Label ID="Discount" runat="server" Text="" Style="text-align: left;"></asp:Label>&nbsp;&nbsp;

        <asp:Label ID="Label12" runat="server" Text="Total:" Style="text-align: left;"></asp:Label>&nbsp;
        <asp:Label ID="Total" runat="server" Text="" Style="text-align: left;"></asp:Label>&nbsp;&nbsp;

    </div>

    <asp:ObjectDataSource ID="CustomerReturnDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="customersReturnLookUp" TypeName="eToolsSystem.BLL.RentalCustomerController">
        <SelectParameters>
            <asp:ControlParameter ControlID="PhoneNumberInput" PropertyName="Text" Name="clientPhoneNumberOrRentalID" Type="String"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ReturnDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="returnitemlist" TypeName="eToolsSystem.BLL.RentalController" DataObjectTypeName="eTools.Data.Entities.RentalEquipment" UpdateMethod="newReturn_Update">
        <SelectParameters>
            <asp:ControlParameter ControlID="selectedCustomerRental" PropertyName="Text" Name="rentalid" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    
</asp:Content>

