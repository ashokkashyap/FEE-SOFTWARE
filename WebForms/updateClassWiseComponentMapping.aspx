<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="updateClassWiseComponentMapping.aspx.cs" Inherits="WebForms_updateClassWiseComponentMapping" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="0" width="98%">
                <tr>
                    <td colspan="4" valign="bottom" align="left" style="color:#FFAB60; font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:18px; border-bottom:solid 2px #FFAB60;">
                        <div style="background-color:#FFFF99; border-top:2px solid #FFAB60; border-left:2px solid #FFAB60; border-right:2px solid #FFAB60; border-top-left-radius:25px 50px; border-top-right-radius:25px 50px; width:278px; height:25px; padding-left:8px;">
                            UPDATE COMPONENT MAPPING
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table cellpadding="0" cellspacing="0" border="0" width="80%">
                            <tr><td colspan="4"><br/></td></tr>
                            <tr>
                                <td class="Calibri14N" style="padding-left:10px;">
                                    Component
                                    <%--<asp:RequiredFieldValidator ID="V3" runat="server" ControlToValidate="ddlSelectComponent" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td class="Calibri14N">
                                    : <asp:DropDownList ID="ddlSelectComponent" runat="server" Width="150px" OnSelectedIndexChanged="ddlSelectComponent_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <td class="Calibri14N" style=" padding-left:20px;">
                                    Amount
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSelectAmount" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                </td>
                                <td class="Calibri14N">
                                    <asp:UpdatePanel ID="up1" runat="server">
                                        <ContentTemplate>
                                            : <asp:DropDownList ID="ddlSelectAmount" runat="server" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectAmount_SelectedIndexChanged"></asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlSelectComponent" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr><td colspan="2"><br /></td></tr>
                            <tr>
                                <td class="Calibri14N" style="padding-left:10px;">
                                    Class
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSelectClass" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                </td>
                                <td class="Calibri14N">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            : <asp:DropDownList ID="ddlSelectClass" runat="server" Width="150px" OnSelectedIndexChanged="ddlSelectClass_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlSelectComponent" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td class="Calibri14N" style=" padding-left:20px;">
                                    Applicable Date
                                </td>
                                <td class="Calibri14N">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>
                                        : <asp:DropDownList ID="ddlApplicableDate" runat="server" Width="120px"></asp:DropDownList>
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
                    <td colspan="4" align="center">
                    <br />
                        <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/Resources/submit.jpg"  ValidationGroup="ADD" OnClick="btnSubmit_Click"/>
                        <ajax:ConfirmButtonExtender ID="CE1" ConfirmText="This will update all previous mappings. Are you sure you want to continue !!!." TargetControlID="btnSubmit" runat="server">
                        </ajax:ConfirmButtonExtender>
                    </td>
                </tr>
                <tr>
                    <td class="Calibri14N" colspan="2" style="padding-left:10px; padding-top:20px;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvMappedStudents" runat="server" AutoGenerateColumns="False" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="Both">
                                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>SNo.</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfStudentID" runat="server" value='<%#Eval("STUDENT_ID") %>' />
                                               <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="NAME" DataField="SNAME" />
                                        <asp:BoundField HeaderText="Admission #" DataField="STUDENT_REGISTRATION_NBR" />
                                        <asp:BoundField HeaderText="Father Name" DataField="FATHER_NAME" />
                                        <asp:BoundField HeaderText="Amount" DataField="AMOUNT_PAYBLE" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="cbHeader" runat="server" AutoPostBack="true" OnCheckedChanged="cbHeader_CheckedChanged"/>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="upgv" runat="server">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="cbRow" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="Tan" />
                                    <HeaderStyle BackColor="#FFFF99" Font-Bold="True" />
                                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                    <SortedAscendingCellStyle BackColor="#FAFAE7" />
                                    <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                                    <SortedDescendingCellStyle BackColor="#E1DB9C" />
                                    <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlSelectClass" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
