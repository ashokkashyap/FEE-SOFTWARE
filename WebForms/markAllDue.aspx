<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="markAllDue.aspx.cs" Inherits="WebForms_markAllDue" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" width="90%" border="0" style="border-color:Black;">
                <tr>
                    <td valign="bottom" colspan="2" align="left" style="color:#FFAB60; font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:18px; border-bottom:solid 2px #FFAB60;">
                        <div style="background-color:#FFFF99; border-top:2px solid #FFAB60; border-left:2px solid #FFAB60; border-right:2px solid #FFAB60; border-top-left-radius:25px 50px; border-top-right-radius:25px 50px; width:135px; height:25px; padding-left:8px;">
                            MARK ALL DUE
                        </div>
                    </td>
                </tr><tr><td colspan="2"><br /></td></tr>
                <tr>
                    <td class="Calibri14N" style="padding-left:10px; border-bottom:1px ridge grey; width:10%">
                        Due Date 
                        <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtDueDate" ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>
                    </td>
                    <td class="Calibri14N" style="border-bottom:1px ridge grey; width:25%;">
                        : <asp:TextBox ID="txtDueDate" runat="server" Width="150px"></asp:TextBox>
                        <ajax:CalendarExtender ID="CalE" runat="server" Format="dd-MMMM-yyyy" PopupButtonID="txtDueDate" TargetControlID="txtDueDate" ></ajax:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <br />
                        <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/Resources/submit.jpg" OnClick="btnSubmit_Click" ValidationGroup="v1"/>
                        <ajax:ConfirmButtonExtender ID="CE1" ConfirmText="Are you sure you want to continue !!!." TargetControlID="btnSubmit" runat="server">
                        </ajax:ConfirmButtonExtender>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

