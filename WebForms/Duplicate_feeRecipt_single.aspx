<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Duplicate_feeRecipt_single.aspx.cs" Inherits="WebForms_Duplicate_feeRecipt_single" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script type = "text/javascript">
         function radioMe(e, CurrentGridRowCheckBoxListID) {
             if (!e) e = window.event;
             var sender = e.target || e.srcElement;

             if (sender.nodeName != 'INPUT') return;
             var checker = sender;
             var chkBox = document.getElementById(CurrentGridRowCheckBoxListID);
             var chks = chkBox.getElementsByTagName('INPUT');
             for (i = 0; i < chks.length; i++) {
                 if (chks[i] != checker)
                     chks[i].checked = false;
             }
         }
</script>
     


</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="0" width="90%">
                <tr>
                    <td valign="bottom" align="left" style="color:#FFAB60; font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:18px; border-bottom:solid 2px #FFAB60;">
                        <div style="background-color:#FFFF99; border-top:2px solid #FFAB60; border-left:2px solid #FFAB60; border-right:2px solid #FFAB60; border-top-left-radius:25px 50px; border-top-right-radius:25px 50px; width:240px; height:25px; padding-left:8px;">
                            SEARCH STUDENT BY NAME
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <table cellpadding="0" cellspacing="5" width="80%">
                            <tr>
                                <td class="Calibri14N" align="left" style="width:15%">
                                    Name
                                    <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="A"></asp:RequiredFieldValidator>
                                </td>
                                <td class="Calibri14N" align="left" style="width:45%">
                                    : <asp:TextBox ID="txtName" runat="server" Width="90%"></asp:TextBox>
                                        <ajax:AutoCompleteExtender ID="txtNameExtender" runat="server" DelimiterCharacters="" Enabled="true" ServicePath="" ServiceMethod="GetCompletionListName" TargetControlID="txtName" UseContextKey="true" MinimumPrefixLength="1"></ajax:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnGetDetails" runat="server" Text="Get Details" ValidationGroup="A" OnClick="btnGetDetails_Click"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:GridView ID="gvStudentDetails" runat="server" AutoGenerateColumns="False" 
                            Width="100%" BackColor="White" BorderColor="#FFAB60" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Both" 
                            onrowcreated="gvStudentDetails_RowCreated">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>D Receipt</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Resources/edit.jpg" Height="15px" Width="15px" OnClick="btnEdit_Click" />
                                        <asp:HiddenField ID="hfStudentID" runat="server" Value='<%#Eval("STUDENT_ID") %>' />
                                         <asp:HiddenField ID="hfdetailid" runat="server" Value='<%#Eval("id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <%--<asp:TemplateField>
                                    <HeaderTemplate>Student</HeaderTemplate>
                                    <ItemTemplate>
                                       <asp:CheckBox ID="chkstudent" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>School</HeaderTemplate>
                                    <ItemTemplate>
                                       <asp:CheckBox ID="chkschool" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                 <asp:TemplateField>
                                    <HeaderTemplate>Receipt</HeaderTemplate>
                                    <ItemTemplate>
                                    

<%--
                                      <asp:CheckBoxList ID="chklstRecpt"  RepeatDirection="Horizontal" runat="server">
                <asp:ListItem Text="Student" Value="Student"></asp:ListItem>
                <asp:ListItem Text="School" Value="School"></asp:ListItem>
                <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
               
            </asp:CheckBoxList>--%>
                                        <asp:RadioButtonList ID="chklstRecpt" RepeatDirection="Horizontal" runat="server">
                                         <asp:ListItem Text="Student" Value="Student"></asp:ListItem>
                <asp:ListItem Text="School" Value="School"></asp:ListItem>
                <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               <%-- <asp:TemplateField>
                                    <HeaderTemplate>Both</HeaderTemplate>
                                    <ItemTemplate>
                                       <asp:CheckBox ID="chkboth" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                 <asp:BoundField DataField="STUDENT_REGISTRATION_NBR" HeaderText="Adm No." />
                                <asp:BoundField DataField="name" HeaderText="Name" />
                                 <asp:BoundField DataField="Rno" HeaderText="Rec.No." />
                                <asp:BoundField DataField="AMOUNT_PAID" HeaderText="Paid Amount" />
                               
                                <asp:BoundField DataField="pay_date" HeaderText="Pay Date" />
                                 <asp:BoundField DataField="months" HeaderText="Month" />
                                <asp:BoundField DataField="MODE" HeaderText="Mode" />
                                <asp:BoundField DataField="CLASS" HeaderText="Class" />
                                
                               
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
            </table>
        </asp:Panel>
    </div>

    </form>
</body>
</html>
