document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('login').addEventListener('click', async function () {
        const email = document.getElementById('email').value;
        const password = document.getElementById('password').value;
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
                    Email: email,
                    PasswordHash: password
                })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            const data = await response.json();
            alert(data);
            console.log(data);
            //window.location.href = "/";
        } catch (error) {
            console.error(error);
        }
    });
})