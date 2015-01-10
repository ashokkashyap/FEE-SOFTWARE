<%@ Page Title="" Language="C#" MasterPageFile="Site.master" AutoEventWireup="true"
    CodeFile="addComponentAmountDetails.aspx.cs" Inherits="WebForms_addComponentAmountDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jquery-1.2.6.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="0" width="90%">
                <tr>
                    <td valign="bottom" align="left" style="color: #FFAB60; font-family: Calibri; letter-spacing: 1px;
                        font-weight: bold; font-size: 18px; border-bottom: solid 2px #FFAB60;">
                        <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60;
                            border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px;
                            width: 315px; height: 25px; padding-left: 8px;">
                            ADD COMPONENT AMOUNT DETAILS
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="80%">
                            <tr>
                                <td colspan="5">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="Calibri14N" style="padding-left: 10px;">
                                    Component
                                    <asp:RequiredFieldValidator ID="V3" runat="server" ControlToValidate="ddlSelectComponent"
                                        ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                </td>
                                <td class="Calibri14N">
                                    :
                                    <asp:DropDownList ID="ddlSelectComponent" runat="server" Width="150px" OnSelectedIndexChanged="ddlSelectComponent_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td class="Calibri14N" style="padding-left: 20px;">
                                    Amount
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAmount"
                                        ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                </td>
                                <td class="Calibri14N">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            :
                                            <asp:TextBox ID="txtAmount" runat="server" Width="100px"></asp:TextBox>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="center">
                                    <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/Resources/submit.jpg"
                                        ValidationGroup="ADD" OnClick="btnSubmit_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" class="Calibri14N">
                                    <br />
                                    <asp:UpdatePanel ID="up1" runat="server">
                                        <ContentTemplate>
                                            <asp:ListView ID="lvAmountDetails" runat="server" GroupPlaceholderID="row" ItemPlaceholderID="item"
                                                GroupItemCount="6">
                                                <LayoutTemplate>
                                                    <table id="Table1" cellpadding="0" cellspacing="5" runat="server">
                                                        <tr runat="server" id="row">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <GroupTemplate>
                                                    <tr id="Tr1" runat="server">
                                                        <td id="item" runat="server">
                                                            <%#Eval("COMPONENT_AMOUNT") %>
                                                        </td>
                                                    </tr>
                                                </GroupTemplate>
                                                <ItemTemplate>
                                                    <td id="item" runat="server">
                                                        <%#Eval("COMPONENT_AMOUNT") %>&nbsp;&nbsp;,&nbsp;&nbsp;
                                                    </td>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlSelectComponent" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
