using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.IO;
using System.Net;
using System.Web;
/// <summary>
/// Summary description for SendSmsClass
/// </summary>
public class SendSmsClass
{
    string varSessionUserName, varSessionScode, varRole;
    public SendSmsClass()
    {
       
    }
    public void SMSSend(string mobileno, string msgtext, string username, string password, string sender)
    {
        WebClient client = new WebClient();
        string baseurl = "";
         username = "litchi";
        {
            
            baseurl = "http://174.143.34.193/MtSendSMS/SingleSMS.aspx?usr=" + username + "&pass=ign@sms&msisdn=" + mobileno + "&msg=" + msgtext + "&sid=HHPSRN &mt=0";

        }

        Stream data = client.OpenRead(baseurl);
        StreamReader reader = new StreamReader(data);
        string s = reader.ReadToEnd();
        data.Close();
        reader.Close();
    }

    public void emailSend(string email_id, string email_txt, string school_website)
    {
       
        
       
    }
}
