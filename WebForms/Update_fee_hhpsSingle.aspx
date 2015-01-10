<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Update_fee_hhpsSingle.aspx.cs" Inherits="WebForms_Update_fee_hhpsSingle" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style type="text/css">
        .style2
        {
            font-family: Calibri;
            font-size: 12px;
            font-weight: normal;
            font-style: normal;
            text-transform: none;
            color: #006600;
            width: 177px;
        }
    </style>
    <script src="../Scripts/jquery-1.2.6.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" width="90%" border="0" style="border-color: Black;">
                <tr>
                    <!--text-shadow: 2px 2px 2px #000-->
                    <td valign="bottom" colspan="4" align="left" style="color: #FFAB60; font-family: Calibri;
                        letter-spacing: 1px; font-weight: bold; font-size: 18px; border-bottom: solid 2px #FFAB60;">
                        <div style="background-color: #FFFF99; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60;
                            border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px;
                            width: 210px; height: 25px; padding-left: 8px;">
                            UPDATE COLLECTED FEE
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
                        <asp:ImageButton ID="btnGetDetails" runat="server" OnClick="btnGetDetails_Click"
                            ValidationGroup="v1" Height="18px" Width="18px" ImageUrl="~/Resources/search.png" />
                    </td>
                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                        width: 15%">
                        Date
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSelectDate" InitialValue=""
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 25%;">
                        :
                        <asp:DropDownList ID="ddlSelectDate" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectDate_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 40%;" valign="top">
                        <asp:Panel ID="pnlStudentDetails" runat="server" BorderStyle="Dotted" BorderColor="Black"
                            BorderWidth="1px">
                            <table cellpadding="0" cellspacing="5" width="99%">
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                        width: 40%">
                                        Name
                                        <asp:Label ID="lblStudentID" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 60%; color: Blue;">
                                        :
                                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;">
                                        Class
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; color: Blue;">
                                        :
                                        <asp:Label ID="lblClass" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;">
                                        Father Name
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; color: Blue;">
                                        :
                                        <asp:Label ID="lblFatherName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;">
                                        Mother Name
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; color: Blue;">
                                        :
                                        <asp:Label ID="lblMotherName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;">
                                        Admission No
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; color: Blue;">
                                        :
                                        <asp:Label ID="lblAdmissionNo" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;">
                                        # of Communication
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; color: Blue;">
                                        :
                                        <asp:Label ID="lblNoOfCommunication" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;">
                                        Address
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; color: Blue;">
                                        :
                                        <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="Calibri16N" style="border-bottom: 1px ridge grey; color: Blue;"
                                        align="center">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                        vertical-align: top;">
                                        MODE</td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; vertical-align: top;">
                                        <asp:DropDownList Width="150px" ID="ddlmode" runat="server">
                                            <asp:ListItem>CASH</asp:ListItem>
                                            <asp:ListItem Value="CHEQUE"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                        vertical-align: top;">
                                        CHEQUE NO</td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; vertical-align: top;">
                                        <asp:TextBox ID="txtchkno" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                        vertical-align: top;">
                                        CHEQUE DATE</td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; vertical-align: top;">
                                        <asp:TextBox ID="txtchkdte" runat="server"></asp:TextBox>
                                         <ajax:CalendarExtender ID="CalendarExtender1" runat="server" 
                                     Format="dd-MMMM-yyyy" PopupButtonID="txtchkdte" 
                                     TargetControlID="txtchkdte">
                                 </ajax:CalendarExtender></td>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                        vertical-align: top;">
                                        BANK DETAIL</td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; vertical-align: top;">
                                        <asp:TextBox ID="txtbankdetail" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                        vertical-align: top;">
                                        Fine</td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; vertical-align: top;">
                                        <asp:TextBox ID="txtfine" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                        width: 50%; vertical-align: top;">
                                        &nbsp;</td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 50%; vertical-align: top;">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                        width: 50%; vertical-align: top;">
                                        &nbsp;</td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 50%; vertical-align: top;">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                        width: 50%; vertical-align: top;">
                                        &nbsp;</td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 50%; vertical-align: top;">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td colspan="2" valign="top" style="width: 60%;">
                        <asp:Panel ID="pnlComponentDetails" runat="server" BorderStyle="Dotted" BorderColor="Black"
                            BorderWidth="1px">
                            <table cellpadding="0" cellspacing="5" width="99%">

                            <tr>
                            <td>Date Of Collection : <asp:TextBox ID="txtDate" runat="server" Width="75%" ></asp:TextBox>
                                 <ajax:CalendarExtender ID="CalendarExtender2" runat="server" 
                                     Format="dd-MMMM-yyyy" PopupButtonID="CalLeftOnDate" 
                                     TargetControlID="txtDate">
                                 </ajax:CalendarExtender></td>
                            </tr>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px;" colspan="2">
                                    <%--<asp:HiddenField ID="hfUNIQUE_ID" runat="server" Value="0" />--%>
                                        <asp:GridView ID="gvFeeAmountDetails" runat="server" AutoGenerateColumns="False"
                                            Width="100%" BackColor="White" BorderColor="#FFAB60" BorderStyle="None" BorderWidth="1px"
                                            CellPadding="4" ForeColor="Black" GridLines="Both">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNO">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfCOMPONENT_ID" runat="server" Value='<%#Eval("COMPONENT_ID") %>' />
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="COMPONENT_NAME" HeaderText="Component" ItemStyle-HorizontalAlign="Left" />
                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblDiscount" runat="server" Text="Rs."></asp:Label>
                                                                <asp:TextBox ID="txtAmount" Width="100px" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
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
                                    <td align="left" class="style2" style="padding-left: 10px;">
                                        
                                                <asp:Label ID="lblTotalAmount" runat="server" Text=""></asp:Label>
                                           
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="style2" style="padding-left: 10px;">
                                        <asp:Button ID="btnVerify" Text="Verify" runat="server" Visible="false" OnClick="btnVerify_Click" />
                                        <asp:Button ID="btnReset"  Text="Reset" runat="server" Visible="false" OnClick="btnReset_Click" />
                                        <asp:Button ID="btnSubmit" Text="Submit" runat="server" Visible="false" OnClick="btnSubmit_Click" ValidationGroup="v1"/>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
