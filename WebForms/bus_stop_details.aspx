<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="bus_stop_details.aspx.cs" Inherits="WebForms_bus_stop_details" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<link rel="Stylesheet" href="../css/facilitator.css" type="text/css" /> 
    <script language="javascript" type="text/javascript">
        function checkField() {
            var x = document.getElementById('<%= ddlRouteNameTab1.ClientID%>');
            var y = x.options[x.selectedIndex].value;
            if (y == "-1") {
                document.getElementById('lbl_requiredTab1').innerHTML = 'All Fields Required';
                return false;
            }
            x = trim(document.getElementById('<%=txtStopNameTab1.ClientID%>').value);
            if (x == '') {
                document.getElementById('lbl_requiredTab1').innerHTML = 'All Fields Required';
                return false;
            }
        }
        function checkField1() {
            var x = document.getElementById('<%= ddlRouteNameTab2.ClientID%>');
            var y = x.options[x.selectedIndex].value;
            if (y == "-1") {
                document.getElementById('lbl_requiredTab2').innerHTML = 'All Fields Required';
                return false;
            }
            x = trim(document.getElementById('<%=txtStopNameTab2.ClientID%>').value);
            if (x == '') {
                document.getElementById('lbl_requiredTab2').innerHTML = 'All Fields Required';
                return false;
            }
        }
        function trim(stringToTrim) {
            return stringToTrim.replace(/^\s+|\s+$/g, "")
        };
    </script>
    <style type="text/css">
        .style1
        {
            color: #006600;
            font-weight: bold;
            font-family: Calibri;
            width: 119px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div align="left" style="height:80%; >
    <span class="pageheading">Bus Stop Details</span>
    <hr />
        <table width="100%" style="height:80%">
            <tr>
                <td align="left" style="width:50%">
                    <cc1:TabContainer ID="TabContainer1" runat="server" width="500px" 
                        Height="300px" ActiveTabIndex="1">                        
                        <cc1:TabPanel runat="server" HeaderText="Add Stop Details" ID="TabPanel1" >
                            <ContentTemplate>
                                <br /><br />
                                <table style="float:left" width="100%">
                                    <tr>
                                        <td class="style1">
                                            Select Route:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRouteNameTab1" runat="server">
                                            </asp:DropDownList>                                            
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                ControlToValidate="ddlRouteNameTab1" ErrorMessage="(Required)" 
                                                ValidationGroup="g1" InitialValue="-1"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                            Stop Name:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStopNameTab1" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                ControlToValidate="txtStopNameTab1" ErrorMessage="(Required)" 
                                                ValidationGroup="g1"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                                                        
                                    <tr>
                                        <td class="style1">                                            
                                            Stop Details:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStopDetailsTab1" runat="server" TextMode="MultiLine" Height="40px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <label id="lbl_requiredTab1" style="color:Red"></label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btnAddStop" runat="server" Text="Button" CssClass="buttonstyle" 
                                                onclick="btnAddStop_Click" ValidationGroup="g1"/>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Update Stop">
                            <HeaderTemplate>
                                Update Stop
                            </HeaderTemplate>
                            <ContentTemplate>
                                <br /><br />
                                <table style="float:left" width="100%">
                                    <tr>
                                        <td class="tablecoltext">
                                            Select Route:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRouteNameTab2" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="ddlRouteNameTab2_SelectedIndexChanged">
                                            </asp:DropDownList>                                            
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                ControlToValidate="ddlRouteNameTab2" ErrorMessage="(Required)" 
                                                InitialValue="-1" ValidationGroup="g2"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tablecoltext">
                                            Select Stop:
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlStopNameTab2" runat="server" AutoPostBack="True" 
                                                        onselectedindexchanged="ddlStopNameTab2_SelectedIndexChanged">
                                                    </asp:DropDownList>                                                
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                                        ControlToValidate="ddlStopNameTab2" ErrorMessage="(Required)" InitialValue="-1" 
                                                        ValidationGroup="g2"></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlRouteNameTab2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tablecoltext">
                                            Stop Name:
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtStopNameTab2" runat="server"></asp:TextBox>                                                                                         
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                                        ControlToValidate="txtStopNameTab2" ErrorMessage="(Required)" 
                                                        ValidationGroup="g2"></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlRouteNameTab2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                                                        
                                    <tr>
                                        <td class="tablecoltext">                                            
                                            Stop Details:
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtStopDetailsTab2" runat="server" TextMode="MultiLine" Height="40px"></asp:TextBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlRouteNameTab2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <label id="lbl_requiredTab2" style="color:Red"></label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btnUpdateStop" runat="server" Text="Update" 
                                                CssClass="buttonstyle" onclick="btnUpdateStop_Click" 
                                               ValidationGroup="g2"/>&nbsp;&nbsp;
                                            <asp:Button ID="btnDeleteStop" runat="server" Text="Delete" 
                                                CssClass="buttonstyle" onclick="btnDeleteStop_Click" ValidationGroup="g2"/>
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

