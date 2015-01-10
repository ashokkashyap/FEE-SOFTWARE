<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true"
    CodeFile="DeleteFeeDetails.aspx.cs" Inherits="WebForms_DeleteFeeDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="0" cellspacing="0" width="90%">
        <tr>
            <td valign="bottom" align="left" style="color: #FFAB60; font-family: Calibri; letter-spacing: 1px;
                font-weight: bold; font-size: 18px; border-bottom: solid 2px #FFAB60;">
                <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60;
                    border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px;
                    width: 210px; height: 25px; padding-left: 8px;">
                   Delete Fee Details
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                <tr><td class="Calibri14N" style=" padding-left:20px; width:150px;">Admission Number:</td><td>
                    <asp:TextBox ID="txtAdmissionNo" runat="server"></asp:TextBox></td><td>
                        <asp:Button ID="Btnsubmit" runat="server" Text="Submit" 
                            onclick="Btnsubmit_Click" /></td></tr>


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
                            CellPadding="4" ForeColor="Black" GridLines="Both">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="SNO">
                                    <ItemTemplate>
                                      
                                        <asp:HiddenField ID="hdn" runat="server" Value='<%#Eval("SCROLL_NO") %>' />
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="SCROLL NO" DataField="SCROLL_NO" />
                                <asp:BoundField HeaderText="MONTH NAME" DataField="mname" />
                                <asp:BoundField HeaderText="AMOUNT" DataField="AMT" />
                                <asp:BoundField HeaderText="Payment Date" DataField="PAID_DATE" dataformatstring="{0:MMMM d, yyyy}" />

                                
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
                <tr><td align="center" colspan="4">
                    <asp:Button ID="Button1" runat="server" Text="Delete Details" onclick="Button1_Click" /></td></tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
