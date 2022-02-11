<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="_203330R_AS.Homepage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid border rounded container-bg">
            <%-- Login --%>
			<div class="form-group row">
                <label class="col-sm-2 col-form-label">Login:</label>
                <div class="col-sm-10">
                    <asp:Button ID="login" OnClick="login_Click" Text="Login" runat="server" />
                </div>
            </div>
            <br />

			<%-- Register --%>
            <div class="form-group row">
                <label class="col-sm-2 col-form-label">Register:</label>
                <div class="col-sm-10">
                    <asp:Button ID="register" OnClick="register_Click" Text="Login" runat="server" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
