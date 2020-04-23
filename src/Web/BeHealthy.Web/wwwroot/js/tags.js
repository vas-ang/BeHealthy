// Reserved ids: tagInput, tagForm(for csrfToken), tagContainer

function mainTags(exerciseId) {
    document.addEventListener("DOMContentLoaded", onLoad.bind(undefined, exerciseId));
}

function onLoad(exerciseId) {
    const tagInput = document.getElementById("tagInput");
    const csrfToken = document.querySelector("#tagForm > input[name=__RequestVerificationToken]").value;
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

    tagInput.addEventListener("keypress", btnDownEventHandler.bind(undefined, exerciseId, csrfToken, container, tagInput));
}

function btnDownEventHandler(exerciseId, csrfToken, container, tagInput, e) {
    if (e.code === "Space") {
        const inputValue = tagInput.value.toLocaleLowerCase().trim();
        tagInput.value = "";

        if (inputValue.match(/^[a-z\-]+$/g) === null) {
            return;
        }

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
            .then(x => container.appendChild(createTag(x)))
            .catch(error => console.log(error));
    }
}

function createTag(obj) {
    const tagContainer = createElement(document, "div", configureTagContainer);

    tagContainer.appendChild(createElement(document, "a", configureButtonProperties.bind(undefined, obj)));
    tagContainer.appendChild(createElement(document, "i", configureDeleteButton.bind(undefined, obj)));

    return tagContainer;
}

function configureTagContainer(el) {
    el.classList.add("btn");
    el.classList.add("btn-primary");
}

function configureButtonProperties(obj, el) {
    el.textContent = obj.name;

    el.href = `/Fitness/Exercises/${obj.name}`;

    el.classList.add("text-light");
}

function configureDeleteButton(obj, el) {
    el.href = `${document.location.origin}/api/Tags/Exercise`;

    el.dataset.id = obj.id;
    el.dataset.action = "delete";

    el.classList.add("fa");
    el.classList.add("fa-minus");
    el.classList.add("text-warning");
    el.classList.add("bg-danger");
    el.classList.add("rounded");
}

function createElement(elementCreator, tagName, tagPropertiesSetter) {
    const element = elementCreator.createElement(tagName);

    if (typeof tagPropertiesSetter === "function") {
        tagPropertiesSetter(element);
    }

    return element;
}