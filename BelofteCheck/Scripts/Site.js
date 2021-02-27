﻿
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
    $(me).parent().parent().find('.bc-area').first().removeAttr('cols');
    //x = $(me).is(":checked");
    // alert(x);
    if ($(me).is(":checked")) {
        // alert("checked");        
        x = $(me).parent().parent().find('.bc-area').first().val(); 
        if (x == "(Niet gekoppeld aan deze wet)") {
            $(me).parent().parent().find('.bc-area').first().val("");
            $(me).parent().parent().find('.bc-area').first().next().text('Toelichting is verplicht indien aangevinkt'); 
        }
        $(me).parent().parent().find('.bc-area').first().attr('required', true);
        $(me).parent().parent().find('.bc-area').first().attr('readonly', false);       
       
    }
    else {
        // alert("not checked");
        $(me).parent().parent().find('.bc-area').first().val("(Niet gekoppeld aan deze wet)"); 
        $(me).parent().parent().find('.bc-area').first().attr('required', false);
        $(me).parent().parent().find('.bc-area').first().attr('readonly', true);
        $(me).parent().parent().find('.bc-area').first().next().text(''); 
        
        
    }
}

$(".bc-areatrigger").trigger("change");

