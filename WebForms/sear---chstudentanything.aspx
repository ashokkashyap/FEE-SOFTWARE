<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="sear---chstudentanything.aspx.cs" Inherits="WebForms_sear___chstudentanything" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <asp:Button ID="Button1" runat="server" Text="Get Details" onclick="Button1_Click" />
    <asp:TextBox ID="TextBox1"
        runat="server"></asp:TextBox>
   

     <asp:GridView ID="GridView1" runat="server" BackColor="#CCCCCC"
                            BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2"
                            ForeColor="Black" Width="100%">
                            <FooterStyle BackColor="#CCCCCC" />
                            <RowStyle BackColor="White" BorderWidth="0" HorizontalAlign="Center" 
                                Font-Bold="True" Font-Names="Arial" Font-Size="14px" />
                   
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Left" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" 
                        HorizontalAlign="Center" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                
            </asp:GridView>
</asp:Content>

