<%@ Page Title="CRUD Board | Registration | Hariom Soni" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="registration.aspx.cs" Inherits="Resume_Project_CRUD_Board.registration"  %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >

    <div class="container">
      <div class="row">
         <div class="col-md-6 mx-auto">
            <div class="card">
               <div class="card-body">
                <div class="row">
                     <div class="col">
                        <center>
                            <img width= "50px" src="imgs/regristration-removebg-preview.png" />
                         
                        </center>
                     </div>
         </div>
                  <div class="row">
                     <div class="col">
                        <center>
                           <h3>Sign Up</h3>
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
                         
                        <label>Name</label>
                        <div class="form-group" aria-busy="True" aria-disabled="False" >
                            
                           <asp:TextBox CssClass="form-control" ID="TextBox4" runat="server" placeholder="Full Name"></asp:TextBox>
                        </div>
                         <label>Organization</label>
                        <div class="form-group" >
                           <asp:TextBox CssClass="form-control" ID="TextBox5" runat="server" placeholder="ABC"></asp:TextBox>
                        </div>
                 
                         <label>Department</label>
                        <div class="form-group">
                           <asp:TextBox CssClass="form-control" ID="TextBox3" runat="server" placeholder="Ex. Product Manager"></asp:TextBox>
                        </div>
                         <div class="row">
                              <div class="col">
                         <br />
                        <center>
                           <span class="badge badge-pill badge-info" style="color: black">Login Credentials</span>
                        </center>
                     </div>
                  </div>
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
                        <div class="form-group">
                            <asp:Button class="btn btn-info btn-block btn-lg" ID="Sign_up_button"  Text="Submit" OnClick= "Sign_up_button_Click" runat="server" ValidateRequestMode="Enabled" CausesValidation="True" UseSubmitBehavior="False" ClientIDMode="AutoID" ViewStateMode="Enabled" ></asp:Button> 
                        </div>
                         </center>
                     </div>
                  </div>
                              
               </div>
            </div>
           
            <a href="homepage.aspx"  style="text-decoration: none; color: inherit;" onmouseover="this.style.color='blue';" onmouseout="this.style.color='inherit';"><< Back to Home</a><br><br>
         </div>
      </div>
   </div>


    <asp:Label ID="labelMessage" runat="server"></asp:Label>

</asp:Content>
