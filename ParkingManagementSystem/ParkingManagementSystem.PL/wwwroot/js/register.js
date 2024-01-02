document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('register').addEventListener('click', async function () {
        const fName = document.getElementById('fName').value;
        const lName = document.getElementById('lName').value;
        const email = document.getElementById('email').value;
        const password = document.getElementById("password").value;
        const phone = document.getElementById("phone").value;

        try {
            const response = await fetch("/api/User/Register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": '@Request.GetAntiforgeryToken()',
                },
                body: JSON.stringify({
                    FirstName: fName,
                    LastName: lName,
                    Email: email,
                    PasswordHash: password,
                    Phone: phone,
                })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            const data = await response.json();
            alert(data);
            console.log(data);
        } catch (error) {
            console.error(error);
        }
    });
})