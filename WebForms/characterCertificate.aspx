<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="characterCertificate.aspx.cs" Inherits="WebForms_characterCertificate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" width="90%" border="0" style="border-color: Black;">
                <tr>
                    <!--text-shadow: 2px 2px 2px #000-->
                    <td valign="bottom" colspan="4" align="left" style="color: #FFAB60; font-family: Calibri;
                        letter-spacing: 1px; font-weight: bold; font-size: 18px; border-bottom: solid 2px #FFAB60;">
                        <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60;
                            border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px;
                            width: 220px; height: 25px; padding-left: 8px;">
                            CHARACTER CERTIFICATE
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
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
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSelectStudent" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                    </td>
                    <td class="Calibri14N">
                        : <asp:DropDownList ID="ddlSelectStudent" runat="server" Width="180px" OnSelectedIndexChanged="ddlSelectStudent_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                     <td class="Calibri14N" style=" padding-left:20px;">
                        Select Component
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlComponent" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                    </td>
                    <td class="Calibri14N">
                        : <asp:DropDownList ID="ddlComponent" runat="server" Width="180px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                    <br />
                        <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/Resources/submit.jpg"  ValidationGroup="ADD" OnClick="btnSubmit_Click" Visible="false"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                    <br />
                        <iframe runat="server" id="ifCharacterCertificate" height="400px" width="80%"></iframe>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

