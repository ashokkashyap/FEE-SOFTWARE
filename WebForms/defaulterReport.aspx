<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="defaulterReport.aspx.cs" Inherits="WebForms_defaulterReport" %>
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
                            width: 150px; height: 25px; padding-left: 8px;">
                            Defaulter Report
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick = "return PrintPanel();" />
                        <br />
                    </td>
 
                </tr>
                <tr>
                    <td class="Calibri14N" style="padding-left: 15px; border-bottom: 1px ridge grey;
                        width: 23%; letter-spacing: 1px;">
                        End Date
                        <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtEndDate"
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>
                    </td>

                   
                    <td style="border-bottom: 1px ridge grey;">
                        :
                        <asp:TextBox ID="txtEndDate" runat="server" Width="200px"></asp:TextBox>
                        <ajax:CalendarExtender ID="CE1" runat="server" Format="D" PopupButtonID="txtEndDate" TargetControlID="txtEndDate"></ajax:CalendarExtender>
                       
                    </td>
                     <td class="Calibri14N" style="padding-left: 15px; border-bottom: 1px ridge grey;
                        width: 23%; letter-spacing: 1px;">
                       
                        
                   
                
                <asp:DropDownList ID="ddlclasswithoutsection" runat="server" 
        AutoPostBack="True" onselectedindexchanged="ddlclasswithoutsection_SelectedIndexChanged">
    </asp:DropDownList>
                 </td>
                    
                     <td class="Calibri14N" style="padding-left: 15px; border-bottom: 1px ridge grey;
                        width: 23%; letter-spacing: 1px;">
                       <asp:DropDownList ID="ddlCLassName" runat="server" AutoPostBack="True" 
                             onselectedindexchanged="ddlCLassName_SelectedIndexChanged">
                         </asp:DropDownList>
                   
                    </td>

                    <td style="border-bottom: 1px ridge grey;">
                         <asp:ImageButton ID="btnGetDetails" runat="server" OnClick="btnGetDetails_Click"
                            ValidationGroup="v1" Height="18px" Width="18px" ImageUrl="~/Resources/search.png" />
                        <asp:CheckBox ID="chkcount" runat="server" />
                    </td>
                    <td colspan="2" style="width:30%; padding-left: 15px; border-bottom: 1px ridge grey;">
                    <asp:ImageButton ID="btnDownloadExcel" runat="server" OnClick="btnDownloadExcel_Click"
                            Height="22px" Width="22px" ImageUrl="~/Resources/excel-logo.jpg" Visible="false" />
                    </td>
                </tr>
               
                <tr>
                    <td colspan="4">
                        <br />
                        <asp:Panel ID="pnlDetails" runat="server"></asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
     <script type = "text/javascript">
         function PrintPanel() {
             var panel = document.getElementById("<%=pnlDetails.ClientID %>");
             var printWindow = window.open('', '', 'height=400,width=800');
             printWindow.document.write('<html><head><title>DIV Contents</title>');
             printWindow.document.write('</head><body >');
             printWindow.document.write(panel.innerHTML);
             printWindow.document.write('</body></html>');
             printWindow.document.close();
             setTimeout(function () {
                 printWindow.print();
             }, 500);
             return false;
         }
    </script>
</asp:Content>

