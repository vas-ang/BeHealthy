﻿// Reserved ids: tagInput, tagForm(for csrfToken), tagContainer

function main(exerciseId) {
    document.addEventListener("DOMContentLoaded", onLoad.bind(undefined, exerciseId));
}

function onLoad(exerciseId) {
    const tagInput = document.getElementById("tagInput");
    const csrfToken = document.querySelector("#tagForm > input[type=hidden]:nth-child(2)").value;
    const container = document.getElementById("tagContainer");

    const clickEventHandler = {
        "delete": function (exerciseId, e) {
            const obj = {
                exerciseId: exerciseId,
                tagId: Number(e.target.dataset.id)
            };

            fetch(`${document.location.origin}/api/Tags/Exercise`, {
                method: "DELETE",
                mode: "cors",
                cache: "no-cache",
                credentials: "same-origin",
                headers: {
                    "Content-Type": "application/json",
                    "CSRF-TOKEN": csrfToken
                },
                body: JSON.stringify(obj)
            })
                .then((response) => {
                    if (!response.ok) {
                        throw new Error('Something went wrong');
                    }
                })
                .then(x => e.target.parentElement.remove())
                .catch(error => console.log(error));
        }
    }

    document.addEventListener("click", function (e) {
        if (typeof clickEventHandler[e.target.dataset.action] === "function") {
            clickEventHandler[e.target.dataset.action](exerciseId, e);
        }
    })

    tagInput.addEventListener("keypress", btnDownEventHandler.bind(undefined, exerciseId, csrfToken, container));

}

function btnDownEventHandler(exerciseId, csrfToken, container, e) {
    if (e.code === "Space" && e.target.value.trim() !== "") {
        const inputValue = e.target.value.toLocaleLowerCase().trim();
        e.target.value = "";

        const json = {
            "exerciseId": exerciseId,
            "tagName": inputValue
        };

        fetch("/api/Tags/Exercise", {
            method: "POST",
            mode: "cors",
            cache: "no-cache",
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
                "CSRF-TOKEN": csrfToken
            },
            body: JSON.stringify(json)
        })
            .then((response) => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error('Something went wrong');
                }
            })
            .then(x => container.appendChild(parseButton(x)))
            .catch(error => console.log(error));
    }
}

function parseButton(obj) {
    return createElement(document, "a", configureButtonProperties.bind(undefined, obj));
}

function configureButtonProperties(obj, el) {
    el.textContent = obj.name;

    el.href = "#";

    el.classList.add("btn");
    el.classList.add("btn-primary");

    el.appendChild(createElement(document, "i", configureDeleteButton.bind(undefined, obj)));
}

function configureDeleteButton(obj, el) {
    el.href = `${document.location.origin}/api/Tags/Exercise`;

    el.dataset.id = obj.id;
    el.dataset.action = "delete";

    el.classList.add("fa");
    el.classList.add("fa-minus");
    el.classList.add("btn");
    el.classList.add("btn-danger");
}

function createElement(elementCreator, tagName, tagPropertiesSetter) {
    const element = elementCreator.createElement(tagName);

    if (typeof tagPropertiesSetter === "function") {
        tagPropertiesSetter(element);
    }

    return element;
}