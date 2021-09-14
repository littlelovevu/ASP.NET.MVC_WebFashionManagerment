$(document).ready(function () {
    loadDuLieu();
});

function loadDuLieu() {
    $.ajax({
        url: "/QuanLy/List",
        type: "GET",
        contentType: "application/json; charset = utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.MaSanPham + '</td>';
                html += '<td>' + item.TenSanPham + '</td>';
                html += '<td>' + item.Gia + '</td>';
                html += '<td>' + item.HinhMinhHoa + '</td>';
                html += '<td>' + item.DanhSachHinh + '</td>';
                html += '<td>' + item.MaLSP + '</td>';
                html += '<td>' + item.MaNSX + '</td>';
                html += '<td><a class="btn btn-dark w-100 text-center" href="#" onclick="return getbyID(' + item.id + ')">SỬA</a></td>';
                html += '<td><a class="btn btn-dark w-100 text-center" href="#" onclick="Delete(' + item.id + ')">XÓA</a></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}