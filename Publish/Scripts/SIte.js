
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


function setreq(me) {
    x = $(me).is(":checked");
    alert(x);
    if ($(me).is(":checked")) {
        alert("checked");
        $(me).next().attr('required', '');
        $(me).next().removeattr('readonly', false);
    }
    else {
        alert("not checked");
        $(me).next().removeAttr('required');
        $(me).next().attr('readonly', true);
    }
}
$(".bc-area").trigger("change");

