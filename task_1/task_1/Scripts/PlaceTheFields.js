(function() {
  $(document).ready(function() {
    var coordinatesOfAllFields, index, separateCoordinates;
    coordinatesOfAllFields = document.getElementById("Coordinates").value;
    separateCoordinates = coordinatesOfAllFields.split(",");
    index = 0;
    return $(".Used").map(function() {
      var offset;
      offset = $(this).offset();
      offset.top = parseInt(separateCoordinates[index]);
      index++;
      offset.left = parseInt(separateCoordinates[index]);
      index++;
      return $(this).offset(offset);
    });
  });

}).call(this);
