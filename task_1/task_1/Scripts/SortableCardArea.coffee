# CoffeeScript
$(document).ready(() ->
        $('#CardArea, #fields').sortable(
            connectWith: "#CardArea, #fields"
        ).disableSelection();
    );