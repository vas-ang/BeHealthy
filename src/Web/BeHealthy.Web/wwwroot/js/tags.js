// Reserved ids: tagInput, tagForm(for csrfToken), tagContainer

function main(exerciseId) {
    document.addEventListener("DOMContentLoaded", onLoad.bind(undefined, exerciseId));
}

function onLoad(exerciseId) {
    const tagInput = document.getElementById("tagInput");
    const csrfToken = document.querySelector("#tagForm > input[type=hidden]:nth-child(2)").value;
    const container = document.getElementById("tagContainer");

    tagInput.addEventListener("keypress", eventHandler.bind(undefined, exerciseId, csrfToken, container));
}

function eventHandler(exerciseId, csrfToken, container, e) {
    if (e.code === "Space") {
        const inputValue = e.target.value.toLocaleLowerCase().trim();
        e.target.value = "";

        const json = {
            "exerciseId": exerciseId,
            "tagName": inputValue
        };

        fetch("/api/Tags/Exercises", {
            method: "POST",
            mode: "cors",
            cache: "no-cache",
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
                "CSRF-TOKEN": csrfToken
            },
            body: JSON.stringify(json),
        })
            .then(x => x.json())
            .then(x => container.appendChild(parseButton(x)));
    }
}

function parseButton(obj) {
    return createElement(document, "a", configureButtonProperties.bind(undefined, obj));
}

function configureButtonProperties(obj, el) {
    el.textContent = obj.name;
    el.href = `${document.location.origin}/api/Tags/Exercises/${obj.id}`;

    el.classList.add("btn");
    el.classList.add("btn-primary");
}

function createElement(elementCreator, tagName, tagPropertiesSetter) {
    const element = elementCreator.createElement(tagName);

    if (typeof tagPropertiesSetter === "function") {
        tagPropertiesSetter(element);
    }

    return element;
}