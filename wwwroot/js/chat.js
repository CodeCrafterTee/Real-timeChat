"use strict";

const roomId = parseInt('@ViewBag.CurrentRoomId');
const userId = '@User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value';
const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(() => {
    connection.invoke("JoinRoom", roomId);
});

document.getElementById("sendBtn").addEventListener("click", () => {
    const input = document.getElementById("messageInput");
    connection.invoke("SendMessage", userId, roomId, input.value);
    input.value = "";
});

connection.on("ReceiveMessage", (user, message, time) => {
    const div = document.createElement("div");
    div.className = "message";
    div.innerHTML = `<strong>${user}:</strong> ${message} <span class="time">${time}</span>`;
    document.getElementById("messages").appendChild(div);
    document.getElementById("messages").scrollTop = document.getElementById("messages").scrollHeight;
});

// Typing indicator
const inputField = document.getElementById("messageInput");
inputField.addEventListener("input", () => {
    connection.invoke("Typing", roomId, '@User.Identity.Name');
});

connection.on("UserTyping", (name) => {
    document.getElementById("typing").innerText = `${name} is typing...`;
    setTimeout(() => document.getElementById("typing").innerText = "", 1000);
});

// Dark mode toggle
document.getElementById("darkModeBtn").addEventListener("click", () => {
    document.body.classList.toggle("dark-mode");
});