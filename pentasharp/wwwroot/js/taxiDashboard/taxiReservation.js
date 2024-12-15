let reservations = [];
function fetchReservations() {
    fetch('/Business/TaxiManagement/GetReservations')  
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                reservations = data.reservations;
                populateReservationTable(reservations); 
                console.log("reservations1",reservations)
            } else {
                console.error("Error fetching reservations:", data.message);
                alert("Error fetching reservations: " + data.message);
            }
        })
        .catch(error => {
            console.error("Error fetching reservations:", error);
            alert("Error fetching reservations: " + error.message);
        });
}

function populateReservationTable(reservations) {
    const reservationList = document.getElementById('reservationList');
    reservationList.innerHTML = '';  

    console.log("reservations2", reservations);

    reservations.forEach((reservation, index) => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${reservation.passengerName}</td>
            <td>${reservation.pickupLocation}</td>
            <td>${reservation.dropoffLocation}</td>
            <td>${reservation.reservationDate} ${reservation.reservationTime}</td>
            <td><span class="badge ${getStatusClass(reservation.status)}">${reservation.status}</span></td>
            <td>${reservation.driverName || 'Unassigned'}</td>
            <td>
                <button class="btn btn-sm btn-outline-info" onclick="openEditReservationModal(${index})">Edit</button>
            </td>
        `;
        reservationList.appendChild(row);
    });
}

function getStatusClass(status) {
    switch (status) {
        case 'Pending': return 'bg-warning';
        case 'Start': return 'bg-primary';
        case 'Completed': return 'bg-success';
        case 'Canceled': return 'bg-danger';
        default: return 'bg-secondary';
    }
}
function convertTo24HourFormat(time) {
    const [hours, minutes, period] = time.split(/[:\s]/);
    let h = parseInt(hours);
    if (period === 'PM' && h < 12) {
        h += 12; 
    } else if (period === 'AM' && h === 12) {
        h = 0;  
    }
    return `${h.toString().padStart(2, '0')}:${minutes}`; 
}
function openEditReservationModal(index) {
    const reservation = reservations[index];
    console.log("rescomplet",reservation)
    const taxiCompanyId = reservation.taxiCompanyId;

    document.getElementById('editReservationId').value = reservation.reservationId;
    document.getElementById('editPassengerName').value = reservation.passengerName;
    document.getElementById('editPickupLocation').value = reservation.pickupLocation;
    document.getElementById('editDropoffLocation').value = reservation.dropoffLocation;

    const reservationDate = reservation.reservationDate.split('T')[0];
    document.getElementById('editReservationDate').value = reservationDate;

    const formattedTime = convertTo24HourFormat(reservation.reservationTime);
    document.getElementById('editReservationTime').value = formattedTime;

    document.getElementById('editStatus').value = reservation.status;
    document.getElementById('editDriver').value = reservation.driver || '';
    document.getElementById('editFare').value = reservation.fare || '';

    fetch(`/Business/TaxiManagement/GetTaxisByTaxiCompany?taxiCompanyId=${taxiCompanyId}`)
        .then(response => response.json())
        .then(data => {
            console.log("Fetched taxis:", data);
            const driverSelect = document.getElementById('editDriver');
            driverSelect.innerHTML = "<option value=''>Select Driver</option>";
            data.forEach(taxi => {
                const option = document.createElement("option");
                option.value = taxi.taxiId;
                option.textContent = `${taxi.driverName} - ${taxi.licensePlate}`;
                driverSelect.appendChild(option);


                console.log("taxiid", taxi.taxiId)

                if (taxi.taxiId === reservation.taxiId) {

                    driverSelect.value = taxi.taxiId;
                    console.log("driverSelected", driverSelect.value);
                }
            });
        })
        .catch(error => console.error("Error fetching drivers:", error));

    const modal = new bootstrap.Modal(document.getElementById('editReservationModal'));
    modal.show();
}

function convertTo24HourFormat(time) {
    const [hours, minutes, period] = time.split(/[:\s]/);
    let h = parseInt(hours);
    if (period === 'PM' && h < 12) {
        h += 12;
    } else if (period === 'AM' && h === 12) {
        h = 0;
    }
    return `${h.toString().padStart(2, '0')}:${minutes}`;
}
function saveReservationChanges() {
    const reservationId = document.getElementById('editReservationId').value.trim();
    const reservationDate = document.getElementById('editReservationDate').value.trim();
    const reservationTime = document.getElementById('editReservationTime').value.trim();

    if (!reservationDate || !reservationTime) {
        alert('Please provide both a valid reservation date and time.');
        return;
    }

    const updatedReservation = {
        taxiId: document.getElementById('editDriver').value.trim(),
        fare: parseFloat(document.getElementById('editFare').value.trim()),
        pickupLocation: document.getElementById('editPickupLocation').value.trim(),
        dropoffLocation: document.getElementById('editDropoffLocation').value.trim(),
        status: document.getElementById('editStatus').value.trim(),
        reservationDate,
        reservationTime
    };
   
    console.log('Updated Reservation Object:', updatedReservation);

    const reservationDateTime = combineDateAndTime(updatedReservation.reservationDate, updatedReservation.reservationTime);

    console.log('Combined DateTime:', reservationDateTime);  
    console.log("Request Payload:", {
        ...updatedReservation,
        reservationDate: reservationDateTime 
    });
 
    fetch(`/Business/TaxiManagement/UpdateReservation/${reservationId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            ...updatedReservation, 
            reservationDate: reservationDateTime 
        })
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(err => {
                    throw new Error(`HTTP error! Status: ${response.status}, Message: ${err.message || 'Unknown error'}`);
                });
            }
            return response.json();
        })
        .then(data => {
            fetchReservations();
            bootstrap.Modal.getInstance(document.getElementById('editReservationModal')).hide();

        })
        .catch(error => {
            console.error('Error updating reservation:', error);
            alert(error.message);
        });
}

function combineDateAndTime(date, time) {
    console.log('Date:', date, 'Time:', time);  

    const [hours, minutes] = time.split(':');

    const dateTime = new Date(date);

    dateTime.setUTCHours(parseInt(hours)); 
    dateTime.setUTCMinutes(parseInt(minutes));

    console.log('Combined DateTime (UTC):', dateTime.toISOString());

    return dateTime.toISOString(); 
}

document.addEventListener('DOMContentLoaded', fetchReservations);
