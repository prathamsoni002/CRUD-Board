<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="createboard.aspx.cs" Inherits="Resume_Project_CRUD_Board.createboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <br />
          <br />
          <br />

      <header class="header-user-hp">

      <div class="welcome-section">
            <span> <a href="Userhomepage.aspx" style="text-decoration: none; color: inherit;"><img width ="50px" src="imgs/back%20icon.png" /> </a></span>
            <center>
            <span style="margin-left:585px;"> Let's Create a new Board &nbsp  &nbsp &nbsp</span>
                </center>
      </div>
          
</header>
          <br />
          <br />
          <br />

    <div class="joinboard-container">
        <div class="form-container">
            <div class="form-row">
                  <div class="label">Board ID:</div>
                <asp:TextBox CssClass="form-control" ID="TextBox1" runat="server"></asp:TextBox>
                  
            </div>
            <div class="form-row">
                  <div class="label">Board Secret Key:</div>
                  <asp:TextBox CssClass="form-control" ID="TextBox2" runat="server"  TextMode="Password"></asp:TextBox>
            </div>
            <div class="form-row">
                  <div class="label">Conform your Board Secret Key:</div>
                  <asp:TextBox CssClass="form-control" ID="TextBox3" runat="server"  TextMode="Password"></asp:TextBox>
            </div>
            <center>
                
                <asp:Button  runat="server" Text="Create Board" ID="create_board_button" CssClass="btn btn-primary btn-lg login-btn .custom-link-user-jump-buttons" BackColor="#D3B754" BorderColor="Blue" ClientIDMode="Static" OnClick="create_board_button_Click" ></asp:Button>
            </center>
         </div>
    </div>

     <br />
          <br />
          <br />



</asp:Content>
