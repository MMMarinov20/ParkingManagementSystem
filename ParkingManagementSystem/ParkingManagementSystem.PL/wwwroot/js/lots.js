import { fetchData } from './utils.js';

var lots;
document.addEventListener('DOMContentLoaded', async function () {
    const capacityLabels = document.getElementsByClassName('capacity');
    const lotLabels = document.getElementsByClassName('lotName');
    const lotSelect = document.getElementById('lot');

    await fetchLots(capacityLabels, lotLabels, lotSelect);
    fetchReservations(capacityLabels);

    handleReservation();
});

const fetchLots = async (capacityLabels, lotLabels, lotSelect) => {
    const data = await fetchData("/api/parkinglot/GetAllLots", "GET");
    lots = data;

    data.forEach((lot, i) => {
        capacityLabels[i].innerHTML = `Available spaces: ${lot.currentAvailability}/${lot.capacity}`
        lotLabels[i].innerHTML = lot.lotName;
        lotSelect.innerHTML += `<option value="${lot.lotID}">${lot.lotName}</option>`
    })

    if (data) toastr.info("Parking lots fetched successfuly.");
    else toastr.error("Error fetching parking lots.");
}

const fetchReservations = async (capacityLabels) => {
    const data = await fetchData("/api/reservation/GetAllReservations", "GET");
    data.forEach((reservation, i) => {
        capacityLabels[reservation.lotID].innerHTML = `Available spaces: ${lots[reservation.lotID].currentAvailability}/${lots[reservation.lotID].capacity}`;
    })
}

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
        } else if (date === "" || new Date(date) <= new Date()) {
            toastr.warning("Please select a future date.");
            return;
        }


        const data = await fetchData("/api/reservation/CreateReservation", "POST", {
            UserID: currentUserData.userID,
            Lot: lot,
            Date: date,
            Timestamp: timestamp,
            Plate: plate
        });
        toastr.success("You can view your reservations in your profile page", "Reservation created.")
        setTimeout(function () {
            window.location.href = "/";
        }, 500);
        document.getElementById('overlay').classList.add('hidden');
        document.getElementById('myModal').classList.add('hidden');

    })
}