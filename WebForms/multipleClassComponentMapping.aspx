<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="multipleClassComponentMapping.aspx.cs" Inherits="WebForms_multipleClassComponentMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="0" width="98%">
                <tr>
                    <td colspan="4" valign="bottom" align="left" style="color: #FFAB60; font-family: Calibri; letter-spacing: 1px; font-weight: bold; font-size: 16px; border-bottom: solid 2px #FFAB60;">
                        <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60; border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px; width: 315px; height: 25px; padding-left: 8px;">
                            MULTIPLE CLASS COMPONENT MAPPING
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table cellpadding="0" cellspacing="0" border="0" width="80%">
                            <tr>
                                <td colspan="4">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="Calibri14N" style="padding-left: 10px;">Component
                                    <asp:RequiredFieldValidator ID="V3" runat="server" ControlToValidate="ddlSelectComponent" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                </td>
                                <td class="Calibri14N" style="height: 35px;">:
                                    <asp:DropDownList ID="ddlSelectComponent" runat="server" Width="150px" OnSelectedIndexChanged="ddlSelectComponent_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <td class="Calibri14N" style="padding-left: 20px;">Amount
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSelectAmount" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                </td>
                                <td class="Calibri14N">
                                    <asp:UpdatePanel ID="up1" runat="server">
                                        <ContentTemplate>
                                            :
                                            <asp:DropDownList ID="ddlSelectAmount" runat="server" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectAmount_SelectedIndexChanged"></asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlSelectComponent" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td class="Calibri14N" style="padding-left: 10px;">Applicable Date
                                </td>
                                <td class="Calibri14N">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>
                                            :
                                            <asp:DropDownList ID="ddlApplicableDate" runat="server" Width="120px"></asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlSelectComponent" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:ListView ID="lvClassList" runat="server" ItemPlaceholderID="td2" GroupItemCount="12" GroupPlaceholderID="tr2" EnableViewState="true">
                                    <LayoutTemplate>
                                        <table id="t2" runat="server" border="1" style="border-color: #FFAB60" width="100%">
                                            <tr id="tr2" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <GroupTemplate>
                                        <tr>
                                            <td id="td2" runat="server"></td>
                                        </tr>
                                    </GroupTemplate>
                                    <ItemTemplate>
                                        <td style="background-color: white; Color: Black; font-family: Calibri; font-size: 11px; font-weight: normal;">
                                            <asp:CheckBox ID="cbField" runat="server" Text='<%#Eval("CLASS") %>' />
                                            <asp:HiddenField ID="hfClassCode" runat="server" Value='<%#Eval("CLASS_CODE") %>' />
                                        </td>
                                    </ItemTemplate>
                                </asp:ListView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlSelectComponent" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                    <br />
                        <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/Resources/submit.jpg"  ValidationGroup="ADD" OnClick="btnSubmit_Click"/>
                        <ajax:ConfirmButtonExtender ID="CE1" ConfirmText="This will update all previous mappings. Are you sure you want to continue !!!." TargetControlID="btnSubmit" runat="server">
                        </ajax:ConfirmButtonExtender>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

