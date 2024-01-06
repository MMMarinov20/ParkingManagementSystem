﻿document.addEventListener('DOMContentLoaded', async function () {
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

    handleModal();
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
                                <button class="text-blue-500 hover:underline mr-2">Edit</button>
                                <button class="text-red-500 hover:underline">Delete</button>
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

const handleModal = () => {
    const password = document.getElementById("password");
    document.getElementById('delete').addEventListener('click', function () {
        document.getElementById('overlay').classList.remove('hidden');
        document.getElementById('myModal').classList.remove('hidden');
        document.body.style.overflow = 'hidden';
    });

    document.getElementById('closeModal').addEventListener('click', function () {
        document.getElementById('overlay').classList.add('hidden');
        document.getElementById('myModal').classList.add('hidden');
        document.body.style.overflow = '';
    });

    window.addEventListener('click', function (event) {
        if (event.target === document.getElementById('overlay')) {
            document.getElementById('overlay').classList.add('hidden');
            document.getElementById('myModal').classList.add('hidden');
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