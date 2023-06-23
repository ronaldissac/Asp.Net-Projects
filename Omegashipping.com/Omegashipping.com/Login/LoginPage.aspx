<%@ Page Title="" Language="C#" MasterPageFile="~/Login/Login.Master" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="Omegashipping.com.Login.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="Login.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="details-section">
            <h2>Track Details</h2>
             <div class="track-section">
                <asp:TextBox ID="Track" runat="server" CssClass="textbox"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="TrackRequired" runat="server" ControlToValidate="Track" ValidationGroup="Tracking" ErrorMessage="required*"></asp:RequiredFieldValidator>
                <asp:Button ID="TrackButton" runat="server" Text="Track" OnClick="TrackButton_Click" ValidationGroup="Tracking" CssClass="button" />
            </div>
            <div class="gridview-container">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="FromLocation" HeaderText="From Location" />
                        <asp:BoundField DataField="ToLocation" HeaderText="To Location" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="Location" HeaderText="current Location" />
                    </Columns>
                </asp:GridView>
            </div>
           
        </div>

        <div class="login-section">
            <h2>Login</h2>
            <div class="row">
            <div class="col">
                 <asp:DropDownList ID="UserTypeDropdown" CssClass="dropdown" runat="server">
                    <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                    <asp:ListItem Text="User" Value="User"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div>
                <label>UserID:</label>
             <asp:TextBox ID="UserID" CssClass="textbox" Text="" runat="server"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserID" ValidationGroup="SubmitValidation" ErrorMessage="required*"></asp:RequiredFieldValidator>
            </div>
           <div>
              <label>Password:</label> <asp:TextBox ID="Password" CssClass="textbox" runat="server" TextMode="Password"></asp:TextBox></p>
            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ValidationGroup="SubmitValidation" ErrorMessage="required*"></asp:RequiredFieldValidator>
           </div>
            <asp:Button ID="LoginButton" runat="server" CssClass="button" OnClick="LoginButton_Click" ValidationGroup="SubmitValidation" Text="Login" />
            <p><asp:Label ID="LoginResultLabel" runat="server" Visible="false"></asp:Label></p>
        </div>
        </div>
    </div>
</asp:Content>
