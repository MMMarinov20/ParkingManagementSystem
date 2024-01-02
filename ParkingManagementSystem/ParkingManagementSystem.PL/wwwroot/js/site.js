// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function () {
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
                    //"RequestVerificationToken": '@Request.GetAntiforgeryToken()',
                },
                body: JSON.stringify({
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
});