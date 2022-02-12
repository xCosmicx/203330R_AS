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

namespace _203330R_AS
{
    public partial class Login : System.Web.UI.Page
    {
        string AccountString = System.Configuration.ConfigurationManager.ConnectionStrings["AccountDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Login
        protected void Button1_Click(object sender, EventArgs e)
        {
            var pin = google_pin.Text;
            var secretCode = "53cr3t1v3";
            var api = new GoogleAuthenticatorAPI();
            var verify = api.ValidatePin(pin, secretCode);

            string email = login_email.Text.ToString().Trim();
            string pwd = password.Text.ToString().Trim();

            var numofFail = Fail_Count(email);
            if (numofFail == 3)
            {
                var dateofFail = getDateTime(email);
                var unlockAccount = dateofFail.AddSeconds(60);
                var currentTime = DateTime.Now;
                if (unlockAccount > currentTime)
                {
                    accountFeedback.Text = "Account has been locked, Please try again later!";
                    accountFeedback.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    UpdateFail_Count(email, 0, dateofFail);
                }
            }
            System.Diagnostics.Debug.WriteLine("Login");
            SHA512Managed hashing = new SHA512Managed();
            string hash = getHashDB(email);
            string salt = getSaltDB(email);
            try
            {
                if (salt != null && salt.Length > 0 && hash != null && hash.Length > 0)
                {
                    string pwd_and_salt = pwd + salt;
                    byte[] hash_and_Salt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd_and_salt));
                    string ehash = Convert.ToBase64String(hash_and_Salt);
                    if (ehash.Equals(hash) && verify == true)
                    {
                        Session["Email_Session"] = email;

                        string guid = Guid.NewGuid().ToString();
                        Session["Token"] = guid;

                        Response.Cookies.Add(new HttpCookie("Token", guid));

                        var countfail = Fail_Count(email);
                        countfail = 0;
                        UpdateFail_Count(email, countfail, DateTime.Now);

                        System.Diagnostics.Debug.WriteLine("Login Successfully");
                        Response.Redirect("Profile.aspx", false);
                    }
                    else
                    {
                        var countfail = Fail_Count(email);
                        countfail += 1;
                        if (countfail < 3)
                        {
                            accountFeedback.Text = "Invalid Email and/or Password and/or PIN!";
                            accountFeedback.ForeColor = Color.Red;
                            UpdateFail_Count(email, countfail, DateTime.Now);
                        }
                        else
                        {
                            accountFeedback.Text = "Account has been locked, Please try again later!";
                            accountFeedback.ForeColor = Color.Red;
                            DateTime failDate = DateTime.Now;
                            countfail = 10;
                            UpdateFail_Count(email, countfail, failDate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
        }

        // Get Salt
        protected string getSaltDB(string email)
        {
            System.Diagnostics.Debug.WriteLine("SaltDB");
            string salt = null;
            SqlConnection con = new SqlConnection(AccountString);
            string sql = "select saltpassword FROM ACCOUNTDB WHERE Email=@USERID";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@USERID", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["saltpassword"] != null)
                        {
                            if (reader["saltpassword"] != DBNull.Value)
                            {
                                salt = reader["saltpassword"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { con.Close(); }
            return salt;
        }

        // Get Hash
        protected string getHashDB(string email)
        {
            System.Diagnostics.Debug.WriteLine("HashDB");
            string hash = null;
            SqlConnection con = new SqlConnection(AccountString);
            string sql = "SELECT hashpassword FROM AccountDB WHERE Email=@USERID";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@USERID", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["hashpassword"] != null)
                        {
                            if (reader["hashpassword"] != DBNull.Value)
                            {
                                hash = reader["hashpassword"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { con.Close(); }
            return hash;
        }

        protected void UpdateFail_Count(string email, int numofFail, DateTime dt)
        {
            System.Diagnostics.Debug.WriteLine("Update Fail Count");
            try
            {
                using (SqlConnection con = new SqlConnection(AccountString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE AccountDB SET failcount=@failcount,lockout_time=@lockout_time WHERE email=@email"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@failcount", numofFail);
                            cmd.Parameters.AddWithValue("@lockout_time", dt);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        protected int Fail_Count(string email)
        {
            System.Diagnostics.Debug.WriteLine("Fail Count");
            int fc = 0;
            SqlConnection con = new SqlConnection(AccountString);
            string sql = "SELECT failcount FROM AccountDB WHERE Email=@USERID";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@USERID", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["failcount"] != null)
                        {
                            if (reader["failcount"] != DBNull.Value)
                            {
                                fc = (int)reader["failcount"];
                            }
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
                con.Close();
            }
            return fc;
        }

        protected DateTime getDateTime(string email)
        {
            DateTime date = DateTime.Now;
            SqlConnection con = new SqlConnection(AccountString);
            string sql = "SELECT lockout_time FROM AccountDB WHERE Email=@USERID";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@USERID", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["lockout_time"] != null)
                        {
                            if (reader["lockout_time"] != DBNull.Value)
                            {
                                date = (DateTime)reader["lockout_time"];
                            }
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
                con.Close();
            }
            return date;
        }
    }
}