
document.getElementById('SearchInput').addEventListener('input', function () {
    const searchValue = this.value.toLowerCase();
    const allMenuItems = document.querySelectorAll('.side-menu__item, .slide-item');
    const resultsContainer = document.querySelector('.sidebar-search-results .list-group');
    resultsContainer.innerHTML = '';  // Clear previous results
    // Only perform search if the user types 3 or more characters
    if (searchValue.length >= 3) {
        let resultsFound = false;
        allMenuItems.forEach(item => {
            const itemText = item.textContent.trim();
            const itemTextLower = itemText.toLowerCase();
            const itemLink = item.getAttribute('href');

            // Check if item text or link contains search value
            if (itemTextLower.includes(searchValue) || itemLink.toLowerCase().includes(searchValue)) {
                resultsFound = true;

                // Dynamic Parent Path
                let parentPath = '';
                let parentElement = item.closest('.slide'); // Find the closest slide section

                if (parentElement) {
                    const parentName = parentElement.querySelector('.side-menu__label').textContent.trim(); // Get the parent label text
                    parentPath = parentName;
                }

                // Highlight the searched term in the result
                const highlightedText = itemText.replace(
                    new RegExp(`(${searchValue})`, 'gi'),
                    '<span class="text-highlight">$1</span>'
                );

                // Create the result item
                const resultItem = document.createElement('div');
                resultItem.className = 'list-group-item';

                resultItem.innerHTML = `
                    <a href="${itemLink}">
                        <strong>${highlightedText}</strong><br>
                        <div class="text-muted">${parentPath} -> ${highlightedText}</div>
                    </a>
                `;

                // Append the result to the results container
                resultsContainer.appendChild(resultItem);
            }
        });

        // Toggle the visibility of the results container
        document.querySelector('.sidebar-search-results').style.display = resultsFound ? 'block' : 'none';
    } else {
        // Hide when search is too short
        document.querySelector('.sidebar-search-results').style.display = 'none';
    }
});
