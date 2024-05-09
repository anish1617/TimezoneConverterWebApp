var timezone = (function () {
    return {
        initialize: function () {
            attachHandler();
        }
    }

    function attachHandler() {
        $('body').on('click', '#btnSubmitConvert', function (e) {
            e.preventDefault();

            var formData = $('form').serialize();

            $.ajax({
                url: '/Timezone/ConvertToTimezone',
                method: 'POST',
                data: formData,
                success: function (data) {
                    $("#dvConvertedDetails").html(data);
                },
                error: function (xhr, status, error) {
                    console.log(error);
                }
            });
        });
    }
})();