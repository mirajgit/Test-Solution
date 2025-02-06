document.getElementById('SearchInput').addEventListener('input', function () {
    const searchValue = this.value.toLowerCase();
    const allMenuItems = document.querySelectorAll('.sub-slide-item, .side-menu__item, .slide-item'); // Select all relevant menu items
    const resultsContainer = document.querySelector('.sidebar-search-results .list-group'); // Results container
    resultsContainer.innerHTML = ''; // Clear previous search results

    // Only proceed if the input has at least 3 characters
    if (searchValue.length >= 3) {
        let resultsFound = false; // Flag to track matches

        allMenuItems.forEach(item => {
            const itemText = item.textContent.trim(); // Get the text content of the link
            const itemTextLower = itemText.toLowerCase(); // Convert to lowercase for comparison
            const itemLink = item.getAttribute('href'); // Get the link (href attribute)
            if (itemLink === '#' || itemLink === '/' || !itemLink) {
                return;
            }
            // Check if the text or link matches the search value
            if (itemTextLower.includes(searchValue) || itemLink.toLowerCase().includes(searchValue)) {
                resultsFound = true;

                // Highlight the matched text
                const highlightedText = itemText.replace(
                    new RegExp(`(${searchValue})`, 'gi'),
                    '<span class="text-highlight">$1</span>'
                );

                // Extract the correct paths for Modelvalue, FeatureValue, or Parent Path
                let parentItem = item.closest('.sub-slide'); // Get the parent menu item (e.g., HRM/Sales)
                let grandParentItem = parentItem ? parentItem.closest('.slide') : null; // Get the grandparent item (HRM, Dashboard)

                // Check if we found a grandparent
                let parentPath = '';
                let Modelvalue = '';
                let FeatureValue = '';
                if (grandParentItem) {
                    parentPath = grandParentItem.querySelector('.side-menu__label') ?
                        grandParentItem.querySelector('.side-menu__label').textContent.trim() : '';
                    // Extract Modelvalue and FeatureValue for first template
                    Modelvalue = grandParentItem.querySelector('.side-menu__label') ?
                        grandParentItem.querySelector('.side-menu__label').textContent.trim() : '';
                    FeatureValue = parentItem.querySelector('.sub-side-menu__label') ?
                        parentItem.querySelector('.sub-side-menu__label').textContent.trim() : '';
                } else {
                    parentItem = item.closest('.slide'); // Otherwise, use the slide item as the parent
                    parentPath = parentItem.querySelector('.side-menu__label') ?
                        parentItem.querySelector('.side-menu__label').textContent.trim() : '';
                }

                // Construct a result item
                const resultItem = document.createElement('div');
                resultItem.className = 'list-group-item';

                // Make the result clickable and conditionally render the HTML
                if (Modelvalue && FeatureValue) {
                    resultItem.innerHTML = `
                        <a href="${itemLink}">
                            <strong>${highlightedText}</strong><br>
                            <div class="text-muted">${Modelvalue} -> ${FeatureValue}</div>
                        </a>
                    `;
                }
                else {
                    resultItem.innerHTML = `
                        <a href="${itemLink}">
                            <strong>${highlightedText}</strong><br>
                            <div class="text-muted">${parentPath} -> ${highlightedText}</div>
                        </a>
                    `;
                }
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
