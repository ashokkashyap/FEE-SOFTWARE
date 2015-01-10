<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true"
    CodeFile="PaidFeeDetails.aspx.cs" Inherits="WebForms_PaidFeeDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                            CellPadding="4" ForeColor="Black" GridLines="Both">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="SNO">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfID" runat="server" Value='<%#Eval("ID") %>' />
                                        <asp:HiddenField ID="hfSTUDENT_ID" runat="server" Value='<%#Eval("STUDENT_ID") %>' />
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Payment Date" DataField="PAID_DATE" />
                                <asp:BoundField HeaderText="Amount" DataField="AMOUNT_PAID" />
                                  <asp:BoundField HeaderText="Rno" DataField="Rno" />
                                <asp:BoundField HeaderText="Payment Mode" DataField="MODE" />
                                <asp:BoundField HeaderText="Fine" DataField="FINE" />
                                <asp:BoundField HeaderText="Months" DataField="Months" />
                                <asp:TemplateField HeaderText="Check For Delete">                                   
                                   <ItemTemplate>
                                       <asp:CheckBox ID="CheckBox1" runat="server" />
                                   </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Submit">                                   
                                   <ItemTemplate>
                                       <asp:Button ID="btnSubmit" runat="server" OnClientClick="return GetConfirmation('jkhuy');"  OnClick="Button1_OnClick" Text="Delete" />
                                       <asp:Button ID="Button1" runat="server"  OnClick="Button2_OnClick" Text="Update" />
                                   </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>


                             <FooterStyle BackColor="#CCCC99" />
                                            <HeaderStyle BackColor="#E6E6FA" Font-Bold="True" ForeColor="black" />
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
                        <asp:Button ID="btnUpdate" Visible="false" runat="server" Text="Update" OnClick="btnUpdate_Click">
                        </asp:Button>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 50%" valign="top">
                        <asp:Panel ID="pnlMaindetails" runat="server" Visible="false">
                            <table cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td class="Calibri14N" style="background: #FFFF99; color: #FFAB60; width: 80px;">
                                        Fee Date :
                                    </td>
                                    <td class="Calibri14N" style="background: #FFFF99; color: Black;">
                                        <asp:TextBox ID="txtFeesDate" runat="server" Width="200px" Text="0"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalE" runat="server" Format="dd-MMMM-yyyy" PopupButtonID="txtFeesDate" TargetControlID="txtFeesDate" ></ajax:CalendarExtender>
                                    </td>
                                    <td class="Calibri14N" style="background: #FFFF99; color: Black;">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:GridView ID="gvFeeAmountDetails" runat="server" AutoGenerateColumns="False"
                            Width="100%" BackColor="White" BorderColor="#FFAB60" BorderStyle="None" BorderWidth="1px"
                            CellPadding="4" ForeColor="Black" GridLines="Both">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="SNO">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfCOMPONENT_ID" runat="server" Value='<%#Eval("Component_ID") %>' />
                                        <asp:HiddenField ID="hfFREQ" runat="server" Value='<%#Eval("FREQ") %>' />
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="COMPONENT_NAME" HeaderText="Component" />
                                <asp:BoundField DataField="AMOUNT_PAYBLE" HeaderText="Amount" />
                                <asp:BoundField DataField="Previous" HeaderText="DUE" />
                                <%--<asp:BoundField DataField="Paid" HeaderText="Paid" />--%>
                                <asp:TemplateField HeaderText="Discount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiscount" runat="server" Text='<%#Eval("Discount") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="A.Discount">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtADiscount" Width="50px" runat="server" Text='<%#Eval("ADISCOUNT") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Paid">
                                    <ItemTemplate>
                                        <asp:UpdatePanel ID="upnl" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtPayment" Width="50px" runat="server" Text='<%#Eval("Paid") %>'></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtADiscount" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ItemTemplate>
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
                    <td colspan="2" style="width: 50%;" valign="top">
                        <asp:Panel ID="pnlExtraDetails" runat="server" Visible="false">
                            <table cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td class="Calibri14N" style="background: #FFFF99; color: #FFAB60; width: 60px;">
                                        Fine :
                                    </td>
                                    <td class="Calibri14N" style="background: #FFFF99; color: Black;">
                                        <asp:TextBox ID="txtFineAmount" runat="server" Width="100px" Text="0"></asp:TextBox>
                                    </td>
                                    <td class="Calibri14N" style="background: #FFFF99; color: Black;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="background:#FFFF99; color:#FFAB60; width:120px;">RE-Adm. Charges : </td>
                                    <td class="Calibri14N" style="background:#FFFF99; color:Black;"><asp:Label ID="lblReAdmCharges" runat="server" Text="0"></asp:Label> </td>
                                    <td class="Calibri14N" style="background:#FFFF99; color:Black;"></td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;" colspan="3">
                                        <asp:UpdatePanel ID="uup" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;" colspan="3">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTotalDiscount" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;" colspan="3">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTotalADiscount" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;" colspan="3">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTotalToBePaid" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;" colspan="3">
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTotalFine" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;" colspan="3">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTotalPaid" runat="server" Text=""></asp:Label>
                                                <asp:HiddenField ID="hfTotalAmountPaid" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left:10px;" colspan="3">
                                        <asp:Button ID="btnVerify" Text="Verify" runat="server" Visible="false" OnClick="btnVerify_Click"/>
                                        <asp:Button ID="btnReset" Text="Reset" runat="server"  Visible="false" OnClick="btnReset_Click"/>
                                        <asp:Button ID="btnSubmit" Text="Submit" runat="server"  Visible="false" OnClick="btnSubmit_Click"/>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
