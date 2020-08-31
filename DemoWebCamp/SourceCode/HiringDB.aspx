<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HiringDB.aspx.cs" Inherits="demomvp.HiringDB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" DataKeyNames="Alias" DataSourceID="SqlDataSource1">
   
</asp:GridView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HiringConnectionString %>" SelectCommand="SELECT * FROM [Engineers] where PodName LIKE 'App Services' or PodName LIKE 'ARR'"></asp:SqlDataSource>
</asp:Content>
