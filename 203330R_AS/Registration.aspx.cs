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
    public partial class Registration : System.Web.UI.Page
    {
        string AccountString = System.Configuration.ConfigurationManager.ConnectionStrings["AccountDB"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        public class MyObject
        {
            public string success { get; set; }
        }

        // Google Captcha
        public bool ValidateCaptcha()
        {
            System.Diagnostics.Debug.WriteLine("ValidateCaptcha");
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
           (" https://www.google.com/recaptcha/api/siteverify?secret=6LdC7lkeAAAAAKj5ObqYCtOMNqhUQp7jZXdyjTFI &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        result = Convert.ToBoolean(jsonObject.success);

                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        // Feedback on Names
        private bool checkName(string name)
        {
            name = HttpUtility.HtmlEncode(name);
            if (Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Feedback on Credit Card
        private bool checkCreditCard(string creditcard)
        {
            creditcard = HttpUtility.HtmlEncode(creditcard);
            if (Regex.IsMatch(creditcard, @"^\d+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Feedback on Email
        private int checkEmail(string email)
        {
            System.Diagnostics.Debug.WriteLine("Display");
            SqlConnection con = new SqlConnection(AccountString);
            string sql = "SELECT * FROM AccountDB WHERE Email=@USERID";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@USERID", email);
            con.Open();
            var present = cmd.ExecuteScalar();
            con.Close();
            System.Diagnostics.Debug.WriteLine(present);
            if (present != null)
            {
                return 2;
            }
            email = HttpUtility.HtmlEncode(email);
            if (Regex.IsMatch(email, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$"))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        // Feedback on Password
        private int checkPassword(string password)
        {
            int score = 0;
            if (password.Length < 12)
            {
                return 1;
            }
            else
            {
                score = 1;
            }

            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "(?=.*?[#?!@$%^&*-])"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "(?=.*?[<>()#&])"))
            {

                return 1;
            }

            return score;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Button");
            if (ValidateCaptcha())
            {
                System.Diagnostics.Debug.WriteLine("Not Bot");
                registerAccount();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Bot");
                registerAccount();
            }
        }

        // Register User Data
        protected void registerAccount()
        {
            System.Diagnostics.Debug.WriteLine("Register");
            bool firstName = checkName(firstname.Text);
            bool lastName = checkName(lastname.Text);
            bool creditCard = checkCreditCard(creditcard.Text);
            int scores = checkPassword(password.Text);
            int email_info = checkEmail(email.Text);
            string status = "";

            // Input Validation: First Name
            if (firstName == true)
            {
                firstnameFeedback.Text = "Valid First Name";
                firstnameFeedback.ForeColor = Color.Green;
            }
            else if (firstName == false)
            {
                firstnameFeedback.Text = "Invalid First Name";
                firstnameFeedback.ForeColor = Color.Red;
                return;
            }

            // Input Validation: Last Name
            if (lastName == true)
            {
                lastnameFeedback.Text = "Valid Last Name";
                lastnameFeedback.ForeColor = Color.Green;
            }
            else if (lastName == false)
            {
                lastnameFeedback.Text = "Invalid Last Name";
                lastnameFeedback.ForeColor = Color.Red;
                return;
            }

            // Input Validation: Credit Card
            if (creditCard == true)
            {
                creditcardFeedback.Text = "Valid Credit Card";
                creditcardFeedback.ForeColor = Color.Green;
            }
            else if (creditCard == false)
            {
                creditcardFeedback.Text = "Invalid Credit Card";
                creditcardFeedback.ForeColor = Color.Red;
                return;
            }

            // Input Validation: Email
            if (email_info == 1)
            {
                emailFeedback.Text = "Valid Email";
                emailFeedback.ForeColor = Color.Green;
            }
            else if (email_info == 0)
            {
                emailFeedback.Text = "Invalid Email";
                emailFeedback.ForeColor = Color.Red;
                return;
            }
            else if (email_info == 2)
            {
                emailFeedback.Text = "Email cannot be use";
                emailFeedback.ForeColor = Color.Red;
                return;
            }

            // Input Validation: Password
            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }
            passwordFeedback.Text = "Status: " + status;
            if (scores < 4)
            {
                passwordFeedback.ForeColor = Color.Red;
                return;
            }
            else
            {
                passwordFeedback.ForeColor = Color.Green;
                string ImagePath = "Photo/" + imageupload.FileName;
                int imgSize = imageupload.PostedFile.ContentLength;

                if (imageupload.PostedFile != null && imageupload.PostedFile.FileName != "")
                {
                    if (imageupload.PostedFile.ContentLength > 102400)
                    {
                        imagesize.Text = "Image Size is too large! Please keep it under 100MB.";
                        imagesize.ForeColor = Color.Red;
                    }
                    else
                    {
                        imageupload.SaveAs(Server.MapPath(ImagePath));
                        photo.ImageUrl = "~/" + ImagePath;

                        string pwd = password.Text.ToString().Trim();
                        RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
                        byte[] saltByte = new byte[8];
                        random.GetBytes(saltByte);
                        salt = Convert.ToBase64String(saltByte);
                        SHA512Managed hashing = new SHA512Managed();
                        string saltpassword = pwd + salt;
                        byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                        byte[] saltandhash = hashing.ComputeHash(Encoding.UTF8.GetBytes(saltpassword));
                        finalHash = Convert.ToBase64String(saltandhash);
                        RijndaelManaged cipher = new RijndaelManaged();
                        cipher.GenerateKey();
                        Key = cipher.Key;
                        IV = cipher.IV;
                        createAccount(ImagePath);
                        System.Diagnostics.Debug.WriteLine("Register Successfully");
                        string Name = firstname.Text + " " + lastname.Text;
                        System.Diagnostics.Debug.WriteLine(Name);
                        Response.Redirect("Pairing.aspx?Name=" + Name);
                    }
                }
                else
                {
                    imagesize.Text = "Please upload an image";
                    imagesize.ForeColor = Color.Red;
                }
            }
        }

        // Create Account
        protected void createAccount(string ImagePath)
        {
            System.Diagnostics.Debug.WriteLine("Create");
            try
            {
                using (SqlConnection con = new SqlConnection(AccountString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO AccountDB VALUES(@firstname,@lastname,@creditcard,@email,@dateofbirth,@hashpassword,@saltpassword,@profilepicture,@failcount,@lockout_time,@IV,@Key)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            System.Diagnostics.Debug.WriteLine(ImagePath);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@firstname", firstname.Text.Trim());
                            cmd.Parameters.AddWithValue("@lastname", lastname.Text.Trim());
                            cmd.Parameters.AddWithValue("@creditcard", Convert.ToBase64String(Encryption(creditcard.Text.Trim().ToString())));
                            cmd.Parameters.AddWithValue("@email", email.Text.Trim());
                            cmd.Parameters.AddWithValue("@dateofbirth", dateofbirth.Text.Trim());
                            cmd.Parameters.AddWithValue("@hashpassword", finalHash);
                            cmd.Parameters.AddWithValue("@saltpassword", salt);
                            cmd.Parameters.AddWithValue("@profilepicture", ImagePath);
                            cmd.Parameters.AddWithValue("@failcount", 0);
                            cmd.Parameters.AddWithValue("@lockout_time", DateTime.Now);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
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

        // Encryption
        protected byte[] Encryption(string data)
        {
            System.Diagnostics.Debug.WriteLine("Encryption");
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }
    }
}