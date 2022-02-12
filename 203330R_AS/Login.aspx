<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="_203330R_AS.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid border rounded container-bg">
            <%-- Email --%>
			<div class="form-group row">
                <label class="col-sm-2 col-form-label">Email:</label>
                <div class="col-sm-10">
                    <asp:TextBox required="true" ID="login_email" TextMode="Email" runat="server"></asp:TextBox>
                </div>
            </div>

			<%-- Password --%>
            <div class="form-group row">
                <label class="col-sm-2 col-form-label">Password:</label>
                <div class="col-sm-10">
                    <asp:TextBox required="true" ID="password" TextMode="Password" onkeyup="javascript:validate()"  runat="server"></asp:TextBox>
                </div>
            </div>

            <%-- 2FA --%>
            <div class="form-group row">
                <label class="col-sm-2 col-form-label">Google PIN:</label>
                <div class="col-sm-10">
                    <asp:TextBox required="true" ID="google_pin" runat="server"></asp:TextBox>
                </div>
            </div>

            <asp:Label ID="accountFeedback" runat="server"></asp:Label>

        </div>
         <br />
        <asp:Button ID="login" OnClick="Button1_Click" Text="Login" runat="server" />
    </form>
    <h5><a href="Registration.aspx">Click here to Register</a></h5>
</body>
</html>
