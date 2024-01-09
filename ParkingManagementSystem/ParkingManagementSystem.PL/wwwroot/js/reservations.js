document.addEventListener('DOMContentLoaded', async function () {
    try {
        console.log(currentUserData.userID);
        const response = await fetch("/api/reservation/GetReservationsByUserId", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                id: currentUserData.userID,
            })
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
            return;
        }

        const data = await response.json();
        generateTable(data);
    }
    catch (e) {
        console.log(e);
    }

    handleDeleteModal();
    handleUpdateModal();
})

const generateTable = (data) => {
    const tableTitle = document.getElementById("tableTitle");
    const tableEl = document.getElementById("table");
    const reservationsLabel = document.getElementById("reservations-label");

    if (data.length == 0) {
        tableTitle.innerHTML = "You have no reservations";
        reservationsLabel.innerHTML = "📉 Reservations: 0";
        tableEl.classList.add("hidden");
    } else {
        tableTitle.innerHTML = "Your reservations";
        reservationsLabel.innerHTML = `📈 Reservations: ${data.length}`;
        tableEl.classList.add("visible");
    }

    const table = document.getElementsByTagName("tbody");
    console.log(table);
    data.forEach((reservation, i) => {
        const options = {
            month: 'numeric',
            day: 'numeric',
            year: 'numeric',
            hour: 'numeric',
            minute: 'numeric',
            hour12: true,
        }

        const html = `
                        <tr id="res-${i}" class="text-center">
                            <td class="py-2 px-4 border-b">${reservation.reservationID}</td>
                            <td class="py-2 px-4 border-b">${reservation.lotID}</td>
                            <td class="py-2 px-4 border-b">${reservation.carPlate}</td>
                            <td class="py-2 px-4 border-b">${new Date(reservation.startTime).toLocaleString('en-US', options)}</td>
                            <td class="py-2 px-4 border-b">${new Date(reservation.endTime).toLocaleString('en-US', options)}</td>
                            <td class="py-2 px-4 border-b">${reservation.status}</td>
                            <td class="py-2 px-4 border-b">
                                <button style="color: #3498db; text-decoration: none; cursor: pointer; margin-right: 2px;">Edit</button>
                                <button style="color: #e74c3c; text-decoration: none; cursor: pointer;">Cancel</button>
                            </td>
                        </tr>
        `

        table[0].innerHTML += html;
    })

    table[0].addEventListener("click", async (e) => {
        const target = e.target;
        const parent = target.parentElement.parentElement;
        const reservationID = parent.id.split("-")[1];
        const reservation = data[reservationID];

        if (target.innerHTML == "Delete") {
            try {
                const response = await fetch("/api/reservation/DeleteReservation", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        id: reservation.reservationID,
                    })
                });

                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                    return;
                }

                const data = await response.json();
                alert(data);
                if (data == "Success!") {
                    location.reload();
                }
            }
            catch (e) {
                console.log(e);
            }
        }
    })
}

const handleDeleteModal = () => {
    const password = document.getElementById("password");
    document.getElementById('delete').addEventListener('click', function () {
        document.getElementById('overlay').classList.remove('hidden');
        document.getElementById('deleteModal').classList.remove('hidden');
        document.body.style.overflow = 'hidden';
    });

    document.getElementById('closeModal').addEventListener('click', function () {
        document.getElementById('overlay').classList.add('hidden');
        document.getElementById('deleteModal').classList.add('hidden');
        document.body.style.overflow = '';
    });

    window.addEventListener('click', function (event) {
        if (event.target === document.getElementById('overlay')) {
            document.getElementById('overlay').classList.add('hidden');
            document.getElementById('deleteModal').classList.add('hidden');
            document.body.style.overflow = '';
        }
    });

    document.getElementById('deleteConfirmation').addEventListener('click', async function () {
        try {
            const response = await fetch("/api/user/DeleteUser", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    password: password.value,
                })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
                return;
            }

            const data = await response.json();
            alert(data);
            if (data == "Success!") {
                window.location.href = "/";
            }
        }
        catch (e) {
            console.log(e);
        }
    })
}

const handleUpdateModal = () => {
    document.getElementById('update').addEventListener('click', function () {
        document.getElementById('overlay').classList.remove('hidden');
        document.getElementById('updateModal').classList.remove('hidden');
        document.body.style.overflow = 'hidden';
    });

    document.getElementById('closeUpdateModal').addEventListener('click', function () {
        document.getElementById('overlay').classList.add('hidden');
        document.getElementById('updateModal').classList.add('hidden');
        document.body.style.overflow = '';
    });

    window.addEventListener('click', function (event) {
        if (event.target === document.getElementById('overlay')) {
            document.getElementById('overlay').classList.add('hidden');
            document.getElementById('updateModal').classList.add('hidden');
            document.body.style.overflow = '';
        }
    });

    document.getElementById('updateConfirmation').addEventListener('click', async function () {
        const firstName = document.getElementById("FirstName").value;
        const lastName = document.getElementById("LastName").value;
        const email = document.getElementById("Email").value;
        const oldPassword = document.getElementById("OldPassword").value;
        const newPassword = document.getElementById("NewPassword").value;
        const phone = document.getElementById("Phone").value;

        const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/;
        if (!passwordRegex.test(newPassword)) {
            alert("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter and one number!");
            return;
        }

        if (firstName == "" || lastName == "" || email == "" || oldPassword == "" || newPassword == "" || phone == "") {
            alert("Please fill in all the fields!");
            return;
        }

        try {
            const response = await fetch("/api/user/UpdateUser", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    FirstName: firstName,
                    LastName: lastName,
                    Email: email,
                    OldPassword: oldPassword,
                    NewPassword: newPassword,
                    Phone: phone,
                })
            })

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
                return;
            }

            const data = await response.json();
            alert(data);
            if (data == "Success!") {
                location.reload();
            }
        }
        catch (e) {
            console.log(e);
        }
    })
}