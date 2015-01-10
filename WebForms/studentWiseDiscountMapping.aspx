<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true"
    CodeFile="studentWiseDiscountMapping.aspx.cs" Inherits="WebForms_studentWiseDiscountMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" width="90%" border="0" style="border-color: Black;">
                <tr>
                    <td valign="bottom" colspan="4" align="left" style="color: #FFAB60; font-family: Calibri; letter-spacing: 1px; font-weight: bold; font-size: 18px; border-bottom: solid 2px #FFAB60;">
                        <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60; border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px; width: 335px; height: 25px; padding-left: 8px;">
                            STUDENT-WISE DISCOUNT MAPPING
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="Calibri14N" style="padding-left: 15px; border-bottom: 1px ridge grey; width: 23%; letter-spacing: 1px;">Admission no
                        <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtAdmissionNo"
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>
                    </td>
                    <td style="border-bottom: 1px ridge grey;">:
                        <asp:TextBox ID="txtAdmissionNo" runat="server" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="btnGetDetails" runat="server" OnClick="btnGetDetails_Click"
                            ValidationGroup="v1" Height="18px" Width="18px" ImageUrl="~/Resources/search.png" />
                    </td>
                    <td colspan="2" style="width: 50%"></td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 40%;" valign="top">
                        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>--%>
                        <asp:Panel ID="pnlStudentDetails" runat="server" BorderStyle="Dotted" BorderColor="Black"
                            BorderWidth="1px">
                            <table cellpadding="0" cellspacing="5" width="90%">
                                <tr>
                                    <td class="Calibri12N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 50%">Name
                                                <asp:Label ID="lblStudentID" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                    <td class="Calibri12N" style="border-bottom: 1px ridge grey; width: 50%; color: Blue;">:
                                                <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri12N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 50%;">Class
                                    </td>
                                    <td class="Calibri12N" style="border-bottom: 1px ridge grey; width: 50%; color: Blue;">:
                                                <asp:Label ID="lblClass" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri12N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 50%">Father Name
                                    </td>
                                    <td class="Calibri12N" style="border-bottom: 1px ridge grey; width: 50%; color: Blue;">:
                                                <asp:Label ID="lblFatherName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri12N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 50%;">Mother Name
                                    </td>
                                    <td class="Calibri12N" style="border-bottom: 1px ridge grey; width: 50%; color: Blue;">:
                                                <asp:Label ID="lblMotherName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri12N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 50%">Admission No
                                    </td>
                                    <td class="Calibri12N" style="border-bottom: 1px ridge grey; width: 50%; color: Blue;">:
                                                <asp:Label ID="lblAdmissionNo" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri12N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 50%;">Address
                                    </td>
                                    <td class="Calibri12N" style="border-bottom: 1px ridge grey; width: 50%; color: Blue;">:
                                                <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <%--</ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGetDetails" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>--%>
                    </td>
                    <td colspan="2" style="width: 60%;" valign="top">
                        <%--<asp:UpdatePanel ID="upnl1" runat="server">
                            <ContentTemplate>--%>
                        <asp:Panel ID="pnlComponentDetails" runat="server" BorderStyle="Dotted" BorderColor="Black"
                            BorderWidth="1px">
                            <table cellpadding="0" cellspacing="5" width="90%">
                                <tr>
                                    <td class="Calibri12N" style="padding-left: 10px;" colspan="2">
                                        <asp:GridView ID="gvComponents" runat="server" AutoGenerateColumns="False" Width="100%"
                                            BackColor="White" BorderColor="#FFAB60" BorderStyle="None" BorderWidth="1px"
                                            CellPadding="4" ForeColor="Black" GridLines="Both">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNO">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfCOMPONENT_ID" runat="server" Value='<%#Eval("COMPONENT_ID") %>' />
                                                        <asp:HiddenField ID="hfCOMPONENT_FREQUENCY" runat="server" Value='<%#Eval("COMPONENT_FREQUENCY") %>' />
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="COMPONENT_NAME" HeaderText="Component" />
                                                <asp:BoundField DataField="AMOUNT_PAYBLE" HeaderText="Amount" />
                                                <asp:TemplateField HeaderText="Discount">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfID" runat="server" Value='<%#Eval("ID") %>' />
                                                        <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Applicable Date">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlApplicableDate" runat="server">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Discounter">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlSelectDiscounter" runat="server">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payable">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel ID="upanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblPayableAmount" runat="server"></asp:Label>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlStatus" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#CCCC99" />
                                            <HeaderStyle BackColor="#FFFF99" Font-Bold="True" ForeColor="#FFAB60" />
                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                            <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                            <SortedAscendingHeaderStyle BackColor="#848384" />
                                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                            <SortedDescendingHeaderStyle BackColor="#575357" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <%--</ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGetDetails" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <br />
                        <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/Resources/submit.jpg"
                            OnClick="btnSubmit_Click" ValidationGroup="v1" />
                        <ajax:ConfirmButtonExtender ID="CE1" ConfirmText="This will update all previous mappings. Are you sure you want to continue !!!."
                            TargetControlID="btnSubmit" runat="server">
                        </ajax:ConfirmButtonExtender>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
