<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="chequeBounceReport.aspx.cs" Inherits="WebForms_chequeBounceReport" %>
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
                            width: 200px; height: 25px; padding-left: 8px;">
                            Cheque Bounce Report
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <br />
                        <asp:ImageButton ID="btnDownloadExcel" runat="server" OnClick="btnDownloadExcel_Click"
                            Height="22px" Width="22px" ImageUrl="~/Resources/excel-logo.jpg" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table cellpadding="0" cellspacing="0" border="1">
                            <tr>
                                <td align="center" style="font-family: Calibri; font-size: 14px; font-weight:bolder; width:40px; background-color:#FFFF99; border-color:#FFAB60;">
                                    SNO.
                                </td>
                                <td align="center" style="font-family: Calibri; font-size: 14px; font-weight:bolder; width:250px; background-color:#FFFF99; border-color:#FFAB60;">
                                    STUDENT NAME
                                </td>
                                <td align="center" style="font-family: Calibri; font-size: 14px; font-weight:bolder; width:120px; background-color:#FFFF99; border-color:#FFAB60;">
                                    CLASS
                                </td>
                                <td align="center" style="font-family: Calibri; font-size: 14px; font-weight:bolder; width:150px; background-color:#FFFF99; border-color:#FFAB60;">
                                    CHEQUE DATE
                                </td>
                                <td align="center" style="font-family: Calibri; font-size: 14px; font-weight:bolder; width:250px; background-color:#FFFF99; border-color:#FFAB60;">
                                    BANK DETAILS
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Repeater ID="rpChequeDetails" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Panel ID="pnlHeader" runat="server">
                                    <table cellpadding="0" cellspacing="0" border="1">
                                        <tr>
                                            <td align="center" style="font-family: Calibri; font-size: 12px; width:40px;">
                                                <asp:Label ID="lblSno" runat="server" Text='<%# Eval("SNO") %>'></asp:Label>
                                            </td>
                                            <td align="center" style="font-family: Calibri; font-size: 12px; width:250px;">
                                                <asp:Label ID="lblStudentName" runat="server" Text='<%# Eval("Student") %>'></asp:Label>
                                            </td>
                                            <td align="center" style="font-family: Calibri; font-size: 12px; width:120px;">
                                                <asp:Label ID="lblStudentClass" runat="server" Text='<%# Eval("Class") %>'></asp:Label>
                                            </td>
                                            <td align="center" style="font-family: Calibri; font-size: 12px; width:150px;">
                                                <asp:Label ID="lblChequeDate" runat="server" Text='<%# Eval("CD") %>'></asp:Label>
                                            </td>
                                            <td align="center" style="font-family: Calibri; font-size: 12px; width:250px;">
                                                <asp:Label ID="lblBankDetails" runat="server" Text='<%# Eval("BANK_NAME") %>'></asp:Label>
                                                <asp:HiddenField ID="hfBounceStatus" runat="server" Value='<%# Eval("BOUNCE_STATUS") %>' />
                                                <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("ID") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

