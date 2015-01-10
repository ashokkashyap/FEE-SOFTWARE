<%@ Page Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="send_sms_fee_defaulter.aspx.cs" Inherits="iguardian_facilitator_send_sms_fee_defaulter" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
    <%--<script language="javascript" type="text/javascript">
        function checkfields()
        {           
            var x = document.getElementById('<%= ddlStaffName.ClientID%>');
            var y = x.options[x.selectedIndex].value;
            if(y=="-1")
            {
                document.getElementById('<%=lbl_msg_req.ClientID%>').innerHTML='All Stared Fields Are Mandatory';
                return false;              
            }
             
            document.getElementById('<%=txt_msg_subject.ClientID%>').value = trim(document.getElementById('<%=txt_msg_subject.ClientID%>').value);
            document.getElementById('<%=txt_ig_msg.ClientID%>').value = trim(document.getElementById('<%=txt_ig_msg.ClientID%>').value);
            document.getElementById('<%=txt_sms_msg.ClientID%>').value = trim(document.getElementById('<%=txt_sms_msg.ClientID%>').value);
            if(document.getElementById('<%=txt_msg_subject.ClientID%>').value == '')
            {                             
                document.getElementById('<%=lbl_msg_req.ClientID%>').innerHTML  = "All Stared Fields Are Mandatory";
                return false;
            }
            
            if((document.getElementById('<%=txt_ig_msg.ClientID%>').value == '') && (document.getElementById('<%=txt_sms_msg.ClientID%>').value == ''))
            {                             
                document.getElementById('<%=lbl_msg_req.ClientID%>').innerHTML  = "Required IG Message Or SMS Message";
                return false;
            }            
            if((document.getElementById('<%=txt_ig_msg.ClientID%>').value) == (document.getElementById('<%=txt_sms_msg.ClientID%>').value))           
            {
                document.getElementById('<%=lbl_msg_req.ClientID%>').innerHTML  = "IG Message And SMS Message Should Not Be Same";
                return false;
            }                        
        }
        function trim(stringToTrim) 
        {
	       return stringToTrim.replace(/^\s+|\s+$/g,"")
	    };
	    
	    
    </script>--%>

    <script language="javascript" type="text/javascript">
        
        function clickFunction(obj)
	    {	        
	        var rowObject = getParentRow(obj);
            if (obj.checked) 
            {
                rowObject.style.backgroundColor = 'Gray';
            }
            else
            {
                rowObject.style.backgroundColor = 'White';
            }
	    }	    
	    function getParentRow(obj)
        {            
            while(obj.tagName != "TR")
            {                
                if(isFireFox())
                {                    
                    obj = obj.parentNode;
                }
                else
                {
                    obj = obj.parentElement;
                }
            }
            return obj;
        }
        function isFireFox()
        {
           return navigator.appName == "Netscape";

        }
    </script>

    <script src="../scripts/jquery-1.4.1.min.js" type="text/javascript">        
    </script>

    <script src="../scripts/ScrollableGridPlugin.js" type="text/javascript"></script>

    <%-- <script type="text/javascript" language="javascript">
        $(document).ready(function() {
        $('#<%=grd_student_list.ClientID %>').Scrollable();
        }
        )
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div align="left" style="width: 100%">
        <span class="pageheading">SMS For FEE (Amount)</span>
        <hr />
        <table>
            <tr>
                <td class="tablecoltext" align="left" style="width: 100px">
                    Class:
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlClassName" runat="server" AutoPostBack="true" 
                        onselectedindexchanged="ddlClassName_SelectedIndexChanged"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlClassName"
                        ErrorMessage="*" InitialValue="-1" ValidationGroup="G1"></asp:RequiredFieldValidator>
                    <asp:Button ID="btnStudentList" runat="server" Text="Get List" 
                        CssClass="buttonstyle1" onclick="btnStudentList_Click" />
                    &nbsp;
                </td>
                <td class="tablecoltext" align="left" style="width: 100px">
                    Due Date:
                </td>
                <td>
                <asp:TextBox ID="txtDate" runat="server" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="G1" 
                        ErrorMessage="Select Date" ControlToValidate="txtDate" ></asp:RequiredFieldValidator>
                    <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="txtDate" Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
            </tr>
        </table>
        <table border="1" width="90%">
            <tr>
                <td>
                   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                            <asp:GridView ID="grd_student_list" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                CellPadding="4" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" Width="100%"
                                ForeColor="Black" CellSpacing="2">
                                <FooterStyle BackColor="#CCCCCC" />
                                <RowStyle HorizontalAlign="Left" BorderWidth="0" BackColor="White" Font-Bold="True"
                                    Font-Names="Arial" Font-Size="11px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SNO">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex +1 %>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("student_id") %>'>
                                            </asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:BoundField DataField="Father_name" HeaderText="Father Name" />
                                    <asp:BoundField DataField="Comm_Nbr" HeaderText="Comm Number" />
                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chk_box_all" runat="server" Text="All" OnCheckedChanged="chk_box_all_checked_changed"
                                                AutoPostBack="true" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_box" runat="server" onClick="clickFunction(this)" />
                                        </ItemTemplate>
                                     </asp:TemplateField>--%>
                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                          Due Amonut
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_due_amnt" runat="server"></asp:TextBox>
                                           
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                         Fine Amonut
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                        <asp:TextBox ID="txt_fine_amnt" runat="server"/>
                                           
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                </Columns>
                                <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle HorizontalAlign="Left" BackColor="Black" Font-Bold="True" ForeColor="White"
                                    Font-Italic="True" Font-Names="Arial" Font-Size="13px" />
                            </asp:GridView>
                       <%-- </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnStudentList" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlClassName" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
        </table>
        <br />
        <table border="1" width="90%" bgcolor="#F5F5F3">
            <tr>
                <td class="tablecoltext" align="left">
                    Msg From:
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlStaffName" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlStaffName"
                        ErrorMessage="*" InitialValue="-1" ValidationGroup="G1"></asp:RequiredFieldValidator>
                    &nbsp;
                </td>
                <td class="tablecoltext" align="left">
                    Msg For:
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddl_msg_for_role" runat="server" Enabled="False">
                        <asp:ListItem Text="PARENT" Value="PARENT"></asp:ListItem>
                        <asp:ListItem Text="STUDENT" Value="STUDENT"></asp:ListItem>
                    </asp:DropDownList>
                    <span style="color: Red">*</span>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="btn_submit" runat="server" Text="Send SMS" OnClick="btn_submit_Click"
                        CssClass="buttonstyle" ValidationGroup="G1" />
                </td>
                <td align="left" colspan="3">
                    <asp:Label ID="lbl_msg_req" runat="server" ForeColor="Red" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
