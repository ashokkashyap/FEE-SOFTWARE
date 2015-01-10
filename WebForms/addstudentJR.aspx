<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="addstudentJR.aspx.cs" Inherits="WebForms_addstudentJR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script src="../Scripts/jquery-1.2.6.min.js" type="text/javascript"></script>
<script type="text/javascript">
//    $(document).ready(function () {
//        $('#Basic').slideUp();
//        $('#Family').slideUp();

//        $('#A').click(function () {
//            $('#Basic').toggle(2000);
//        });
//        $('#B').click(function () {
//            $('#Family').toggle(2000);
//        });
//    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="0" width="90%">
                <tr>
                    <td valign="bottom" align="left" style="color:#FFAB60; font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:18px; border-bottom:solid 2px #FFAB60;">
                        <div style="background-color:#FFFF99; border-top:2px solid #FFAB60; border-left:2px solid #FFAB60; border-right:2px solid #FFAB60; border-top-left-radius:25px 50px; border-top-right-radius:25px 50px; width:200px; height:25px; padding-left:8px;">
                            ADD STUDENT DETAILS
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left:10px;">
                    <br />
                        <div id="A" style="padding-top:5px; width:99%; background-color:GrayText; height:20px; color:White; border:1px solid black; padding-left:5px;">
                            <span style="font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:12px;">Basic Details</span>
                        </div>
                        <div id="Basic" style="height:300px; width:98%; padding-left:10px; border:1px dashed black;">
                            <table cellpadding="0" cellspacing="5" width="95%">
                            <tr><td colspan="4"><br /></td></tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        First Name
                                        <asp:RequiredFieldValidator ID="v1" runat="server" ControlToValidate="txtFirstName" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtFirstName" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Middle Name
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtMiddleName" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Last Name
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtLastName" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Admission #
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAdmissionNo" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtAdmissionNo" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Class
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSelectClass" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:DropDownList ID="ddlSelectClass" runat="server" Width="95%"></asp:DropDownList>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Roll #
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtRollNo" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Date of Admission
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAdmissionDate" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtAdmissionDate" runat="server" Width="75%"></asp:TextBox>
                                        <asp:ImageButton ID="CalAdmissionDate" runat="server" ImageUrl="~/Resources/calendarGreen.jpg" Height="18px" Width="18px" />
                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMMM-yyyy" PopupButtonID="CalAdmissionDate" TargetControlID="txtAdmissionDate" ></ajax:CalendarExtender>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Date of Birth
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtBirthDate" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtBirthDate" runat="server" Width="75%"></asp:TextBox>
                                        <asp:ImageButton ID="CalBirthDate" runat="server" ImageUrl="~/Resources/calendarGreen.jpg" Height="18px" Width="18px" />
                                        <ajax:CalendarExtender ID="CalE" runat="server" Format="dd-MMMM-yyyy" PopupButtonID="CalBirthDate" TargetControlID="txtBirthDate" ></ajax:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Gender
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:DropDownList ID="ddlSelectGender" runat="server" Width="95%">
                                            <asp:ListItem Text="MALE" Value="MALE" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="FEMALE" Value="FEMALE"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        # of Communication
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtCommunicationNo" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Religion
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:DropDownList ID="ddlSelectReligion" runat="server" Width="95%">
                                            <asp:ListItem Text="Select" Value="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Hindu" Value="Hindu"></asp:ListItem>
                                            <asp:ListItem Text="Muslim" Value="Muslim"></asp:ListItem>
                                            <asp:ListItem Text="Sikh" Value="Sikh"></asp:ListItem>
                                            <asp:ListItem Text="Christian" Value="Christian"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Caste
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtCaste" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Category
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:DropDownList ID="ddlSelectCategory" runat="server" Width="95%">
                                            <asp:ListItem Text="Select" Value="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="General" Value="General"></asp:ListItem>
                                            <asp:ListItem Text="SC" Value="SC"></asp:ListItem>
                                            <asp:ListItem Text="ST" Value="ST"></asp:ListItem>
                                            <asp:ListItem Text="OBC" Value="OBC"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Mother Tounge
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtMotherTounge" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        City
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtCity" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Address
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray; vertical-align:top;">
                                        : <asp:TextBox ID="txtAddress" runat="server" Width="95%" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div><br />
                        <div id="B" style="padding-top:5px; width:99%; background-color:GrayText; height:20px; color:White; border:1px solid black; padding-left:5px;">
                            <span style="font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:12px;">Family Details</span>
                        </div>
                        <div id="Family" style="height:250px; width:98%; padding-left:10px; border:1px dashed black;">
                            <table cellpadding="0" cellspacing="5" width="95%">
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Father Name
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFatherName" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtFatherName" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Father Occupation
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtFatherOccupation" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Father email
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtFatherEmail" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Father Mobile #
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtFatherMobile" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Father Organization
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtFatherOrganization" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Father Office #
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtFatherOfficeNo" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Mother Name
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtMotherName" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtMotherName" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Mother Occupation
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtMotherOccupation" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Mother email
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtMotherEmail" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Mother Mobile #
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtMotherMobile" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Mother Organization
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtMotheOrganization" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Mother Office #
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtMotherOfficeNo" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Emergency Contact Name
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtEmergencyName" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                    <td class="Calibri14N" style="width:15%; padding-left:10px; border-bottom:1px ridge gray;">
                                        Contact #
                                    </td>
                                    <td class="Calibri14N" style="width:30%; border-bottom:1px ridge gray;">
                                        : <asp:TextBox ID="txtEmergencyContactNo" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <br />
                        <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/Resources/submit.jpg" ValidationGroup="ADD" OnClick="btnAdd_Click"/>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

