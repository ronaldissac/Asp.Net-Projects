<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="StatusAdmin.aspx.cs" Inherits="Omegashipping.com.Admin.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent1" runat="server">
    <link rel="stylesheet" type="text/css" href="../CSS/Status.css">
    <style>
        @keyframes blink {
            0% { opacity: 1; }
            50% { opacity: 0; }
            100% { opacity: 1; }
        }

        .blink-text {
            animation: blink 1.5s infinite;
        }
    </style>
    <div class="maindiv">
        <div id="t1" class="blink-text" style="align-content:center">Add Tracking Details</div><br /><hr />
        <div class="row">
            <div class="col-md-2">
                <label for="Track">Tracking ID: </label>
            </div>
            <div class="col-md-4">
                <asp:DropDownList ID="Track" CssClass="dropdown" style="width:100%"  OnSelectedIndexChanged="Track_SelectedIndexChanged" runat="server">
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label for="Status">Status:&nbsp;</label>
            </div>
            <div class="col-md-4">
                <asp:DropDownList ID="Status" style="width:100%" runat="server">
                    <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Rejected" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Pending" Value="3"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" style="color:crimson" runat="server" ControlToValidate="Status" ValidationGroup="SubmitValidation" ErrorMessage="required*"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2 text-right">
                <label for="From">From:&nbsp;</label>
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="From" Enabled="false" Style="width: 100%" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-2 text-right">
                <label for="To">To:&nbsp;</label>
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="To" style="width:100%" Enabled="false" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2 text-right">
                <label for="Payment">Payment:&nbsp;</label>
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="Payment" Enabled="false" style="width:100%" runat="server"></asp:TextBox>
               
            </div>
            <div class="col-md-2 text-right">
                <label for="Estimation">Estimation:&nbsp;</label>
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="Estimation" style="width:100%" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" style="color:crimson" runat="server" ControlToValidate="Estimation" ValidationGroup="Estimation" ErrorMessage="required*"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2 text-right">
                <label for="Vessel">Vessel:&nbsp;</label>
            </div>
            <div class="col-md-4">
                <asp:DropDownList ID="Vessel" style="width:100%" runat="server">
                    <asp:ListItem Text="Wan hai" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Evergreen Line" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Hyundai Merchant Marine" Value="3"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" style="color:crimson" runat="server" ControlToValidate="Vessel" ValidationGroup="SubmitValidation" ErrorMessage="required*"></asp:RequiredFieldValidator>
            </div>
            <div class="col-md-2 text-right">
                <label for="Location">Location:&nbsp;</label>
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="Location" style="width:100%" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" style="color:crimson" runat="server" ControlToValidate="Location" ValidationGroup="SubmitValidation" ErrorMessage="required*"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div>
            <asp:Button ID="Button1" runat="server" CssClass="Button1" ValidationGroup="SubmitValidation" Text="Update" OnClick="Button1_Click" />
        </div>
    </div>
</asp:Content>
