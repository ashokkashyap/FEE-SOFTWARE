<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="ClassWisePaidFeeDetails.aspx.cs" Inherits="WebForms_ClassWisePaidFeeDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style5
        {
            font-family: Calibri;
            font-size: 14px;
            font-weight: normal;
            font-style: normal;
            text-transform: none;
            color: #006600;
            width: 14%;
        }
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" width="90%" border="0" style="border-color: Black;">
                <tr>
                    <td valign="bottom" colspan="4" align="left" style="color: #FFAB60; font-family: Calibri;
                        letter-spacing: 1px; font-weight: bold; font-size: 18px; border-bottom: solid 2px #FFAB60;">
                        <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60;
                            border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px;
                            width: 257px; height: 25px; padding-left: 8px;">
                            Class Wise PAID FEE DETAILS
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="style5" style="padding-left: 15px; letter-spacing: 1px;">
                        CLASS
                    <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="ddlClassList"
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="rv"></asp:RequiredFieldValidator>
                    </td>
                    <td class="style8">
                        :<asp:DropDownList ID="ddlClassList" runat="server" >
                        </asp:DropDownList>
&nbsp;</td>
                    <td class="Calibri14N" style="padding-left: 10px;
                        width: 50%" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td class="style5" style="padding-left: 15px; letter-spacing: 1px;">
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
                        width: 50%">
                        :<asp:TextBox ID="txtEndDate" runat="server" Width="37%"></asp:TextBox>
                        <ajax:CalendarExtender ID="ce2" runat="server" Format="D" PopupButtonID="txtEndDate" TargetControlID="txtEndDate"></ajax:CalendarExtender>
                        </td>
                </tr>
                <tr>
                    <td class="style5" style="padding-left: 15px; letter-spacing: 1px;">
                        &nbsp;</td>
                    <td class="style8">
                        &nbsp;</td>
                    <td class="style7" style="padding-left: 10px; ">
                        <asp:ImageButton ID="btnGetDetails" runat="server" Height="28px" 
                            ImageUrl="~/images/submit.jpg" ValidationGroup="rv" Width="83px" 
                            onclick="btnGetDetails_Click" />
                    </td>
                    <td class="Calibri14N" style="padding-left: 10px;
                        width: 50%">
                        <asp:ImageButton ID="dwnExlFile" runat="server" Visible="false" 
                            ImageUrl="~/images/excel-logo.jpg" Height="25px" Width="25px" 
                            onclick="dwnExlFile_Click" /> </td>
                </tr>
                <tr>
                    <td colspan="4" style="padding-top: 5px; padding-bottom: 5px; padding-left: 5px;">
                        <asp:GridView ID="gvRecords" runat="server" AutoGenerateColumns="False" Width="100%"
                            BackColor="White" BorderColor="#FFAB60" BorderStyle="None" BorderWidth="1px"
                            CellPadding="4" ForeColor="Black" GridLines="Both">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="SNO">
                                    <ItemTemplate>
                                    
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="ADMISSION NO" DataField="STUDENT_REGISTRATION_NBR" />
                                <asp:BoundField HeaderText="STUDENT NAME" DataField="NAME" />
                                <asp:BoundField HeaderText="PAYMENT DATE" DataField="PAID_DATE" />
                                <asp:BoundField HeaderText="AMOUNT" DataField="AMOUNT_PAID" />
                                <asp:BoundField HeaderText="PAYMENT MODE" DataField="PAYMENT_MODE" />
                                <asp:BoundField HeaderText="FINE" DataField="FINE" />
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
                    <td colspan="4" style="padding-top: 5px; padding-bottom: 5px; padding-left: 5px;">
                        &nbsp;</td>
                </tr>
                
            </table>
        </asp:Panel>
    </div>
</asp:Content>

