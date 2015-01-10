<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="monthWiseCollectionReport.aspx.cs" Inherits="WebForms_monthWiseCollectionReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" width="90%" border="0" style="border-color: Black;">
                <tr>
                    <td valign="bottom" colspan="4" align="left" style="color: #FFAB60; font-family: Calibri; letter-spacing: 1px; font-weight: bold; font-size: 18px; border-bottom: solid 2px #FFAB60;">
                        <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60; border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px; width: 240px; height: 25px; padding-left: 8px;">
                            FEE RECONCILE STATEMENT
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 23%; letter-spacing: 1px;">Select Session
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSelectMonth"
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td style="border-bottom: 1px ridge grey;">
                        :<asp:DropDownList ID="ddlSelectSession" runat="server"></asp:DropDownList>
                    </td>
                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 23%; letter-spacing: 1px;">Select Month
                       <%-- <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="ddlSelectMonth"
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td style="border-bottom: 1px ridge grey; width: 30%">
                        :<asp:DropDownList ID="ddlSelectMonth" runat="server" Width="200px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <br />
                        <asp:ImageButton ID="btnGetDetails" runat="server" OnClick="btnGetDetails_Click"
                            ValidationGroup="v1" Height="18px" Width="18px" ImageUrl="~/Resources/search.png" />
                            <asp:ImageButton ID="btnDownloadExcel" runat="server" OnClick="btnDownloadExcel_Click"
                            Height="22px" Width="22px" ImageUrl="~/Resources/excel-logo.jpg" Visible="false" />
                        <%--<div style="height:500px; width:500px; overflow:scroll; background-color:red;">--%>
                            <asp:Panel ID="pnlDetails" runat="server"></asp:Panel>
                        <%--</div>--%>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

