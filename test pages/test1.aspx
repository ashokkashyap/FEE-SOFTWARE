<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site.master" AutoEventWireup="true" CodeFile="test1.aspx.cs" Inherits="test_pages_test1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:GridView ID="gv" runat="server">
        <Columns>
  <asp:TemplateField  ShowHeader="False">
  <HeaderTemplate>
    <input id="chkAll" onclick="javascript:SelectAllCheckboxes1(this);" runat="server" type="checkbox" />                                        
  </HeaderTemplate>
  <ItemTemplate>
    <asp:CheckBox ID="chk" runat="server" />
  </ItemTemplate>
  <ControlStyle CssClass="Label_Small" />
  </asp:TemplateField>                                
  <asp:BoundField DataField="ApplianceName" HeaderText="Appliance Name" ReadOnly="True" >
    <HeaderStyle HorizontalAlign="Left" />
  </asp:BoundField>                                                                        
</Columns>
    </asp:GridView>
   
    <script src='http://code.jquery.com/jquery-latest.min.js' type='text/javascript'> </script>
     <script>
         function SelectAllCheckboxes1(chk) {
             $('#<%=gv.ClientID%>').find("input:checkbox").each(function () {
                 if (this != chk) { this.checked = chk.checked; }
             });
         }
          </script>
</asp:Content>

