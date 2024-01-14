import { fetchData } from './utils.js'
document.addEventListener('DOMContentLoaded', async function () {
    const stars = Array.from(document.getElementsByClassName('fa-star'));
    const submitButton = document.getElementById('submitFeedback');

    var rating;
    stars.forEach(star => {
        star.addEventListener('click', () => {
            const value = star.dataset.value;
            const starsToFill = stars.filter(s => s.dataset.value <= value);
            starsToFill.forEach(s => s.style.color = 'gold');
            const starsToEmpty = stars.filter(s => s.dataset.value > value);
            starsToEmpty.forEach(s => s.style.color = 'white');
            rating = star.dataset.value;
        })
        star.addEventListener('mouseover', () => {
            const value = star.dataset.value;
            const starsToFill = stars.filter(s => s.dataset.value <= value);
            const starsToEmpty = stars.filter(s => s.dataset.value > value);
            starsToFill.forEach(s => s.style.color = 'gold');
            starsToEmpty.forEach(s => s.style.color = 'white');
        })
    })

    submitButton.addEventListener('click', async () => {
        const input = document.getElementById('feedback');
        if (input.value == "" || rating == 0) {
            toastr.error("Please fill in all fields!");
            return;
        }

        const data = await fetchData("/api/feedback/CreateFeedback", "POST", {
            Comment: input.value,
            Rating: rating
        });

        if (data == "Success!") {
            toastr.success("Feedback sent!");
            stars.forEach(s => s.style.color = 'white');
            input.value = "";
        } else {
            toastr.error("Feedback failed!");
        }
    });
});