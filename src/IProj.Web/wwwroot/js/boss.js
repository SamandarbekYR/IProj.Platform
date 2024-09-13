const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start()
    .then(() => {
        console.log("Connection started successfully.");
    })
    .catch((err) => {
        console.error(`Error starting connection: ${err}`);
    });


connection.on("UpdateUserStatus", (email, isOnline) => {
    const userElement = document.querySelector(`[data-email='${email}']`);
    if (userElement) {
        const statusElement = userElement.closest('tr').querySelector('.status-text');
        if (statusElement) {
            if (isOnline) {
                statusElement.classList.remove('text-red-500');
                statusElement.classList.add('text-green-500');
                statusElement.textContent = 'Online';
            } else {
                statusElement.classList.remove('text-green-500');
                statusElement.classList.add('text-red-500');
                statusElement.textContent = 'Offline';
            }
        }
    }
});
document.getElementById("sendMessageButton").addEventListener("click", async function (event) {
    event.preventDefault();

    const message = document.getElementById("commentmessage").value;
    const selectedUsers = getSelectedUserEmails();

    if (message.length === 0) {
        alert('Please enter a message.');
        return;
    } else if (selectedUsers.length === 0) {
        alert('Please select at least one user.');
        return;
    }
    document.getElementById("progressContainer").style.display = "block";
    document.getElementById("progressText").style.display = "block";
    document.getElementById("sendMessageButton").style.display = "none";
    document.getElementById("loadingButton").style.display = "inline-flex";

    // Show progress bar
    document.getElementById("progressContainer").style.display = "block";

    let totalSuccess = 0;
    let totalFailure = 0;
    let divSuccessCount = document.getElementById('divsuccessfullMessageCount');
    divSuccessCount.textContent = `Successfully sent ${totalSuccess} person`;
    let divFailedMessageCount = document.getElementById('divFailedMessageCount');
    divFailedMessageCount.textContent = `Failed to send: ${totalFailure}`;
    let divWhoMessageIsSending = document.getElementById('divWhoMessageisSending');

    let progressBar = document.getElementById('progressBar');
    let progressText = document.getElementById('progressText');
    let totalUsers = selectedUsers.length;
    let i = 0;

    async function sendNextMessage() {
        if (i >= selectedUsers.length) {
            alert('Message sending process completed!');
            document.getElementById("commentmessage").value = '';
            document.getElementById("loadingButton").style.display = "none";
            document.getElementById("sendMessageButton").style.display = "inline-flex";

            // Hide progress bar and reset to 0%
            document.getElementById("progressContainer").style.display = "none";
            document.getElementById("progressText").style.display = "none";
            progressBar.style.width = '0%';
            progressText.textContent = '0%';

            $('.userCheckbox').prop('checked', false);
            $('#checkbox-all-search').prop('checked', false);
            setTimeout(function () {
                document.getElementById('divtoastr').style.display = 'none';
            }, 3000);

            return;
        }


        await connection.invoke("SendMessageToSelectedUsers", selectedUsers[i], message)
            .then(() => {
                totalSuccess++;
                divSuccessCount.textContent = `Successfully sent to ${totalSuccess} Person`;
            })
            .catch((err) => {
                console.error("Error sending message: ", err);
                alert('An error occurred while sending the message.');
                totalFailure++;
                divFailedMessageCount.textContent = `Failed to send: ${totalFailure}`;
            })
            .finally(() => {
                i++;
                let progress = (i / totalUsers) * 100;
                progressBar.style.width = progress + '%';
                progressText.textContent = Math.round(progress) + '% ...';

                document.getElementById('divtoastr').style.display = 'block';
                setTimeout(sendNextMessage, 3000);
            });
    }

    sendNextMessage();
});

document.getElementById("notificationView").addEventListener("click", function (event) {
    event.preventDefault();
    document.getElementById("divtoastr").style.display = "none";
    document.getElementById("divMain").style.display = "block";

 })
function getSelectedUserEmails() {
    const checkboxes = document.querySelectorAll(".userCheckbox:checked");
    return Array.from(checkboxes).map(checkbox => {
        return {
            id: checkbox.dataset.id,
            email: checkbox.dataset.email
        };
    });
}

document.addEventListener('DOMContentLoaded', function () {


    setTimeout(function () {
        document.getElementById('loading').classList.add('hidden');
        document.getElementById('table-content').classList.remove('hidden');
    }, 3000); 
});



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
