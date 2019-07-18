<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Rental.aspx.cs" Inherits="eTools_2018_E01_Team_F_WebSite.TeamF.Rentals.Rental" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-header">
        <h1>Rental</h1>
    </div>

    <%--Just dragged MessageUserControl.ascx on to this web form--%>
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />

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

    <div class="container">
        <%-- OrderID--%>
         <div class="row">
            <asp:Label runat="server" ID="Label6" Text="RentalID:"/>&nbsp;&nbsp;
             <%-- --%>
             <%--asp:Label ID="selectedCustomerRental" runat="server" style="text-align: right;"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;--%>
            <asp:Label ID="selectedCustomerRental" runat="server" style="text-align: right;"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="newRental" runat="server" visible="false" OnClick="newRental_Click ">New Rental</asp:LinkButton> &nbsp;
           
             <%--This is hidden, internal use--%>
            <asp:Label ID="HIDDEN_LABEL_selectedCustomerID" runat="server" Visible="false"></asp:Label>
        </div>
        <br/><br/>
        <%--Phone number/ customer slection row--%>
        <div class="row">
      
              <div class="col-sm-2">
                <div class="row">
                    <asp:Label ID="Label3" runat="server" Text="Phone Number:" style="text-align: right;"></asp:Label>
                </div>

                <div class="row">
                     <br/>
                    <asp:Label ID="Label7" runat="server" Text="Profile:" style="text-align: right;"></asp:Label>
                </div>
              </div>

              <div class="col-sm-3">
                  <div class="row">
                     <asp:TextBox ID="PhoneNumberInput" runat="server"></asp:TextBox>
                     <asp:LinkButton ID="phoneNumberSubmitBtn" runat="server">Search</asp:LinkButton>
                  </div>
                  <div class="row">
                     <br/>
                    <asp:Label ID="selectedCustomerName"      runat="server" style="text-align: right;"></asp:Label><br/>
                    <asp:Label ID="selectedCustomerAddress"   runat="server" style="text-align: right;"></asp:Label><br/>
                    <asp:Label ID="selectedCustomerCity"      runat="server" style="text-align: right;"></asp:Label><br/>
                    <br/>
                  </div>  
              </div>

              <div class="col-sm-7">
                  <div class="row">
                      <asp:ListView ID="CustomerListView" runat="server" DataSourceID="CustomerDataSource" OnItemCommand="selectCustomer_Click">               
                          <EmptyDataTemplate>
                              <table runat="server" style="">
                                  <tr>
                                      <td>No data was returned.</td>
                                  </tr>
                              </table>
                          </EmptyDataTemplate>
                          <ItemTemplate>
                              <tr style="">
                                  <td><asp:LinkButton ID="selectedCustomer" runat="server"
                                        CssClass="btn" CommandArgument='<%# Eval("ID") %>'>
                                        <span aria-hidden="true" class="glyphicon glyphicon-plus">&nbsp;</span>
                                      </asp:LinkButton> </td>

                                  <td><asp:Label Text='<%# Eval("Fullname") %>' runat="server" ID="FullnameLabel" />&nbsp;&nbsp</td>
                                  <td><asp:Label Text='<%# Eval("Address") %>' runat="server" ID="AddressLabel" />&nbsp;&nbsp</td>
                                  <td><asp:Label Text='<%# Eval("PhoneNumber") %>' runat="server" ID="PhoneNumberLabel" />&nbsp;&nbsp</td>
                              </tr>
                          </ItemTemplate>
                          <LayoutTemplate>
                              <table runat="server">
                                  <tr runat="server">
                                      <td runat="server">
                                          <table runat="server" id="itemPlaceholderContainer" style="" border="0">
                                              <tr runat="server" style="">
                                                  <th runat="server"></th>
                                                  <th runat="server">Name</th>
                                                  <th runat="server">Address</th>
                                                  <th runat="server">PhoneNumber</th>
                                              </tr>
                                              <tr runat="server" id="itemPlaceholder"></tr>
                                          </table>
                                      </td>
                                  </tr>
                                  <tr runat="server">
                                    <td runat="server" style="text-align: center; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
                                        <asp:DataPager runat="server" ID="DataPager1" PagedControlID="CustomerListView" PageSize="3">
                                            <Fields>
                                                <asp:NumericPagerField ButtonType="Link" ></asp:NumericPagerField>  
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                              </table>
                          </LayoutTemplate>
                          
                      </asp:ListView>
                  </div>
              </div>
        </div>

         <%--credit card row--%>
        <div class="row">
            <div class="col-sm-2">
                <div class="row">
                    <asp:Label ID="Label5" runat="server" Text="Credit Card:" style="text-align: right;"></asp:Label>
                </div>
            </div>

            <div class="col-sm-3">   
                <div class="row">
                    <asp:TextBox ID="creditcardinput" runat="server"></asp:TextBox>
                    <%--Not sure to keep this 
                    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="createRental_Click">Create</asp:LinkButton>--%>
                </div>
            </div>
        </div>
    </div>
    
    <%--ListView labels--%>
    <br/><br/>
    <div class="container">
        <div class="row">
            <div class="col-lg-6">
                <asp:Label runat="server" ID="Label2" Text="Available Rental Equipment" Font-Bold="true"/>
            </div>
            <div class="col-lg-6">
              <asp:Label runat="server" ID="Label4" Text="Rental Cart" style="text-align:right;" Font-Bold="true"/>
              <br/>
            </div>
        </div>
    </div>

    <%--coupon and cancel btn--%> 
    <div class="container">
        <div class="row">
            <div class="col-lg-6">
            </div>
            <div class="col-lg-6">
                <asp:Label runat="server" ID="Label1" Text="Coupon:" Font-Bold="true"/>
                <asp:TextBox ID="couponinput" runat="server"></asp:TextBox>

                <asp:LinkButton ID="applyCoupon" runat="server">
                    <span aria-hidden="true" class="glyphicon glyphicon-refresh">&nbsp;&nbsp;</span>
                </asp:LinkButton> 
                <asp:LinkButton ID="LinkButton4" runat="server" OnClick="deleteRental_Click">Cancel</asp:LinkButton>
                <br/>
            </div>
        </div>
    </div>
   
    <br/><br/>
    <div class="container">
        <div class="row">
             <%--Available Rental Equipment Listview--%>
            <div class="col-lg-6">
                <asp:ListView ID="RentalEquipmentListview" runat="server" DataSourceID="EquipmentDataSource" OnItemCommand="addRentalEquipment_Click">
                    <EmptyDataTemplate>
                        <table runat="server" style="">
                            <tr>
                                <td>No data was returned.</td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr style="">
                            <td><asp:LinkButton ID="AddToRental" runat="server"
                                 CssClass="btn" CommandArgument='<%# Eval("eqiupmentID") %>'>
                                <span aria-hidden="true" class="glyphicon glyphicon-plus">&nbsp;</span>
                            </asp:LinkButton> </td>

                            <td><asp:Label Text='<%# Eval("eqiupmentID") %>' runat="server" ID="eqiupmentIDLabel" />&nbsp;&nbsp</td>
                            <td><asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" />&nbsp;&nbsp</td>
                            <td><asp:Label Text='<%# Eval("SerailNumber") %>' runat="server" ID="SerailNumberLabel" />&nbsp;&nbsp</td>
                            <td><asp:Label Text='<%# Eval("Rate") %>' runat="server" ID="RateLabel" />&nbsp;&nbsp</td>
                        </tr>
                    </ItemTemplate>
                    <LayoutTemplate>
                        <table runat="server">
                            <tr runat="server">
                                <td runat="server">
                                    <table runat="server" id="itemPlaceholderContainer" style="" 
                                        border="0">
                                        <tr runat="server" style="" >
                                            <th runat="server"></th>
                                            <th runat="server">ID</th>
                                            <th runat="server">Description</th>
                                            <th runat="server">Serial#</th>
                                            <th runat="server">Rate</th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder"></tr>
                                    </table>
                                </td>
                            </tr>
                             <tr runat="server">
                                <td runat="server" style="text-align: center; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
                                    <asp:DataPager runat="server" ID="DataPager1" PagedControlID="RentalEquipmentListview" PageSize="10" ShowFirstPageButton="true" ShowLastPageButton="true">
                                        <Fields>
                                            <asp:NumericPagerField ButtonType="Link" ></asp:NumericPagerField>     
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
             <%--Pending Rental Listview--%>
            <div class="col-lg-6">
                <asp:ListView ID="PendingRentalListView" runat="server" DataSourceID="CurrentRentalDataSource"
                    OnItemCommand="removeRentalEquipment_Click">        
                    <EmptyDataTemplate>
                        <table runat="server" style="">
                            <tr>
                                <td>No data was returned.</td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr style="">
                            <td><asp:LinkButton ID="removeSelectedEquipment" runat="server"
                                CssClass="btn" CommandArgument='<%# Eval("eqiupmentID") %>'>
                                <span aria-hidden="true" class="glyphicon glyphicon-minus">&nbsp;</span>
                                </asp:LinkButton> </td>
                            <td><asp:Label Text='<%# Eval("eqiupmentID") %>' runat="server" ID="eqiupmentIDLabel" />&nbsp;&nbsp</td>
                            <td><asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" />&nbsp;&nbsp</td>
                            <td><asp:Label Text='<%# Eval("SerailNumber") %>' runat="server" ID="SerailNumberLabel" />&nbsp;&nbsp</td>
                            <td><asp:Label Text='<%# Eval("Rate") %>' runat="server" ID="RateLabel" />&nbsp;&nbsp</td>
                            <td><asp:Label Text='<%# Eval("outDate") %>' runat="server" ID="outDateLabel" />&nbsp;&nbsp</td>
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
                                            <th runat="server">Description</th>
                                            <th runat="server">Serial#</th>
                                            <th runat="server">Rate</th>
                                            <th runat="server">Out</th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder"></tr>
                                    </table>
                                </td>
                            </tr>
                            <tr runat="server">
                            <td runat="server" style="text-align: center; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
                                <asp:DataPager runat="server" ID="DataPager1" PagedControlID="PendingRentalListView" PageSize="10" ShowFirstPageButton="true" ShowLastPageButton="true">
                                    <Fields>
                                        <asp:NumericPagerField ButtonType="Link" ></asp:NumericPagerField>     
                                    </Fields>
                                </asp:DataPager>
                            </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
        </div>
     </div>
  
    <asp:ObjectDataSource ID="CurrentRentalDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="currentCustomerRental" TypeName="eToolsSystem.BLL.RentalDetailController">
        <SelectParameters>
            <asp:ControlParameter ControlID="selectedCustomerRental" PropertyName="Text" Name="rentalId" Type="Int32" DefaultValue="0"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="CustomerDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="customersByPhoneNumber" TypeName="eToolsSystem.BLL.RentalCustomerController">
        <SelectParameters>
            <asp:ControlParameter ControlID="PhoneNumberInput" PropertyName="Text" DefaultValue="VinceLovesVince" Name="pNumber" Type="String"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
   
    <asp:ObjectDataSource ID="EquipmentDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="allAvailableEquipmentList" TypeName="eToolsSystem.BLL.RentalEquipmentController"></asp:ObjectDataSource>
</asp:Content>
