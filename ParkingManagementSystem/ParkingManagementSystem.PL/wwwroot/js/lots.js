var lots;
document.addEventListener('DOMContentLoaded', async function () {
    const capacityLabels = document.getElementsByClassName('capacity');
    const lotLabels = document.getElementsByClassName('lotName');
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
        })
    }
    catch (e) {
        console.log(e);
    }

    //try {
    //    const response = await fetch("/api/reservation/GetAllReservations", {
    //        method: "GET",
    //        headers: {
    //            "Content-Type": "application/json",
    //        }
    //    })

    //    if (!response.ok) {
    //        throw new Error(`HTTP error! Status: ${response.status}`);
    //        return;
    //    }

    //    const data = await response.json();
    //    console.log(data);
    //    data.forEach((reservation, i) => {
    //        const now = new Date();
    //        const startTime = new Date(reservation.startTime);
    //        const endTime = new Date(reservation.endTime);
    //        if (now >= startTime && now <= endTime) {
    //            lots[reservation.lotID].currentAvailability--;
    //        }

    //        capacityLabels[reservation.lotID].innerHTML = `Available spaces: ${lots[reservation.lotID].currentAvailability}/${lots[reservation.lotID].capacity}`
    //    })
        
    //}
    //catch (e) {
    //    console.log(e);
    //}

    handleReservation();
});

const handleReservation = () => {
    document.getElementById('reserve').addEventListener('click', function () {
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
    document.getElementById('goToCheckout').addEventListener('click', async function () {
        var lot = await parseFloat(document.getElementById('lot').value);
        var date = await document.getElementById('date').value;
        var timestamp = await parseFloat(document.getElementById('timestamp').value);
        var plate = await document.getElementById('plate').value;

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
            alert(data);
            document.getElementById('overlay').classList.add('hidden');
            document.getElementById('myModal').classList.add('hidden');
            window.location.href = "/";
        } catch (error) {
            console.error(error);
        }
    })
}