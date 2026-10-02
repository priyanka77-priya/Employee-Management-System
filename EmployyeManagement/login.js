const BASE_URL = "https://localhost:7141/api/Employee";

document.getElementById("loginForm").addEventListener("submit", async function (e) {
  e.preventDefault();

  const username = document.getElementById("username").value.trim();
  const password = document.getElementById("password").value.trim();
  const errorDiv = document.getElementById("error");

  try {
    const response = await fetch(`${BASE_URL}/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ username, password }),
    });

    if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || "Login failed");
        }

        const data = await response.json();

        // Save token to localStorage
        localStorage.setItem("token", data.token);

        console.log("JWT Token:", data.token); // You’ll see it in DevTools Console

        // Redirect to dashboard
        window.location.replace("DashBoard.html");
        // window.location.href = "DashBoard.html";

    } catch (err) {
        console.error("Login error:", err.message);
        alert("Login failed: " + err.message);
    }
});
