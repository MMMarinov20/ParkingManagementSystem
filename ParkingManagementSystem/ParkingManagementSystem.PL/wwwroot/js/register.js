document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('register').addEventListener('click', async function () {
        const fName = document.getElementById('fName').value;
        const lName = document.getElementById('lName').value;
        const email = document.getElementById('email').value;
        const password = document.getElementById("password").value;
        const phone = document.getElementById("phone").value;

        if (fName == "" || lName == "" || email == "" || password == "" || phone == "") {
            alert("Please fill all the fields");
            return;
        }

        const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/;
        if (!passwordRegex.test(password)) {
            alert("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter and one number!");
            return;
        }

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