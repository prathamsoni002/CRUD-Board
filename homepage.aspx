<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="homepage.aspx.cs" Inherits="Resume_Project_CRUD_Board.homepage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <header>
        <div class="logo-container">
            <img src="imgs/CRUD-logo.png" alt="CRUD Board Logo">
        </div>
        <div class="title-container">
            <h1 id="crud-title">CRUD Board</h1>
            <p id="crud-description">Create Read Update Delete</p>
        </div>
    </header>

    <section id="section1">
        <div class="content-container">
            <h2>EASY C-R-U-D</h2>
            <p>Content and description here...</p>
        </div>
        <div class="image-container">
            <img src="imgs/cute%20robot%20typing%20in%20computer%20realistc.%20Show%20the%20c.jpg" alt="C-R-U-D" />
        </div>
    </section>

    <section id="section2">
        <div class="image-container">
            <img src="imgs/4%20cute%20robots%20chatting%20each%20other%20on%20computer.jpg" alt="C-R-U-D" />
        </div>
        <div class="content-container">
            <h2>Conversation Groups</h2>
            <p>Content and description here...</p>
        </div>
    </section>

    <section id="section3">
        <div class="content-container">
            <h2>Efficient Task Management</h2>
            <p>Content and description here...</p>
        </div>
        <div class="image-container">
            <img src="imgs/robot%20standing%20with%20a%20pencil%20in%20hand.jpg" alt="C-R-U-D" />
        </div>
    </section>

    <section id="section4">
        <div class="cta-container">
            <h2>Become a board member today by clicking the Sign In button.</h2>
            <a href="registration.aspx" class="cta-button">Sign In</a>
        </div>
    </section>

    
    <script>
        // Intersection Observer for triggering animations
        const observer = new IntersectionObserver(
            (entries) => {
                entries.forEach((entry) => {
                    if (entry.isIntersecting) {
                        gsap.from(entry.target, {
                            opacity: 0,
                            y: 50,
                            duration: 1,
                        });
                        observer.unobserve(entry.target);
                    }
                });
            },
            {
                threshold: 0.3,
            }
        );

        // Animation for "CRUD Board" title and description
        gsap.from("#crud-title", { opacity: 0, x: -100, duration: 1 });
        gsap.from("#crud-description", { opacity: 0, x: 100, duration: 1, delay: 0.5 });

        // Start observing the sections for animations
        const sections = document.querySelectorAll("section");
        sections.forEach((section) => {
            observer.observe(section);
        });
    </script>
</asp:Content>