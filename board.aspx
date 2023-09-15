<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="board.aspx.cs" Inherits="Resume_Project_CRUD_Board.board" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <header class="header-board-hp">
        <div class="header-left">
            <a href="Userhomepage.aspx" style="text-decoration: none; color: inherit;">
                <i class="fa-solid fa-circle-left fa-2xl"></i>
            </a>
        </div>
        <div class="header-center">
            <asp:Label runat="server" ID="boardIdLabel" CssClass="board-id"></asp:Label>
        </div>
        <div class="header-right">
            <asp:Button ID="addStatementButton" runat="server" CssClass="add-statement-button" Text="Add Statement" OnClientClick="event.preventDefault(); openAddStatementPopup();" />
        </div>
    </header>
    <br />
    <center>
        <div class="outer-container">
            <asp:Repeater ID="statementRepeater" runat="server">
                <HeaderTemplate>
                    <div class="statement-container">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="Internal-statement-container">
                        <span class="statement">
                            <span class="statement-link" style="cursor: pointer;" onclick='openStatementDescriptionPopup("<%# Eval("Description") %>")'><%# Eval("Title") %></span>
                        </span>
                        <span class="statement-by" name="statementByLabel">
                            <%# Eval("CreatedBy") %>
                        </span>
                        <span class="timestamp" name="timestampByLabel">
                            <%# Eval("CreatedAt") %>
                        </span>
                        <span class="image-container">
                            <i class="fa-solid fa-trash-can fa-2xl delete-icon" data-statement='<%# Eval("Title") %>' data-statementBy='<%# Eval("CreatedBy") %>' data-timestamp='<%# Eval("CreatedAt") %>' data-statementID='<%# Eval("StatementID") %>'></i>
                            <i class="fa-sharp fa-solid fa-pen-to-square fa-2xl" onclick='openEditStatementPopup("<%# Eval("Title") %>","<%# Eval("Description") %>","<%# Eval("CreatedBy") %>","<%# Eval("CreatedAt") %>","<%# Eval("StatementID") %>")'></i>
                            &nbsp;&nbsp;&nbsp;
                        </span>
                        <span onclick='openshowUpdatePopup("<%# Eval("UpdatedBy") %>","<%# Eval("UpdatedAt") %>")'>
                            <%# Eval("UpdatedBy") != DBNull.Value ? "<span class='updated-by'>" + "Updated By: " + Eval("UpdatedBy") + "</span><br/>" : "" %>
                            <%# Eval("UpdatedAt") != DBNull.Value ? "<span class='updated-at'>" + "Updated At: " + Eval("UpdatedAt") + "</span>" : "" %>
                        </span>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Panel ID="noStatementsPanel" runat="server" Visible="false">
                <div class="no-statements-container">
                    No Statements on the Board. Click the Add button to write the first statement of the board.
                </div>
            </asp:Panel>
        </div>
    </center>
    <br />
    <!-- Add Statement Popup -->
    <div id="addStatementPopup" class="popup-container">
        <div class="popup-content">
            <span id="closePopupBtn" class="close-popup">&times;</span>
            <h3 class="add-statement-heading">Add Statement</h3>
            <label class="add-statement-label" for="statementInput">Statement (MAX 100 characters):</label>
            <textarea id="statementInput" class="add-statement-input" maxlength="100" rows="4" required name="statementInput"></textarea>
            <label class="add-statement-label" for="descriptionInput">Description (MAX 1000 characters):</label>
            <textarea id="descriptionInput" class="add-statement-input" maxlength="1000" required name="descriptionInput"></textarea>
            <asp:Button class="add-statement-button" ID="addButton" runat="server" Text="ADD" OnClick="addButton_Click" UseSubmitBehavior="false"></asp:Button>
        </div>
    </div>
    <!-- Edit Statement Popup -->
    <div id="editStatementPopup" class="popup-container">
        <div class="popup-content">
            <span id="closeEditPopupBtn" class="close-popup">&times;</span>
            <h3 class="edit-statement-heading">Edit Statement</h3>
            <label class="edit-statement-label" for="editStatementInput">Statement (MAX 100 characters):</label>
            <textarea id="editStatementInput" class="edit-statement-input" maxlength="100" rows="4" required name="editStatementInput"></textarea>
            <label class="edit-statement-label" for="editDescriptionInput">Description (MAX 1000 characters):</label>
            <textarea id="editDescriptionInput" class="edit-statement-input" maxlength="1000" required name="editDescriptionInput"></textarea>
            <asp:Button class="edit-statement-button" ID="editButton" runat="server" Text="EDIT" OnClick="editButton_Click" UseSubmitBehavior="false"></asp:Button>
        </div>
    </div>
    <!-- Delete confirmation popup -->
    <div id="deleteConfirmationPopup" class="popup-container">
        <div class="popup-content">
            <h1 class="popup-heading">Do you want to delete this statement?</h1>
            <hr />
            <p class="statement-text" id="_statement_"></p>
            <br />
            <div class="button-container">
                <asp:Button ID="deleteYesButton" runat="server" CssClass="delete-yes-button" Text="YES" OnClick="deleteYesButton_Click" UseSubmitBehavior="false" />
                <button id="deleteNoButton" class="delete-no-button">NO</button>
            </div>
        </div>
    </div>
    <div id="statementDescriptionPopup" class="popup-container">
        <div class="popup-content">
            <h1 class="statement-description-heading">Statement Description</h1>
            <hr />
            <div class="statement-description-container">
                <br />
                "<p id="statementDescription" class="statement-description"></p>"
            </div>
            <div class="button-container">
                <button id="closeButton" class="close-button" onclick="closeStatementDescriptionPopup()">Close</button>
            </div>
        </div>
    </div>
    <div id="showUpdatePopup" class="popup-container">
        <div class="popup-content">
            <h1 class="update-history-heading">Updated By</h1>
            <hr />
            <div class="update-history-container">
                <br />
                <p id="updatehistory" class="update-history"></p>
                <hr width="50%" style="margin-left: auto; margin-right: auto;">
            </div>
            <div class="button-container">
                <button id="closeButton_up" class="close-button" onclick="closeshowUpdatePopup()">Close</button>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="statementHiddenField" runat="server" />
    <asp:HiddenField ID="statementByHiddenField" runat="server" />
    <asp:HiddenField ID="timestampHiddenField" runat="server" />
    <asp:HiddenField ID="statementIDHiddenField" runat="server" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/7.2.0/js/all.min.js"></script>
    <script>
        // Open the Add Statement Popup
        function openAddStatementPopup() {
            document.getElementById("statementInput").value = "";
            document.getElementById("descriptionInput").value = "";
            var popup = document.getElementById("addStatementPopup");
            popup.style.display = "block";
        }

        // Close the Add Statement Popup
        function closeAddStatementPopup() {
            var popup = document.getElementById("addStatementPopup");
            popup.style.display = "none";
        }

        // Attach event listener to the Close Add Popup button
        var closePopupBtn = document.getElementById("closePopupBtn");
        closePopupBtn.addEventListener("click", closeAddStatementPopup);

        // Attach event listener to the trash can icon
        var deleteIcons = document.querySelectorAll('.delete-icon');
        deleteIcons.forEach(function (icon) {
            icon.addEventListener('click', function () {
                var statement = this.getAttribute('data-statement');
                var statementBy = this.getAttribute('data-statementBy');
                var timestamp = this.getAttribute('data-timestamp');
                var statementID = this.getAttribute('data-statementID');
                showDeleteConfirmationPopup(statement, statementBy, timestamp, statementID);
            });
        });

        // Function to show the delete confirmation popup
        function showDeleteConfirmationPopup(statement, statementBy, timestamp, statementID) {
            var popup = document.getElementById('deleteConfirmationPopup');
            var statementText = popup.querySelector('.statement-text');
            var yesButton = popup.querySelector('.delete-yes-button');

            // Set the statement text in the popup
            statementText.innerHTML = "<textarea class='popup-textarea' readonly>" + statement + "</textarea>";

            // Set the values of the hidden fields
            var statementHiddenField = document.getElementById('<%= statementHiddenField.ClientID %>');
            statementHiddenField.value = statement;

            var statementByHiddenField = document.getElementById('<%= statementByHiddenField.ClientID %>');
            statementByHiddenField.value = statementBy;

            var timestampHiddenField = document.getElementById('<%= timestampHiddenField.ClientID %>');
            timestampHiddenField.value = timestamp;

            var statementIDHiddenField = document.getElementById('<%= statementIDHiddenField.ClientID %>');
            statementIDHiddenField.value = statementID;

            // Show the popup
            popup.style.display = 'block';
        }

        // Attach event listener to the No button in the delete confirmation popup
        var deleteNoButton = document.getElementById("deleteNoButton");
        deleteNoButton.addEventListener("click", function () {
            closeDeletePopup();
        });

        // Function to close the delete confirmation popup
        function closeDeletePopup() {
            var popup = document.getElementById("deleteConfirmationPopup");
            popup.style.display = "none";
        }

        // Open the Statement Description Popup
        function openStatementDescriptionPopup(description) {
            var statementDescriptionPopup = document.getElementById("statementDescriptionPopup");
            var statementDescription = document.getElementById("statementDescription");

            // Change the statementDescription element to a textarea
            statementDescription.innerHTML = "<textarea class='popup-textarea' readonly>" + description + "</textarea>";

            statementDescriptionPopup.style.display = "block";
        }

        // Close the Statement Description Popup
        function closeStatementDescriptionPopup() {
            var statementDescriptionPopup = document.getElementById("statementDescriptionPopup");
            statementDescriptionPopup.style.display = "none";
        }

        // Open the Edit Statement Popup
        function openEditStatementPopup(statement, description, statementBy, timestamp, statementID) {
            var editStatementPopup = document.getElementById("editStatementPopup");
            var editStatementInput = document.getElementById("editStatementInput");
            var editDescriptionInput = document.getElementById("editDescriptionInput");

            // Set the values of the textboxes
            editStatementInput.value = statement;
            editDescriptionInput.value = description;

            // Set the values of the hidden fields
            var statementHiddenField = document.getElementById('<%= statementHiddenField.ClientID %>');
            statementHiddenField.value = statement;

            var statementByHiddenField = document.getElementById('<%= statementByHiddenField.ClientID %>');
            statementByHiddenField.value = statementBy;

            var timestampHiddenField = document.getElementById('<%= timestampHiddenField.ClientID %>');
            timestampHiddenField.value = timestamp;

            var statementIDHiddenField = document.getElementById('<%= statementIDHiddenField.ClientID %>');
            statementIDHiddenField.value = statementID;

            // Change the editDescriptionInput element to a textarea
            editDescriptionInput.outerHTML = "<textarea id='editDescriptionInput' class='edit-statement-input edit-statement-textarea' maxlength='1000' required name='editDescriptionInput'>" + description + "</textarea>";

            // Show the popup
            editStatementPopup.style.display = "block";
        }

        // Close the Edit Statement Popup
        function closeEditStatementPopup() {
            var editStatementPopup = document.getElementById("editStatementPopup");
            editStatementPopup.style.display = "none";
        }

        // Attach event listener to the Close Edit Popup button
        var closeEditPopupBtn = document.getElementById("closeEditPopupBtn");
        closeEditPopupBtn.addEventListener("click", closeEditStatementPopup);

        // Open the Update History Popup
        function openshowUpdatePopup(updatedBy, updatedAt) {
            var showUpdatePopup = document.getElementById("showUpdatePopup");
            var updatehistory_value = document.getElementById("updatehistory");
            updatehistory_value.innerHTML = "<strong>Updated By:</strong> " + updatedBy + "<br /><strong>Updated At:</strong> " + updatedAt;
            showUpdatePopup.style.display = "block";
        }


        // Close the Update History Popup
        function closeshowUpdatePopup() {
            var showUpdatePopup = document.getElementById("showUpdatePopup");
            showUpdatePopup.style.display = "none";
        }
    </script>
</asp:Content>
