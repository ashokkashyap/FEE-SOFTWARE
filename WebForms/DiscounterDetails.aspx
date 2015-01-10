<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="DiscounterDetails.aspx.cs" Inherits="WebForms_DiscounterDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" width="90%" border="0" style="border-color: Black;">
                <tr>
                    <td valign="bottom" colspan="4" align="left" style="color: #FFAB60; font-family: Calibri; letter-spacing: 1px; font-weight: bold; font-size: 18px; border-bottom: solid 2px #FFAB60;">
                        <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60; border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px; width: 235px; height: 25px; padding-left: 8px;">
                            ADD/UPDATE DISCOUNTER
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 50%" class="Calibri14N">
                        <div style="border: solid 2px #FFAB60; border-radius: 8px;" align="center">
                            <div style="background-color: #FFFF99; border-top-left-radius: 8px; color: #FFAB60; border-top-right-radius: 8px; height: 25px; padding-top: 5px; font-weight: bold;" align="center">Add Discounter</div>
                            <br />
                            <table cellpadding="0" cellspacing="5" width="100%">
                                <tr>
                                    <td class="Calibri12N" style="width: 49%">Discounter Name
                                        <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtAddDiscounterName" ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="Calibri18B" style="width: 1%; vertical-align: top;">:</td>
                                    <td class="Calibri12N" style="width: 50%">
                                        <asp:TextBox ID="txtAddDiscounterName" runat="server" BorderWidth="1" BorderColor="Black" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri12N" style="width: 49%; vertical-align: top;">Description
                                    </td>
                                    <td class="Calibri18B" style="width: 1%; vertical-align: top;">:</td>
                                    <td class="Calibri12N" style="width: 50%">
                                        <asp:TextBox ID="txtAddDescription" runat="server" BorderWidth="1" BorderColor="Black" TextMode="MultiLine" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:ImageButton ID="btnInsert" runat="server" ImageUrl="~/Resources/submit.jpg" Height="20px" ValidationGroup="v1" OnClick="btnInsert_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <div style="border: solid 2px #FFAB60; border-radius: 8px;" align="center">
                            <div style="background-color: #FFFF99; border-top-left-radius: 8px; color: #FFAB60; border-top-right-radius: 8px; height: 25px; padding-top: 5px; font-weight: bold;" align="center">Update Discounter</div>
                            <br />
                            <table cellpadding="0" cellspacing="5" width="100%">
                                <tr>
                                    <td class="Calibri12N" style="width: 49%">Select Discounter
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSelectDiscounter" ErrorMessage="*" ForeColor="Red" ValidationGroup="v2"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="Calibri18B" style="width: 1%; vertical-align: top;">:</td>
                                    <td class="Calibri12N" style="width: 50%">
                                        <asp:DropDownList ID="ddlSelectDiscounter" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectDiscounter_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri12N" style="width: 49%">Discounter Name
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUDiscounterName" ErrorMessage="*" ForeColor="Red" ValidationGroup="v2"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="Calibri18B" style="width: 1%; vertical-align: top;">:</td>
                                    <td class="Calibri12N" style="width: 50%">
                                        <asp:UpdatePanel runat="server" ID="up1">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtUDiscounterName" runat="server" BorderWidth="1" BorderColor="Black" Width="150px"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlSelectDiscounter" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri12N" style="width: 49%; vertical-align: top;">Description
                                    </td>
                                    <td class="Calibri18B" style="width: 1%; vertical-align: top;">:</td>
                                    <td class="Calibri12N" style="width: 50%">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtUDiscounterDescription" runat="server" BorderWidth="1" BorderColor="Black" TextMode="MultiLine" Width="150px"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlSelectDiscounter" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:ImageButton ID="btnUpdateDetails" runat="server" ImageUrl="~/Resources/submit.jpg" Height="20px" ValidationGroup="v2" OnClick="btnUpdateDetails_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td colspan="2" style="width: 50%; vertical-align: top;">
                        <div style="border: solid 2px #FFAB60; border-radius: 8px;" align="center">
                            <div style="background-color: #FFFF99; border-top-left-radius: 8px; color: #FFAB60; border-top-right-radius: 8px; height: 25px; padding-top: 5px; font-weight: bold;" align="center">Discounter(s)</div>
                            <asp:Repeater ID="repDiscounterList" runat="server">
                                <ItemTemplate>
                                    <br />
                                    <asp:Panel ID="p1" runat="server" BackColor="#FFFF99" BorderWidth="1px" BorderColor="#FFAB60">
                                        <div align="left"><b>Name: </b><%#DataBinder.Eval(Container.DataItem, "NAME")%>.<br />
                                            <b>Description:</b> <%#DataBinder.Eval(Container.DataItem, "DESCRIPTION")%></div>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

