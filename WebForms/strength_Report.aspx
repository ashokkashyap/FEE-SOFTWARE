<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="strength_Report.aspx.cs" Inherits="WebForms_strength_Report" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <style type="text/css">
        .style1
        {
            width: 202px;
        }
        .style7
        {
            font-family: Calibri;
            font-size: 14px;
            font-weight: normal;
            font-style: normal;
            text-transform: none;
            color: #006600;
            width: 15%;
        }
     .style4
     {
         width: 114px;
     }
     .style5
     {
         width: 159px;
     }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div align="center">
        <asp:Panel ID="Panel1" runat="server">
            <table cellpadding="0" cellspacing="0" style="width: 90%">
                <tr style="height: 50px;">
                   <td valign="bottom" colspan="4" align="left" 
                        
                        
                        
                        style="color: #FFAB60; font-family: Calibri;
                        letter-spacing: 1px; font-weight: bold; font-size: 18px; border-bottom: solid 2px #FFAB60;">
                        <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60;
                            border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px;
                            width: 310px; height: 25px; padding-left: 8px;">
                            Class Wise Strength Report
                        </div>
                    </td>
                </tr>
                <tr style="height: 50px;">
                    <td class="style7" style="padding-left: 15px; letter-spacing: 1px;">
                        Select Report type
                    </td>
                    <td class="style1">
                        <asp:DropDownList ID="ddlReportType" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                            <asp:ListItem Text="Select Report" Selected="True" Value="Select Report"></asp:ListItem>
                            <asp:ListItem Text="ClassWiseStrengthReport" Value="ClassWiseStrengthReport"></asp:ListItem>
                            <%--<asp:ListItem Text="ClassGenderWiseStrengthReport" Value="ClassGenderWiseStrengthReport"></asp:ListItem>--%>
                            <asp:ListItem Text="AdmNoWiseReport" Value="AdmNoWiseReport"></asp:ListItem>
                            <%--<asp:ListItem Text="AgeWiseReport" Value="AgeWiseReport"></asp:ListItem>--%>
                        </asp:DropDownList>
                    </td>
                    <td class="style4" align="left">
                        <asp:ImageButton ID="btnGenerateReport" ImageUrl="~/images/details.gif" Visible="false"
                            runat="server" OnClick="btnGenerateReport_Click" />
                    </td>
                    <td>
                        <asp:ImageButton ID="btnReport" runat="server" Height="35px" 
                            ImageUrl="~/images/pdfimg.png" OnClick="btnReport_Click" Visible="false" 
                            Width="40px" />
                    </td>
                </tr>
                <tr style="height: 50px;">
                    <td class="style7" style="padding-left: 15px; letter-spacing: 1px;">
                        &nbsp;</td>
                    <td class="style1">
                        <asp:Label ID="lblTotalStd" runat="server" Font-Size="Large" ForeColor="Green"></asp:Label> </td>
                    <td align="left" class="style4">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:GridView ID="gvRecords" runat="server" BackColor="#DEBA84" BorderColor="#DEBA84"
                            BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2">
                            <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                            <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                            <PagerStyle ForeColor="#8C4510" HorizontalAlign="left" />
                            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

