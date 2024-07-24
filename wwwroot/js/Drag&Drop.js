
var dropArea = document.getElementById('dropArea');
var fileInput = document.getElementById('ImagePath');

// Prevent default drag behaviors
['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
    dropArea.addEventListener(eventName, preventDefaults, false);
});

// Handle drop event
dropArea.addEventListener('drop', handleDrop, false);

function preventDefaults(e) {
    e.preventDefault();
    e.stopPropagation();
}

function handleDrop(e) {
    var dt = e.dataTransfer;
    var files = dt.files;

    if (files.length > 0) {
        fileInput.files = files; // Assign dropped files to the hidden file input
        handleFiles(files); // Handle the dropped files for preview or other operations
    }
}

// Optional: Handle file preview or display
function handleFiles(files) {
    var file = files[0]; // Assuming only one file is dropped
    var reader = new FileReader();

    reader.onload = function (e) {
        // Display the selected image or do something with it
        var img = new Image();
        img.src = e.target.result;
        img.style.maxWidth = '100%'; // Adjust styling as needed
        dropArea.innerHTML = ''; // Clear drop area
        dropArea.appendChild(img); // Append the image to the drop area
    }

    reader.readAsDataURL(file);
}

// Clicking on drop area triggers file input click
dropArea.addEventListener('click', function () {
    fileInput.click();
});

// Handle file input change event
fileInput.addEventListener('change', function (e) {
    var files = this.files;
    if (files.length > 0) {
        handleFiles(files); // Handle selected files for preview or other operations
    }
});