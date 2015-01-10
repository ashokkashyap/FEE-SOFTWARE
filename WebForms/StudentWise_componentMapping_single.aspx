<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentWise_componentMapping_single.aspx.cs" Inherits="WebForms_StudentWise_componentMapping_single" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 900px;
            border: 3px solid #0DA9D0;
        }
        .modalPopup .header
        {
            background-color: #77BFC7;
            height: 30px;
            color: WHITE;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
            font-size:large;
        }
        .modalPopup .body
        {
            min-height: 500px;
            line-height: 20px;
            text-align: center;
            
        }
        .modalPopup .footer
        {
            padding: 3px;
             background-color: #BCC6CC;
        }
        .modalPopup .button
        {
            height: 23px;
            color: White;
            line-height: 23px;
            text-align: center;
            font-weight: bold;
            cursor: pointer;
            background-color: #9F9F9F;
            border: 1px solid #5C5C5C;
        }
        .modalPopup td
        {
            text-align:left;
        }
        .txtbxcomp
{
    width: 120px;
    height: 25px;
    background-color: #F9F9F9;
    border: 1px solid #CCCCCC;
    text-transform:uppercase;

    font-family: Calibri;
    font-size: 14px;
    border-right-width:1px;
    border-right-color:red;
    border-bottom-width:1px;
    border-bottom-color:red;
 }

.border
{
    width: 100px;
    height: 1px;
    background:#CCCCCC;
    margin-top:-1px;
    z-index: 100;
    position: relative;
}

.ddl
        {
            border:2px solid #7d6754;
            border-radius:5px;
            padding:3px;
            -webkit-appearance: none; 
            background-image:url('images/details.jpg');
            background-position:88px;
            background-repeat:no-repeat;
            text-indent: 0.01px;/*In Firefox*/
            text-overflow: '';/*In Firefox*/
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
      <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" width="90%" border="0" 
                style="border-color: Black; background-color: #F5F5F5;">
                <tr>
                    <td valign="bottom" colspan="3" align="left" 
                        style="color: #000000; font-family: Calibri;
                        letter-spacing: 1px; font-weight: bold; font-size: 18px; border-bottom: solid 2px #000000;">
                        <div style="background-color: #E6E6FA; border-top: 2px solid #FFAB60; border-left: 2px solid #FFAB60;
                            border-right: 2px solid #FFAB60; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px;
                            width: 335px; height: 25px; padding-left: 8px;">
                            STUDENT-WISE COMPONENT MAPPING
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
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
                        <asp:Button ID="Button1" runat="server" Text="Button" />
                    </td>
                    <td style="width: 50%">
                    </td>
                </tr>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <tr>
                    <td colspan="2" style="width: 50%;" valign="top">
                        <asp:Panel ID="pnlStudentDetails" runat="server" BorderStyle="Dotted" BorderColor="Black"
                            BorderWidth="1px">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table cellpadding="0" cellspacing="5" width="90%">
                                        <tr>
                                            <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                                width: 50%">
                                                Name
                                                <asp:Label ID="lblStudentID" runat="server" Text="" Visible="false"></asp:Label>
                                            </td>
                                            <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 50%; color: Blue;">
                                                :
                                                <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                                width: 50%;">
                                                Class
                                            </td>
                                            <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 50%; color: Blue;">
                                                :
                                                <asp:Label ID="lblClass" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                                width: 50%">
                                                Father Name
                                            </td>
                                            <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 50%; color: Blue;">
                                                :
                                                <asp:Label ID="lblFatherName" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                                width: 50%;">
                                                Mother Name
                                            </td>
                                            <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 50%; color: Blue;">
                                                :
                                                <asp:Label ID="lblMotherName" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                                width: 50%">
                                                Admission No
                                            </td>
                                            <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 50%; color: Blue;">
                                                :
                                                <asp:Label ID="lblAdmissionNo" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;
                                                width: 50%;">
                                                Address
                                            </td>
                                            <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 50%; color: Blue;">
                                                :
                                                <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnGetDetails" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                    <td valign="top" style="width: 50%;">
                        <asp:Panel ID="pnlComponentDetails" runat="server" BorderStyle="Dotted" BorderColor="Black"
                            BorderWidth="1px">
                            <table cellpadding="0" cellspacing="5" width="90%">
                                <%--                            <tr>
                                <td class="Calibri14N" style="padding-left:10px; border-bottom:1px ridge grey; width:50%">
                                    Applicable Date 
                                </td>
                                <td class="Calibri14N" style="border-bottom:1px ridge grey; width:50%;">
                                    : <asp:DropDownList ID="ddlApplicableDate" runat="server" Width="120px"></asp:DropDownList>
                                </td>
                            </tr>--%>
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px;">
                                        <asp:UpdatePanel ID="up1" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvComponents" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    BackColor="White" BorderColor="#FFAB60" BorderStyle="None" BorderWidth="1px"
                                                    CellPadding="4" ForeColor="Black" GridLines="Both">
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SNO">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hfCOMPONENT_ID" runat="server" Value='<%#Eval("COMPONENT_ID") %>' />
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="COMPONENT_NAME" HeaderText="Component" />
                                                        <asp:TemplateField HeaderText="Amount">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hfID" runat="server" />
                                                                <asp:DropDownList ID="ddlStatus" runat="server">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Applicable Date">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlApplicableDate" runat="server">
                                                                </asp:DropDownList>
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
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGetDetails" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <br />
                        <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/Resources/submit.jpg"
                            OnClick="btnSubmit_Click" ValidationGroup="v1" />
                        <ajax:ConfirmButtonExtender ID="CE1" ConfirmText="This will update all previous mappings. Are you sure you want to continue !!!."
                            TargetControlID="btnSubmit" runat="server">
                        </ajax:ConfirmButtonExtender>
                    </td>
                </tr>
            </table>
            
        </asp:Panel>

         <ajax:AnimationExtender id="MyExtender"  runat="server" TargetControlID="pnltarnsport">
  <Animations>
    <OnClick>
      <FadeOut Duration=".5" Fps="20" />
    </OnClick>
  </Animations>
</ajax:AnimationExtender>




                        <asp:HyperLink ID="HyperLink1" style="display:none" runat="server">HyperLink</asp:HyperLink>
                        <ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnltarnsport" TargetControlID="HyperLink1"
                            CancelControlID="btnClose" BackgroundCssClass="modalBackground">
                        </ajax:ModalPopupExtender>
                       <%-- <asp:Panel ID="Panel1" CssClass="modalPopup" runat="server">
                        <div class="header">
                            TRANSPORT DETAILS
    </div>
    <div class="body">
        <asp:Panel ID="pnldetail" runat="server"></asp:Panel>
    </div>
    <div class="footer" align="right">
        
        <asp:ImageButton ID="btnClose" ImageUrl="~/images/CLOSE2.jpg" Width="17PX" Height="17PX" runat="server" />
    </div>
                        </asp:Panel>
                     
         
    </div>--%>







        <asp:Panel ID="pnltarnsport" style="display:none"   runat="server">  
            <asp:GridView ID="gvtransport" runat="server" AutoGenerateColumns="false" 
                            Width="100%" BackColor="White" BorderColor="#FFAB60" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Both">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                 
                                
                               
                                <asp:TemplateField>
                                    <HeaderTemplate>TPT</HeaderTemplate>
                                    <ItemTemplate>
                                       <asp:CheckBox ID="chkboth" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="rute" HeaderText="Name" />
                                <asp:BoundField DataField="AMOUNt" HeaderText="Paid Amount" />
                
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#FFFF99" Font-Bold="True" ForeColor="#FFAB60" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center"/>
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                            <SortedAscendingHeaderStyle BackColor="#848384" />
                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                            <SortedDescendingHeaderStyle BackColor="#575357" />
                        </asp:GridView>
        
        </asp:Panel>
    </div>
    </form>
</body>
</html>
