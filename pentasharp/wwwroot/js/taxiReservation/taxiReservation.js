let currentUser = null;

function fetchCurrentUser() {
    return fetch('/Authenticate/GetCurrentUser')
        .then(response => response.json())
        .then(data => {
            console.log("curr",data.data.userId)
            currentUser = {
                userId: data.data.userId,
                name: `${data.data.firstName} ${data.data.lastName}`
            };
            console.log("currUs", currentUser)
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
                                          <div class="card shadow-lg border-0 p-4 rounded-4" style="background: linear-gradient(145deg, #ffffff, #f9f9f9);">
                                              <div class="row align-items-center">
                                                    <div class="col-3 text-center">
                                                        <div class="btn btn-primary" style="width: 60px; height: 60px; border-radius: 50%; display: flex; align-items: center; justify-content: center;">
                                                            <i class="fas fa-taxi text-white" style="font-size: 1.5rem;"></i>
                                                        </div>
                                                    </div>
                                                    <div class="col-9">
                                                        <h5 class="fw-bold text-primary mb-1">${company.companyName}</h5>
                                                        <p class="text-muted mb-2">Contact: <span class="fw-semibold">${company.contactInfo}</span></p>
                                                        <button class="btn btn-primary px-4 py-2 rounded-pill fw-bold" onclick="openReservationModal('${company.companyName}', ${company.taxiCompanyId})">
                                                            <i class="fas fa-check-circle me-2 text-white"></i> Reserve Now
                                                        </button>
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
        reservationTime: reservationTimeRaw.includes(":") ? reservationTimeRaw : `${reservationTimeRaw}:00`,
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
                const messageElement = document.getElementById('confirmationMessage');
                if (messageElement) {
                    messageElement.textContent = "Your reservation will be confirmed by management and you will be notified.";
                    messageElement.style.display = "block";
                }
                setTimeout(() => {
                    const reservationModal = bootstrap.Modal.getInstance(document.getElementById('reservationModal'));
                    reservationModal.hide();
                }, 2000);
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