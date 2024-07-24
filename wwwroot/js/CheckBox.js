$(document).ready(function () {
    $("#Parent").click(function () {
        $(".child").prop("checked", this.checked);
        if (this.checked) {
            $("#delete").css("display", "block"); // Show #delete element
        } else {
            $("#delete").css("display", "none"); // Hide #delete element
        }
    });

    $(".child").click(function () {
        if ($(".child:checked").length === $(".child").length) {
            $("#Parent").prop("checked", true);
        } else {
            $("#Parent").prop("checked", false);
        }
        if ($(".child:checked").length > 0) {
            $("#delete").css("display", "block"); // Show #delete element if any child is checked
       
        } else {
            $("#delete").css("display", "none"); // Hide #delete element if no child is checked
        }
    });
});
