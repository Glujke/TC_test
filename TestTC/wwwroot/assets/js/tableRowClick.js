

document.addEventListener('DOMContentLoaded', function () {
    const rows = document.querySelectorAll('.list-selected');

    rows.forEach(row => {
        row.addEventListener('click', function () {
            const id = this.getAttribute('data-id');
            const controller = this.getAttribute('data-controller');
            const action = this.getAttribute('data-action');
            if (id) {
                window.location.href = `/${controller}/${action}/${id}`;
            }
        });
    });
});