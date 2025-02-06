document.getElementById('SearchInput').addEventListener('input', function () {
    const searchValue = this.value.toLowerCase(); // Convert search input to lowercase
    const allMenuItems = document.querySelectorAll('.sub-slide-item'); // Select all menu items
    const resultsContainer = document.querySelector('.sidebar-search-results .list-group'); // Results container

    // Clear previous search results
    resultsContainer.innerHTML = '';

    // Only proceed if the input has at least 3 characters
    if (searchValue.length >= 3) {
        let resultsFound = false; // Flag to track matches

        allMenuItems.forEach(item => {
            const itemText = item.textContent.trim(); // Get the text content of the link
            const itemTextLower = itemText.toLowerCase(); // Convert to lowercase for comparison
            const itemLink = item.getAttribute('href'); // Get the link (href attribute)

            // Check if the text or link matches the search value
            if (itemTextLower.includes(searchValue) || itemLink.toLowerCase().includes(searchValue)) {
                resultsFound = true;

                // Highlight the matched text
                const highlightedText = itemText.replace(
                    new RegExp(`(${searchValue})`, 'gi'),
                    '<span class="text-highlight">$1</span>'
                );

                // Now, extract the correct path for Modelvalue and FeatureValue
                let parentItem = item.closest('.sub-slide'); // Get the parent menu item (the section of HRM/Sales, etc.)
                let grandParentItem = parentItem.closest('.slide'); // Get the grandparent item (HRM, Dashboard, etc.)

                // Extract the correct paths based on the parent/child hierarchy
                const Modelvalue = grandParentItem.querySelector('.side-menu__label') ?
                    grandParentItem.querySelector('.side-menu__label').textContent.trim() : '';
                const FeatureValue = parentItem.querySelector('.sub-side-menu__label') ?
                    parentItem.querySelector('.sub-side-menu__label').textContent.trim() : '';

                // Construct a result item
                const resultItem = document.createElement('div');
                resultItem.className = 'list-group-item';

                // Make the result clickable
                resultItem.innerHTML = `
                    <a href="${itemLink}">
                        <strong>${highlightedText}</strong><br>
                        <div class="text-muted">${Modelvalue} -> ${FeatureValue}</div>
                    </a>
                `;

                // Add the result to the container
                resultsContainer.appendChild(resultItem);
            }
        });

        // Show or hide the results container
        document.querySelector('.sidebar-search-results').style.display = resultsFound ? 'block' : 'none';
    } else {
        // Hide the results container if less than 3 characters
        document.querySelector('.sidebar-search-results').style.display = 'none';
    }
});
