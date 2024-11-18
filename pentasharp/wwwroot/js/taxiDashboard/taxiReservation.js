const reservations = [
    { name: 'John Doe', pickup: 'Central Park', dropoff: 'Times Square', reservationDate: '2024-11-20', reservationTime: '08:00 AM', status: 'Pending', driver: 'Alex Smith - XYZ-1234' },
    { name: 'Jane Smith', pickup: 'Brooklyn Bridge', dropoff: 'Empire State Building', reservationDate: '2024-11-21', reservationTime: '09:30 AM', status: 'In Progress', driver: 'David Miller - ABC-5678' }
];

const reservationList = document.getElementById('reservationList');

reservations.forEach((reservation, index) => {
    const row = document.createElement('tr');
    row.innerHTML = `
        <td>${reservation.name}</td>
        <td>${reservation.pickup}</td>
        <td>${reservation.dropoff}</td>
        <td>${reservation.reservationDate} ${reservation.reservationTime}</td>
        <td><span class="badge bg-warning">${reservation.status}</span></td>
        <td>${reservation.driver}</td>
        <td>
            <button class="btn btn-sm btn-outline-info" onclick="openEditReservationModal(${index})">Edit</button>
            <button class="btn btn-sm btn-outline-success" onclick="openConfirmReservationModal(${index})">Confirm</button>
        </td>
    `;
    reservationList.appendChild(row);
});

function openEditReservationModal(index) {
    const reservation = reservations[index];
    document.getElementById('editPassengerName').value = reservation.name;
    document.getElementById('editPickupLocation').value = reservation.pickup;
    document.getElementById('editDropoffLocation').value = reservation.dropoff;
    document.getElementById('editReservationDate').value = reservation.reservationDate;
    document.getElementById('editReservationTime').value = reservation.reservationTime;
    document.getElementById('editStatus').value = reservation.status;
    document.getElementById('editDriver').value = reservation.driver.split(' - ')[0];
    new bootstrap.Modal(document.getElementById('editReservationModal')).show();
}

function openConfirmReservationModal(index) {
    document.getElementById('confirmDriver').value = '';
    document.getElementById('confirmPrice').value = '';
    new bootstrap.Modal(document.getElementById('confirmReservationModal')).show();
}

function cancelReservation() {
    document.getElementById('editReservationModal').querySelector('.btn-close').click();
}

function saveReservationChanges() {
    document.getElementById('editReservationModal').querySelector('.btn-close').click();
}

function confirmReservation() {
    const driver = document.getElementById('confirmDriver').value;
    const price = document.getElementById('confirmPrice').value;
    document.getElementById('confirmReservationModal').querySelector('.btn-close').click();
}