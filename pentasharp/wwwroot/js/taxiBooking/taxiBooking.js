let userId = null;

function HandleSearch(event) {
    event.preventDefault();
    const ResultsContainer = document.getElementById('ResultsContainer');
    if (!ResultsContainer) return;

    fetch('/api/TaxiBooking/SearchAvailableTaxis', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            pickupLocation: document.getElementById('PickupLocation').value,
            dropoffLocation: document.getElementById('DropoffLocation').value,
            bookingTime: new Date().toISOString()
        })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            if (data.success && Array.isArray(data.companies)) {
                ResultsContainer.replaceChildren();
                if (data.companies.length === 0) {
                    ResultsContainer.innerHTML = '<p>No companies available for your search.</p>';
                } else {
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
                                                        <button class="btn btn-primary px-4 py-2 rounded-pill fw-bold" onclick="OpenBookingModal('${company.companyName}', ${company.taxiCompanyId})">
                                                            <i class="fas fa-check-circle me-2 text-white"></i> Reserve Now
                                                        </button>
                                                    </div>
                                               </div>
                                          </div>
                                   </div>`;

                        ResultsContainer.insertAdjacentHTML('beforeend', companyCard);
                    });
                }
            } else {
                ResultsContainer.innerHTML = '<p>No companies found.</p>';
            }
        })
        .catch(error => {
            console.error('Error fetching companies:', error);
            ResultsContainer.innerHTML = '<p>An error occurred while fetching companies. Please try again later.</p>';
        });
}

function OpenBookingModal(companyName, companyId) {
    const PickupLocation = document.getElementById('PickupLocation').value;
    const DropoffLocation = document.getElementById('DropoffLocation').value;
    const BookingTime = document.getElementById('BookingTime').value;
    const PassengerCount = document.getElementById('PassengerCount').value;

    document.getElementById('ModalPickupLocation').textContent = PickupLocation;
    document.getElementById('ModalDropoffLocation').textContent = DropoffLocation;
    document.getElementById('ModalPickupTime').textContent = BookingTime || "Invalid Time";
    document.getElementById('ModalPassengerCount').textContent = PassengerCount;

    const taxiCompanyIdInput = document.getElementById('ModalTaxiCompanyId');
    if (taxiCompanyIdInput) {
        taxiCompanyIdInput.value = companyId;
    }
   
    const passengerNameInput = document.getElementById('ModalPassengerName');
    if (passengerNameInput) {
        passengerNameInput.value = "Loading...";
        fetch('/Authenticate/GetCurrentUser')
            .then(response => response.json())
            .then(data => {
                if (data.data && data.data.firstName && data.data.lastName) {
                    passengerNameInput.value = `${data.data.firstName} ${data.data.lastName}`;
                    userId = data.data.userId;
                } else {
                    passengerNameInput.value = "You need to log in to make a booking";
                }
            })
            .catch(() => {
                passengerNameInput.value = "Guest";
            });
    }

    document.getElementById('AwaitingConfirmation').classList.add('d-none');
    new bootstrap.Modal(document.getElementById('BookingModal')).show();
}

function SubmitBooking() {
    const pickupTimeContent = document.getElementById('ModalPickupTime').textContent;
    if (!pickupTimeContent) {
        alert('Invalid time. Please check your input.');
        return;
    }

    const [hours, minutes] = pickupTimeContent.split(':');
    const currentDate = new Date();
    const bookingTime = new Date(Date.UTC(
        currentDate.getFullYear(),
        currentDate.getMonth(),
        currentDate.getDate(),
        parseInt(hours),
        parseInt(minutes)
    ));

    const bookingData = {
        taxiCompanyId: document.getElementById('ModalTaxiCompanyId').value,
        pickupLocation: document.getElementById('ModalPickupLocation').textContent,
        dropoffLocation: document.getElementById('ModalDropoffLocation').textContent,
        bookingTime: bookingTime.toISOString(),
        passengerCount: parseInt(document.getElementById('ModalPassengerCount').textContent),
        userId: userId,
    };

    fetch('/api/TaxiBooking/CreateBooking', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(bookingData)
    })
        .then(response => response.json())
        .then(data => {
            const confirmationMessage = document.getElementById('AwaitingConfirmation');
            if (data.success) {
                confirmationMessage.textContent = "Your reservation will be confirmed by management, and you will be notified.";
                confirmationMessage.classList.remove('d-none', 'alert-danger');
                confirmationMessage.classList.add('alert-success');
                setTimeout(() => {
                    const modal = bootstrap.Modal.getInstance(document.getElementById('BookingModal'));
                    modal.hide();
                }, 3000);
            } else {
                confirmationMessage.textContent = 'Failed to create booking: ' + (data.message || 'Unknown error');
                confirmationMessage.classList.remove('d-none', 'alert-success');
                confirmationMessage.classList.add('alert-danger');
            }
        })
        .catch(() => {
            const confirmationMessage = document.getElementById('AwaitingConfirmation');
            confirmationMessage.textContent = 'An error occurred. Please try again later.';
            confirmationMessage.classList.remove('d-none', 'alert-success');
            confirmationMessage.classList.add('alert-danger');
        });
}