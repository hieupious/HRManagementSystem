$(function () {
    var table = $("table"),
        count = $("tr", table).length - 3,
        checkoutRow = $("tr", table).eq(1),
        output = $("<textarea style='width: 100%; height: 1000px'></textarea>");

    for (var checkinIndex = 0; checkinIndex < count; checkinIndex++) {
        for (var checkoutIndex = 0; checkoutIndex < count; checkoutIndex++) {
            var row = $("tr", table)[checkinIndex + 2],
                cell = $("td", row).eq(checkoutIndex + 1),
                checkin = $("td", row).eq(0).html(),
                checkinHour = Number(checkin.split(":")[0]),
                checkinMinute = Number(checkin.split(":")[1]),
                checkout = $("td", checkoutRow).eq(checkoutIndex + 1).html(),
                checkoutHour = Number(checkout.split(":")[0]),
                checkoutMinute = Number(checkout.split(":")[1]),
                lackingTime = cell.html(),
                lackingTimeHours = Number(lackingTime.split(":")[0]),
                lackingTimeMinutes = Number(lackingTime.split(":")[1]),
                lackingTimeInMinutes = lackingTimeHours * 60 + lackingTimeMinutes;

            if (lackingTime != "_") {
                output.append("[InlineData(" + checkinHour + ", " + checkinMinute + ", " + checkoutHour + ", " + checkoutMinute + ", " + lackingTimeInMinutes + ")]\n");
            }
        }
    }

    $("body").append(output);
});
