document.addEventListener('DOMContentLoaded', function (event) {
    console.log(12450)
    $.ajax({
        url: '/Home/GetAll/',
        type: 'GET',
        success: function (data) {
            console.log(1)
            for(let i = 0; i < data.length; i++){
                console.log(2)
                let imageUrl = data[i];
                console.log(imageUrl)
                const img = document.createElement('img');
                img.src = imageUrl;
                img.alt = 'Image';
                img.style.height = '400px';
                img.style.width = '400px';
                img.style.margin = '5px';
                img.style.objectFit = 'contain';
                $('#gallery').append(img)
            }
        }
    });
});

function upload_file() {    
    console.log("HALLo")
    const fileInput = document.getElementById('file');
    const file = fileInput.files[0];

    const formData = new FormData();
    formData.append('file', file);

    const xhr = new XMLHttpRequest();

    xhr.open('POST', 'Home/UploadImage/', true);

    xhr.onload = function () {
        if (xhr.status === 200) {
            window.location.reload();
        } else {
            alert('An error occurred while uploading the file.');
        }
    };

    xhr.send(formData);
}

function start_gallery() {
    window.location.href = "Home/Gallery" 
}