var lots;
document.addEventListener('DOMContentLoaded', async function () {
    const capacityLabels = document.getElementsByClassName('capacity');
    const lotLabels = document.getElementsByClassName('lotName');
    const lotSelect = document.getElementById('lot');

    try {
        const response = await fetch("/api/parkinglot/GetAllLots", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            }
        })

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
            return;
        }

        const data = await response.json();
        lots = data;

        data.forEach((lot, i) => {
            capacityLabels[i].innerHTML = `Available spaces: ${lot.currentAvailability}/${lot.capacity}`
            lotLabels[i].innerHTML = lot.lotName;
            lotSelect.innerHTML += `<option value="${lot.lotID}">${lot.lotName}</option>`
        })

        toastr.info("Parking lots fetched successfuly.");
    }
    catch (e) {
        console.log(e);
    }

    try {
        const response = await fetch("/api/reservation/GetAllReservations", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            }
        })

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
            return;
        }

        const data = await response.json();
        data.forEach((reservation, i) => {
            capacityLabels[reservation.lotID].innerHTML = `Available spaces: ${lots[reservation.lotID].currentAvailability}/${lots[reservation.lotID].capacity}`;
        })

    }
    catch (e) {
        console.log(e);
    }

    handleReservation();
});

const handleReservation = () => {
    const reserveButtons = document.getElementsByClassName('reserve');

    Array.from(reserveButtons).forEach((button, i) => {
        button.addEventListener('click', function () {
            if (lots[i].currentAvailability === 0) {
                toastr.warning("This parking lot is full. Please select another one.")
                return;
            }
            toastr.info("Please fill all fields to create a reservation.")
            document.getElementById('overlay').classList.remove('hidden');
            document.getElementById('myModal').classList.remove('hidden');
            document.body.style.overflow = 'hidden';
        });
    })

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
    document.getElementById('goToCheckout').addEventListener('click', async function () {
        const lot = await parseFloat(document.getElementById('lot').value);
        const date = await document.getElementById('date').value;
        const timestamp = await parseFloat(document.getElementById('timestamp').value);
        const plate = await document.getElementById('plate').value;

        if (lot === 0 || timestamp === 0 || plate === "") {
            toastr.warning("Please fill all fields to create a reservation.");
            return;
        } else if (date === "" || new Date(date) < new Date()) {
            toastr.warning("Please select a future date.");
            return;
        }

        try {
            const response = await fetch("/api/reservation/CreateReservation", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    UserID: currentUserData.userID,
                    Lot: lot,
                    Date: date,
                    Timestamp: timestamp,
                    Plate: plate
                })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            const data = await response.json();
            toastr.success("You can view your reservations in your profile page", "Reservation created.")
            setTimeout(function () {
                window.location.href = "/";
            }, 500);
            document.getElementById('overlay').classList.add('hidden');
            document.getElementById('myModal').classList.add('hidden');

        } catch (error) {
            console.error(error);
        }
    })
}