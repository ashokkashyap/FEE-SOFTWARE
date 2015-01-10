<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="ComponentWisePaidFeeDetails.aspx.cs" Inherits="WebForms_ComponentWisePaidFeeDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .style7
        {
            font-family: Calibri;
            font-size: 14px;
            font-weight: normal;
            font-style: normal;
            text-transform: none;
            color: #006600;
            width: 15%;
        }
        .style8
        {
            width: 104px;
        }
    .style11
    {
    }
    .style12
    {
        width: 149px;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" width="90%" border="0" style="border-color: Black;">
                <tr>
                    <td valign="bottom" colspan="5" align="left" 
                        
                        
                        style="color: #FFAB60; font-family: Calibri;
                        letter-spacing: 1px; font-weight: bold; font-size: 18px; border-bottom: solid 2px #FFAB60;">
                        <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60;
                            border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px;
                            width: 310px; height: 25px; padding-left: 8px;">
                            Component Wise PAID FEE DETAILS
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="style7" style="padding-left: 15px; letter-spacing: 1px;">
                        COMPONENT
                    <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="ddlComponenetList"
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="rv"></asp:RequiredFieldValidator>
                    </td>
                    <td class="style8">
                        :<asp:DropDownList ID="ddlComponenetList" runat="server" AutoPostBack="true" SkinID="longest"
                            onselectedindexchanged="ddlComponenetList_SelectedIndexChanged" 
                            Height="22px" Width="147px">
                        </asp:DropDownList>
&nbsp;</td>
                    <td class="Calibri14N" style="padding-left: 10px;
                        width: 50%" colspan="3">
                    </td>
                </tr>
                <tr>
                    <td class="style7" style="padding-left: 15px; letter-spacing: 1px;">
                        START DATE
                        <asp:RequiredFieldValidator ID="rv2" runat="server" ControlToValidate="txtStrtDate" ErrorMessage="*" ForeColor="Red" ValidationGroup="rv"></asp:RequiredFieldValidator>
                        </td>
                    <td class="style8">
                        :<asp:TextBox ID="txtStrtDate" runat="server" ValidationGroup="rv2"></asp:TextBox>
                        <ajax:CalendarExtender ID="ce1" runat="server" Format="D" PopupButtonID="txtStrtDate" TargetControlID="txtStrtDate"></ajax:CalendarExtender>
                        </td>
                    <td class="style7" style="padding-left: 10px; ">
                        END DATE
                        <asp:RequiredFieldValidator ID="rv3" runat="server" ControlToValidate="txtEndDate" ErrorMessage="*" ForeColor="Red" ValidationGroup="rv"></asp:RequiredFieldValidator>
                        </td>
                    <td class="Calibri14N" style="padding-left: 10px;
                        width: 50%" colspan="2">
                        :<asp:TextBox ID="txtEndDate" runat="server" Width="37%"></asp:TextBox>
                        <ajax:CalendarExtender ID="ce2" runat="server" Format="D" PopupButtonID="txtEndDate" TargetControlID="txtEndDate"></ajax:CalendarExtender>
                        </td>
                </tr>
                <tr>
                    <td class="style7" style="padding-left: 15px; letter-spacing: 1px;">
                        &nbsp;</td>
                    <td class="style8">
                        &nbsp;</td>
                    <td class="style7" style="padding-left: 10px; ">
                        <asp:ImageButton ID="btnGetDetails" runat="server" Height="28px" 
                            ImageUrl="~/images/submit.jpg" ValidationGroup="rv" Width="83px" 
                            onclick="btnGetDetails_Click" />
                    </td>
                    <td class="Calibri14N" style="padding-left: 10px;
                        width: 50%" colspan="2">
                        <asp:ImageButton ID="dwnExlFile" runat="server" Visible="false" 
                            ImageUrl="~/images/excel-logo.jpg" Height="25px" Width="25px" 
                            onclick="dwnExlFile_Click" /> </td>
                </tr>
                <tr>
                    <td style="padding-left: 15px; letter-spacing: 1px;" class="style7">
                        &nbsp;</td>
                    <td class="style8">
                        &nbsp;</td>
                    <td class="style7" style="padding-left: 10px; ">
                        &nbsp;</td>
                    <td class="Calibri14N" style="padding-left: 10px;
                        width: 50%" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="5" 
                        style="padding-top: 5px; padding-bottom: 5px; padding-left: 5px;">
                        <asp:GridView ID="gvRecords" runat="server" AutoGenerateColumns="False" OnRowCreated="gvRecords_RowCreated"
                         BackColor="White" BorderColor="#FFAB60" BorderStyle="None" BorderWidth="1px" ShowFooter="true"
                            CellPadding="4" ForeColor="Black" GridLines="Both" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="SNO">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="STUDENT_REGISTRATION_NBR" 
                                    HeaderText="ADMISSION NO" />
                                <asp:BoundField DataField="name" HeaderText="STUDENT NAME" />
                                <asp:BoundField DataField="class" HeaderText="CLASS" />
                                <asp:BoundField DataField="paidDate" HeaderText="PAYMENT DATE" />
                               

                                
        <asp:TemplateField HeaderText="Total" SortExpression="AMOUNT">
               <ItemTemplate>
                     <asp:Label ID="lbltot" runat="server" Text="Label"></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label ID="lbltotal" runat="server" Text="Label12"></asp:Label>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label7" Text='<%# Eval("paid")%>' runat="server"></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="100px" />

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
                <tr>
                    <td style="padding-top: 5px; padding-bottom: 5px; padding-left: 5px;">
                        &nbsp;</td>
                    <td style="padding-top: 5px; padding-bottom: 5px; padding-left: 5px;">
                        &nbsp;</td>
                    <td style="padding-top: 5px; padding-bottom: 5px; padding-left: 5px;">
                        &nbsp;</td>
                    <td style="padding-top: 5px; padding-bottom: 5px; padding-left: 5px;" 
                        class="style12">
                        &nbsp;</td>
                    <td style="padding-top: 5px; padding-bottom: 5px; padding-left: 5px;" 
                        class="style11">
                        &nbsp;</td>
                </tr>
                
            </table>
        </asp:Panel>
    </div>

</asp:Content>

