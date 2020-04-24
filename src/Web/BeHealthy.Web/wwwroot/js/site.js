function changeFileInputLabel() {
    document.addEventListener("DOMContentLoaded", function () {
        var input = document.querySelector('.custom-file-input');

        input.addEventListener('change', function (e) {
            var fileName = input.files[0].name;
            var nextSibling = e.target.nextElementSibling;
            nextSibling.innerText = fileName;
        });
    });
}