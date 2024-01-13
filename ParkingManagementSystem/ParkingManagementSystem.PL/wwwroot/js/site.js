document.addEventListener('DOMContentLoaded', async function () {
    const logoutButton = document.getElementById('logout');

    if (logoutButton) {
        logoutButton.addEventListener('click', async function () {

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
                if (data == "Success!") {
                    toastr.success("Logout successful!");
                    setTimeout(function () {
                        if (window.location.pathname === "/Dashboard") {
                            return window.location.href = "/";
                        }
                        location.reload();

                    }, 500)
                }
            }
            catch (error) {
                toastr.error(error);
            }
        });
    }
});