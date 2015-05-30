(function() {
  $(document).ready(function() {
    return $('#CardArea, #fields').sortable({
      connectWith: "#CardArea, #fields"
    }).disableSelection();
  });

}).call(this);
