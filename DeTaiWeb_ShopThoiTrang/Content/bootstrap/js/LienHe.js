$(document).ready(function () {
    loadData();
});

function loadData() {
    $.ajax({
        url: "/Admin/ListLienHe",
        type: "GET",
        contentType: "application/json; charset = utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td class="text-center">' + item.MaLienHe + '</td>';
                html += '<td class="text-center">' + item.Ten + '</td>';
                html += '<td class="text-center">' + item.SDT + '</td>';
                html += '<td class="text-center">' + item.Email + '</td>';
                html += '<td class="text-center">' + item.TieuDe + '</td>';
                html += '<td class="text-center">' + item.NoiDung + '</td>';
                html += '<td><a class="btn btn-dark w-100 text-center" href="#" onclick="Delete(' + item.MaLienHe + ')">XÓA</a></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Delete(ID) {
    var thongbao = confirm("BẠN CÓ MUỐN XÓA LIÊN HỆ NÀY ?? !!");
    if (thongbao) {
        $.ajax({
            url: "/Admin/XoaLienHe/" + ID,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                loadData();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}