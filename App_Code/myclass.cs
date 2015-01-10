using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;

/// <summary>
/// Summary description for Class1
/// </summary>
public class myclass
{
    public static void EnableForm(ControlCollection Controls)
    {

        foreach (System.Web.UI.Control ctl in Controls)
        {
            if (ctl != null)
            {
                if (ctl.ToString().ToLower().EndsWith("textbox"))
                    ((System.Web.UI.WebControls.TextBox)ctl).Enabled = true;
                else if (ctl.ToString().ToLower().EndsWith("dropdownlist"))
                    ((System.Web.UI.WebControls.DropDownList)ctl).Enabled = true;
                else if (ctl.ToString().ToLower().EndsWith("radiobuttonlist"))
                    ((System.Web.UI.WebControls.RadioButtonList)ctl).Enabled = true;
                else if (ctl.ToString().ToLower().EndsWith("checkbox"))
                    ((System.Web.UI.WebControls.CheckBox)ctl).Enabled = true;
                else if (ctl.ToString().ToLower().EndsWith("radiobutton"))
                    ((System.Web.UI.WebControls.RadioButton)ctl).Enabled = true;
                else if (ctl.ToString().ToLower().EndsWith("listbox"))
                    ((System.Web.UI.WebControls.ListBox)ctl).Enabled = true;
                if (ctl.HasControls())
                {
                    EnableForm(ctl.Controls);
                }

            }
        }
        return;
    }
    public static void DisableForm(ControlCollection Controls)
    {

        foreach (System.Web.UI.Control ctl in Controls)
        {
            if (ctl != null)
            {
                if (ctl.ToString().ToLower().EndsWith("textbox"))
                    ((System.Web.UI.WebControls.TextBox)ctl).Enabled = false;
                else if (ctl.ToString().ToLower().EndsWith("dropdownlist"))
                    ((System.Web.UI.WebControls.DropDownList)ctl).Enabled = false;
                else if (ctl.ToString().ToLower().EndsWith("radiobuttonlist"))
                    ((System.Web.UI.WebControls.RadioButtonList)ctl).Enabled = false;
                else if (ctl.ToString().ToLower().EndsWith("checkbox"))
                    ((System.Web.UI.WebControls.CheckBox)ctl).Enabled = false;
                else if (ctl.ToString().ToLower().EndsWith("radiobutton"))
                    ((System.Web.UI.WebControls.RadioButton)ctl).Enabled = false;
                else if (ctl.ToString().ToLower().EndsWith("listbox"))
                    ((System.Web.UI.WebControls.ListBox)ctl).Enabled = false;

                if (ctl.HasControls())
                {
                    DisableForm(ctl.Controls);
                }

            }
        }
        return;
    }
    public static void ResetForm(ControlCollection Controls)
    {

        foreach (System.Web.UI.Control ctl in Controls)
        {
            if (ctl != null)
            {
                if (ctl.ToString().ToLower().EndsWith("textbox"))
                    ((System.Web.UI.WebControls.TextBox)ctl).Text = string.Empty;
                else if (ctl.ToString().ToLower().EndsWith("checkbox"))
                    ((System.Web.UI.WebControls.CheckBox)ctl).Checked = false;
                else if (ctl.ToString().ToLower().EndsWith("checkboxlist"))
                    ((System.Web.UI.WebControls.CheckBoxList)ctl).SelectedIndex = -1;
                else if (ctl.ToString().ToLower().IndexOf("dropdownlist") >= 0)
                    ((System.Web.UI.WebControls.DropDownList)ctl).SelectedIndex = -1;

                if (ctl.HasControls())
                {
                    ResetForm(ctl.Controls);
                }

            }
        }
        return;
    }

    public static string padlc(Int64 Number, int totalcharactes)
    {
        String result = "";
        Int64 temp = Number;
        for (int i = 1; i < totalcharactes; i++)
        {
            temp /= 10;
            if (temp == 0)
                result += "0";
        }
        result += Number.ToString();
        return result;
    }
    public static DataTable searchDataTable(string searchText, DataTable input)
    {
        DataTable output = input.Clone();
        foreach (DataColumn dc in input.Columns)
        {
            if (dc.ColumnName.ToUpper().Contains(searchText.ToUpper()))
            {
                return input;
            }
        }
        foreach (DataRow dr in input.Rows)
        {
            for (int i = 0; i < input.Columns.Count; i++)
            {
                if (dr[i].ToString().ToUpper().Contains(searchText.ToUpper()))
                {
                    DataRow drnew = output.NewRow();
                    drnew.ItemArray = dr.ItemArray;
                    output.Rows.Add(drnew);
                    break;
                }
            }
        }
        return output;
    }
    
}