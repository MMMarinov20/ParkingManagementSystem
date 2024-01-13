import { fetchData, isPasswordValid } from "./utils.js";

var reservations;
document.addEventListener('DOMContentLoaded', async function () {
    const showReservations = document.getElementById("showReservations");
    const showUsers = document.getElementById("showUsers");
    const tableTitleReservations = document.getElementById("tableTitleReservations");
    const tableElReservations = document.getElementById("tableReservations");

    const tableTitleUsers = document.getElementById("tableTitleUsers");
    const tableElUsers = document.getElementById("tableUsers");

    if (currentUserData.isAdmin) {
        showUsers.addEventListener('click', () => {
            tableTitleUsers.classList.remove("hidden");
            tableElUsers.classList.remove("hidden");
            tableTitleReservations.classList.add("hidden");
            tableElReservations.classList.add("hidden");
        })

        showReservations.addEventListener('click', () => {
            tableTitleUsers.classList.add("hidden");
            tableElUsers.classList.add("hidden");
            tableTitleReservations.classList.remove("hidden");
            tableElReservations.classList.remove("hidden");
        })
    }

    await fetchReservations();
    await fetchUsers();
    await fetchLots();
    handleUpdateReservation()
    handleDeleteModal();
    handleUpdateModal();
})

const handleUpdateReservation = () => {
    const table = document.getElementById("tbodyReservations");
    table.addEventListener('click', async (e) => {
        const target = e.target;
        if (target.innerText != "Edit") return;
        const parent = target.parentElement.parentElement;
        const id = parent.id.split('-')[1];
        const reservation = reservations[id];

        const now = new Date();
        const reservationDate = new Date(reservation.startTime);
        if (reservationDate < now) {
            toastr.error("You cannot update a reservation that has already started.");
            return;
        }

        const lot = document.getElementById('lot');
        const plate = document.getElementById('plate');
        const startDate = document.getElementById('startDate');
        const timeStamp = document.getElementById('timestamp');

        lot.value = reservation.lotID;
        plate.value = reservation.carPlate;
        startDate.value = reservation.startTime;

        toastr.warning("Edit your reservation before it has started!", "Warning");
        document.getElementById('overlay').classList.remove('hidden');
        document.getElementById('updateReservationModal').classList.remove('hidden');
        document.body.style.overflow = 'hidden';
        document.getElementById('closeReservationModal').addEventListener('click', function () {
            document.getElementById('overlay').classList.add('hidden');
            document.getElementById('updateReservationModal').classList.add('hidden');
            document.body.style.overflow = '';
        });

        window.addEventListener('click', function (event) {
            if (event.target === document.getElementById('overlay')) {
                document.getElementById('overlay').classList.add('hidden');
                document.getElementById('updateReservationModal').classList.add('hidden');
                document.body.style.overflow = '';
            }
        });

        document.getElementById('updateReservationConfirmation').addEventListener('click', async () => {
            if (lot.value == "" || plate.value == "" || startDate.value == "" || timeStamp.value == "") {
                toastr.warning("Please fill all fields to update your reservation");
                return;
            }
            const now = new Date();
            const reservationDate = new Date(reservation.startTime);
            if (reservationDate < now) {
                toastr.error("You cannot update a reservation that has already started.");
                return;
            }

            const data = await fetchData("/api/reservation/EditReservation", "POST", {
                ReservationID: reservation.reservationID,
                UserID: currentUserData.userID,
                Lot: lot.value,
                Date: startDate.value,
                TimeStamp: timeStamp.value,
                Plate: plate.value,
            })

            if (data == "Success!") {
                toastr.success("Reservation updated successfuly.");
                document.getElementById('overlay').classList.add('hidden');
                document.getElementById('updateReservationModal').classList.add('hidden');
                document.body.style.overflow = '';
                setTimeout(() => {
                    location.reload();
                }, 500)
            } else {
                toastr.error("Error updating reservation.");
            }
        })
    })

}

const fetchLots = async () => {
    const lotSelect = document.getElementById('lot');
    const data = await fetchData("/api/parkinglot/GetAllLots", "GET");

    data.forEach((lot, i) => {
        lotSelect.innerHTML += `<option value="${lot.lotID}">${lot.lotName}</option>`
    })

    if (data) toastr.info("Parking lots fetched successfuly.");
    else toastr.error("Error fetching parking lots.");
}

const fetchUsers = async () => {
    const data = await fetchData("/api/user/GetUsers", "GET");
    generateUsersTable(data);
}

const generateUsersTable = (data) => {
    const tableTitle = document.getElementById("tableTitleUsers");
    const tableEl = document.getElementById("tableUsers");
    const usersLabel = document.getElementById("usersLabel");
    const table = document.getElementById("tbodyUsers");

    usersLabel.innerHTML = `ADMIN: Users (${data.length})`;
    data.forEach((user, i) => {
        const html = `
                        <tr id="res-${i}" class="text-center">
                            <td class="py-2 px-4 border-b">${user.userID}</td>
                            <td class="py-2 px-4 border-b">${user.firstName}</td>
                            <td class="py-2 px-4 border-b">${user.lastName}</td>
                            <td class="py-2 px-4 border-b">${user.email}</td>
                            <td class="py-2 px-4 border-b">${user.phone}</td>
                            <td class="py-2 px-4 border-b">${reservations.length}</td>
                            <td class="py-2 px-4 border-b">${user.isAdmin.toString().toUpperCase()}</td>
                            <td class="py-2 px-4 border-b">
                                ${!user.isAdmin ? '<button style="color: #3498db; text-decoration: none; cursor: pointer; margin-right: 2px;">Promote</button>' : " "}
                                <button style="color: #e74c3c; text-decoration: none; cursor: pointer;">Delete</button>
                            </td>
                        </tr>
        `

        table.innerHTML += html;
    })

    table.addEventListener('click', async (e) => {
        const target = e.target;
        const parent = target.parentElement.parentElement;
        const id = parent.id.split('-')[1];
        const user = data[id];
        if (target.innerText === "Delete") deleteUserById(user.userID);
        else if (target.innerText === "Promote") promoteUser(user.userID);
        
    })
}

const deleteUserById = async(userId) => {
    if (userId == currentUserData.userID) {
        toastr.warning("You can't delete yourself!");
        return;
    }
    else if (user.isAdmin) {
        toastr.warning("You can't delete other admins");
        return;
    }

    const data = await fetchData("/api/user/DeleteUserById", "POST", { id: user.userID });

    if (data == "Success!") {
        toastr.success("User deleted successfully!");
        setTimeout(() => {
            location.reload();
        }, 500);
    }
    else {
        toastr.error("Something went wrong!");
    }
}

const promoteUser = async (userId) => {
    const data = await fetchData("/api/user/PromoteUser", "POST", { id: userId });

    if (data == "Success!") {
        toastr.success("User promoted successfully!");
        setTimeout(() => {
            location.reload();
        }, 500);
    }
    else {
        toastr.error("Something went wrong!");
    }
}

const fetchReservations = async () => {
    const data = await fetchData("/api/reservation/GetReservationsByUserId", "POST", { id: currentUserData.userID });
    reservations = data;
    generateReservationsTable(data);
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
                                <button style="color: #3498db; text-decoration: none; cursor: pointer; margin-right: 2px;">Edit</button>
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
            const data = await fetchData("/api/reservation/DeleteReservation", "POST", { id: reservation.reservationID });
            if (data == "Success!") {
                toastr.success("Reservation Cancelled");
                setTimeout(function () {
                    location.reload();
                }, 500)

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

        const data = await fetchData("/api/user/DeleteUser", "POST", {
            password: password.value,
        });
        if (data == "Success!") {
            toastr.success("Account Deleted!");

            setTimeout(function () {
                window.location.href = "/";
            }, 500)
        }
        else {
            toastr.error("Wrong password");
        }
    })
}

const handleUpdateModal = () => {
    const firstName = document.getElementById("FirstName");
    const lastName = document.getElementById("LastName");
    const email = document.getElementById("Email");
    const phone = document.getElementById("Phone");

    document.getElementById('update').addEventListener('click', function () {
        toastr.info("Fill the needed details to update your account.");
        firstName.value = currentUserData.firstName;
        lastName.value = currentUserData.lastName;
        email.value = currentUserData.email;
        phone.value = currentUserData.phone;
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
        const oldPassword = document.getElementById("OldPassword").value;
        const newPassword = document.getElementById("NewPassword").value;

        if (!isPasswordValid(oldPassword)) {
            toastr.error("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter and one number!");
            return;
        }

        if (firstName == "" || lastName == "" || email == "" || oldPassword == "" || newPassword == "" || phone == "") {
            toastr.error("Please fill in all the fields!");
            return;
        }

        const data = await fetchData("/api/user/UpdateUser", "POST", {
            FirstName: firstName.value,
            LastName: lastName.value,
            Email: email.value,
            OldPassword: oldPassword,
            NewPassword: newPassword,
            Phone: phone.value,
        });

        if (data == "Success!") {
            toastr.success("Account Updated!");

            setTimeout(function () {
                location.reload();
            }, 500)
        }
    })
}