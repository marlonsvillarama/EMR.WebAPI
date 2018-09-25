function initFieldMasks() {

    jQuery('.ehr-fld-dt')
        .datepicker({
            changeMonth: true,
            changeYear: true
        })
        .mask('00/00/0000');

    //jQuery('.ehr-fld-phone').mask('(000) 000-0000');

}



