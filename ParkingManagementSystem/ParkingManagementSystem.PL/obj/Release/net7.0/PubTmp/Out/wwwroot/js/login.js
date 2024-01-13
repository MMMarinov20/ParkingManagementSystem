import { fetchData } from './utils.js'
document.addEventListener('DOMContentLoaded', function () {
    const loginButton = document.getElementById("login");
    loginButton.addEventListener('click', async function () {
        const email = document.getElementById('email');
        const password = document.getElementById('password');

        if (email == "" || password == "") {
            toastr.error("Please fill all fields!");
            return;
        }

        const data = await fetchData("/api/User/Login", "POST", {
            Email: email.value,
            PasswordHash: password.value
        });
        if (data == "Success!") {
            toastr.success("Login successful!");
            toastr.options.closeDuration = 500;
            setTimeout(function () {
                window.location.href = "/";
            }, 500);
            email.value = "";
            password.value = "";
        } else {
            toastr.error("Login failed!");
            toastr.options.closeDuration = 500;
        }

    });
})