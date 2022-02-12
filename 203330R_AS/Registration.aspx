<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="_203330R_AS.Registration" %>

<script src="//code.jquery.com/jquery-1.11.2.min.js" type="text/javascript"></script>
	<script type="text/javascript">
        function Image(image) {
            if (image.files && image.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#photo').attr('src', e.target.result).width(100).height(100);
                }
                reader.readAsDataURL(image.files[0]);
            }
        };

        function passwordvalidate() {
            var string = document.getElementById('<%=password.ClientID%>').value;
            if (string.search(/(?=.*?[<>()#&])/) == false) {
                document.getElementById("passwordFeedback").innerHTML = "Invalid Symbol";
                document.getElementById("passwordFeedback").style.color = "Red";
                return ("invalid_symbol")
            }
            else if (string.length < 12) {
                document.getElementById("passwordFeedback").innerHTML = "Password Length must be at least 12 characters";
                document.getElementById("passwordFeedback").style.color = "Red";
                return ("too_short");
            }
            else if (string.search(/[0-9]/) == -1) {
                document.getElementById("passwordFeedback").innerHTML = "Password require at least 1 number";
                document.getElementById("passwordFeedback").style.color = "Red";
                return ("no_number");
            }
            else if (string.search(/[A-Z]/) == -1) {
                document.getElementById("passwordFeedback").innerHTML = "Password require at least a Uppercase characters";
                document.getElementById("passwordFeedback").style.color = "Red";
                return ("no_uppercase");
            }
            else if (string.search(/[a-z]/) == -1) {
                document.getElementById("passwordFeedback").innerHTML = "Password require at least a Lowercase characters";
                document.getElementById("passwordFeedback").style.color = "Red";
                return ("no_lowercase");
            }
            else if (string.search(/(?=.*?[?!@$%^*-])/) == -1) {
                document.getElementById("passwordFeedback").innerHTML = "Password require at least a Special characters";
                document.getElementById("passwordFeedback").style.color = "Red";
                return ("no_specialchar");
            }

            document.getElementById("passwordFeedback").innerHTML = "Excellent!";
            document.getElementById("passwordFeedback").style.color = "Green";

        };

        function emailvalidate() {
            var string = document.getElementById('<%=email.ClientID%>').value;

            if (string.search(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/)) {
                document.getElementById("emailFeedback").innerHTML = "Invalid Email!";
                document.getElementById("emailFeedback").style.color = "Red";
                return ("invalid_email");
            }
            document.getElementById("emailFeedback").innerHTML = "Valid Email!";
            document.getElementById("emailFeedback").style.color = "Green";
        };

        function firstnamevalidate() {
            var string = document.getElementById('<%=firstname.ClientID%>').value;

            if (string.search(/^[a-zA-Z\s]+$/)) {
                document.getElementById("firstnameFeedback").innerHTML = "Invalid First Name!";
                document.getElementById("firstnameFeedback").style.color = "Red";
                return ("invalid_firstname");
            }
            document.getElementById("firstnameFeedback").innerHTML = "Valid First Name!";
            document.getElementById("firstnameFeedback").style.color = "Green";
        };

        function lastnamevalidate() {
            var string = document.getElementById('<%=lastname.ClientID%>').value;

            if (string.search(/^[a-zA-Z\s]+$/)) {
                document.getElementById("lastnameFeedback").innerHTML = "Invalid Last Name!";
                document.getElementById("lastnameFeedback").style.color = "Red";
                return ("invalid_lastname");
            }
            document.getElementById("lastnameFeedback").innerHTML = "Valid Last Name!";
            document.getElementById("lastnameFeedback").style.color = "Green";
        };

        function creditcardvalidate() {
            var string = document.getElementById('<%=creditcard.ClientID%>').value;

            if (string.search(/^\d+$/)) {
                document.getElementById("creditcardFeedback").innerHTML = "Invalid Credit Card!";
                document.getElementById("creditcardFeedback").style.color = "Red";
                return ("invalid_creditcard");
            }
            document.getElementById("creditcardFeedback").innerHTML = "Valid Credit Card!";
            document.getElementById("creditcardFeedback").style.color = "Green";
        };

        grecaptcha.ready(function () {
            grecaptcha.execute('SECRET', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration</title>
	<script src="https://www.google.com/recaptcha/api.js?render=SECRET"></script>
</head>
<body>
    <form id="form1" runat="server">
		<div class="container-fluid border rounded container-bg">

			<%-- First Name --%>
			<div class="form-group row">
                <label class="col-sm-2 col-form-label">First Name:</label>
                <div class="col-sm-10">
                    <asp:TextBox required="true" ID="firstname" onkeyup="javascript:firstnamevalidate()" runat="server"></asp:TextBox>
                </div>
                <asp:Label ID="firstnameFeedback" runat="server"></asp:Label>
            </div>

			<%-- Last Name --%>
			<div class="form-group row">
                <label class="col-sm-2 col-form-label">Last Name:</label>
                <div class="col-sm-10">
                    <asp:TextBox required="true" ID="lastname" onkeyup="javascript:lastnamevalidate()" runat="server"></asp:TextBox>
                </div>
                <asp:Label ID="lastnameFeedback" runat="server"></asp:Label>
            </div>

			<%-- Credit Card --%>
			<div class="form-group row">
                <label class="col-sm-2 col-form-label">Credit Card:</label>
                <div class="col-sm-10">
                    <asp:TextBox required="true" ID="creditcard" MaxLength="16" onkeyup="javascript:creditcardvalidate()" runat="server"></asp:TextBox>
                </div>
                <asp:Label ID="creditcardFeedback" runat="server"></asp:Label>
            </div>
			
			<%-- Email --%>
			<div class="form-group row">
                <label class="col-sm-2 col-form-label">Email:</label>
                <div class="col-sm-10">
                    <asp:TextBox required="true" ID="email" TextMode="Email" onkeyup="javascript:emailvalidate()" runat="server"></asp:TextBox>
                </div>
                <asp:Label ID="emailFeedback" runat="server"></asp:Label>
            </div>

			<%-- Password --%>
            <div class="form-group row">
                <label class="col-sm-2 col-form-label">Password:</label>
                <div class="col-sm-10">
                    <asp:TextBox required="true" ID="password" TextMode="Password" onkeyup="javascript:passwordvalidate()"  runat="server"></asp:TextBox>
                </div>
                <asp:Label ID="passwordFeedback" runat="server"></asp:Label>
            </div>

			<%-- Date of Birth --%>
			<div class="form-group row">
                <label class="col-sm-2 col-form-label">Date of Birth:</label>
                <div class="col-sm-10">
                    <asp:TextBox required="true" ID="dateofbirth" TextMode="Date" runat="server"></asp:TextBox>
                </div>
            </div>

			<%-- Photo --%>
			<div class="form-group row">
                <label class="col-sm-2 col-form-label">Photo:</label>
                <div class="col-sm-10">
                    <asp:FileUpload ID="imageupload" runat="server" onchange="Image(this);" />
					<asp:Label ID="imagesize" runat="server"></asp:Label>
					<hr />
					<asp:Image ID="photo" Height="100px" Width="100px" runat="server"/>
                </div>
            </div>

			<%-- reCaptcha --%>
			<div class="form-group row">
                <div class="col-sm-10">
					<input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
                </div>
            </div>
		</div>
        <br />
		<asp:Button ID="register" OnClick="Button1_Click" Text="Register" runat="server" />
	</form>
    <h5><a href="Login.aspx">Click here to Login</a></h5>
</body>
</html>