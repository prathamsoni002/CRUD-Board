<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="loginpage.aspx.cs" Inherits="Resume_Project_CRUD_Board.loginpage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="login-window" class="container">
      <div class="row">
         <div class="col-md-6 mx-auto">
            <div class="card">
               <div class="card-body">
                <div class="row">
                     <div class="col">
                        <center>
                            <img width="95px" src="imgs/login%20image.png" />
                        </center>
                     </div>
                  </div>
                  <div class="row">
                     <div class="col">
                        <center>
                           <h3>Login</h3>
                        </center>
                     </div>
                  </div>
                  <div class="row">
                     <div class="col">
                        <hr>
                     </div>
                  </div>
                  <div class="row">
                     <div class="col">
                              <label>User Name</label>
                        <div class="form-group">
                           <asp:TextBox CssClass="form-control" ID="TextBox1" runat="server" placeholder="User Name"></asp:TextBox>
                        </div>
                        <label>Password</label>
                        <div class="form-group">
                           <asp:TextBox CssClass="form-control" ID="TextBox2" runat="server" placeholder="Password" TextMode="Password"></asp:TextBox>
                        </div>
                         <br />
                        <center>
                       <%-- <div class="form-group"> --%>
                            <asp:Button class="btn btn-success btn-block btn-lg" ID="Button1" runat="server" Text="Login" OnClick="Button1_Click" UseSubmitBehavior="false"></asp:Button>
                       <%-- </div> --%>
                         </center>
                         <br />
                         <center>
                             <%-- <div class="form-group"> --%>
                                 <asp:Button class="btn btn-info btn-block btn-lg" ID="Button2a" runat="server" Text="Sign Up" OnClick="Button2a_Click" UseSubmitBehavior="false"></asp:Button>
                              <%-- </div> --%>
                         </center>
                     </div>
                  </div>
              
               </div>
            </div>
           
            <a href="homepage.aspx"  style="text-decoration: none; color: inherit;" onmouseover="this.style.color='blue';" onmouseout="this.style.color='inherit';"><< Back to Home</a><br><br>
         </div>
      </div>
   </div>

</asp:Content>
