document.addEventListener('DOMContentLoaded', async function () {
    const logoutButton = document.getElementById('logout');
    const loginButton = document.getElementById('login');

    if (logoutButton) {
        logoutButton.addEventListener('click', async function () {
            console.log(currentUserData);

            try {
                const response = await fetch("/api/User/Logout", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                })

                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }

                const data = await response.json();
                alert(data);
                if (data == "Success!") {
                    if (window.location.pathname === "/Reservations") {
                        return window.location.href = "/";
                    }
                    location.reload();
                }
            }
            catch (error) {
                console.error(error);
            }
        });
    }
});