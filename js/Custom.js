// custom.js

// Function to fetch active board data from the database (you need to implement this on your server)
function fetchActiveBoards() {
    // Replace this with your server API endpoint to fetch the data from the database
   // return $.ajax({
     //   url: '/api/get_active_boards',
       // method: 'GET',
    //});


    var mockData = [
        { boardName: 'Board 1', courseName: 'Math', thumbnailUrl: 'url_to_thumbnail_1.jpg' },
        { boardName: 'Board 2', courseName: 'Science', thumbnailUrl: 'url_to_thumbnail_2.jpg' },
        // Add more mock data as needed
    ];

    // Return a resolved promise with the mock data
    return Promise.resolve(mockData);
}

// Function to populate the active boards container with board items
function populateActiveBoards() {
    // Fetch active board data from the database
    fetchActiveBoards()
        .done(function (data) {
            // Assuming the data is an array of board objects with properties like boardName and courseName
            // Example: [{ boardName: 'Board 1', courseName: 'Math' }, { boardName: 'Board 2', courseName: 'Science' }]

            // Clear the container before populating it with board items
            $('.active-boards-container').empty();

            // Loop through the data and create board items dynamically
            data.forEach(function (boardObj) {
                var boardItem = `
          <div class="board-item">
            <div class="board-thumbnail" style="background-image: url('${boardObj.thumbnailUrl}')"></div>
            <h2>${boardObj.boardName}</h2>
            <p>${boardObj.courseName}</p>
            <!-- Add other board details as needed -->
          </div>
        `;
                $('.active-boards-container').append(boardItem);
            });
        })
        .fail(function () {
            console.log('Failed to fetch active board data.');
        });
}

// Call the populateActiveBoards function to load the data on page load
$(document).ready(function () {
    populateActiveBoards();
});


// Function to change color of an element
function changeColor(element, color) {
    element.style.color = color;
}

// Example of using the changeColor function
$(document).ready(function () {
    // Get the element with the ID "myElement" and change its color to red
    var elementToChange = document.getElementById('myElement');
    changeColor(elementToChange, 'red');
});