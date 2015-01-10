<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="MapClassToClassTeacher.aspx.cs" Inherits="WebForms_MapClassToClassTeacher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Panel ID="pnlMapping" runat="server">
<table>
<tr>

<td style="font-family: arial; font-size: medium; color: #008000">Select Teacher</td>
<td>  &nbsp; &nbsp; </td>
<td> <asp:DropDownList ID="ddlteacher" CssClass="ddl" runat="server"></asp:DropDownList>  </td>
</tr>
<tr>
<td style="font-family: arial; font-size: medium; color: #008000">Select Class  </td>
<td>&nbsp; &nbsp;  </td>
<td><asp:DropDownList ID="ddlclass" CssClass="ddl" runat="server"></asp:DropDownList> </td>
</tr>

<tr>
<td>
<asp:Button ID="btnsubmit" CssClass="big-button" Text="Submit" runat="server" onclick="btnsubmit_Click" />
</td>
</tr>

</table>
</asp:Panel>

</asp:Content>

