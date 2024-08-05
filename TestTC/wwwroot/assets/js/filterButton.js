
window.onload = function () {
    var filterContainer = document.getElementById('filterContainer');
    var filterButton = document.getElementById('filterButton');

    if (filterContainer.style.display === "none") {
        filterButton.textContent = "Показать фильтр";
    } else {
        filterButton.textContent = "Скрыть фильтр";
    }

    filterButton.onclick = toggleFilter;
};