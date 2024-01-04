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
                //"RequestVerificationToken": '@Request.GetAntiforgeryToken()',
            },
            body: JSON.stringify({
                id: currentUserData.userID,
            })
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        console.log(data);
    }
    catch (e) {
        console.log(e);
    }
    
})