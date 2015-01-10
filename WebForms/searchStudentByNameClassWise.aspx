<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="searchStudentByNameClassWise.aspx.cs" Inherits="WebForms_searchStudentByNameClassWise" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="0" width="90%">
                <tr>
                    <td valign="bottom" align="left" style="color:#FFAB60; font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:18px; border-bottom:solid 2px #FFAB60;">
                        <div style="background-color:#FFFF99; border-top:2px solid #FFAB60; border-left:2px solid #FFAB60; border-right:2px solid #FFAB60; border-top-left-radius:25px 50px; border-top-right-radius:25px 50px; width:320px; height:25px; padding-left:8px;">
                            SEARCH STUDENT BY NAME IN CLASS
                        </div>
                    </td>
                </tr>
                <tr><td><br /></td></tr>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="5" width="90%">
                            <tr>
                                <td align="left" class="Calibri14N" style="padding-left:15px; border-bottom:1px ridge grey; width:20%; letter-spacing:1px;">
                                    Select Class
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSelectClass" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="A"></asp:RequiredFieldValidator>
                                </td>
                                <td align="left" class="Calibri14N" style="border-bottom:1px ridge grey;">
                                    : <asp:DropDownList ID="ddlSelectClass" runat="server" Width="150px"></asp:DropDownList>
                                </td>
                                <td class="Calibri14N" align="left" style="border-bottom:1px ridge grey; width:15%;">
                                    Name
                                    <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="A"></asp:RequiredFieldValidator>
                                </td>
                                <td class="Calibri14N" align="left" style="border-bottom:1px ridge grey;">
                                    : <asp:TextBox ID="txtName" runat="server" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="btnGetDetails" runat="server" Text="Get Details" ValidationGroup="A" OnClick="btnGetDetails_Click"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:GridView ID="gvStudentDetails" runat="server" AutoGenerateColumns="False" 
                            Width="100%" BackColor="White" BorderColor="#FFAB60" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Both">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>Edit</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Resources/edit.jpg" Height="15px" Width="15px" OnClick="btnEdit_Click" />
                                        <asp:HiddenField ID="hfStudentID" runat="server" Value='<%#Eval("STUDENT_ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="STUDENT_REGISTRATION_NBR" HeaderText="Admission #" />
                                <asp:BoundField DataField="FIRST_NAME" HeaderText="First Name" />
                                <asp:BoundField DataField="MIDDLE_NAME" HeaderText="Middle Name" />
                                <asp:BoundField DataField="LAST_NAME" HeaderText="Last Name" />
                                <asp:BoundField DataField="CLASS" HeaderText="Class" />
                                <asp:BoundField DataField="FATHER_NAME" HeaderText="Father Name" />
                                <asp:BoundField DataField="MOTHER_NAME" HeaderText="Mother Name" />
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#FFFF99" Font-Bold="True" ForeColor="#FFAB60" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center"/>
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
    </div>
</asp:Content>

