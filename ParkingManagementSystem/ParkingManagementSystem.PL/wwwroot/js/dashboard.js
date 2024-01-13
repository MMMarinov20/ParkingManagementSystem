let areReservationsVisible = true;
document.addEventListener('DOMContentLoaded', async function () {
    const showReservations = document.getElementById("showReservations");
    const showUsers = document.getElementById("showUsers");
    const tableTitleReservations = document.getElementById("tableTitleReservations");
    const tableElReservations = document.getElementById("tableReservations");

    const tableTitleUsers = document.getElementById("tableTitleUsers");
    const tableElUsers = document.getElementById("tableUsers");

    if (currentUserData.isAdmin) {
        console.log("Asd");
        showUsers.addEventListener('click', () => {
            //areReservationsVisible = !areReservationsVisible;
            tableTitleUsers.classList.remove("hidden");
            tableElUsers.classList.remove("hidden");
            tableTitleReservations.classList.add("hidden");
            tableElReservations.classList.add("hidden");
        })

        showReservations.addEventListener('click', () => {
            //areReservationsVisible = !areReservationsVisible;
            tableTitleUsers.classList.add("hidden");
            tableElUsers.classList.add("hidden");
            tableTitleReservations.classList.remove("hidden");
            tableElReservations.classList.remove("hidden");
        })
    }

    fetchReservations();
    fetchUsers();
    handleDeleteModal();
    handleUpdateModal();
})

const fetchUsers = async () => {
    try {
        const response = await fetch("/api/user/GetUsers", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            },
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
            return;
        }

        const data = await response.json();
        console.log(data);
        generateUsersTable(data);
    }
    catch (e) {
        console.log(e);
    }
}

const generateUsersTable = (data) => {
    const tableTitle = document.getElementById("tableTitleUsers");
    const tableEl = document.getElementById("tableUsers");
    //const usersLabel = document.getElementById("usersLabel");
    const table = document.getElementById("tbodyUsers");

    data.forEach((user, i) => {
        const html = `
                        <tr id="res-${i}" class="text-center">
                            <td class="py-2 px-4 border-b">${user.userID}</td>
                            <td class="py-2 px-4 border-b">${user.firstName}</td>
                            <td class="py-2 px-4 border-b">${user.lastName}</td>
                            <td class="py-2 px-4 border-b">${user.email}</td>
                            <td class="py-2 px-4 border-b">${user.phone}</td>
                            <td class="py-2 px-4 border-b">soon</td>
                            <td class="py-2 px-4 border-b">
                                <button style="color: #3498db; text-decoration: none; cursor: pointer; margin-right: 2px;">Edit</button>
                                <button style="color: #e74c3c; text-decoration: none; cursor: pointer;">Cancel</button>
                            </td>
                        </tr>
        `

        table.innerHTML += html;
    })
}

const fetchReservations = async () => {
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
        generateReservationsTable(data);
    }
    catch (e) {
        console.log(e);
    }
}

const generateReservationsTable = (data) => {
    const tableTitle = document.getElementById("tableTitleReservations");
    const tableEl = document.getElementById("tableReservations");
    const reservationsLabel = document.getElementById("reservationsLabel");

    if (data.length == 0) {
        toastr.info("Why don't you make one?", "You don't have any reservations!");
        tableTitle.innerHTML = "You have no reservations";
        reservationsLabel.innerHTML = "📉 Reservations: 0";
        tableEl.classList.add("hidden");
    } else {
        toastr.info("You can delete or edit your reservations by clicking on the actions buttons!", "Reservations fetched!");
        tableTitle.innerHTML = "Your reservations";
        reservationsLabel.innerHTML = `📈 Reservations: ${data.length}`;
        tableEl.classList.add("visible");
    }

    const table = document.getElementById("tbodyReservations");
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
                                <button style="color: #e74c3c; text-decoration: none; cursor: pointer;">Edit</button>
                                <button style="color: #e74c3c; text-decoration: none; cursor: pointer;">Cancel</button>
                            </td>
                        </tr>
        `

        table.innerHTML += html;
    })

    table.addEventListener("click", async (e) => {
        const target = e.target;
        const parent = target.parentElement.parentElement;
        const reservationID = parent.id.split("-")[1];
        const reservation = data[reservationID];

        if (target.innerHTML == "Cancel") {
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
                if (data == "Success!") {
                    toastr.success("Reservation Cancelled");
                    setTimeout(function () {
                        location.reload();
                    }, 500)

                }
            }
            catch (e) {
                toastr.error(e);
            }
        }
    })
}

const handleDeleteModal = () => {
    const password = document.getElementById("password");
    document.getElementById('delete').addEventListener('click', function () {
        toastr.info("Please enter your password to confirm the action!");
        toastr.warning("Are you sure you want to do this?", "Delete Account");
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
            if (data == "Success!") {
                toastr.success("Account Deleted!");

                setTimeout(function () {
                    window.location.href = "/";
                }, 500)
            }
            else {
                toastr.error("Wrong password");
            }
        }
        catch (e) {
            toastr.error(e);
        }
    })
}

const handleUpdateModal = () => {
    document.getElementById('update').addEventListener('click', function () {
        toastr.info("Fill the needed details to update your account.")
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
            toast.error("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter and one number!");
            return;
        }

        if (firstName == "" || lastName == "" || email == "" || oldPassword == "" || newPassword == "" || phone == "") {
            toastr.error("Please fill in all the fields!");
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
            if (data == "Success!") {
                toastr.success("Account Updated!");

                setTimeout(function () {
                    location.reload();
                }, 500)
            }
        }
        catch (e) {
            console.log(e);
        }
    })
}