<%@ Page Title="" Language="C#" MasterPageFile="Site.master" AutoEventWireup="true" CodeFile="addUpdateComponentDetails.aspx.cs" Inherits="WebForms_addUpdateComponentDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../Scripts/jquery-1.2.6.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //            $('#AddDetailsMinus').click(function () {
            //                $('#b1').slideUp(2000);
            //                $(this).css("display", "none");
            //                $('#AddDetailsPlus').css("display","block");
            //            });
            //            $('#AddDetailsPlus').click(function () {
            //                $('#b1').slideDown(2000);
            //                $(this).css("display", "none");
            //                $('#AddDetailsMinus').css("display", "block");
            //            });

            //            $('#UpdateDetailsMinus').click(function () {
            //                $('#b2').slideUp(2000);
            //                $(this).css("display", "none");
            //                $('#UpdateDetailsPlus').css("display", "block");
            //            });
            //            $('#UpdateDetailsPlus').click(function () {
            //                $('#b2').slideDown(2000);
            //                $(this).css("display", "none");
            //                $('#UpdateDetailsMinus').css("display", "block");
            //            });
            //        });



////            $(document).ready(function () {
////                $('#b1').slideUp();
////                $('#b2').slideUp();
////            });

////            $('#ADD').click(function () {
////                $('#b1').toggle(2000);
////            });
////            $('#UPDATE').click(function () {
////                $('#b2').toggle(2000);
////            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="0" width="90%">
                <tr>
                    <td valign="bottom" align="left" style="color:#FFAB60; font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:18px; border-bottom:solid 2px #FFAB60;">
                        <div style="background-color:#FFFF99; border-top:2px solid #FFAB60; border-left:2px solid #FFAB60; border-right:2px solid #FFAB60; border-top-left-radius:25px 50px; border-top-right-radius:25px 50px; width:315px; height:25px; padding-left:8px;">
                            ADD/UPDATE COMPONENT DETAILS
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left:10px;">
                    <br />
                        <div id="ADD" style="padding-top:5px; width:99%; background-color:GrayText; height:20px; color:White; border:1px solid black; padding-left:5px;">
                            <%--<img src="../Resources/minus.gif" id="AddDetailsMinus" alt="../Resources/minus.gif" />
                            <img src="../Resources/plus.gif" id="AddDetailsPlus" alt="../Resources/plus.gif" style="display:none;"/>--%>
                            <span style="font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:12px;">Add Component Details</span>
                        </div>
                        <div id="b1" style="height:150px; width:90%; padding-left:50px;">
                            <table cellpadding="0" border="0" cellspacing="0" width="70%">
                                <tr><td colspan="2"><br/></td></tr>
                                <tr>
                                    <td class="Calibri14N" style="width:50%; padding-left:40px;">
                                        Name
                                        <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtAComponentName" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="Calibri14N">
                                        : <asp:TextBox ID="txtAComponentName" runat="server" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:50%; padding-left:40px;">
                                        Frequency
                                    </td>
                                    <td class="Calibri14N">
                                        : <asp:DropDownList ID="ddlAFrequency" runat="server" Width="80px"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:50%; padding-left:40px;">
                                        Start Month
                                    </td>
                                    <td class="Calibri14N">
                                        : <asp:DropDownList ID="ddlAStartMonth" runat="server" Width="80px"></asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="ddlAStartYear" runat="server" Width="80px"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center"><br />
                                        <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/Resources/submit.jpg" ValidationGroup="ADD" OnClick="btnAdd_Click"/>
                                    </td>
                                </tr>
                            </table>
                        </div><br />
                        <div id="UPDATE" style="padding-top:5px; width:99%; background-color:GrayText; height:20px; color:White; border:1px solid black; padding-left:5px;">
                            <%--<img src="../Resources/minus.gif" id="UpdateDetailsMinus" alt="../Resources/minus.gif" />
                            <img src="../Resources/plus.gif" id="UpdateDetailsPlus" alt="../Resources/plus.gif" style="display:none;"/>--%>
                            <span style="font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:12px;">Update Component Details</span>
                        </div>
                        <div id="b2" style="height:200px; width:90%; padding-left:50px;">
                             <table cellpadding="0" border="0" cellspacing="0" width="70%">
                                <tr><td colspan="2"><br/></td></tr>
                                <tr>
                                    <td class="Calibri14N" style="width:50%; padding-left:40px;">
                                        Component
                                        <asp:RequiredFieldValidator ID="V3" runat="server" ControlToValidate="ddlSelectComponent" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="UPDATE"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="Calibri14N">
                                        : <asp:DropDownList ID="ddlSelectComponent" runat="server" Width="150px" OnSelectedIndexChanged="ddlSelectComponent_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:50%; padding-left:40px;">
                                        Name
                                        <asp:RequiredFieldValidator ID="v2" runat="server" ControlToValidate="txtUComponentName" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="UPDATE"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="Calibri14N">
                                        <asp:UpdatePanel ID="up1" runat="server">
                                        <ContentTemplate>
                                        : <asp:TextBox ID="txtUComponentName" runat="server" Width="150px"></asp:TextBox>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlSelectComponent" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:50%; padding-left:40px;">
                                        Frequency
                                    </td>
                                    <td class="Calibri14N">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                        : <asp:DropDownList ID="ddlUFrequency" runat="server" Width="80px"></asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlSelectComponent" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:50%; padding-left:40px;">
                                        Start Month
                                    </td>
                                    <td class="Calibri14N">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                        : <asp:DropDownList ID="ddlUStartMonth" runat="server" Width="80px"></asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="ddlUStartYear" runat="server" Width="80px"></asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlSelectComponent" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center"><br />
                                        <asp:ImageButton ID="btnUpdate" runat="server" ImageUrl="~/Resources/submit.jpg"  ValidationGroup="UPDATE" OnClick="btnUpdate_Click"/>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

