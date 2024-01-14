import { fetchData, isPasswordValid } from './utils.js'
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('register').addEventListener('click', async function () {
        const fName = document.getElementById('fName').value;
        const lName = document.getElementById('lName').value;
        const email = document.getElementById('email').value;
        const password = document.getElementById("password").value;
        const phone = document.getElementById("phone").value;

        if (fName == "" || lName == "" || email == "" || password == "" || phone == "") {
            toastr.error("Please fill all fields.")
            return;
        }

        if (!isPasswordValid(password)) {
            toastr.error("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter and one number!");
            return;
        }

        const data = await fetchData("/api/User/Register", "POST", {
            FirstName: fName,
            LastName: lName,
            Email: email,
            PasswordHash: password,
            Phone: phone,
        });

        if (data == "Success!") {
            toastr.success("Registration completed.");
            setTimeout(() => window.location.href = "../Login", 500)
        }
        else toastr.error(data);
    });
})