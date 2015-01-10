<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="studentFeeBill.aspx.cs" Inherits="WebForms_studentFeeBill" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<form id="aspnetForm" >
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" width="90%" border="0" style="border-color: Black;">
                <tr>
                    <td valign="bottom" colspan="4" align="left" style="color: #FFAB60; font-family: Calibri;
                        letter-spacing: 1px; font-weight: bold; font-size: 18px; border-bottom: solid 2px #FFAB60;">
                        <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60;
                            border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px;
                            width: 140px; height: 25px; padding-left: 8px;">
                            Student Fee Bill
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <br />
                        
                    </td>
                </tr>
                <tr>
                    <td class="Calibri14N" style="padding-left:10px;">
                        Select Class
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSelectClass" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                    </td>
                    <td class="Calibri14N">
                        : <asp:DropDownList ID="ddlSelectClass" runat="server" Width="150px" OnSelectedIndexChanged="ddlSelectClass_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td class="Calibri14N" style=" padding-left:20px;">
                        Select Student
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue="-1" runat="server" ControlToValidate="ddlSelectStudent" ErrorMessage="*" ForeColor="Red" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                    </td>
                    <td class="Calibri14N">
                        : <asp:DropDownList ID="ddlSelectStudent" runat="server" Width="180px" 
                            InitialValue="-1" AutoPostBack="True" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                <td class="Calibri14N" style=" padding-left:20px;">
                        Select Month
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" InitialValue="-1" runat="server" ControlToValidate="ddlSelectStudent" ErrorMessage="*" ForeColor="Red" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                    </td>
                    <td class="Calibri14N">
                         <asp:TextBox ID="txtmnth" runat="server"></asp:TextBox>
                        <ajax:CalendarExtender ID="CalendarExtender1" PopupButtonID="txtmnth" TargetControlID="txtmnth" Animated="true" Format="dd-MMMM-yyyy" runat="server">
                       
                        </ajax:CalendarExtender>
                    </td>
                    <td>
                    <asp:RadioButtonList ID="radiobtnNEW_old" style="Display:none" runat="server" 
                RepeatDirection="Horizontal" RepeatLayout="Table">
                <asp:ListItem Text="NEW" Value="NEW"></asp:ListItem>
                <asp:ListItem Text="OLD" Value="OLD"></asp:ListItem>
                
            </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                <td colspan="2" align="center">
                    <br />
                      
                        <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/Resources/submit.jpg"  OnClick="btnSubmit_Click" OnClientClick="aspnetForm.target ='_blank';" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</asp:Content>

