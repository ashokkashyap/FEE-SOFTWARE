<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="Download_EWS_STUDENTS.aspx.cs" Inherits="WebForms_Download_EWS_STUDENTS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
<tr>
                    <td valign="bottom" align="left" style="color:#FFAB60; font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:18px; border-bottom:solid 2px #FFAB60;">
                        <div style="background-color:#FFFF99; border-top:2px solid #FFAB60; border-left:2px solid #FFAB60; border-right:2px solid #FFAB60; border-top-left-radius:25px 50px; border-top-right-radius:25px 50px; width:320px; height:25px; padding-left:8px;">
                            Download Ews Students  
                        </div>
                    </td>
                </tr>
<tr>
<td>
Select Class
    SectionWise</td>
<td>

    <asp:DropDownList ID="ddlclass" runat="server" AutoPostBack="True" 
        onselectedindexchanged="ddlclass_SelectedIndexChanged">
    </asp:DropDownList>
     <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="PrintGridData();" />
</td>
<td>

    &nbsp;</td>
</tr>
<tr>
<td>
Select Class
    </td>
<td>

    <asp:DropDownList ID="ddlclasswithoutsection" runat="server" 
        AutoPostBack="True" 
        onselectedindexchanged="ddlclasswithoutsection_SelectedIndexChanged">
    </asp:DropDownList> 
</td>
<td>

    &nbsp;</td>

</tr>
<tr>
<td>
Select category For student master
</td>
<td>
<asp:DropDownList ID="ddlcatogery" runat="server" AutoPostBack="True" 
        onselectedindexchanged="ddlcatogery_SelectedIndexChanged">
    <asp:ListItem>--Select--</asp:ListItem>
    <asp:ListItem>GENRAL</asp:ListItem>
    <asp:ListItem>EWS</asp:ListItem>
    <asp:ListItem>TUTION FEE(50%)</asp:ListItem>
    </asp:DropDownList>


    </td>
<td>
<asp:Button ID="btnsubmit" runat="server" Text="Submit" 
        onclick="btnsubmit_Click" />
  </td>


</tr>

<tr>
<td colspan="2">
 <asp:GridView ID="gvRecords" runat="server" AutoGenerateColumns="False" OnRowCreated="gvRecords_RowCreated"
                         BackColor="White" BorderColor="#FFAB60" BorderStyle="None" HeaderStyle-Font-Size="Larger" BorderWidth="1px" ShowFooter="true"
                            CellPadding="1" ForeColor="Black" GridLines="Both" Width="100%" >
                            <Columns>
                               
                                <asp:TemplateField HeaderText="SNO">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="STUDENT_REGISTRATION_NBR" HeaderText="A.NO" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="NAME" HeaderText="STUDENT NAME" ItemStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="CLASS_NAME" HeaderText="CLASS" ItemStyle-HorizontalAlign="Left"/>
                                 
                                   <asp:BoundField DataField="FATHER_NAME" HeaderText="FATHER NAME" ItemStyle-HorizontalAlign="Left"/>
                                 <asp:BoundField DataField="MOTHER_NAME" HeaderText="MOTHER NAME" ItemStyle-HorizontalAlign="Left"/>

                                <asp:BoundField DataField="GENDER" HeaderText="GENDER" ItemStyle-HorizontalAlign="Left"/>
                                 <asp:BoundField DataField="adm_date" HeaderText="ADM DATE" ItemStyle-HorizontalAlign="Left"/>
                               
                                 <asp:BoundField DataField="BRTH_date" HeaderText="BIRTH DATE" ItemStyle-HorizontalAlign="Left"/>

                                <asp:BoundField DataField="ADDRESS_LINE1" HeaderText="ADDRESH" ItemStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="NO_OF_COMMUNICATION" HeaderText="NUMBER" ItemStyle-HorizontalAlign="Left"/>

                               
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" />
                                            <HeaderStyle BackColor="#E6E6FA" Font-Size="X-Small" Font-Bold="true" ForeColor="black" />
                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                            <RowStyle BackColor="#F7F7DE" Font-Size="X-Small" Font-Names="calibri" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="FALSE" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                            <SortedAscendingHeaderStyle BackColor="#848384" />
                                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                            <SortedDescendingHeaderStyle BackColor="#575357" />
                                        </asp:GridView>
</td>
</tr>
    
<tr>

<td>
    
   
    </td>
     </tr>
</table>

  <script type="text/javascript">
      function PrintGridData() {
          var prtGrid = document.getElementById('<%=gvRecords.ClientID %>');
          var prtwin = window.open('', 'PrintGridView',
'left=0,top=0,width=1,height=1,tollbar=0,scrollbars=1,status=0');
          prtwin.document.write(prtGrid.outerHTML);
          prtwin.document.close();
          prtwin.focus();
          prtwin.print();
          prtwin.close();
      }
   </script>
</asp:Content>

