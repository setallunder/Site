# CoffeeScript
$(document).ready(() ->
            coordinatesOfAllFields = document.getElementById("Coordinates").value
            separateCoordinates = coordinatesOfAllFields.split(",")
            index = 0
            $(".Used").map(() ->
                offset = $(this).offset()
                offset.top = parseInt(separateCoordinates[index])
                index++
                offset.left = parseInt(separateCoordinates[index])
                index++
                $(this).offset(offset)
            )
        )