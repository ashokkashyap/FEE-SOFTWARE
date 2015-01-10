<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="Cashreport.aspx.cs" Inherits="WebForms_Cashreport" %>
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
                            width: 187px; height: 25px; padding-left: 8px;">
                            Cash Report Total
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
                        :<asp:TextBox ID="txtStartDate" runat="server" Width="60%"></asp:TextBox>
                        <ajax:CalendarExtender ID="CE1" runat="server" Format="D" PopupButtonID="txtStartDate" TargetControlID="txtStartDate"></ajax:CalendarExtender>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkdetails" Text="Check For Less Details" runat="server" /></td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <br />
                        <asp:ImageButton ID="btnGetDetails" runat="server" OnClick="btnGetDetails_Click"
                            ValidationGroup="v1" Height="18px" Width="18px" ImageUrl="~/Resources/search.png" />
                            
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <br />
                        <div style="max-height:600px; max-width:1600px; overflow:auto;">
                            <asp:Panel ID="pnlDetails" runat="server">
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

