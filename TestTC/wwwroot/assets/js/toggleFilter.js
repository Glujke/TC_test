
function toggleFilter() {
    var filterContainer = document.getElementById('filterContainer');
    var filterButton = document.getElementById('filterButton');

    if (filterContainer.style.display === "none") {
        filterContainer.style.display = "block";
        filterButton.textContent = "Скрыть фильтр";
    } else {
        filterContainer.style.display = "none";
        filterButton.textContent = "Показать фильтр";
    }
}