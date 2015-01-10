<%@ Page Title="" Language="C#" MasterPageFile="Site.master" AutoEventWireup="true" CodeFile="addDiscountDetails.aspx.cs" Inherits="WebForms_addDiscountDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <asp:Panel ID="mainPanel" runat="server">
            <table cellpadding="0" cellspacing="0" width="90%">
                <tr>
                    <td valign="bottom" align="left" style="color:#FFAB60; font-family:Calibri; letter-spacing:1px; font-weight:bold; font-size:18px; border-bottom:solid 2px #FFAB60;">
                        <div style="background-color:#FFFF99; border-top:2px solid #FFAB60; border-left:2px solid #FFAB60; border-right:2px solid #FFAB60; border-top-left-radius:25px 50px; border-top-right-radius:25px 50px; width:210px; height:25px; padding-left:8px;">
                            ADD DISCOUNT DETAILS
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="80%">
                            <tr><td colspan="2"><br/></td></tr>
                            <tr>
                                <td class="Calibri14N" style=" padding-left:20px; width:150px;">
                                    Discount Name
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDiscountName" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                </td>
                                <td class="Calibri14N">
                                    : <asp:TextBox ID="txtDiscountName" runat="server" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="Calibri14N" style=" padding-left:20px; width:150px;">
                                    Type
                                </td>
                                <td class="Calibri14N" valign="middle">
                                    <asp:RadioButtonList ID="rblDiscountType" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="FIX" Value="FIX" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="PERCENTAGE" Value="PERCENTAGE"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="Calibri14N" style=" padding-left:20px; width:150px;">
                                    Discount Value
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDiscountValue" ErrorMessage="*" ForeColor="Red" InitialValue="" ValidationGroup="ADD"></asp:RequiredFieldValidator>
                                </td>
                                <td class="Calibri14N">
                                    : <asp:TextBox ID="txtDiscountValue" runat="server" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr><td colspan="2"><br /></td></tr>
                            <tr>
                                <td align="left" style="padding-left:140px;" colspan="2">
                                    <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/Resources/submit.jpg"  ValidationGroup="ADD" OnClick="btnSubmit_Click"/>
                                </td>
                            </tr>
                            <tr><td colspan="2" class="Calibri14N" style=" padding-left:20px;">Discounts :</td></tr>
                            <tr>
                                <td colspan="2" class="Calibri14N" style=" padding-left:20px;">
                                    <asp:GridView ID="gvDiscounts" runat="server" AutoGenerateColumns="false" BackColor="LightGoldenrodYellow" BorderColor="#FFAB60" BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="Both" Width="70%">
                                        <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                        <FooterStyle BackColor="Tan" />
                                        <HeaderStyle BackColor="#FFFF99" Font-Bold="True" ForeColor="Black" Font-Size="16px"/>
                                        <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                        <SortedAscendingCellStyle BackColor="#FAFAE7" />
                                        <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                                        <SortedDescendingCellStyle BackColor="#E1DB9C" />
                                        <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>SNo.</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hfDiscountID" runat="server" Value='<%#Eval("DISCOUNT_ID") %>' />
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="NAME" DataField="DISCOUNT_NAME" ItemStyle-Width="30%"/>
                                            <asp:BoundField HeaderText="TYPE" DataField="DISCOUNT_TYPE"  ItemStyle-Width="30%"/>
                                            <asp:BoundField HeaderText="VALUE" DataField="DISCOUNT_VALUE"  ItemStyle-Width="30%"/>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>