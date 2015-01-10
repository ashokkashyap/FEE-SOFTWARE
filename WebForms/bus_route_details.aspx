<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="bus_route_details.aspx.cs" Inherits="WebForms_bus_route_details" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link rel="Stylesheet" href="../css/facilitator.css" type="text/css" /> 
    <script language="javascript" type="text/javascript">
        function confirmation() {
            var answer = confirm("Sure To Delete")
            if (answer) {
                //alert(answer);		        
            }
            else {
                //alert("Thanks for sticking around!");	
                return false;
            }
        }
        function checkField() {
            var x = trim(document.getElementById('<%=txtRouteNameTab1.ClientID%>').value);
            if (x == '') {
                document.getElementById('lbl_requiredTab1').innerHTML = 'All Fields Required';
                return false;
            }
            x = document.getElementById('<%= ddlStaffNameTab1.ClientID%>');
            var y = x.options[x.selectedIndex].value;
            if (y == "-1") {
                document.getElementById('lbl_requiredTab1').innerHTML = 'All Fields Required';
                return false;
            }
        }

        function trim(stringToTrim) {
            return stringToTrim.replace(/^\s+|\s+$/g, "")
        };
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div align="left" style="height:80%">
    <span class="pageheading"> <b>Bus Route Details</b></span>
    <hr />
        <table width="100%" style="height:80%">
            <tr>
                <td align="left" style="width:50%">
                    <cc1:TabContainer ID="TabContainer1" runat="server" width="500px" 
                        Height="300px" ActiveTabIndex="0">
                        <cc1:TabPanel runat="server" HeaderText="Add Route" ID="TabPanel1" >
                            <HeaderTemplate>
                                Add Route
                            
</HeaderTemplate>
                            
<ContentTemplate>
                                <br />
<br />
                                <table style="float:left" width="100%">
                                    <tr>
                                        <td class="tablecoltext">
                                            Route Name:                                            
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRouteNameTab1" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                ControlToValidate="txtRouteNameTab1" ErrorMessage="(Required)" ValidationGroup="g1"></asp:RequiredFieldValidator>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tablecoltext">
                                            Driver Name:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDriverNameTab1" runat="server"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tablecoltext">
                                            Helper Name:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHelperNameTab1" runat="server"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tablecoltext">
                                            Incharge:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStaffNameTab1" AutoPostBack="true" runat="server" SkinID="longest">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                ControlToValidate="ddlStaffNameTab1" ErrorMessage="(Required)" 
                                                InitialValue="-1" ValidationGroup="g1"></asp:RequiredFieldValidator>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tablecoltext">
                                            Route Details:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRouteDetailsTab1" runat="server" Height="40px" 
                                                TextMode="MultiLine"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <label id="lbl_requiredTab1" style="color:Red">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btnAddRoute" runat="server" Text="Button" 
                                                CssClass="buttonstyle" onclick="btnAddRoute_Click" ValidationGroup="g1"/>
                                        </td>
                                    </tr>
                                    </table>
                            
</ContentTemplate>
                        
</cc1:TabPanel>
                        <cc1:TabPanel runat="server" HeaderText="Update/Delete Route" ID="TabPanel2" >
                            <HeaderTemplate>
                                Update/Delete Route
                            
</HeaderTemplate>
                            
<ContentTemplate>
                                <br />
<br />
                                <table style="float:left" width="100%">
                                    <tr>
                                        <td class="tablecoltext">
                                            Select Route:                                            
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRouteNameTab2" runat="server" AutoPostBack="True" 
                                                OnSelectedIndexChanged="ddlRouteNameTab2_SelectedIndexChanged" >
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                ControlToValidate="ddlRouteNameTab2" ErrorMessage="(Required)" 
                                                InitialValue="-1" ValidationGroup="g2"></asp:RequiredFieldValidator>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tablecoltext">
                                            Route Name:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRouteNameTab2" runat="server"></asp:TextBox>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                                ControlToValidate="txtRouteNameTab2" ErrorMessage="(Required)" ValidationGroup="g2"></asp:RequiredFieldValidator>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tablecoltext">
                                            Driver Name:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDriverNameTab2" runat="server"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tablecoltext">
                                            Helper Name:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHelperNameTab2" runat="server"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tablecoltext">
                                            Incharge:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStaffNameTab2" runat="server" AutoPostBack="true" >
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                                ControlToValidate="ddlStaffNameTab2" ErrorMessage="(Required)" 
                                                InitialValue="-1" ValidationGroup="g2"></asp:RequiredFieldValidator>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tablecoltext">
                                            Route Details:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRouteDetailsTab2" runat="server" Height="40px" 
                                                TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <label id="lbl_requiredTab2" style="color:Red">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  align="center">
                                            <asp:Button ID="Delete" runat="server" Text="Delete" 
                                                CssClass="buttonstyle" ValidationGroup="g2" onclick="Delete_Click"/>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td  align="center">
                                            <asp:Button ID="btnUpdate" runat="server" Text="Update" 
                                                CssClass="buttonstyle" onclick="btnUpdate_Click" ValidationGroup="g2"/>
                                            &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            
</ContentTemplate>
                        
</cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

