<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="collectFeeAdmissionNo.aspx.cs" Inherits="WebForms_collectFeeAdmissionNo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .FixedHeader {
            position:static;
            font-weight: bold;
        }     
    
     </style>
    



<
   
    <script type="text/javascript" src="http://code.jquery.com/jquery-latest.min.js"></script>
    <link href="../source/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script src="../source/jquery.fancybox.js" type="text/javascript"></script>
    
     <script type="text/javascript">
         $(document).ready(function () {
             $(".various").fancybox({

                 maxWidth: 1700,
                 maxHeight: 1000,
                 fitToView: true,
                 width: '91%',
                 height: '88%',
                 helpers: {
                     overlay: {
                         closeClick: false
                     }
                 },

                 autoSize: false,
                 escKey: false,
                 titlle: true,
                 openEffect: 'elastic',
                 closeEffect: 'elastic'
             });
         });
</script>
    
 <script type="text/javascript">
     $(document).bind("keydown.cbox_close", function (e) {


         if (e.keyCode === 27) {

             cboxPublic.close();
         }
     });
 
 
 </script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#<%=grddetail.ClientID %>').Scrollable();
        }
        )
    </script>

    <script type = "text/javascript">
        function DisableButton() {
            document.getElementById("<%=btnSubmit.ClientID %>").disabled = true;
        }
        window.onbeforeunload = DisableButton;
</script>





<style type="text/css">

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
.button_example{
    border-left: 0px solid #3A4C7D;
        border-right: 0px solid #3A4C7D;
        border-top: 0px solid #3A4C7D;
        border-bottom: 2px solid #3A4C7D;
        -webkit-border-radius: 3px; -moz-border-radius: 3px;border-radius: 3px;font-size:12px;font-family:arial, helvetica, sans-serif; padding: 10px 10px 10px 10px; text-decoration:none; display:inline-block;text-shadow: -1px -1px 0 rgba(0,0,0,0.3);font-weight:bold; color: #FFFFFF;
        background-color: #a5b8da; background-image: linear-gradient(to bottom, #a5b8da, #7089b3);
 }

.button_example:hover{
    border-left: 0px solid #3A4C7D;
        border-right: 0px solid #3A4C7D;
        border-top: 0px solid #3A4C7D;
        border-bottom: 2px solid #3A4C7D;
        background-color: #819bcb; background-image: linear-gradient(to bottom, #819bcb, #536f9d);
 }
</style>
    <style type="text/css">
        .CompletionListCssClass1
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: 10px;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 40px;
            padding: 5px;
            background-color: white;
            margin-left: 1px;
        }
        .CompletionListItemCssClass1
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: blue;
        }
        .CompletionListHighlightedItemCssClass1
        {
            color:white ;
              font-size: 10px;
               font-weight: normal;
            background-color: #006699;
            cursor: pointer;
        }
        .inputs
        
         {
    background: #D3D3D3;
    background: -moz-linear-gradient(top, #D3D3D3 0%, #D9D9D9 38%, #E5E5E5 82%, #E7E7E7 100%);
    background: -webkit-gradient(linear, left top, left bottom, color-stop(0%, #D3D3D3), color-stop(38%, #D9D9D9), color-stop(82%, #E5E5E5), color-stop(100%, #E7E7E7));
    background: -webkit-linear-gradient(top, #D3D3D3 0%, #D9D9D9 38%, #E5E5E5 82%, #E7E7E7 100%);
    background: -o-linear-gradient(top, #D3D3D3 0%, #D9D9D9 38%, #E5E5E5 82%, #E7E7E7 100%);
    background: -ms-linear-gradient(top, #D3D3D3 0%, #D9D9D9 38%, #E5E5E5 82%, #E7E7E7 100%);
    background: linear-gradient(to bottom, #D3D3D3 0%, #D9D9D9 38%, #E5E5E5 82%, #E7E7E7 100%);
    filter: progid: DXImageTransform.Microsoft.gradient( startColorstr='#d3d3d3', endColorstr='#e7e7e7', GradientType=0 );
    display: block;
    padding: 3px 3px;
    color: #000000;
    font-size: 1em;
    font-weight: bold;
    text-shadow: 1px 1px 1px #FFF;
    border: 1px solid rgba(16, 103, 133, 0.6);
    box-shadow: 0 0 2px rgba(255, 255, 255, 0.5), inset 0 1px 2px rgba(0, 0, 0, 0.2);
    border-radius: 2px;
    outline: 0;
    width: 270px;
           }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<div align="right">

<a class="various" data-fancybox-type="iframe" href="StudentWise_componentMapping_single.aspx">Component Mapping</a>

    <a class="various" data-fancybox-type="iframe" href="SearchStudent_byAny_Detail_Single.aspx">Student Details</a>

     <a class="various" data-fancybox-type="iframe" href="fee_detail_single.aspx">Fee Details</a>

       <a class="various" data-fancybox-type="iframe" href="Duplicate_feeRecipt_single.aspx">Fee Reciept </a>
       
       </div>



    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="5" width="90%" border="0" style="border-color: Black;">
                <tr>
                    <td valign="bottom" colspan="4" align="left" style="color: black; font-family: Calibri; letter-spacing: 1px; font-weight: bold; font-size: 18px; border-bottom: solid 2px black;">
                        <div style="background-color: #E6E6FA; border-top: 2px solid white; border-left: 2px solid black; border-right: 2px solid black; border-top-left-radius: 25px 50px; border-top-right-radius: 25px 50px; width: 120px; height: 25px; padding-left: 8px;">
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
                <td class="Calibri14N" style="padding-left: 15px; border-bottom: 1px ridge grey; width: 1%; letter-spacing: 1px;">Student Name
               
                </td>
                <td>
                 <asp:TextBox ID="txtName" runat="server" CssClass="inputs" Width="90%"   ontextchanged="txtName_TextChanged"></asp:TextBox>

                <ajax:AutoCompleteExtender ID="txtNameExtender" CompletionListCssClass="CompletionListCssClass1"
             CompletionListItemCssClass="CompletionListItemCssClass1" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass1"
                  
                    
                     runat="server" DelimiterCharacters="" Enabled="true"  ServicePath="" ServiceMethod="GetCompletionListName" TargetControlID="txtName" UseContextKey="true" MinimumPrefixLength="1"></ajax:AutoCompleteExtender>
                               
                </td>
                <td style="width:50px;">
                
                 <asp:ImageButton ID="btnname" runat="server"   ValidationGroup="v1ccccccc" 
                        Height="18px" Width="18px" ImageUrl="~/Resources/search.png" 
                        onclick="btnname_Click" />
                
                </td>
               
                </tr>
                <tr>
                    <td class="Calibri14N" style="padding-left: 15px; border-bottom: 1px ridge grey; width: 23%; letter-spacing: 1px;">Admission no
                        <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtAdmissionNo" ErrorMessage="*" ForeColor="Red" ValidationGroup="v1"></asp:RequiredFieldValidator>
                    </td>
                    <td style="border-bottom: 1px ridge grey;">:
                        <asp:TextBox ID="txtAdmissionNo" runat="server" CssClass="txtbxcomp" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="btnGetDetails" runat="server" OnClick="btnGetDetails_Click" ValidationGroup="v1" Height="18px" Width="18px" ImageUrl="~/Resources/search.png" />
                         
                
                    </td>
                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey; width: 25%">Calculation Date 
                    </td>
                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; width: 25%;">:
                        <asp:TextBox ID="txtCalculationDate" CssClass="txtbxcomp" runat="server" Width="150px"></asp:TextBox>
                        <ajax:CalendarExtender ID="CalE" runat="server" Format="dd-MMMM-yyyy" PopupButtonID="txtCalculationDate" TargetControlID="txtCalculationDate"></ajax:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td class="Calibri14N" style="padding-left: 15px;height:5px; border-bottom: 1px ridge grey; width: 23%;"></td>
                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; height:5px;">For the period of
                    </td>
                    <td class="Calibri14N" style="padding-left: 10px; background-color:#FAFAFA; height:5px; border-bottom: 1px ridge grey; width: 25%" >
                        <asp:DropDownList ID="ddlStartMonth"   runat="server" Width="80%" 
                            AutoPostBack="True" onselectedindexchanged="ddlStartMonth_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; background-color:#FAFAFA;  height:5px; width: 25%;">-
                        <asp:DropDownList ID="ddlEndMonth"    runat="server"  Width="80%" 
                            AutoPostBack="True" onselectedindexchanged="ddlEndMonth_SelectedIndexChanged"></asp:DropDownList>
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
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey;">Category
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; color: Blue;">:
                                        <asp:Label ID="lblcategory" Font-Bold="true"  ForeColor="Red" Font-Size="Medium" runat="server" Text=""></asp:Label> 
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
                               
                                <tr>
                                    <td class="Calibri14N" style="padding-left: 10px; border-bottom: 1px ridge grey; vertical-align: top;">Payment Mode
                                    </td>
                                    <td class="Calibri14N" style="border-bottom: 1px ridge grey; vertical-align: top;">:
                                        <asp:DropDownList ID="ddlSelectPaymentMode" CssClass="dll" AutoPostBack="true" runat="server" 
                                            onselectedindexchanged="ddlSelectPaymentMode_SelectedIndexChanged">
                                            <asp:ListItem Text="CHEQUE" Value="CHEQUE" ></asp:ListItem>
                                            <asp:ListItem Text="CASH" Value="CASH" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="NET BANKING" Value="NET BANKING"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>

                                    <td>  
                                        <asp:CheckBox ID="checkCts" Text="Cts" runat="server" /></td>
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
                                     
                            </table>
                            <div style="overflow:scroll">
                            <table>
                                <tr>
                                    <td>

                                        
                                         <asp:GridView ID="grddetail"  runat="server" AutoGenerateColumns="true" BackColor="#CCCCCC"
                            BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2"
                            ForeColor="Black" Width="100%">
                            <FooterStyle BackColor="#CCCCCC" />
                            <RowStyle BackColor="White" BorderWidth="0" HorizontalAlign="Center" 
                                Font-Bold="True" Font-Names="Arial" Font-Size="14px" />
                                             <Columns>

                                                 <asp:TemplateField>
            <HeaderTemplate>
            S No.</HeaderTemplate>
            <ItemTemplate>
            <asp:Label ID="lblSRNO" runat="server" 
                Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>


                                             </Columns>
                  
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Left" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" 
                        HorizontalAlign="Center" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                
            </asp:GridView>
                                    </td>


                                </tr>



                            </table>
                                
                                            </div>
                        </asp:Panel>
                    </td>
                    <td colspan="2" valign="top" style="width: 60%;">
                        <asp:Panel ID="pnlComponentDetails" runat="server" BorderStyle="Dotted" BorderColor="Black" BorderWidth="1px">
                            <table cellpadding="0" cellspacing="5" width="99%">
                                <tr>
                                    <td align="left" class="Calibri14N" style="padding-left: 10px; border: 1px solid black; background-color: black;">
                                        <table cellpadding="5" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="Calibri14N" style="background: #E6E6FA; color: black; width: 60px;">Scroll # : 
                                               
                                                </td>
                                                <td class="Calibri14N" style="background: #E6E6FA; color: Black;">
                                                    <asp:TextBox ID="txtScrollingNumber" CssClass="txtbxcomp" runat="server" Width="60px"></asp:TextBox>
                                                </td>
                                                <td class="Calibri12N" style="background: #E6E6FA; color: Black;">
                                                    <asp:Label ID="lblScrollingNumbers" Style="display:none" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lblInfo" runat="server" Text="" ForeColor="Black"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" colspan="3">
                                     <asp:UpdatePanel ID="upnl" runat="server">
                                                            <ContentTemplate>
                                        <asp:GridView ID="gvFeeAmountDetails" runat="server" AutoGenerateColumns="False"
                                            Width="100%" BackColor="White" BorderColor="black" BorderStyle="None"
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
                                                <asp:BoundField DataField="PREVIOUS" HeaderText="Prev Due" />
                                                <asp:TemplateField HeaderText="Disc">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiscount" runat="server" Text='<%#Eval("Discount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Addl.Disc">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtADiscount" CssClass="inputs" Width="50px" runat="server" Text='<%#Eval("ADISCOUNT") %>' AutoPostBack="true" OnTextChanged="txtFineAmount_TextChanged"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payment">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel ID="upnl" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtPayment" CssClass="inputs" Width="50px"  AutoPostBack="true" OnTextChanged="txtFineAmount_TextChanged" runat="server" Text='<%#Eval("ToBePaid") %>'></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtADiscount" EventName="TextChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
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
                                          </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtFineAmount" EventName="TextChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri14N" style="padding-left: 10px; border: 1px solid black; background-color: black;">
                                        <table cellpadding="5" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="Calibri14N" style="background: #E6E6FA; color: black; width: 120px;">Fine : </td>
                                                <td class="Calibri14N" style="background: #E6E6FA; color: Black;">
                                                 <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                    <asp:TextBox ID="txtFineAmount" AutoPostBack="true" runat="server" CssClass="inputs" Width="70px" Text="0" 

                                                        ontextchanged="txtFineAmount_TextChanged"></asp:TextBox>
                                                         </ContentTemplate>
                                        </asp:UpdatePanel>
                                                </td>
                                                <td class="Calibri14N" style="background: #E6E6FA; color: Black;"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri14N" style="padding-left: 10px; border: 1px solid black; background-color: black;">
                                        <table cellpadding="5" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="Calibri14N" style="background: #E6E6FA; color: black; width: 120px;">Re-Adm. Charges : </td>
                                                <td class="Calibri14N" style="background: #E6E6FA; color: Black;">
                                                 <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                                    <asp:TextBox ID="txtReAdmissionCharges" CssClass="inputs" runat="server" AutoPostBack="true" OnTextChanged="txtFineAmount_TextChanged" Width="70px" Text="0"></asp:TextBox>
                                                      </ContentTemplate>
                                        </asp:UpdatePanel>
                                                </td>
                                                <td class="Calibri14N" style="background: #E6E6FA; color: Black;"></td>
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
                                                <asp:Label ID="lblTotalPaid" runat="server" Text="" Font-Size="X-Large" ForeColor="Red"></asp:Label>
                                                <asp:HiddenField ID="hfTotalAmountPaid" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="Calibri12N" style="padding-left: 10px;">
                                         
                                        <asp:Button ID="btnReset" Text="Reset" CssClass="button_example"  runat="server" Visible="false" OnClick="btnReset_Click" />



                                        <asp:Button ID="btnSubmit" Text="Submit" CssClass="button_example" runat="server" OnClick="btnSubmit_Click" />

                                          <asp:RadioButtonList ID="radioprint" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem>PRINT</asp:ListItem>
                        <asp:ListItem>REC</asp:ListItem>
                        <asp:ListItem>NO REC</asp:ListItem>
                    </asp:RadioButtonList>
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
