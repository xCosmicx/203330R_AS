using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Diagnostics;


namespace _203330R_AS
{
    public partial class Profile : System.Web.UI.Page
    {
        string AccountString = System.Configuration.ConfigurationManager.ConnectionStrings["AccountDB"].ConnectionString;
        byte[] Key;
        byte[] IV;
        byte[] encryptcreditcard = null;
        string login_email = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Email_Session"] != null && Session["Token"] != null && Request.Cookies["Token"] != null)
            {
                System.Diagnostics.Debug.WriteLine("Session Not Null");
                if (!Session["Token"].ToString().Equals(Request.Cookies["Token"].Value))
                {
                    System.Diagnostics.Debug.WriteLine("Session is not Equal");
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Invalid Access Permission, Redirecting to Login')", true);
                    logOut(sender, e);
                }
                else
                {
                    login_email = (string)Session["Email_Session"];
                    display(login_email);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Session is Null");
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Invalid Access Permission, Redirecting to Login')", true);
                logOut(sender, e);
            }
        }

        // Display Details
        protected void display(string login_email)
        {
            System.Diagnostics.Debug.WriteLine("Display");
            SqlConnection con = new SqlConnection(AccountString);
            string sql = "SELECT * FROM AccountDB WHERE Email=@USERID";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@USERID", login_email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["firstname"] != DBNull.Value && reader["lastname"] != DBNull.Value)
                        {
                            fullname.Text = reader["firstname"].ToString() + " " + reader["lastname"].ToString();
                        }
                        if (reader["creditcard"] != DBNull.Value)
                        {
                            encryptcreditcard = Convert.FromBase64String(reader["creditcard"].ToString());
                        }
                        if (reader["email"] != DBNull.Value)
                        {
                            email.Text = reader["email"].ToString();
                        }
                        if (reader["dateofbirth"] != DBNull.Value)
                        {
                            dateofbirth.Text = reader["dateofbirth"].ToString();
                        }
                        if (reader["profilepicture"] != DBNull.Value)
                        {
                            profile.Attributes["src"] = reader["profilepicture"].ToString();
                        }
                        if (reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());
                        }
                        if (reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());
                        }
                    }
                    creditcard.Text = decryptData(encryptcreditcard);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        // Decryption
        protected string decryptData(byte[] encryptcreditcard)
        {
            System.Diagnostics.Debug.WriteLine("Decryption");
            string plainText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform decrypt = cipher.CreateDecryptor();

                using (MemoryStream memorystreamDecrypt = new MemoryStream(encryptcreditcard))
                {
                    using (CryptoStream cryptostreamDecrypt = new CryptoStream(memorystreamDecrypt, decrypt, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamreaderDecrypt = new StreamReader(cryptostreamDecrypt))
                        {
                            plainText = streamreaderDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex) 
            { 
                throw new Exception(ex.ToString()); 
            }
            finally 
            {

            }
            return plainText;
        }

        // Logout
        protected void logOut(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Logout");
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);

            if (Request.Cookies[""] != null)
            {
                Response.Cookies["SessionID"].Value = string.Empty;
                Response.Cookies["SessionID"].Expires = DateTime.Now.AddMonths(-20);
                if (Request.Cookies["Token"] != null)
                {
                    Response.Cookies["Token"].Value = string.Empty;
                    Response.Cookies["Token"].Expires = DateTime.Now.AddMonths(-20);
                }

            }
        }
    }
}