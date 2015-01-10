﻿<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="Duplicate_Fee_Recept.aspx.cs" Inherits="WebForms_Duplicate_Fee_Recept" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="0" width="90%">
                <tr>
                    <td valign="bottom" align="left" style="color:#FFAB60; font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:18px; border-bottom:solid 2px #FFAB60;">
                        <div style="background-color:#FFFF99; border-top:2px solid #FFAB60; border-left:2px solid #FFAB60; border-right:2px solid #FFAB60; border-top-left-radius:25px 50px; border-top-right-radius:25px 50px; width:240px; height:25px; padding-left:8px;">
                            SEARCH STUDENT BY NAME
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <table cellpadding="0" cellspacing="5" width="80%">
                            <tr>
                                <td class="Calibri14N" align="left" style="width:15%">
                                    Name
                                    <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="A"></asp:RequiredFieldValidator>
                                </td>
                                <td class="Calibri14N" align="left" style="width:45%">
                                    : <asp:TextBox ID="txtName" runat="server" Width="90%"></asp:TextBox>
                                        <ajax:AutoCompleteExtender ID="txtNameExtender" runat="server" DelimiterCharacters="" Enabled="true" ServicePath="" ServiceMethod="GetCompletionListName" TargetControlID="txtName" UseContextKey="true" MinimumPrefixLength="1"></ajax:AutoCompleteExtender>
                                </td>
                                <td>
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
                                    <HeaderTemplate>D Receipt</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Resources/edit.jpg" Height="15px" Width="15px" OnClick="btnEdit_Click" />
                                        <asp:HiddenField ID="hfStudentID" runat="server" Value='<%#Eval("STUDENT_ID") %>' />
                                         <asp:HiddenField ID="hfdetailid" runat="server" Value='<%#Eval("id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderTemplate>Student</HeaderTemplate>
                                    <ItemTemplate>
                                       <asp:CheckBox ID="chkstudent" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>School</HeaderTemplate>
                                    <ItemTemplate>
                                       <asp:CheckBox ID="chkschool" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>Both</HeaderTemplate>
                                    <ItemTemplate>
                                       <asp:CheckBox ID="chkboth" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="name" HeaderText="Name" />
                                <asp:BoundField DataField="AMOUNT_PAID" HeaderText="Paid Amount" />
                                <asp:BoundField DataField="pay_date" HeaderText="Pay Date" />
                                <asp:BoundField DataField="MODE" HeaderText="Mode" />
                                <asp:BoundField DataField="CLASS" HeaderText="Class" />
                                
                               
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

