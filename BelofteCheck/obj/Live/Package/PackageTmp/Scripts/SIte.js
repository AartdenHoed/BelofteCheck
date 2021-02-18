
$(".bc-active").change (function ()  {
    
    if ($(".bc-dacheck").is(":checked")) {
        deleteallowed = "Y";
    }
    else {
        deleteallowed = "N";
    }

    // alert(deleteallowed); 
    if (deleteallowed == "Y") {
        $(".bc-active").prop("disabled", false); //Delete allowed, enable delete button
    }
    else {
        $(".bc-active").prop("disabled", true); //Delete not allowed, disable delete button
    }
}) 
$(".bc-active").trigger("change");