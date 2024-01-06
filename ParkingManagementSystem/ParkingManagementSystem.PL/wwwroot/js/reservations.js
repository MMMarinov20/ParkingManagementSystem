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

})

const generateTable = (data) => {
    const tableTitle = document.getElementById("tableTitle");
    const tableEl = document.getElementById("table");

    if (data.length == 0) {
        tableTitle.innerHTML = "You have no reservations";
        tableEl.classList.add("hidden");
    } else {
        tableTitle.innerHTML = "Your reservations";
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