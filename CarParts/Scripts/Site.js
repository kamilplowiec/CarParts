$(function () {

    DateInit();

    function DateInit()
    {
        $(".datepicker").datepicker({

            changeMonth: true,
            changeYear: true,
            dateFormat: 'yy-mm-dd',

            onClose: function () {
                $(this).valid();
            }
        });
    }

    

});