

document.addEventListener('DOMContentLoaded', function () {
    const rows = document.querySelectorAll('.list-selected');

    rows.forEach(row => {
        row.addEventListener('click', function () {
            const id = this.getAttribute('data-id');
            if (id) {
                window.location.href = `/ToDoItem/Show/${id}`;
            }
        });
    });
});