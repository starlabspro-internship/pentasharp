﻿let currentUser = null;

function fetchCurrentUser() {
    return fetch('/Authenticate/GetCurrentUser')
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch current user.');
            }
            return response.json();
        })
        .then(data => {
            currentUser = {
                userId: data.userId,
                name: `${data.firstName} ${data.lastName}`
            };
        })
        .catch(error => {
            console.error('Error fetching user:', error);
            currentUser = { userId: null, name: 'Guest' };
        });
}

function handleSearch(event) {
    event.preventDefault();
    fetch('/api/TaxiReservation/SearchAvailableTaxis', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                const resultsContainer = document.getElementById('resultsContainer');
                resultsContainer.innerHTML = '';
                data.companies.forEach(company => {
                    const companyCard = `
                        <div class="container my-3" style="max-width: 600px;">
                            <div class="card shadow-sm border-0 p-4 rounded-4">
                                <div class="row">
                                    <div class="col-12">
                                        <h5 class="fw-bold">${company.companyName}</h5>
                                        <p>Contact: ${company.contactInfo}</p>
                                        <button class="btn btn-primary" onclick="openReservationModal('${company.companyName}', ${company.taxiCompanyId})">Reserve Now</button>
                                    </div>
                                </div>
                            </div>
                        </div>`;
                    resultsContainer.insertAdjacentHTML('beforeend', companyCard);
                });
            }
        })
        .catch(error => console.error('Error:', error));
}

function openReservationModal(companyName, companyId) {
    const pickupLocation = document.getElementById('pickupLocation').value;
    const dropoffLocation = document.getElementById('dropoffLocation').value;
    const reservationDate = document.getElementById('reservationDate').value;
    const reservationTime = document.getElementById('reservationTime').value;
    const passengerCount = document.getElementById('passengerCount').value;

    document.getElementById('modalPickupLocation').textContent = pickupLocation;
    document.getElementById('modalDropoffLocation').textContent = dropoffLocation;
    document.getElementById('modalReservationDate').textContent = reservationDate;
    document.getElementById('modalReservationTime').textContent = reservationTime;
    document.getElementById('modalPassengerCount').textContent = passengerCount;
    document.getElementById('modalCompanyName').textContent = companyName;
    document.getElementById('modalCompanyName').setAttribute('data-id', companyId);
    document.getElementById('modalPassengerName').textContent = currentUser ? currentUser.name : 'Guest';

    const reservationModal = new bootstrap.Modal(document.getElementById('reservationModal'));
    reservationModal.show();
}

function confirmReservation() {
    const reservationTimeRaw = document.getElementById('modalReservationTime').textContent;

    const reservation = {
        taxiCompanyId: parseInt(document.getElementById('modalCompanyName').getAttribute('data-id')),
        userId: currentUser?.userId ? parseInt(currentUser.userId) : 0,
        pickupLocation: document.getElementById('modalPickupLocation').textContent,
        dropoffLocation: document.getElementById('modalDropoffLocation').textContent,
        reservationDate: document.getElementById('modalReservationDate').textContent,
        reservationTime: reservationTimeRaw.includes(":") ? reservationTimeRaw : `${reservationTimeRaw}:00`, // Ensure TimeSpan format
        passengerCount: parseInt(document.getElementById('modalPassengerCount').textContent)
    };

    console.log('Reservation Data:', reservation);

    fetch('/api/TaxiReservation/CreateReservation', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(reservation)
    })
        .then(response => {
            console.log('Raw Response:', response);
            if (!response.ok) {
                return response.json().then(err => {
                    console.error('Backend Error:', err);
                    throw new Error(err.message || "Failed to create reservation.");
                });
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                alert('Reservation confirmed successfully!');
                const reservationModal = bootstrap.Modal.getInstance(document.getElementById('reservationModal'));
                reservationModal.hide();
            } else {
                console.error('Error:', data.errors);
                alert('Failed to confirm reservation. Check errors in the console.');
            }
        })
        .catch(error => console.error('Error:', error));
}

document.addEventListener('DOMContentLoaded', () => {
    fetchCurrentUser();
});