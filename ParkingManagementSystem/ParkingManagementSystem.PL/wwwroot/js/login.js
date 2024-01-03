document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('login').addEventListener('click', async function () {
        const email = document.getElementById('email');
        const password = document.getElementById('password');
        console.log(JSON.stringify({ email: email, password: password }));
        console.log('@Request.GetAntiforgeryToken()');

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
            alert(data);
            if (data == "Success!") {
                email.value = "";
                password.value = "";
                window.location.href = "/";
            }
                        
        } catch (error) {
            console.error(error);
        }
    });
})