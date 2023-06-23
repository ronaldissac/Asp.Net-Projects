<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="PaymentAdmin.aspx.cs" Inherits="Omegashipping.com.WebForm7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent1" runat="server">
     <link rel="stylesheet" type="text/css" href="../CSS/PaymentAdmin.css">
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
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function () {
            $('input[name="PaymentOption"]').change(function () {
                var selectedOption = $('input[name="PaymentOption"]:checked').val();
                $('.payment-section').hide();
                $('#' + selectedOption + 'Section').show();
            });
        });
    </script>
    <div class="maindiv">
        <div id="t1" class="blink-text" style="align-content:center">Payment Status</div><br /><hr />
        <div style="height:150px;"> 
           <label style="height:50px;" for="Track">Tracking ID:&nbsp;&nbsp; <asp:DropDownList ID="DropDownList1" CssClass="dropdown" runat="server"></asp:DropDownList>
               <asp:Button ID="Trackbtn" CssClass="Button2" runat="server" OnClick="Trackbtn_Click" Text="Search" ValidationGroup="SubmitValidation"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" style="color:crimson" runat="server" ControlToValidate="DropDownList1" ValidationGroup="SubmitValidation" ErrorMessage="required*"></asp:RequiredFieldValidator>
           </label>
            <asp:GridView ID="GridView1" runat="server"></asp:GridView>     
            
          </div> </div>
</asp:Content>
