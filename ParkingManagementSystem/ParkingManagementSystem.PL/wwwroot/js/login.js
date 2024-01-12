document.addEventListener('DOMContentLoaded', function () {
    const loginButton = document.getElementById("login");
    loginButton.addEventListener('click', async function () {
        const email = document.getElementById('email');
        const password = document.getElementById('password');

        if (email == "" || password == "") {
            toastr.error("Please fill all fields!");
            return;
        }

        try {
            const response = await fetch("/api/User/Login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": '@Request.GetAntiforgeryToken()',
                },
                body: JSON.stringify({
                    Email: email.value,
                    PasswordHash: password.value
                })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            const data = await response.json();
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

        } catch (error) {
            console.error(error);
        }
    });
})