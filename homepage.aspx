<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="homepage.aspx.cs" Inherits="Resume_Project_CRUD_Board.homepage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/gsap/3.9.1/gsap.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/gsap/3.9.1/gsap.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <header>
        <div class="logo-container">
            <img src="imgs/CRUD-logo.png" alt="CRUD Board Logo">
        </div>
        <div class="vl"></div>
        <div class="title-container">
            <h1 id="crud-title">CRUD Board</h1>
            <p id="crud-description">Empowering Seamless Collaboration and Communication</p>
        </div>
    </header>

    <section id="section1" class="section">
        <div class="content-container">
            <h2>Unleash the Power of CRUD</h2>
            <p>Experience the power of Create, Read, Update, and Delete operations like never before. With CRUD Board, unlock the true potential of secure conversation channels. Seamlessly connect with peers and share ideas, knowing you have complete control over your discussions.</p>
        </div>
        <div class="image-container">
            <img src="imgs/cute%20robot%20typing%20in%20computer%20realistc.%20Show%20the%20c.jpg" alt="C-R-U-D" />
        </div>
    </section>

    <section id="section2" class="section">
        <div class="image-container">
            <img src="imgs/4%20cute%20robots%20chatting%20each%20other%20on%20computer.jpg" alt="C-R-U-D" />
        </div>
        <div class="content-container">
            <h2>Boundless Collaboration, Limitless Growth</h2>
            <p>Embrace a new era of collaboration with CRUD Board. Create vibrant groups, where ideas flow freely and innovation knows no bounds. Enjoy the freedom of unlimited posting, as you cultivate a community that thrives on shared knowledge and collective progress.</p>
        </div>
    </section>

    <section id="section3" class="section">
        <div class="content-container">
            <h2>Master Your Productivity, Conquer Goals</h2>
            <p>Elevate your task management game with CRUD Board. Empower your board members to be true taskmasters, effortlessly organizing schedules and conquering goals. Stay on top of every project, as you navigate your way to success, together.</p>
        </div>
        <div class="image-container">
            <img src="imgs/robot%20standing%20with%20a%20pencil%20in%20hand.jpg" alt="C-R-U-D" />
        </div>
    </section>

    <section id="section4" class="section">
        <div class="cta-container">
            <h2>Become a board member today by clicking the Sign In button.</h2>
            <a href="registration.aspx" class="cta-button">Sign In</a>
        </div>
    </section>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const sections = document.querySelectorAll(".section");
            const observer = new IntersectionObserver(
                (entries) => {
                    entries.forEach((entry) => {
                        if (entry.isIntersecting) {
                            gsap.from(entry.target, {
                                opacity: 0,
                                y: 50,
                                duration: 0.9,
                            });
                            observer.unobserve(entry.target);
                        }
                    });
                },
                {
                    threshold: 0.3,
                }
            );

            sections.forEach((section) => {
                observer.observe(section);
            });

            gsap.from("#crud-title", { opacity: 0, x: -55, duration: 1 });
            gsap.from("#crud-description", { opacity: 0, x: 100, duration: 1, delay: 0.5 });
        });
    </script>
</asp:Content>
