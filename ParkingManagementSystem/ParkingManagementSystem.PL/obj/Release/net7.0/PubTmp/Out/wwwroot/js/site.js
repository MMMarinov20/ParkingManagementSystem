import { fetchData } from './utils.js'
document.addEventListener('DOMContentLoaded', async function () {
    const logoutButton = document.getElementById('logout');

    if (logoutButton) {
        logoutButton.addEventListener('click', async function () {
            const data = await fetchData("/api/User/Logout", "POST");
            if (data == "Success!") {
                toastr.success("Logout successful!");
                setTimeout(function () {
                    if (window.location.pathname === "/Dashboard") {
                        return window.location.href = "/";
                    }
                    location.reload();

                }, 500)
            }
        });
    }
});