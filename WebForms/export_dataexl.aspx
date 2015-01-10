<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="export_dataexl.aspx.cs" Inherits="WebForms_export_dataexl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>
<b>Please Select Excel File: </b>
<asp:FileUpload ID="fileuploadExcel" runat="server" />&nbsp;&nbsp;
<asp:Button ID="btnImport" runat="server" Text="Import Data" OnClick="btnImport_Click" />
<br />
<asp:Label ID="lblMessage" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label><br />
<asp:GridView ID="grvExcelData" runat="server">
<HeaderStyle BackColor="#df5015" Font-Bold="true" ForeColor="White" />
</asp:GridView>
</div>
</asp:Content>

