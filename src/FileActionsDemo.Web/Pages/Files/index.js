$(function () {
    var DOWNLOAD_ENDPOINT = "/download";
    var downloadForm = $("form#DownloadFile");

    

    downloadForm.submit(function (event) {
        event.preventDefault();

        var fileName = $("#fileName").val().trim();

        var downloadWindow = window.open(
            DOWNLOAD_ENDPOINT + "/" + fileName,
            "_blank"
        );
        downloadWindow.focus();
    });

    $("#UploadFileDto_File").change(function () {
        var fileName = $(this)[0].files[0].name;

        $("#UploadFileDto_Name").val(fileName);
    })

    async function AJAXSubmit(oFormElement) {
        var resultElement = oFormElement.elements.namedItem("result");
        const formData = new FormData(oFormElement);

        try {
            const response = await fetch(oFormElement.action, {
                method: 'POST',
                body: formData
            });

            if (response.ok) {
                window.location.href = '/';
            }

            resultElement.value = 'Result: ' + response.status + ' ' +
                response.statusText;
        } catch (error) {
            console.error('Error:', error);
        }
    }
});