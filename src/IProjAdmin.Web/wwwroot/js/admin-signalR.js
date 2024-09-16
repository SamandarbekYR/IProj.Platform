document.addEventListener('DOMContentLoaded', function() {
    setTimeout(function () {
        document.getElementById('table-content').classList.remove('hidden');
        document.getElementById('loading').classList.add('hidden');
    }, 1000);
});




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

    console.log("ishlayapman sendmessa")
    const message = document.getElementById("commentmessage").value;
    const selectedUsers = getSelectedUserEmails();

    if (message.length === 0) {
        alert('Please enter a message.');
        return;
    } else if (selectedUsers.length === 0) {
        alert('Please select at least one user.');
        return;
    }

    $('#notificationsModal').modal('show');

    document.getElementById("sendMessageButton").style.display = "none";
    document.getElementById("loadingButton").style.display = "inline-flex";

    // Show progress bar
    let totalSuccess = 0;
    let totalFailure = 0;

    let successCount = document.getElementById('successCount');
    let failedCount = document.getElementById('failedCount');
    let newProgresBar = document.getElementById('newProgresBar');
    let newProgressText = document.getElementById('newProgressText');
    let whoInfo = document.getElementById('whoInfo');
    let userEmail = document.getElementById('userEmail');
    let userTableBody = document.getElementById('userTableBody');

    // Clear existing rows in the table body
    userTableBody.innerHTML = '';

    // Add selected users to the table
    selectedUsers.forEach(user => {
        let row = document.createElement('tr');
        row.innerHTML = `
            <td>
                <div class="d-flex align-items-center">
                    <img src="https://mdbootstrap.com/img/new/avatars/8.jpg"
                         alt=""
                         style="width: 45px; height: 45px"
                         class="rounded-circle" />
                    <div class="ms-3">
                        <p class="fw-bold mb-1">${user.firstname}</p>
                        <p class="text-muted mb-0">${user.email}</p>
                    </div>
                </div>
            </td>
            <td>
               <span class="bg-yellow-100 text-yellow-800 text-xs font-medium me-2 px-2.5 py-0.5 rounded-full dark:bg-yellow-900 dark:text-yellow-300">Pending</span>
            </td>
            <td>${user.position}</td>
        `;
        userTableBody.appendChild(row);
    });

    let totalUsers = selectedUsers.length;
    document.getElementById('usersCount').textContent = totalUsers.toString();
    let i = 0;

    async function sendNextMessage() {
        if (i >= selectedUsers.length) {
            document.getElementById("commentmessage").value = '';
            document.getElementById("loadingButton").style.display = "none";
            document.getElementById("sendMessageButton").style.display = "inline-flex";

            // Hide progress bar and reset to 0%
            newProgresBar.style.width = '0%';
            newProgressText.textContent = '0%';

            $('.userCheckbox').prop('checked', false);
            $('#checkbox-all-search').prop('checked', false);

            newProgressText.textContent = ' ';
            document.getElementById('modalheader').textContent = 'The process has been completed';
            document.getElementById('usersCount').textContent = ' ';
            whoInfo.textContent = ' ';
            userEmail.textContent = ' ';
            return;
        }

        userEmail.textContent = selectedUsers[i].email;


        await connection.invoke("SendMessageToSelectedUsers", selectedUsers[i], message)
            .then(() => {
                totalSuccess++;
                successCount.textContent = totalSuccess.toString();

                let statusElement = userTableBody.children[i].children[1].children[0];
                statusElement.textContent = 'Success';

                statusElement.classList.remove('bg-yellow-100', 'text-yellow-800');
                statusElement.classList.add('bg-green-100', 'text-green-800', 'text-xs', 'font-medium', 'me-2', 'px-2.5', 'py-0.5', 'rounded-full', 'dark:bg-green-900', 'dark:text-green-300');
            })
            .catch((err) => {
                totalFailure++;
                failedCount.textContent = totalFailure.toString();


                // Update status in table
                let statusElement = userTableBody.children[i].children[1].children[0];
                statusElement.textContent = 'Failed';

                statusElement.classList.remove('bg-yellow-100', 'text-yellow-800');
                statusElement.classList.add('bg-red-100', 'text-red-800', 'text-xs', 'font-medium', 'me-2', 'px-2.5', 'py-0.5', 'rounded-full', 'dark:bg-red-900', 'dark:text-red-300');
            })
            .finally(() => {
                i++;
                let progress = (i / totalUsers) * 100;
                newProgresBar.style.width = progress + '%';
                newProgressText.textContent = Math.round(progress) + '% ...';

                setTimeout(sendNextMessage, 3000);
            });
    }
    sendNextMessage();

});


function getSelectedUserEmails() {
    const checkboxes = document.querySelectorAll(".userCheckbox:checked");
    return Array.from(checkboxes).map(checkbox => {
        return {
            id: checkbox.dataset.id,
            email: checkbox.dataset.email,
            firstname: checkbox.dataset.firstname,
            position: checkbox.dataset.position
        };
    });
}



document.getElementById("notificationView").addEventListener("click", function (event) {
    event.preventDefault();
    document.getElementById("divMain").style.display = "block";

})

//document.addEventListener('DOMContentLoaded', function (event) {
//    //setTimeout(function () {
//    //    document.getElementById('loading').classList.add('hidden');
//    //}, 3000);
//        document.getElementById('table-content').classList.remove('hidden');
//});



let showAllUsers = true; 
let filterStatus = '';   

function filterUsersByStatus(status) {
    const rows = userTableBody.getElementsByTagName('tr');
    for (let i = 0; i < rows.length; i++) {
        const statusElement = rows[i].children[1].children[0];
        const userStatus = statusElement.textContent.trim();

        if (status === 'All' || userStatus === status) {
            rows[i].style.display = ''; 
        } else {
            rows[i].style.display = 'none'; 
        }
    }
}

document.querySelector(".success-btn").addEventListener("click", function () {
    if (filterStatus === 'Success') {
        showAllUsers = true;
        filterStatus = '';
        filterUsersByStatus('All'); 
        this.classList.remove("active");
    } else {
        showAllUsers = false;
        filterStatus = 'Success';
        filterUsersByStatus('Success'); 
        this.classList.add("active"); 
        document.querySelector(".error-btn").classList.remove("active"); 
    }
});

document.querySelector(".error-btn").addEventListener("click", function () {
    if (filterStatus === 'Failed') {
        showAllUsers = true;
        filterStatus = '';
        filterUsersByStatus('All'); 
        this.classList.remove("active"); 
    } else {
        showAllUsers = false;
        filterStatus = 'Failed';
        filterUsersByStatus('Failed');
        this.classList.add("active"); 
        document.querySelector(".success-btn").classList.remove("active"); 
    }
});


document.getElementById('closeModal').addEventListener("click", function () {
    $('#notificationsModal').modal('hide')
})


