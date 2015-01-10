<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="WebForms_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="ICON" type="image/x-icon" href="../Resources/iguardian.ico" />
    <title>i-ERP ver 1.0</title>
    <link href="../Style/Styler.css" rel="stylesheet" type="text/css" />
</head>
<body bgcolor="#006600">
    <form id="form1" runat="server">
        <div id="header">
            <span style="font-family:Georgia; padding-left:50px; font-size:30px; color:#FFFDFF;">i</span>
            <span style="font-family:Georgia; font-size:50px; color:#FFFDFF;">-</span>
            <span style="font-family:Georgia; letter-spacing:2px; font-size:40px; color:#FFFDFF;">ERP</span><br />
            <span style="font-family:Georgia; font-size:12px; color:#FFFDFF; padding-left:90px;">a complete ERP Solution</span>
            <div align="center" style="margin-top:50px; padding-top:140px;">
                <div style="border:2px solid white; border-radius:10px 10px; width:420px;" align="center">
                <table cellpadding="0" cellspacing="0" width="400px">
                <tr>
                    <td colspan="3" 
                        style="letter-spacing:1px; font-family:Calibri; font-size:18px; font-weight:bold; color:White; padding-top:5px;" 
                        align="left">
                        LOGIN TO I-ERP<asp:DropDownList ID="ddl_lkg_ukg_senior" style="Display:none"  runat="server">
                        <asp:ListItem  Text="Junior(LKJ - UKG)" Value="JR"></asp:ListItem>
                        <asp:ListItem  Text="I-XII" Selected=True Value="SR"></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                    </td>
                </tr>
                <tr style="height:30px;">
                    <td colspan="3" class="Calibri10N"></td>
                </tr>
                <tr style="height:40px;">
                    <td align="left" style="text-transform:none; border-top:ridge 1px Gray; font-family:Calibri; font-size:16px; font-weight:normal; color:White;">
                        Username 
                    </td>
                    <td align="left" style="text-transform:none; border-top:ridge 1px Gray; font-family:Calibri; font-size:16px; font-weight:normal; color:White;">
                        &nbsp;</td>
                    <td align="left" style="border-top:ridge 1px Gray; font-family:Calibri; font-size:16px; font-weight:normal; color:White;">
                        &nbsp;:&nbsp;<asp:TextBox ID="txtUserName" runat="server" class="roundedTextBox" Width="150px"></asp:TextBox><asp:RequiredFieldValidator ID="V1" runat="server" ControlToValidate="txtUserName" EnableClientScript="true" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="G1"></asp:RequiredFieldValidator></td>
                </tr>
                <tr style="height:40px;">
                    <td align="left" style="text-transform:none; border-top:ridge 1px Gray; font-family:Calibri; font-size:16px; font-weight:normal; color:White;">
                        Password 
                    </td>
                    <td align="left" style="text-transform:none; border-top:ridge 1px Gray; font-family:Calibri; font-size:16px; font-weight:normal; color:White;">
                        &nbsp;</td>
                    <td align="left" style="border-top:ridge 1px Gray; font-family:Calibri; font-size:16px; font-weight:normal; color:White;">
                        &nbsp;:&nbsp;<asp:TextBox ID="txtPassword" runat="server" class="roundedTextBox" Width="150px" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator ID="V2" runat="server" ControlToValidate="txtPassword" EnableClientScript="true" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="G1"></asp:RequiredFieldValidator></td>
                </tr>
                <tr style="height:40px;">
                    <td align="center" colspan="3" style="border-top:ridge 1px Gray;">
                        <asp:Button ID="btnLogin" class="Button" Width="75px" Height="25px" runat="server" Text="Login" ValidationGroup="G2" OnClick="btnLogin_Click"/>
 

</body>                   </td>
                </tr>
            </table>
            </div>
            </div>
        </div>
        <div id="footer"  align="right">
            <p style="font-family:Georgia; font-size:12px; color:#FFFDFF;">copyright © 2013. All rights reserved Litchi Knowledge Center &nbsp;&nbsp;&nbsp;&nbsp;</p>
        </div>
    </form>
</body>
</html>
