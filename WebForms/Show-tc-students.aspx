<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="Show-tc-students.aspx.cs" Inherits="WebForms_Show_tc_students" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
<tr>
<td>
Select Class
    <asp:DropDownList ID="ddlcls" runat="server" AutoPostBack="True" 
        onselectedindexchanged="ddlcls_SelectedIndexChanged">
    </asp:DropDownList>

</td>

</tr>
<tr>
<td>  
    <asp:GridView ID="grddetail" runat="server" AutoGenerateColumns="true" BackColor="#CCCCCC"
                            BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2"
                            ForeColor="Black" Width="100%">
                            <FooterStyle BackColor="#CCCCCC" />
                            <RowStyle BackColor="White" BorderWidth="0" HorizontalAlign="Center" 
                                Font-Bold="True" Font-Names="Arial" Font-Size="14px" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>                                                
                    </Columns>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Left" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" 
                        HorizontalAlign="Center" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                
            </asp:GridView>
</td>
</tr>
<tr>
<td> 


    <asp:Button ID="btnsubmit" runat="server" Text="Submit" 
        onclick="btnsubmit_Click" /></td>

</tr>
</table>
</asp:Content>

