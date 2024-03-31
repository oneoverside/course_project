adjustImageSize()

document.addEventListener('keydown', function (event) {
    if (event.key === 'ArrowRight') { // Replace 'Enter' with the desired key
        console.log(1)
        $.ajax({
            url: '/Home/NextImage',
            type: 'GET',
            success: function (data) {
                if(data.type === "image") {
                    console.log(data.path)
                    const img = $('#img');
                    const video = $('#video');
                    img.attr('src', data.path);
                    img.show();
                    video.hide();
                    adjustImageSize()
                }
            }
        });
    }

    if (event.key === 'Delete') { // Replace 'Enter' with the desired key
        let x = encodeURIComponent($('#img').attr('src'));
        $.ajax({
            url: '/Home/DelImage/?image=' + x,
            type: 'GET',
            success: function (data) {
                $('#img').attr('src', data.path);
                adjustImageSize()
            }
        });
    }

    if (event.key === 'ArrowLeft') { // Replace 'Enter' with the desired key
        $.ajax({
            url: '/Home/PreviousImage',
            type: 'GET',
            success: function (data) {
                if(data.type === "image") {
                    $('#img').attr('src', data.path);
                    adjustImageSize()
                }
            }
        });
    }

    if (event.key === 'Backspace') { // Replace 'Enter' with the desired key
        
        $.ajax({
            url: '/Home/PreviousImage',
            type: 'GET',
            success: function (data) {
                if(data.type === "image") {
                    $('#img').attr('src', data.path);
                    adjustImageSize()
                }
            }
        });
    }
});

function adjustImageSize() {
    const img = document.getElementById('img'); // Убедитесь, что у изображения есть id="img"
    // console.log(img.naturalWidth)
    // console.log(img.naturalHeight)
    // const imgContainer = img.parentElement;

    const containerRatio = 2560/1440;
    const imageRatio = img.naturalWidth / img.naturalHeight;

    if (imageRatio > containerRatio) {
        console.log("Изображение слишком широкое, нужны отступы по высоте")
        img.style.width = '100vw';
        img.style.height = 'auto';
    } else {
        console.log("Изображение слишком высокое, нужны отступы по ширине")
        img.style.width = 'auto';
        img.style.height = '100vh';
    }
}


