// SignalR Hub bog'lanishini o'rnatish
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start()
    .then(() => {
        console.log("SignalR bog'lanishi boshlandi.");
    })
    .catch(err => {
        console.error(`Bog'lanishni boshlashda xato: ${err}`);
    });

connection.on("ReceiveMessage", function (message) {
    addNotificationToModal(message);
    playNotificationSound();
});

function addNotificationToModal(message) {
    const row = document.createElement("tr");
    row.setAttribute("data-message-id", message.messageId);
    const dateCell = document.createElement("td");
    dateCell.textContent = new Date().toLocaleString();
    const messageCell = document.createElement("td");
    messageCell.textContent = message.messageContent;
    console.log(message)
    row.appendChild(dateCell);
    row.appendChild(messageCell);

    document.getElementById("notificationsTable").appendChild(row);

    const countElement = document.getElementById("notificationCount");
    countElement.textContent = parseInt(countElement.textContent) + 1;
}

document.getElementById("notificationBell").addEventListener("click", function () {
    $('#notificationsModal').modal('show');

    const messageIds = [];
    document.querySelectorAll("#notificationsTable tr").forEach(row => {
        const messageId = row.getAttribute("data-message-id");
        if (messageId) {
            messageIds.push(messageId);
        }
    });

    if (messageIds.length > 0) {
        connection.invoke("UpdateMessageStatus", messageIds, true)
            .catch(function (err) {
                console.error("Xabar holatini yangilashda xatolik: ", err);
            });
    } else {
        console.warn("Yangilanadigan xabar IDlari topilmadi.");
    }
    resetNotificationCount();
});

$('.close, .btn-secondary').click(function () {
    $('#notificationsModal').modal('hide');
});

function resetNotificationCount() {
    const countElement = document.getElementById("notificationCount");
    countElement.textContent = "0";
}

function playNotificationSound() {
    const audio = new Audio('/assets/audios/RamBellSound.mp3');
    audio.play();
}

$(document).ready(function () {
    $('#profileImage').on('click', function () {
        $(this).parent('.dropdown').toggleClass('show');
    });

    // Optional: Hide the dropdown when clicking outside
    $(document).on('click', function (event) {
        if (!$(event.target).closest('.dropdown').length) {
            $('.dropdown').removeClass('show');
        }
    });
});