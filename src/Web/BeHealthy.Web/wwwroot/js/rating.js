function main(initialGrade) {
    document.addEventListener("DOMContentLoaded", onLoad.bind(undefined, initialGrade))
}

function onLoad(grade) {
    const urlParams = new URLSearchParams(window.location.search);
    const exerciseId = urlParams.get('exerciseId');

    let lastGrade = grade;

    const jsonData = {
        "exerciseId": exerciseId,
        "rating": grade
    };

    const ratingForm = document.getElementById("ratingForm");
    const csrf = document.querySelector("#ratingForm > input[name=__RequestVerificationToken]").value;

    ratingForm.addEventListener("click", function (e) {
        if (e.target.dataset.value) {

            // Clear star colour
            Array.prototype.map.call(ratingForm.children, x => x.style = "");

            const clickedNumber = Number(e.target.dataset.value);

            if (lastGrade !== clickedNumber) {
                let method = jsonData.rating === 0 ? "POST" : "PUT";

                function onSuccess() {
                    lastGrade = clickedNumber;
                    // Colour stars
                    Array.from(ratingForm.children).filter(x => Number(x.dataset.value) <= clickedNumber).map(x => x.style = "color: gold;");
                }

                jsonData.rating = clickedNumber;

                fetch("/api/Ratings/Exercise", {
                    method: method,
                    mode: "cors",
                    cache: "no-cache",
                    credentials: "same-origin",
                    headers: {
                        "Content-Type": "application/json",
                        "CSRF-TOKEN": csrf
                    },
                    body: JSON.stringify(jsonData)
                })
                    .then((response) => {
                        if (response.ok) {
                            return response.json();
                        } else {
                            throw new Error('Something went wrong');
                        }
                    })
                    .then(x => onSuccess())
                    .catch(error => { console.log(error); jsonData.rating = lastGrade; });
            } else {

                fetch("/api/Ratings/Exercise", {
                    method: "DELETE",
                    mode: "cors",
                    cache: "no-cache",
                    credentials: "same-origin",
                    headers: {
                        "Content-Type": "application/json",
                        "CSRF-TOKEN": csrf
                    },
                    body: JSON.stringify(jsonData.exerciseId)
                })
                    .then((response) => {
                        if (!response.ok){
                            throw new Error('Something went wrong');
                        }
                    })
                    .then(x => { jsonData.rating = 0; lastGrade = 0; })
                    .catch(console.log);
            }
        }
    });
}