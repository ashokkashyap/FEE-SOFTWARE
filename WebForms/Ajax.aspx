<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ajax.aspx.cs" Inherits="WebForms_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ajax Testing</title>
    <script src="../Scripts/jquery-1.2.6.min.js" type="text/javascript"></script>
    <script type = "text/javascript">
        $(document).ready(function () {
            $('#txtEnterText').keydown(function () {
                //$('#txtReplicatedText').text($(this).text);
                DisplayMessageCall();
            })
        });
        function DisplayMessageCall() {


            var pageUrl = '<%=ResolveUrl("../serviceA.asmx")%>'


            $.ajax({
                type: "POST",
                url: pageUrl + "/HelloWorld",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessCall,
                error: OnErrorCall
            });


        }


        function OnSuccessCall(response) {
            $('#<%=lblOutput.ClientID%>').html(response.d);
        }


        function OnErrorCall(response) {
            alert(response.status + " " + response.statusText);
        }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnGetMsg" runat="server" Text="Click Me" OnClientClick="DisplayMessageCall();return false;" /><br />
        <asp:TextBox ID="txtEnterText" CssClass="txt1" runat="server"></asp:TextBox>
        <%--<asp:TextBox ID="txtReplicatedText" CssClass="txt2" runat="server"></asp:TextBox>--%>
        <asp:Label ID="lblOutput" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
