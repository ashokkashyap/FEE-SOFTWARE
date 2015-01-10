<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="busroute_student_mapping.aspx.cs" Inherits="WebForms_busroute_student_mapping" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link rel="Stylesheet" href="../css/facilitator.css" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div align="left" style="height:80%">
    <span class="pageheading">Route Student Mapping</span>
    <hr />
        <table width="100%" style="height:80%">
            <tr>                            
                <td class="tablecoltext">
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
                        RepeatDirection="Horizontal" AutoPostBack="true"
                        onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">
                        <asp:ListItem Text="Route To Student" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Student To Route" Value="2"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td class="tablecoltext" align="left">
                    Bus Route:
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlRouteName" runat="server" AutoPostBack="true" 
                        onselectedindexchanged="ddlRouteName_SelectedIndexChanged" 
                        style="height: 22px" >
                        <asp:ListItem Text="SELECT" Value="0"></asp:ListItem>
                    </asp:DropDownList> 
                </td>
                <td align="left">
                    Class
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlClassName" runat="server" Enabled="False">
                    </asp:DropDownList>
                </td>
                <td align="center">
                    <asp:Button ID="btnStudentList" runat="server" Text="Student List" CssClass="buttonstyle"
                        onclick="btnStudentList_Click" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server" Width="60%" Visible="false">
            <table width="100%">                
                <tr>
                    <td>
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
                                <asp:BoundField DataField="fname" HeaderText="Father Name" ReadOnly="True" SortExpression="fname" />
                                <asp:BoundField DataField="STUDENT_REGISTRATION_NBR" HeaderText="STUDENT REGISTRATION NBR" ReadOnly="True" SortExpression="STUDENT_REGISTRATION_NBR" />
                                <asp:TemplateField HeaderText="Status">                                   
                                   <ItemTemplate>
                                       <asp:CheckBox ID="CheckBox1" runat="server" />
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
                    <td>
                        <asp:Button ID="btnSubmit1" runat="server" Text="Submit Details" 
                            CssClass="buttonstyle" Visible="false" onclick="btnSubmit1_Click"/>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" Visible="false">
            <table width="100%">                
                <tr>
                    <td>
                        <br />
                        <asp:GridView ID="grdStudentlist1" runat="server" AutoGenerateColumns="False" 
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
                                <asp:BoundField DataField="fname" HeaderText="Father Name" ReadOnly="True" SortExpression="fname" />
                                <asp:BoundField DataField="STUDENT_REGISTRATION_NBR" HeaderText="STUDENT REGISTRATION NBR" ReadOnly="True" SortExpression="STUDENT_REGISTRATION_NBR" />
                                
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
                    <td>
                        <asp:Button ID="btnSubmit2" runat="server" Text="Submit Details" 
                            CssClass="buttonstyle" Visible="false" onclick="btnSubmit2_Click"/>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>

</asp:Content>

