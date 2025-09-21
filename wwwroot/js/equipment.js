// Equipment-specific JavaScript
document.addEventListener('DOMContentLoaded', function () {
    // Equipment search functionality
    const searchForm = document.getElementById('equipment-search-form');
    if (searchForm) {
        searchForm.addEventListener('submit', function (e) {
            e.preventDefault();
            const searchInput = this.querySelector('input[name="search"]');
            const categorySelect = this.querySelector('select[name="categoryId"]');

            let url = '/Equipment';
            const params = new URLSearchParams();

            if (searchInput.value) {
                params.append('search', searchInput.value);
            }

            if (categorySelect.value) {
                params.append('categoryId', categorySelect.value);
            }

            if (params.toString()) {
                url += '?' + params.toString();
            }

            window.location.href = url;
        });
    }

    // Image upload preview
    const imageInput = document.getElementById('ImageFile');
    if (imageInput) {
        const imagePreview = document.getElementById('image-preview');

        imageInput.addEventListener('change', function () {
            const file = this.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    if (!imagePreview) {
                        const previewDiv = document.createElement('div');
                        previewDiv.id = 'image-preview';
                        previewDiv.className = 'mt-3';
                        previewDiv.innerHTML = '<img src="" class="img-fluid rounded" style="max-height: 200px;">';
                        imageInput.parentNode.appendChild(previewDiv);
                    }
                    document.querySelector('#image-preview img').src = e.target.result;
                };
                reader.readAsDataURL(file);
            }
        });
    }

    // Booking date validation
    const startDateInput = document.getElementById('StartDate');
    const endDateInput = document.getElementById('EndDate');

    if (startDateInput && endDateInput) {
        const today = new Date().toISOString().split('T')[0];
        startDateInput.min = today;
        endDateInput.min = today;

        startDateInput.addEventListener('change', function () {
            if (endDateInput.value && new Date(endDateInput.value) < new Date(this.value)) {
                endDateInput.value = this.value;
            }
            endDateInput.min = this.value;
            calculateBookingTotal();
        });

        endDateInput.addEventListener('change', function () {
            calculateBookingTotal();
        });
    }

    // Calculate booking total
    function calculateBookingTotal() {
        if (startDateInput && endDateInput && startDateInput.value && endDateInput.value) {
            const startDate = new Date(startDateInput.value);
            const endDate = new Date(endDateInput.value);

            if (startDate > endDate) {
                return;
            }

            const days = Math.ceil((endDate - startDate) / (1000 * 60 * 60 * 24)) + 1;
            const pricePerDay = parseFloat(document.getElementById('PricePerDay').value);
            const totalPrice = days * pricePerDay;

            document.getElementById('TotalDays').textContent = days;
            document.getElementById('TotalPrice').textContent = formatCurrency(totalPrice);
            document.getElementById('TotalPriceValue').value = totalPrice;
        }
    }


    // Equipment filter sidebar
    const filterToggle = document.getElementById('filter-toggle');
    const filterSidebar = document.getElementById('filter-sidebar');

    if (filterToggle && filterSidebar) {
        filterToggle.addEventListener('click', function () {
            filterSidebar.classList.toggle('show');
        });
    }

    // Price range slider
    const priceRange = document.getElementById('price-range');
    const priceValue = document.getElementById('price-value');

    if (priceRange && priceValue) {
        priceRange.addEventListener('input', function () {
            priceValue.textContent = formatCurrency(this.value);
            filterEquipment();
        });
    }

    // Real-time equipment filtering
    function filterEquipment() {
        const searchTerm = document.getElementById('search-filter').value.toLowerCase();
        const categoryId = document.getElementById('category-filter').value;
        const maxPrice = parseFloat(document.getElementById('price-range').value);

        const equipmentCards = document.querySelectorAll('.equipment-card');

        equipmentCards.forEach(card => {
            const name = card.querySelector('.card-title').textContent.toLowerCase();
            const description = card.querySelector('.card-text').textContent.toLowerCase();
            const price = parseFloat(card.querySelector('.price-tag').textContent.replace('$', ''));
            const cardCategory = card.dataset.categoryId;

            const matchesSearch = !searchTerm || name.includes(searchTerm) || description.includes(searchTerm);
            const matchesCategory = !categoryId || cardCategory === categoryId;
            const matchesPrice = price <= maxPrice;

            if (matchesSearch && matchesCategory && matchesPrice) {
                card.style.display = 'block';
            } else {
                card.style.display = 'none';
            }
        });
    }

    // Initialize filters
    const searchFilter = document.getElementById('search-filter');
    const categoryFilter = document.getElementById('category-filter');

    if (searchFilter) {
        searchFilter.addEventListener('input', filterEquipment);
    }

    if (categoryFilter) {
        categoryFilter.addEventListener('change', filterEquipment);
    }


    // Equipment comparison functionality
    const compareCheckboxes = document.querySelectorAll('.compare-checkbox');
    const compareButton = document.getElementById('compare-button');

    if (compareCheckboxes.length && compareButton) {
        compareCheckboxes.forEach(checkbox => {
            checkbox.addEventListener('change', updateCompareButton);
        });

        compareButton.addEventListener('click', showComparisonModal);
    }

    function updateCompareButton() {
        const selectedItems = document.querySelectorAll('.compare-checkbox:checked');
        compareButton.disabled = selectedItems.length < 2;
        compareButton.textContent = `Compare (${selectedItems.length})`;
    }

    function showComparisonModal() {
        const selectedIds = Array.from(document.querySelectorAll('.compare-checkbox:checked'))
            .map(checkbox => checkbox.value);

        // Load comparison data and show modal
        fetch('/Equipment/Compare?ids=' + selectedIds.join(','))
            .then(response => response.json())
            .then(data => {
                const modalBody = document.getElementById('comparison-modal-body');
                modalBody.innerHTML = generateComparisonTable(data);

                const comparisonModal = new bootstrap.Modal(document.getElementById('comparison-modal'));
                comparisonModal.show();
            });
    }

    function generateComparisonTable(data) {
        // Implementation for comparison table generation
        return '<div class="table-responsive"><table class="table table-bordered">...</table></div>';
    }



    // Equipment image gallery
    const galleryImages = document.querySelectorAll('.gallery-thumbnail');
    const mainGalleryImage = document.getElementById('main-gallery-image');

    if (galleryImages.length && mainGalleryImage) {
        galleryImages.forEach(img => {
            img.addEventListener('click', function () {
                mainGalleryImage.src = this.src;
                galleryImages.forEach(i => i.classList.remove('active'));
                this.classList.add('active');
            });
        });
    }


});

// Utility function for currency formatting
function formatCurrency(amount) {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
    }).format(amount);
}