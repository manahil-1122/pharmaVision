// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
<script>
    document.addEventListener("DOMContentLoaded", function () {
        const storedOrderCount = localStorage.getItem('orderCount');
    if (storedOrderCount !== null) {
        document.getElementById('orderCount').textContent = storedOrderCount;
        }
    });

    function addOrder() {
        // Logic to add a new order
        let currentOrderCount = parseInt(localStorage.getItem('orderCount')) || 0;
    currentOrderCount += 1;
    localStorage.setItem('orderCount', currentOrderCount);

    // Update the order count in the navbar
    document.getElementById('orderCount').textContent = currentOrderCount;
    };

    

</script>


// Write your JavaScript code.
