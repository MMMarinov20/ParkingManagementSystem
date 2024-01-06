document.addEventListener('DOMContentLoaded', async function () {
    const logout = document.getElementById("logout");

    //logout.addEventListener("click", async function () {
    //    currentUserData = null;
    //    window.location.href = "/";
    //})
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

        const table = document.getElementsByTagName("tbody");
        data.forEach(reservation => {
            const options = {
                month: 'numeric',
                day: 'numeric',
                year: 'numeric',
                hour: 'numeric',
                minute: 'numeric',
                hour12: true,
            }

            const html = `
                        <tr class="text-center">
                            <td class="py-2 px-4 border-b">${reservation.reservationID}</td>
                            <td class="py-2 px-4 border-b">${reservation.lotID}</td>
                            <td class="py-2 px-4 border-b">${reservation.carPlate}</td>
                            <td class="py-2 px-4 border-b">${new Date(reservation.startTime).toLocaleString('en-US', options)}</td>
                            <td class="py-2 px-4 border-b">${new Date(reservation.endTime).toLocaleString('en-US', options) }</td>
                            <td class="py-2 px-4 border-b">${reservation.status}</td>
                            <td class="py-2 px-4 border-b">
                                <button class="text-blue-500 hover:underline mr-2">Edit</button>
                                <button class="text-red-500 hover:underline">Delete</button>
                            </td>
                        </tr>
        `

            table[0].innerHTML += html;

        })
    }
    catch (e) {
        console.log(e);
    }

})