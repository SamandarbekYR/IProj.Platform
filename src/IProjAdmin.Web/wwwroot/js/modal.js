document.addEventListener('DOMContentLoaded', function (event) {
    setTimeout(function () {
        document.getElementById('table-content').classList.remove('hidden');
        document.getElementById('loading').classList.add('hidden');
    }, 3000);
});
document.getElementById('closeModal').addEventListener("click", function () {
    $('#notificationsModal').modal('hide')
})