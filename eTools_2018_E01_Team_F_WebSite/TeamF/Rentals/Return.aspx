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
        <div class="row">
            <asp:Label runat="server" ID="Label6" Text="RentalID:" />&nbsp;&nbsp;
            <asp:Label ID="selectedCustomerRental" runat="server" Style="text-align: right;"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="newReturn" runat="server" Visible="false" OnClick="newReturn_Click">New Return</asp:LinkButton>

            <%--This is hidden, internal use--%>
            <asp:Label ID="HIDDEN_LABEL_selectedCustomerID" runat="server" Visible="false"></asp:Label>
        </div>
        <br />
        <br />

        <%--Phone number/ customer slection row--%>
        <div class="row">

            <div class="col-sm-2">
                <div class="row">
                    <asp:Label ID="Label3" runat="server" Text="Phone Number:" Style="text-align: right;"></asp:Label>
                </div>

                <div class="row">
                    <br />
                    <asp:Label ID="Label7" runat="server" Text="Profile:" Style="text-align: right;"></asp:Label>
                </div>
            </div>

            <div class="col-sm-4">
                <div class="row">
                    <asp:TextBox ID="PhoneNumberInput" runat="server"></asp:TextBox>
                    <asp:LinkButton ID="phoneNumberSubmitBtn" runat="server">Search</asp:LinkButton>
                </div>
                <div class="row">
                    <br />
                    <asp:Label ID="selectedCustomerName" runat="server" Style="text-align: right;"></asp:Label><br />
                    <asp:Label ID="selectedCustomerAddress" runat="server" Style="text-align: right;"></asp:Label><br />
                    <asp:Label ID="selectedCustomerCity" runat="server" Style="text-align: right;"></asp:Label><br />
                    <br />
                </div>
            </div>


            <div class="col-sm-6">
                <div class="row">
                    <asp:ListView ID="customerLookup" runat="server" DataSourceID="CustomerReturnDataSource" OnItemCommand="selectCustomer_Click">
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
                                    <asp:LinkButton ID="selectedCustomerBtn" runat="server"
                                        CssClass="btn" CommandArgument='<%# Eval("rentalid") %>'>
                                        <span aria-hidden="true" class="glyphicon glyphicon-plus">&nbsp;</span>
                                    </asp:LinkButton>
                                </td>

                                <td>
                                    <asp:Label Text='<%# Eval("rentalid") %>' runat="server" ID="rentalidLabel" />&nbsp;&nbsp;</td>
                                <td>
                                    <asp:Label Text='<%# Eval("fullname") %>' runat="server" ID="fullnameLabel" />&nbsp;&nbsp;</td>
                                <td>
                                    <asp:Label Text='<%# Eval("address") %>' runat="server" ID="addressLabel" />&nbsp;&nbsp;</td>
                                <td>
                                    <asp:Label Text='<%# Eval("mmddyy") %>' runat="server" ID="mmddyyLabel" />&nbsp;&nbsp;</td>
                            </tr>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <table runat="server">
                                <tr runat="server">
                                    <td runat="server">
                                        <table runat="server" id="itemPlaceholderContainer" style="" border="0">
                                            <tr runat="server" style="">
                                                <th runat="server"></th>
                                                <th runat="server">ID</th>
                                                <th runat="server">Name</th>
                                                <th runat="server">Address</th>
                                                <th runat="server">Date</th>
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
                    </asp:ListView>
                </div>
            </div>
        </div>

        <br />
        <br />
        <div class="row">

            <div class="col-sm-2">
                <div class="row">
                    <asp:Label ID="Label5" runat="server" Text="Credit Card:" Style="text-align: right;"></asp:Label>
                </div>
            </div>

            <div class="col-sm-3">
                <div class="row">
                    <asp:Label ID="CreditCard" runat="server" Style="text-align: right;"></asp:Label>
                </div>
            </div>

            <div class="col-sm-2">
                <div class="row">
                    <asp:Label ID="Labe1" runat="server" Text="Payment:" Style="text-align: right;"></asp:Label>
                </div>
            </div>

            <div class="col-sm-5">
                <div class="row">
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatLayout="Table" RepeatColumns="3"
                        Width="100%">
                        <asp:ListItem Value="C">Credit</asp:ListItem>
                        <asp:ListItem Value="D">Debit</asp:ListItem>
                        <asp:ListItem Value="M">Cash</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>

        </div>

        <br />
        <br />
        <div class="row">
            <div class="col-sm-1">
                <div class="row">
                    <asp:Label ID="Label8" runat="server" Text="Date Out:" Style="text-align: right;"></asp:Label>&nbsp;&nbsp;
                </div>
            </div>

            <div class="col-sm-4">
                <div class="row">
                    <asp:Label ID="DateOut" runat="server" Style="text-align: right;"></asp:Label>
                </div>
            </div>

            <div class="col-sm-1">
                <asp:LinkButton ID="ReturnBtn" runat="server">Process</asp:LinkButton>
            </div>

            <div class="col-sm-1">
            </div>

            <div class="col-sm-1">
            </div>

        </div>

        <div class="row">
            <div class="container-fluid">
                <br />
                <br />
                <asp:ListView ID="ReturnListView" runat="server" DataSourceID="ReturnObjectDataSource" HorizontalOptions="center" OnSelectedIndexChanged="ReturnListView_SelectedIndexChanged">
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
                                <asp:Label Text='<%# Eval("eqiupmentID") %>' runat="server" ID="eqiupmentIDLabel" />&nbsp;&nbsp&nbsp;&nbsp</td>
                            <td>
                                <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" />&nbsp;&nbsp;&nbsp&nbsp</td>
                            <td>
                                <asp:Label Text='<%# Eval("SerialNumber") %>' runat="server" ID="SerialNumberLabel" />&nbsp;&nbsp;&nbsp&nbsp</td>
                            <td>
                                <asp:Label Text='<%# Eval("Rate") %>' runat="server" ID="RateLabel" />&nbsp;&nbsp;&nbsp&nbsp</td>
                            <td>
                                <asp:Label Text='<%# Eval("outDate") %>' runat="server" ID="outDateLabel" />&nbsp;&nbsp;&nbsp&nbsp</td>
                            <td>
                                <asp:TextBox Text='<%# Eval("coditionCommets") %>' runat="server" ID="coditionCommetsLabel" />&nbsp;&nbsp;&nbsp&nbsp</td>
                            <td>
                                <asp:TextBox Text='<%# Eval("customerCommets") %>' runat="server" ID="customerCommetsLabel" />&nbsp&nbsp;&nbsp;&nbsp</td>
                            <td>
                                <asp:CheckBox Checked='<%# Eval("Av") %>' runat="server" ID="AvCheckBox" Enabled="true" />&nbsp;&nbsp;&nbsp&nbsp</td>
                        </tr>
                    </ItemTemplate>
                    <LayoutTemplate>
                        <table runat="server">
                            <tr runat="server">
                                <td runat="server">
                                    <table runat="server" id="itemPlaceholderContainer" style="" border="0">
                                        <tr runat="server" style="">
                                            <th runat="server">ID</th>
                                            <th runat="server">Description</th>
                                            <th runat="server">Serial#</th>
                                            <th runat="server">Rate</th>
                                            <th runat="server">Out</th>
                                            <th runat="server">In</th>
                                            <th runat="server">Comments</th>
                                            <th runat="server">Available</th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder"></tr>
                                    </table>
                                </td>
                            </tr>
                            <tr runat="server">
                                <td runat="server" style="text-align: center; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
                                    <asp:DataPager runat="server" ID="DataPager1" PagedControlID="ReturnListView" PageSize="3">
                                        <Fields>
                                            <asp:NumericPagerField ButtonType="Link"></asp:NumericPagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
        </div>

        <div class="row">
            <asp:Label ID="Label9" runat="server" Text="Subtotal:" Style="text-align: right;"></asp:Label>&nbsp;
            <asp:Label ID="Subtotal" runat="server" Text="" Style="text-align: right;"></asp:Label>&nbsp;&nbsp;
           
            <asp:Label ID="Label10" runat="server" Text="GST:" Style="text-align: right;"></asp:Label>&nbsp;
            <asp:Label ID="GST" runat="server" Text="" Style="text-align: right;"></asp:Label>&nbsp;&nbsp;
            
            <asp:Label ID="Label11" runat="server" Text="Discount:" Style="text-align: right;"></asp:Label>&nbsp;
            <asp:Label ID="Discount" runat="server" Text="" Style="text-align: right;"></asp:Label>&nbsp;&nbsp;

            <asp:Label ID="Label12" runat="server" Text="Total:" Style="text-align: right;"></asp:Label>&nbsp;
            <asp:Label ID="Total" runat="server" Text="" Style="text-align: right;"></asp:Label>&nbsp;&nbsp;
        </div>

    </div>

    <asp:ObjectDataSource ID="CustomerReturnDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="customersReturnLookUp" TypeName="eToolsSystem.BLL.RentalCustomerController">
        <SelectParameters>
            <asp:ControlParameter ControlID="PhoneNumberInput" PropertyName="Text" Name="clientPhoneNumberOrRentalID" Type="String"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="ReturnObjectDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="returnitemlist" TypeName="eToolsSystem.BLL.RentalController">
        <SelectParameters>
            <asp:ControlParameter ControlID="selectedCustomerRental" PropertyName="Text" DefaultValue="238" Name="rentalid" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
