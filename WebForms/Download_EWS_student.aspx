<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="Download_EWS_student.aspx.cs" Inherits="WebForms_Download_EWS_student" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style3
        {
            height: 21px;
            width: 78px;
        }
        .style6
        {
            font-family: Calibri;
            font-size: 14px;
            font-weight: normal;
            font-style: normal;
            text-transform: none;
            color: #006600;
            width: 12%;
        }
        .style7
        {
            height: 21px;
            }
        .style8
        {
            height: 21px;
            width: 126px;
        }
        .style9
        {
            height: 21px;
            width: 379px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
<tr>
                    <td valign="bottom" align="left" 
                        style="color:#FFAB60; font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:18px; border-bottom:solid 2px #FFAB60;" 
                        colspan="5">
                        <div style="background-color:#FFFF99; border-top:2px solid #FFAB60; border-left:2px solid #FFAB60; border-right:2px solid #FFAB60; border-top-left-radius:25px 50px; border-top-right-radius:25px 50px; width:952px; height:25px; padding-left:8px;">
                            Download Ews Student</div>
                    </td>
                </tr>
<tr>
<td class="style6"  colspan="1"
        style="padding-left:15px; border-bottom:1px ridge gray; letter-spacing:1px;" >
Select Class:
</td>
<td class="style7" align="left" colspan="1">

    <asp:DropDownList ID="ddlclass" runat="server" AutoPostBack="true" 
        onselectedindexchanged="ddlclass_SelectedIndexChanged">
    </asp:DropDownList>
</td>
<td class="style8" >
Total Student Details:
    </td>
<td class="style9" >
    <asp:Label ID="lblTotalStudentMap" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Green"></asp:Label></td>
<td class="style3" >
    &nbsp;</td>
</tr>
<tr>
<td colspan="5">
<br />
</td>
</tr>
    
<tr>

<td colspan="5">
    <asp:Button ID="btnsubmit" runat="server" Text="Submit" 
        onclick="btnsubmit_Click" />
   
    </td>
     </tr>
</table>
</asp:Content>

