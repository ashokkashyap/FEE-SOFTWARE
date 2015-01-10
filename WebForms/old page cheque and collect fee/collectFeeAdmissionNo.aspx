<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="collectFeeAdmissionNo.aspx.cs" Inherits="WebForms_collectFeeAdmissionNo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" width="90%" border="0" style="border-color: Black;">
                <tr>
                    <td valign="bottom" colspan="4" align="left" style="color: #FFAB60; font-family: Calibri; letter-spacing: 1px; font-weight: bold; font-size: 18px; border-bottom: solid 2px #FFAB60;">
                        <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60; border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px; width: 120px; height: 25px; padding-left: 8px;">
                            COLLECT FEE
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="Calibri14N" style="padding-left: 15px; border-bottom: 1px ridge grey; width: 23%; letter-spacing: 1px;">Admission no
                        <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtAdmissionNo" ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>
                    </td>
                    <td style="border-bottom: 1px ridge grey;">:
                        <asp:TextBox ID="txtAdmissionNo" runat="server" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="btnGetDetails" runat="server" OnClick="btnGetDetails_Click" ValidationGroup="v1" Height="18px" Width="18px" ImageUrl="~/Resources/search.png" />
                          <asp:RadioButtonList ID="radiobtnNEW_old" runat="server" 
                RepeatDirection="Horizontal" RepeatLayout="Table">
                <asp:ListItem Text="NEW" Selected=True Value="NEW"></asp:ListItem>
                <asp:ListItem Text="OLD" Value="OLD"></asp:ListItem>
                
            </asp:RadioButtonList>
                    </td>
                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 25%">Calculation Date 
                    </td>
                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 25%;">:
                        <asp:TextBox ID="txtCalculationDate" runat="server" Width="150px"></asp:TextBox>
                        <ajax:CalendarExtender ID="CalE" runat="server" Format="dd-MMMM-yyyy" PopupButtonID="txtCalculationDate" TargetControlID="txtCalculationDate"></ajax:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td class="Calibri14N" style="padding-left: 15px; border-bottom: 1px ridge grey; width: 23%;"></td>
                    <td class="Calibri14N" style="border-bottom: 1px ridge grey;">For the period of
                    </td>
                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 25%">
                        <asp:DropDownList ID="ddlStartMonth" runat="server" Width="80%"></asp:DropDownList>
                    </td>
                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 25%;">-
                        <asp:DropDownList ID="ddlEndMonth" runat="server" Width="80%"></asp:DropDownList>
                        <asp:CompareValidator ID="compare1" runat="server" ControlToValidate="ddlEndMonth" ControlToCompare="ddlStartMonth" Type="Date" ErrorMessage="*" ForeColor="Red" ValidationGroup="v1" Operator="GreaterThanEqual"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 40%;" valign="top">
                        <asp:Panel ID="pnlStudentDetails" runat="server" BorderStyle="Dotted" BorderColor="Black" BorderWidth="1px">
                            <table cellpadding="0" cellspacing="5" width="99%">
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 40%">Name
                                        <asp:Label ID="lblStudentID" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 60%; color: Blue;">:
                                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;">Class
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; color: Blue;">:
                                        <asp:Label ID="lblClass" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;">Father Name 
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; color: Blue;">:
                                        <asp:Label ID="lblFatherName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;">Mother Name
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; color: Blue;">:
                                        <asp:Label ID="lblMotherName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;">Admission No 
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; color: Blue;">:
                                        <asp:Label ID="lblAdmissionNo" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;">Address
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; color: Blue;">:
                                        <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="Calibri16N" style="border-bottom: 1px ridge grey; color: Blue;" align="center">-- Other Fee Details --
                                    </td>
                                </tr>
                                <%--<tr>
                                <td class="Calibri14N" style="padding-left:10px; border-bottom:1px ridge grey; vertical-align:top;">
                                    Fine Amount
                                </td>
                                <td class="Calibri14N" style="border-bottom:1px ridge grey; vertical-align:top;">
                                    : <asp:TextBox ID="txtFineAmount" runat="server" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="Calibri14N" style="padding-left:10px; border-bottom:1px ridge grey; vertical-align:top;">
                                    Fine Details
                                </td>
                                <td class="Calibri14N" style="border-bottom:1px ridge grey; vertical-align:top;">
                                    : <asp:TextBox ID="txtFineDetails" runat="server" Width="90%" Height="50px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>--%>
                                <%-- <tr>
                                <td class="Calibri14N" style="padding-left:10px; border-bottom:1px ridge grey; vertical-align:top;">
                                    Discount Amount
                                </td>
                                <td class="Calibri14N" style="border-bottom:1px ridge grey; vertical-align:top;">
                                    : <asp:TextBox ID="txtDiscountAmount" runat="server" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="Calibri14N" style="padding-left:10px; border-bottom:1px ridge grey; vertical-align:top;">
                                    Discount Details
                                </td>
                                <td class="Calibri14N" style="border-bottom:1px ridge grey; vertical-align:top;">
                                    : <asp:TextBox ID="txtDiscountDetails" runat="server" Width="90%" Height="50px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>--%>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey; vertical-align: top;">Payment Mode
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; vertical-align: top;">:
                                        <asp:DropDownList ID="ddlSelectPaymentMode" runat="server">
                                            <asp:ListItem Text="CASH" Value="CASH" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="CHEQUE" Value="CHEQUE"></asp:ListItem>
                                            <asp:ListItem Text="NET BANKING" Value="NET BANKING"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 50%; vertical-align: top;">Cheque #
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 50%; vertical-align: top;">:
                                        <asp:TextBox ID="txtChequeNo" runat="server" Width="90%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 50%; vertical-align: top;">Cheque Date
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 50%; vertical-align: top;">:
                                        <asp:TextBox ID="txtChequeDate" runat="server" Width="90%"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalE2" runat="server" Format="dd-MMMM-yyyy" PopupButtonID="txtChequeDate" TargetControlID="txtChequeDate"></ajax:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 50%; vertical-align: top;">Bank Details
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 50%; vertical-align: top;">:
                                        <asp:TextBox ID="txtBankDetails" runat="server" Width="90%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td colspan="2" valign="top" style="width: 60%;">
                        <asp:Panel ID="pnlComponentDetails" runat="server" BorderStyle="Dotted" BorderColor="Black" BorderWidth="1px">
                            <table cellpadding="0" cellspacing="5" width="99%">
                                <tr>
                                    <td align="left" class="Calibri14N" style="padding-left: 10px; border: 1px solid #FFAB60; background-color: #FFAB60;">
                                        <table cellpadding="5" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="Calibri14N" style="background: #FFFF99; color: #FFAB60; width: 60px;">Scroll # : 
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtScrollingNumber" ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="Calibri14N" style="background: #FFFF99; color: Black;">
                                                    <asp:TextBox ID="txtScrollingNumber" runat="server" Width="60px"></asp:TextBox>
                                                </td>
                                                <td class="Calibri12N" style="background: #FFFF99; color: Black;">
                                                    <asp:Label ID="lblScrollingNumbers" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lblInfo" runat="server" Text="" ForeColor="Black"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" colspan="3">
                                        <asp:GridView ID="gvFeeAmountDetails" runat="server" AutoGenerateColumns="False"
                                            Width="100%" BackColor="White" BorderColor="#FFAB60" BorderStyle="None"
                                            BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Both">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNO">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfCOMPONENT_ID" runat="server" Value='<%#Eval("Component_ID") %>' />
                                                        <asp:HiddenField ID="hfFREQ" runat="server" Value='<%#Eval("FREQ") %>' />
                                                        <%--<asp:HiddenField ID="hfSTUDENT_ID" runat="server" Value='<%#Eval("varStudentID") %>' />--%>
                                                        <%--<asp:HiddenField ID="hfID" runat="server" Value='<%#Eval("ID") %>' />--%>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="COMPONENT_NAME" HeaderText="Component" />
                                                <asp:BoundField DataField="AMOUNT_PAYBLE" HeaderText="Amount" />
                                                <asp:BoundField DataField="PREVIOUS" HeaderText="Due" />
                                                <asp:TemplateField HeaderText="Discount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiscount" runat="server" Text='<%#Eval("Discount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="A.Discount">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtADiscount" Width="50px" runat="server" Text='<%#Eval("ADISCOUNT") %>' AutoPostBack="true" OnTextChanged="txtADiscount_TextChanged"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payment">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel ID="upnl" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtPayment" Width="50px" runat="server" Text='<%#Eval("ToBePaid") %>'></asp:TextBox>
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
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri14N" style="padding-left: 10px; border: 1px solid #FFAB60; background-color: #FFAB60;">
                                        <table cellpadding="5" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="Calibri14N" style="background: #FFFF99; color: #FFAB60; width: 120px;">Fine : </td>
                                                <td class="Calibri14N" style="background: #FFFF99; color: Black;">
                                                    <asp:TextBox ID="txtFineAmount" runat="server" Width="70px" Text="0"></asp:TextBox>
                                                </td>
                                                <td class="Calibri14N" style="background: #FFFF99; color: Black;"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri14N" style="padding-left: 10px; border: 1px solid #FFAB60; background-color: #FFAB60;">
                                        <table cellpadding="5" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="Calibri14N" style="background: #FFFF99; color: #FFAB60; width: 120px;">RE-Adm. Charges : </td>
                                                <td class="Calibri14N" style="background: #FFFF99; color: Black;">
                                                    <asp:TextBox ID="txtReAdmissionCharges" runat="server" Width="70px" Text="0"></asp:TextBox>
                                                </td>
                                                <td class="Calibri14N" style="background: #FFFF99; color: Black;"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;">
                                        <asp:UpdatePanel ID="uup" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTotalDue" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTotalDiscount" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTotalADiscount" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTotalToBePaid" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;">
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTotalFine" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTotalPaid" runat="server" Text=""></asp:Label>
                                                <asp:HiddenField ID="hfTotalAmountPaid" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;">
                                        <asp:Button ID="btnVerify" Text="Verify" runat="server" Visible="false" OnClick="btnVerify_Click" />
                                        <asp:Button ID="btnReset" Text="Reset" runat="server" Visible="false" OnClick="btnReset_Click" />
                                        <asp:Button ID="btnSubmit" Text="Submit" runat="server" Visible="false" OnClick="btnSubmit_Click" />
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
