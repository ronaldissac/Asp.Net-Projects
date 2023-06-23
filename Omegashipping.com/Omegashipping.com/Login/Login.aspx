<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Omegashipping.com.Login.Login1" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="Login.css" />
</head>
<body>
    <div class="header">
        <div>
            <img src="Logo.png" height="60" width="150" />
        </div>
        <div>
            <h1>Omega Shipping Agency PVT LTD</h1>
        </div>
    </div>

    <form id="form1" runat="server">
        <div class="container">
            <div class="details-section">
                 <div class="track-section">
                    <h2 style="text-align:center">Track Details</h2>
                    <asp:TextBox ID="Track" runat="server" CssClass="textbox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="TrackRequired" runat="server" ControlToValidate="Track" ValidationGroup="Tracking" ErrorMessage="required*"></asp:RequiredFieldValidator>
                    <asp:Button ID="TrackButton" runat="server" Text="Track" OnClick="TrackButton_Click" ValidationGroup="Tracking" CssClass="button" />
                </div>
            </div>

            <div class="login-section">
                <h2 style="text-align:center">Login</h2>
                <div class="row">
                    <div class="col">
                        <select class="dropdown" id="UserTypeDropdown" runat="server">
                            <option value="Admin">Admin</option>
                            <option value="User">User</option>
                        </select>
                    </div>
                    <div>
                        <asp:TextBox ID="UserID" runat="server" placeholder="     UserID"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserID" ValidationGroup="SubmitValidation" ErrorMessage="required*"></asp:RequiredFieldValidator>
                    </div>
                    <div>
                        <asp:TextBox ID="Password" runat="server" TextMode="Password" placeholder="     Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ValidationGroup="SubmitValidation" ErrorMessage="required*"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Button ID="LoginButton" runat="server" style="text-align:center" CssClass="button" OnClick="LoginButton_Click" ValidationGroup="SubmitValidation" Text="Login" />
                    <p><asp:Label ID="LoginResultLabel" runat="server" Visible="false"></asp:Label></p>
                </div>
            </div>
        </div>
        <div>
            <div class="gridview-container">
                    <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="FromLocation" HeaderText="From Location" />
                            <asp:BoundField DataField="ToLocation" HeaderText="To Location" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="Location" HeaderText="current Location" />
                        </Columns>
                    </asp:GridView>
                </div>
        </div>
        </form>``
       
</body>
</html>