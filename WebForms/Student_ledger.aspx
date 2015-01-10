<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="Student_ledger.aspx.cs" Inherits="WebForms_Student_ledger" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
<tr>
<td>
<asp:TextBox ID="txtregsNo" runat="server" Width="200px" placeholder="Student registration No"></asp:TextBox>

</td>
<td>  </td>
<td>
<asp:Button ID="btnsubmit" Text="Submit" runat="server" onclick="btnsubmit_Click" />
</td>
</tr>
<tr>
<td>
    <asp:DropDownList ID="ddlSelectClass" Visible="false" runat="server">
    </asp:DropDownList>
</td>

</tr>
</table>
</asp:Content>

