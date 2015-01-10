<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="UpdateFeemode.aspx.cs" Inherits="WebForms_UpdateFeemode" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
                            width: 155px; height: 25px; padding-left: 8px;">
                            PAID FEE DETAILS
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="Calibri14N" style="padding-left: 15px; border-bottom: 1px ridge grey;
                        width: 23%; letter-spacing: 1px;">
                        Admission no
                        <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtAdmissionNo"
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>
                    </td>
                    <td style="border-bottom: 1px ridge grey;">
                        :
                        <asp:TextBox ID="txtAdmissionNo" runat="server" Width="100px"></asp:TextBox>
                        <asp:ImageButton ID="btnGetDetails" OnClick="btnGetDetails_Click" runat="server"
                            ValidationGroup="v1" Height="18px" Width="18px" ImageUrl="~/Resources/search.png" />
                    </td>
                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                        width: 50%" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; background-color: #FFFF99; padding-top: 5px; padding-bottom: 5px;
                        padding-left: 5px;">
                        <span style="font-weight: bold; font-family: Calibri; font-size: 12px; color: Black;">
                            Name:</span>
                        <asp:Label ID="lblStudentID" runat="server" Visible="false"></asp:Label>
                        <span style="font-weight: bold; font-family: Calibri; font-size: 12px; color: Blue;">
                            <asp:Label ID="lblName" runat="server"></asp:Label></span>
                    </td>
                    <td style="width: 25%; background-color: #FFFF99; padding-top: 5px; padding-bottom: 5px;
                        padding-left: 5px;">
                        <span style="font-weight: bold; font-family: Calibri; font-size: 12px; color: Black;">
                            Class:</span> <span style="font-weight: bold; font-family: Calibri; font-size: 12px;
                                color: Blue;">
                                <asp:Label ID="lblClass" runat="server"></asp:Label></span>
                    </td>
                    <td style="width: 25%; background-color: #FFFF99; padding-top: 5px; padding-bottom: 5px;
                        padding-left: 5px;">
                        <span style="font-weight: bold; font-family: Calibri; font-size: 12px; color: Black;">
                            Father's Name:</span> <span style="font-weight: bold; font-family: Calibri; font-size: 12px;
                                color: Blue;">
                                <asp:Label ID="lblFatherName" runat="server"></asp:Label></span>
                    </td>
                    <td style="width: 25%; background-color: #FFFF99; padding-top: 5px; padding-bottom: 5px;
                        padding-left: 5px;">
                        <span style="font-weight: bold; font-family: Calibri; font-size: 12px; color: Black;">
                            Mother's Name:</span> <span style="font-weight: bold; font-family: Calibri; font-size: 12px;
                                color: Blue;">
                                <asp:Label ID="lblMotherName" runat="server"></asp:Label></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="padding-top: 5px; padding-bottom: 5px; padding-left: 5px;">
                        <asp:Label ID="lblNote" runat="server" Text="Note: Last Collection can only be updated."
                            ForeColor="#FFAB60"></asp:Label>
                        <asp:GridView ID="gvRecords" runat="server" AutoGenerateColumns="False" Width="100%"
                            BackColor="White" BorderColor="#FFAB60" BorderStyle="None" BorderWidth="1px"
                            CellPadding="4" ForeColor="Black" GridLines="Both" 
                           >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                            <asp:TemplateField>
                            
                             <HeaderTemplate>update</HeaderTemplate>
                                    <ItemTemplate>
                                        
                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/fee.png" Height="15px" Width="15px" OnClick="btnEdit_Click" />
                                    </ItemTemplate>
                            
                            </asp:TemplateField>
                                <asp:TemplateField HeaderText="SNO">
                                    <ItemTemplate>

                                        <asp:HiddenField ID="hfID" runat="server" Value='<%#Eval("ID") %>' />
                                        <asp:HiddenField ID="hfSTUDENT_ID" runat="server" Value='<%#Eval("STUDENT_ID") %>' />
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Payment Date" DataField="PAID_DATE" />
                                <asp:BoundField HeaderText="Amount" DataField="AMOUNT_PAID" />
                                <asp:BoundField HeaderText="Payment Mode" DataField="MODE" />
                                <asp:BoundField HeaderText="Fine" DataField="FINE" />
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
                       
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlupdate" runat="server">
        <table>
        <tr>
        <td style="font-size:14px; font-weight:bold">
        Mode
        </td>
        <td style="font-size:14px; font-weight:bold ; width:50px" >

        </td>
        <td>
            <asp:DropDownList ID="ddlmode" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddlmode_SelectedIndexChanged">
                <asp:ListItem>--SELECT--</asp:ListItem>
                <asp:ListItem>CASH</asp:ListItem>
                <asp:ListItem>CHEQUE</asp:ListItem>
            </asp:DropDownList>
        </td>
        </tr>
        <tr>
        <td style="font-size:14px; font-weight:bold">
        Cheque date
        </td>
        <td style="font-size:14px; font-weight:bold ; width:50px">

        </td>
        <td>
        
            <asp:TextBox ID="txtchequedate" runat="server"></asp:TextBox></td>
             <ajax:CalendarExtender ID="CalE" runat="server" Format="yyyy-MM-dd" PopupButtonID="txtchequedate" TargetControlID="txtchequedate"></ajax:CalendarExtender>
        </tr>

        <tr>
        <td style="font-size:14px; font-weight:bold">
        Bank Detail
        </td>
        
        <td style="font-size:14px; font-weight:bold ; width:50px" >
        
        </td>
        <td>
        
            <asp:TextBox ID="txtbank" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
        <td></td>
        
        </tr>
         <tr>
        <td style="font-size:14px; font-weight:bold">
        Cheque No
        </td>
        
        <td style="font-size:14px; font-weight:bold ; width:50px" >
        
        </td>
        <td>
        
            <asp:TextBox ID="txtchkno" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
        <td></td>
        
        </tr>
        <tr>
        <td>
         <asp:Button ID="btnUpdate" runat="server" Text="Update" 
                            onclick="btnUpdate_Click" >
                        </asp:Button>
        </td>
        </tr>
        </table>
        
        </asp:Panel>
    </div>
</asp:Content>

