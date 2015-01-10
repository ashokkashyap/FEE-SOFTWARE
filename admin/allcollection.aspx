<%@ Page Title="" Language="C#" MasterPageFile="~/admin/AdminMasterPage.master" AutoEventWireup="true"
    CodeFile="allcollection.aspx.cs" Inherits="admin_allcollection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 122px;
        }
        .style2
        {
            width: 34%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" border="0" 
                style="border-color: Black; height: 393px; width: 94%;">
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
                        <br />
                        <asp:Label ID="lblInfo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="Calibri14N" style="padding-left: 15px; border-bottom: 1px ridge grey;
                        width: 23%; letter-spacing: 1px;">
                        Start Date
                        <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtStartDate"
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>
                    </td>
                    <td style="border-bottom: 1px ridge grey; width: 30%">
                        :<asp:TextBox ID="txtStartDate" runat="server" Width="90%"></asp:TextBox>
                        <ajax:CalendarExtender ID="CE1" runat="server" Format="D" PopupButtonID="txtStartDate"
                            TargetControlID="txtStartDate">
                        </ajax:CalendarExtender>
                    </td>
                    <td class="Calibri14N" style="padding-left: 5px; border-bottom: 1px ridge grey; width: 23%;
                        letter-spacing: 1px;">
                        End Date
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEndDate"
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>
                    </td>
                    <td class="style2">
                        :<asp:TextBox ID="txtEndDate" runat="server" Width="90%"></asp:TextBox>
                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" Format="D" PopupButtonID="txtEndDate"
                            TargetControlID="txtEndDate">
                        </ajax:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <br />
                        <asp:ImageButton ID="btnGetDetails" runat="server" OnClick="btnGetDetails_Click"
                            ValidationGroup="v1" Height="18px" Width="18px" ImageUrl="~/Resources/search.png" />
                        <%--<asp:ImageButton ID="btnDownloadExcel" runat="server" OnClick="btnDownloadExcel_Click"
                            Height="22px" Width="22px" ImageUrl="~/Resources/excel-logo.jpg" Visible="false" />--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="left">
                        <br />
                        <div style="max-height: 100%; max-width: 1600px; overflow: auto;">
                            <table border="2" cellspacing="5px" cellpadding="5px">
                                <tr>
                                    <td class="style1">
                                        <b>S.no. </b>
                                    </td>
                                    <td>
                                        <b>Shivalik Mohali </b>
                                    </td>
                                    <td>
                                        <b>Shivalik Patiala </b>
                                    </td>
                                    <td>
                                        <b>Shivalik Chandigarh </b>
                                    </td>
                                    <td>
                                        <b>Shivalik Nawashahr </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        <b>Total Collection </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblspsmhlCollection" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblspsptlCollection" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblspschdCollection" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblspsnsrCollection" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                 <tr>
                                    <td class="style1">
                                        <b>Total Discount </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblspsmhlDiscount" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblspsptlDiscount" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblspschdDiscount" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblspsnsrDiscount" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="style1">
                                        <b>Defaulter </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblspsmhlDefaulter" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblspsptlDefaulter" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblspschdDefaulter" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblspsnsrDefaulter" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
