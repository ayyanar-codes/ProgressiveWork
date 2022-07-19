$(document).ready(function () {
   
});




$('#btnSearch').click(function () {
    var name = $('#txtName').val();
    var address = $('#txtAddress').val();
    var number = $('#txtNumber').val();

    var userObj = {
        Name: name,
        Address: address,
        Number: number,
    }

    $.ajax({
        url: "/Home/SaveData",
        data: JSON.stringify(userObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {

            if (response.Result == 'OK') {
                fnRenderData(response.Date);
            } else {
                alert(response.Result);
            }

        },
        error: function (xhr, status, error) {
            fnNotify(error);
        }
    });


    function fnRenderData(employees) {
        var htmRows = '';


        for (var i = 0; i < employees.length; i++) {

            htmRows += '<tr>';
            htmRows += '<td>' + employees[i].Name + '</td>';
            htmRows += '<td>' + employees[i].Address + '</td>';
            htmRows += '<td>' + employees[i].Number + '</td>';
            htmRows += '</tr>';

        }

        htmRows = htmRows || '<td style="text-align:center" colspan="3">No data found</td>';

        $('#tbyData').html(htmRows);
    }

});