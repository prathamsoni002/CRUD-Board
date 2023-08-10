<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="userInformations.aspx.cs" Inherits="Resume_Project_CRUD_Board.userInformations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="user-information-window">
        <div class="container">
            <div class="row">
                <div class="col-md-6 mx-auto">
                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <div class="col">
                                    <center>
                                        <img width="100px" src="imgs/login%20image.png" />
                                    </center>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <center>
                                        <h3>Your Profile</h3>
                                    </center>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <hr />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <label>Name</label>
                                    <div class="form-group">
                                        <asp:TextBox CssClass="form-control" ID="TextBox4" runat="server" ></asp:TextBox>
                                    </div>
                                    <label>Organization</label>
                                    <div class="form-group">
                                        <asp:TextBox CssClass="form-control" ID="TextBox5" runat="server" ></asp:TextBox>
                                    </div>
                                    <label>Department</label>
                                    <div class="form-group">
                                        <asp:TextBox CssClass="form-control" ID="TextBox3" runat="server" ></asp:TextBox>
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
                                        <asp:TextBox CssClass="form-control" ID="TextBox1" runat="server" placeholder="User Name" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <label>Password</label>
                                    <div class="form-group">
                                        <asp:TextBox CssClass="form-control" ID="TextBox2" runat="server" placeholder="Password" TextMode="Password" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <br />
                                    <center>
                                        <div class="form-group">
                                            <asp:Button ID="updateButton" runat="server" Text="Update" OnClick="updateButton_Click" CssClass="btn btn-info btn-block btn-lg" />
                                        </div>
                                    </center>
                                </div>
                            </div>
                        </div>
                    </div>
                    <a href="Userhomepage.aspx" style="text-decoration: none; color: white;" onmouseover="this.style.color='blue';" onmouseout="this.style.color='white';"><< Back to Home</a><br /><br />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
