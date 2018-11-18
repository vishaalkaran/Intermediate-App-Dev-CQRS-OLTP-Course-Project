<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="eTools_2018_E01_Team_F_WebSite.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h3>Team Members</h3>
    <p>David Wen <br />
    Alex Marshall <br />
    Ramandeep Singh <br />
    Vincent Karan</p>
    <h3>Site Default Security</h3>
    <p>???</p>
    <h3>Connection Strings</h3>
    <p>name="eToolsDB" <br />
    Data Source=. <br />
    Initial Catalog =eTools <br />
    Integrated Security=true <br />
    providerName="System.Data.SqlClient</p>
</asp:Content>
