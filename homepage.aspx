<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="homepage.aspx.cs" Inherits="Resume_Project_CRUD_Board.homepage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="hp-heading">
        <h1 class="homepage-heading">CRUD Board</h1>
        <h2 class="homepage-subheading">Keep your group in the loop</h2>
    </div>
    <section id="hp-info">
        <div class="slideshow-container">
            <div class="slideshow-slide">
                <center>
                    <figure>
                <img width = '50%' height="450px" src="imgs/task%20completion.jpeg" />
                        <figcaption>
                            <div class="slideshow-content">
                    <h3>Collaborate and Stay Updated</h3>
                    <p>
                        With CRUDboard, you can easily create, share, and update important information with your group. This powerful communication tool enables you to keep everyone in the loop with one-line updates and collaborate effectively to achieve your goals.
                    </p>
                </figcaption>
                </figure></center>
            </div>
            <div class="slideshow-slide">
                <center>
                    <figure>

                <img  height="450px" src="imgs/communication.jpeg" />
                
              <figcaption>
                            <div class="slideshow-content">
                    <h3>Streamlined Communication</h3>
                    <p>
                        Simplify your group communication process with CRUDboard. Share announcements, tasks, and updates effortlessly, ensuring everyone stays informed and aligned towards common objectives.
                    </p>
                </figcaption>
                </figure></center>
            </div>
            <div class="slideshow-slide">
                <center>
                    <figure>
                    <img  height="450px" src="imgs/time%20management%20clock.jpeg" />
                         <figcaption> <div class="slideshow-content">
                    <h3>Efficient Task Management</h3>
                    <p>
                        Manage tasks and projects efficiently using CRUDboard's intuitive interface. Assign responsibilities, track progress, and collaborate seamlessly, making teamwork more productive and organized.
                    </p>
                </div></figcaption></figure></center>
                    
            </div>
            </div>
        </div>
    </section>
   
    <script>
        var slideIndex = 0;
        showSlides();

        function showSlides() {
            var slides = document.getElementsByClassName("slideshow-slide");
            for (var i = 0; i < slides.length; i++) {
                slides[i].style.display = "none";
            }
            slideIndex++;
            if (slideIndex > slides.length) {
                slideIndex = 1;
            }
            slides[slideIndex - 1].style.display = "block";
            setTimeout(showSlides, 2000); 
        }
    </script>
</asp:Content>

