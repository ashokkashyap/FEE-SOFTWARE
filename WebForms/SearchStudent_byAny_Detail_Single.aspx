<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchStudent_byAny_Detail_Single.aspx.cs" Inherits="WebForms_SearchStudent_byAny_Detail_Single" %>

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
     <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
    <asp:TextBox ID="TextBox1"
        runat="server"></asp:TextBox><asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
   

    <asp:GridView ID="gvStudentDetails" runat="server" AutoGenerateColumns="true" Width="100%"
                            BackColor="White" BorderColor="#FFAB60" BorderStyle="None" BorderWidth="1px"
                            CellPadding="4" ForeColor="Black" GridLines="Both">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                
                                 
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        View</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnView" runat="server" style="height:20px; width:50px" ImageUrl="~/images/more.jpg"
                                            Height="15px" Width="15px" OnClick="btnView_Click" />
                                        <asp:HiddenField ID="hfview" runat="server" Value='<%#Eval("AdmNo") %>' />
                                        <asp:HiddenField ID="hfStudentID" runat="server" Value='<%#Eval("STUDENT_ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#3090C7" Font-Bold="True" ForeColor="white" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                        <ajax:AnimationExtender id="MyExtender"  runat="server" TargetControlID="pnlStudentDetails">
  <Animations>
    <OnClick>
      <FadeOut Duration=".5" Fps="20" />
    </OnClick>
  </Animations>
</ajax:AnimationExtender>




                        <asp:HyperLink ID="HyperLink1" style="display:none" runat="server">HyperLink</asp:HyperLink>
                        <ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlStudentDetails" TargetControlID="HyperLink1"
                            CancelControlID="btnClose" BackgroundCssClass="modalBackground">
                        </ajax:ModalPopupExtender>
                        <asp:Panel ID="pnlStudentDetails" CssClass="modalPopup" runat="server">
                        <div class="header">
      STUDENT DETAILS
    </div>
    <div class="body">
        <asp:Panel ID="pnldetail" runat="server"></asp:Panel>
    </div>
    <div class="footer" align="right">
        
        <asp:ImageButton ID="btnClose" ImageUrl="~/images/CLOSE2.jpg" Width="17PX" Height="17PX" runat="server" />
    </div>
                        </asp:Panel>
                     
         
    </div>
    </form>
</body>
</html>
