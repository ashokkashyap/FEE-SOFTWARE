<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="busstop_student_mapping.aspx.cs" Inherits="WebForms_busstop_student_mapping" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link rel="Stylesheet" href="../css/facilitator.css" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div align="left" style="height:80%">
    <span class="pageheading">Stop Student Mapping</span>
    <hr />
        <table>
            <tr>
                <td class="tablecoltext" align="left">
                    Bus Route:
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlRouteName" runat="server">
                    </asp:DropDownList> 
                </td>                
                <td align="center">
                    <asp:Button ID="btnStudentList" runat="server" Text="Student List" 
                        CssClass="buttonstyle" onclick="btnStudentList_Click"/>
                </td>
            </tr>            
        </table>
        <br />
        <asp:Panel ID="Panel1" runat="server">
            <table width="100%">                
                <tr>
                    <td colspan="5">
                        <br />
                        <asp:GridView ID="grdStudentlist" runat="server" AutoGenerateColumns="False" 
                            Width="700px" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="None" >
                            <FooterStyle BackColor="#CCCC99" />
                            <RowStyle HorizontalAlign="Center" BackColor="#F7F7DE" Font-Names="Calibri" />                            
                            <Columns>                                
                                <asp:TemplateField HeaderText="S No">
                                    <ItemTemplate><%# Container.DataItemIndex +1 %>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("student_id") %>'></asp:HiddenField>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="student_name" HeaderText="Name" ReadOnly="True" SortExpression="student_name" />
                                <asp:BoundField DataField="CLASS1" HeaderText="Class" ReadOnly="True" SortExpression="CLASS1" />
                                <asp:BoundField DataField="fname" HeaderText="Father Name" ReadOnly="True" SortExpression="fname" />
                                <asp:TemplateField HeaderText="Status">                                   
                                   <ItemTemplate>
                                       <asp:DropDownList ID="DropDownList1" runat="server">
                                       </asp:DropDownList>
                                   </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" BackColor="#F7F7DE" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle HorizontalAlign="Center" BackColor="#6B696B" Font-Bold="True" 
                                ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </td>                    
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit Details" 
                            CssClass="buttonstyle" Visible="false" onclick="btnSubmit_Click"/>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>

</asp:Content>

