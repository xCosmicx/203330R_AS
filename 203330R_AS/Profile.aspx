<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="_203330R_AS.Profile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div class="container-fluid border rounded container-bg">
        <br />
        <%-- Photo --%>
        <div class="form-group row">
            <label class="col-sm-2 col-form-label">Photo:</label>
            <div class="col-sm-10">
                <asp:Image ID="profile" Height="100" Width="100" runat="server" />
            </div>
        </div>

        <%-- Full Name --%>
		<div class="form-group row">
            <label class="col-sm-2 col-form-label">Name:</label>
            <div class="col-sm-10">
                <asp:Label ID="fullname" runat="server" Text="Label"></asp:Label>
            </div>
        </div>

		<%-- Credit Card --%>
		<div class="form-group row">
            <label class="col-sm-2 col-form-label">Credit Card:</label>
            <div class="col-sm-10">
                <asp:Label ID="creditcard" runat="server" Text="Label"></asp:Label>
            </div>
		</div>
			
		<%-- Email --%>
		<div class="form-group row">
            <label class="col-sm-2 col-form-label">Email:</label>
            <div class="col-sm-10">
                <asp:Label ID="email" runat="server" Text="Label"></asp:Label>
            </div>
        </div>

		<%-- Date of Birth --%>
        <div class="form-group row">
            <label class="col-sm-2 col-form-label">Date of Birth:</label>
            <div class="col-sm-10">
                <asp:Label ID="dateofbirth" runat="server" Text="Label"></asp:Label>
            </div>
        </div>

        <%-- Logout --%>
        <form id="form1" runat="server">
            <asp:Button ID="Logout" OnClick="logOut" runat="server" Text="Sign Out" />
        </form>
    </div>
</body>
</html>
