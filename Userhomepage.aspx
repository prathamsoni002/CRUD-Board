<%@ Page Title="CRUD Board | UserHomepage | Hariom Soni" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Userhomepage.aspx.cs" Inherits="Resume_Project_CRUD_Board.Userhomepage"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <br />
    <br />

  <header class="header-user-hp">
      <div class="welcome-section">
            <span>WELCOME! &nbsp  &nbsp &nbsp</span>
            <span id="user-name"> <asp:LinkButton ID="username_link" runat="server"  CssClass="custom-link-button" OnClick="username_link_Click" >Test</asp:LinkButton></span>
      </div>

            <div class=".button-section-header">
               <%-- <button type="button" class="btn btn-primary btn-lg login-btn" id="btn-header1" style="background-color:  #d3b754;  "><a href="joinboard.aspx"  style="text-decoration: none; color: inherit;" onmouseover="this.style.color='blue';" onmouseout="this.style.color='inherit';">Join Board</a></button>--%>
                <asp:LinkButton ID="joinButton" runat="server" CssClass="btn btn-primary btn-lg login-btn .custom-link-user-jump-buttons"  BackColor="#D3B754" BorderColor="Blue" ClientIDMode="Static" OnClick="joinButton_Click" >Join Board</asp:LinkButton>

               &nbsp&nbsp
                <asp:LinkButton ID="createBoardButton" runat="server" CssClass="btn btn-primary btn-lg login-btn .custom-link-user-jump-buttons" BackColor="#D3B754" BorderColor="Blue" ClientIDMode="Static" OnClick="createBoardButton_Click">Create Board</asp:LinkButton>
                
               &nbsp&nbsp
                <asp:LinkButton ID="logoutButton" runat="server" CssClass="btn btn-primary btn-lg login-btn .custom-link-user-jump-buttons" BackColor="#D3B754" BorderColor="Blue" ClientIDMode="Static" OnClick="logoutButton_Click">Logout</asp:LinkButton>

              
                
      </div>
</header>

     <div class="row">
        <div class="col">
        <br />
            <br />
     </div>
<center>
  <div class="outer-container">
    <asp:Repeater ID="boardRepeater" runat="server">
      <HeaderTemplate>
        <div class="row">
      </HeaderTemplate>
      <ItemTemplate>
        <div class="board-item-template">
          <div class="individual-covering">
            <div class="circle-board">
              <asp:LinkButton ID="boardNameLink" runat="server" CssClass="board-name" Text='<%# Eval("Board ID") %>' OnClick="boardNameLink_Click"></asp:LinkButton>
            </div>
            <div class="message-section">
              Members:&nbsp<%# Eval("memberNumber") %>
            </div>
            <div class="notification-section">
              Notifications:&nbsp<%# Eval("notifications") %>
            </div>
              <div class="leave-board-btn" onclick='showLeaveBoardConfirmationPopup("<%# Eval("Board ID") %>")'>
                X
            </div>
          </div>
        </div>
      </ItemTemplate>
      <FooterTemplate>
        </div>
      </FooterTemplate>
    </asp:Repeater>

    <asp:Panel ID="noBoardPanel" runat="server" Visible="false">
      <p><h4>No board has joined.</h4></p>
    </asp:Panel>
  </div>
</center>
    </div>
    <br />
    <br />

    <div id="leaveBoardModal" class="popup-container">
        <div class="popup-content">
            <span id="closePopupBtn" class="close-popup" onclick="closeLeaveBoardConfirmationPopup()">&times;</span>
            <h3 class="leaveBoard-statement-heading">Are you sure you want to leave the "<span class="BoardName-text" id="_boardname_"></span>" PERMANENTLY?</h3>
            <hr />
            <br />
            <p id="leaveBoard-statement-content">Confirm the Board Secret Key:</p>
            <asp:TextBox ID="deleteBoardSecretKeyInput" runat="server" TextMode="Password"></asp:TextBox>
            <div class="button-container-uh">
                <asp:Button ID="leaveBtn" runat="server" CssClass="leaveBtn-button" Text="YES" OnClick="leaveBtn_Click" UseSubmitBehavior="false" />
                <button id="cancelBtn" class="cancelBtn-button" onclick="closeLeaveBoardConfirmationPopup()">NO</button>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="boardNameHiddenField" runat="server" />

    <script>
        // Function to show the Leave Board Confirmation Popup
        function showLeaveBoardConfirmationPopup(boardname) {
            var leaveBoardModal = document.getElementById("leaveBoardModal");
            var boardNameText = leaveBoardModal.querySelector('.BoardName-text');
            boardNameText.textContent = boardname;
            leaveBoardModal.style.display = "block";

            var boardNameHiddenField = document.getElementById('<%= boardNameHiddenField.ClientID %>');
            boardNameHiddenField.value = boardname;
        }

        // Function to close the Leave Board Confirmation Popup
        function closeLeaveBoardConfirmationPopup() {
            var leaveBoardModal = document.getElementById("leaveBoardModal");
            leaveBoardModal.style.display = "none";
        }
    </script>

</asp:Content>
