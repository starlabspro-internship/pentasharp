function loadBookings() {
    fetch('/Business/TaxiManagement/GetBookings')
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                const bookings = data.data;
                const passengerList = document.getElementById("passengerList");
                passengerList.innerHTML = "";
                bookings.forEach(booking => {
                    const row = document.createElement("tr");
                    row.innerHTML = `
                        <td>${booking.passengerName}</td>
                        <td>${booking.pickupLocation}</td>
                        <td>${booking.dropoffLocation}</td>
                        <td>${booking.bookingTime}</td>
                        <td>
                            <span class="badge bg-${booking.status === "Pending"
                            ? "warning"
                            : booking.status === "InProgress"
                                ? "primary"
                                : booking.status === "Confirmed"
                                    ? "success"
                                    : booking.status === "Completed"
                                        ? "success"
                                        : "danger"}">${booking.status}
                            </span>
                        </td>
                        <td>${booking.driverName || "No Driver Assign"}</td>
                        <td>
                            <button class="btn btn-sm btn-outline-success" onclick="openEditBookingModal(${booking.bookingId})">Edit</button>
                        </td>`;
                    passengerList.appendChild(row);
                });
            } else {
                alert("Failed to load bookings: " + data.message);
            }
        })
        .catch(() => {
            alert("An error occurred while loading bookings.");
        });
}

function openEditBookingModal(bookingId) {
    fetch(`/Business/TaxiManagement/GetBooking?id=${bookingId}`)
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                const booking = data.data;

                document.getElementById("editPassengerName").value = booking.passengerName;
                document.getElementById("editPickupLocation").value = booking.pickupLocation;
                document.getElementById("editDropoffLocation").value = booking.dropoffLocation;
                document.getElementById("editPickupTime").value = booking.bookingTime;
                document.getElementById("editStatus").value = booking.status;

                const driverSelect = document.getElementById("editDriver");
                const selectedTaxiId = booking.taxiId === null ? "null" : booking.taxiId;

                driverSelect.innerHTML = `<option value="null">No Driver Assign</option>`;

                fetch('/Business/TaxiCompany/GetTaxis')
                    .then(response => response.json())
                    .then(drivers => {
                        drivers.forEach(driver => {
                            driverSelect.innerHTML += `<option value="${driver.taxiId}">${driver.driverName}</option>`;
                        });

                        driverSelect.value = selectedTaxiId;

                        if (!driverSelect.value) {
                            driverSelect.value = "null";
                        }
                    });

                document.getElementById('editBookingModal').dataset.bookingId = booking.bookingId;
                new bootstrap.Modal(document.getElementById('editBookingModal')).show();
            } else {
                alert("Failed to load booking.");
            }
        })
        .catch(() => {
            alert("An error occurred while loading booking.");
        });
}

function saveBookingChanges() {
    const pickupLocation = document.getElementById('editPickupLocation').value.trim();
    const dropoffLocation = document.getElementById('editDropoffLocation').value.trim();
    const pickupTimeContent = document.getElementById('editPickupTime').value.trim();
    const status = document.getElementById('editStatus').value;
    const taxiId = document.getElementById('editDriver').value;

    console.log("taxiid", taxiId);

    console.log("Selected Taxi ID:", taxiId);

    if (!pickupLocation || !dropoffLocation || !pickupTimeContent) {
        alert("Please fill in all required fields.");
        return;
    }

    const [hours, minutes] = pickupTimeContent.split(':');
    const currentDate = new Date();
    const bookingTime = new Date(Date.UTC(
        currentDate.getFullYear(),
        currentDate.getMonth(),
        currentDate.getDate(),
        parseInt(hours, 10),
        parseInt(minutes, 10)
    )).toISOString();

    const updatedBooking = {
        bookingId: document.getElementById('editBookingModal').dataset.bookingId,
        pickupLocation,
        dropoffLocation,
        bookingTime,
        status,
        taxiId: taxiId === "null" ? null : parseInt(taxiId, 10),
        updatedAt: new Date().toISOString() 
    };

    console.log("Payload to server:", JSON.stringify(updatedBooking));

    fetch('/Business/TaxiManagement/UpdateBooking', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(updatedBooking)
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(data => {
                    throw new Error(data.message || "Invalid request");
                });
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                loadBookings();
                bootstrap.Modal.getInstance(document.getElementById('editBookingModal')).hide();
            } else {
                alert("Failed to update booking: " + data.message);
            }
        })
        .catch(error => {
            alert("Failed to update booking: " + error.message);
        });
}

document.addEventListener("DOMContentLoaded", loadBookings);