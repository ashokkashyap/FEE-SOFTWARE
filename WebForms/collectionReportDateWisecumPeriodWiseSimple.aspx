<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="collectionReportDateWisecumPeriodWiseSimple.aspx.cs" Inherits="WebForms_collectionReportDateWisecumPeriodWiseSimple" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" width="90%" border="0" style="border-color: Black;">
                <tr>
                    <td valign="bottom" colspan="4" align="left" style="color: #FFAB60; font-family: Calibri;
                        letter-spacing: 1px; font-weight: bold; font-size: 18px; border-bottom: solid 2px #FFAB60;">
                        <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60;
                            border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px;
                            width: 160px; height: 25px; padding-left: 8px;">
                            Collection Report
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br /><asp:Label ID="lblInfo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="Calibri14N" style="padding-left: 15px; border-bottom: 1px ridge grey;
                        width: 23%; letter-spacing: 1px;">
                        Start Date
                        <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtStartDate"
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>
                    </td>
                    <td style="border-bottom: 1px ridge grey; width:30%">
                        :<asp:TextBox ID="txtStartDate" runat="server" Width="90%"></asp:TextBox>
                        <ajax:CalendarExtender ID="CE1" runat="server" Format="D" PopupButtonID="txtStartDate" TargetControlID="txtStartDate"></ajax:CalendarExtender>
                    </td>
                    <td class="Calibri14N" style="padding-left: 5px; border-bottom: 1px ridge grey;
                        width: 23%; letter-spacing: 1px;">
                        End Date
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEndDate"
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>
                    </td>
                    <td style="border-bottom: 1px ridge grey; width:30%;">
                        :<asp:TextBox ID="txtEndDate" runat="server" Width="90%"></asp:TextBox>
                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" Format="D" PopupButtonID="txtEndDate" TargetControlID="txtEndDate"></ajax:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <br />
                        <asp:ImageButton ID="btnGetDetails" runat="server" OnClick="btnGetDetails_Click"
                            ValidationGroup="v1" Height="18px" Width="18px" ImageUrl="~/Resources/search.png" />
                            <asp:ImageButton ID="btnDownloadExcel" runat="server" OnClick="btnDownloadExcel_Click"
                            Height="22px" Width="22px" ImageUrl="~/Resources/excel-logo.jpg" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="left">
                        <br />
                        <div style="max-height:300px; max-width:750px; overflow:auto;">
                            <asp:Panel ID="pnlDetails" runat="server">
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

