function changeVatPercent(index) {
    tinhTienVat(index);
}

function changeOtherCurrency(index) {
    var selectedCurrency = $('#' + 'cuocPhi_maTienTe_' + index).find(':selected').val();

    $('.cuocPhi_maTienTe').each(function (index, element) {
        // element == this
        $(element).val(selectedCurrency);
    });

    if ($('#cuocPhi_totalTongCuoc').text() != "") {
        $('#cuocPhi_totalTongCuoc').text($('#cuocPhi_totalTongCuoc').text().split(" ")[0] + " " + $('#cuocPhi_maTienTe_1').find(':selected').text());
    }
}

function tinhTienVat(index) {
    //tinh VAT theo số tiền và %VAT
    var sotien = $('#' + 'cuocPhi_soTien_' + index).val();
    var stringPercent = $('#' + 'cuocPhi_vatPercent_' + index).find(':selected').text();
    var percent = 0;
    if ($.isNumeric(stringPercent.trim())) {
        percent = parseFloat(stringPercent.trim()) / 100;
    }
    var tienVat = sotien * percent;
    $('#' + 'cuocPhi_tienVat_' + index).val(parseFloat(tienVat).toFixed(2));

    reCalculate_SoTien_VAT_TongCuoc();

    ////Tính tổng số tiền và update text
    //var totalSoTien = 0;
    //$('.cuocPhi_soTien').each(function (index, element) {
    //    // element == this
    //    if ($.isNumeric($(element).val())) {
    //        totalSoTien = parseFloat(totalSoTien);
    //        totalSoTien += parseFloat($(element).val());
    //    };
    //});
    //$('#cuocPhi_totalSoTien').text(totalSoTien);

    ////Tính tổng số phí VAT và update text
    //var totalSoTienVAT = 0;
    //$('.cuocPhi_tienVat').each(function (index, element) {
    //    // element == this
    //    if ($.isNumeric($(element).val())) {
    //        var temp = $(element).val();
    //        var temp2 = parseFloat($(element).val());
    //        totalSoTienVAT = parseFloat(totalSoTienVAT) + parseFloat($(element).val());
    //    };
    //});
    //$('#cuocPhi_totalTienVat').text(parseFloat(totalSoTienVAT).toFixed(2));

    ////Tính tổng cước và update text
    //$('#cuocPhi_totalTongCuoc').text((totalSoTienVAT + totalSoTien) + ' ' + $('#cuocPhi_maTienTe_1').find(':selected').text());
}

function parseToInt(stringInput) {
    return (stringInput == null || (stringInput!= null && stringInput.trim() == "")) ? 0 : parseInt(stringInput.trim(),10);
}

function parseToFloat(stringInput) {
    return (stringInput == null || (stringInput != null && stringInput.trim() == "")) ? 0 : parseFloat(stringInput.trim()).toFixed(2);
}

function changeValueOfElement(elementId, changedValue) {
    $('#' + elementId).val(changedValue);

    //Loại PTVT: duong hang khong thì Tên PTVC disbale
    if (elementId == "internationalBill_loaiPTVT_select" || elementId == "internationalBill_loaiPTVT") {
        if (changedValue == "1") {
            $('#internationalBill_tenPTVC_Code').prop('disabled', true);
        } else {
            $('#internationalBill_tenPTVC_Code').prop('disabled', false);
        }
    } else {
        $('#internationalBill_tenPTVC_Code').prop('disabled', false);
    }

}

function onSoLuong_ThongTinHangHoa() {
    var total = 0;
    $('.numberARow').each(function (index, element) {
        // element == this
        if ($.isNumeric($(element).val())) {
            total = parseInt(total, 10);
            total += parseInt($(element).val(), 10);
        };
    });
    $('#totalNumber').text(total);
}

var currentNumber_thongTinHangHoa = 1;
function addNewRow_thongTinHangHoa() {
    currentNumber_thongTinHangHoa++;
    $("#tbodyThongTinHangHoa").append("<tr><td scope = \"row\" >" + currentNumber_thongTinHangHoa + "</td><td><input class=\"form-control input-sm\" type=\"text\" id=\"mota_" + currentNumber_thongTinHangHoa + "\"></td><td><select class=\"form-control input-sm\" id=\"dvt_" + currentNumber_thongTinHangHoa + "\"></select></td><td><input class=\"form-control input-sm numberARow\" onchange=\"onSoLuong_ThongTinHangHoa()\" type=\"number\" id=\"soluong_" + currentNumber_thongTinHangHoa + "\"></td></tr>");
    var htmlText = $("#donViTinhHtml_temp").val();
    $("#dvt_" + currentNumber_thongTinHangHoa).append(htmlText);
}

function removeLastRow_thongTinHangHoa() {
    if (currentNumber_thongTinHangHoa > 1) {
        $('#table_thongTinHangHoa tr:last').remove();
        currentNumber_thongTinHangHoa--;
    }
    onSoLuong_ThongTinHangHoa();
}

var currentNumber_cuocPhi = 1;
function addNewRow_cuocPhi() {
    currentNumber_cuocPhi++;
    $("#tbodyCuocPhi").append("<tr><td scope = \"row\">" + currentNumber_cuocPhi + "</td><td><select class=\"form-control input-sm\" id=\"cuocPhi_loaiCuocPhi_" + currentNumber_cuocPhi + "\"><option value=\"1\">Cước chính</option><option value=\"2\">Xăng dầu</option><option value=\"3\">Quá khổ</option><option value=\"4\">Hàng đặc biệt</option><option value=\"5\">Đóng kiện gỗ</option><option value=\"6\">Vùng sâu vùng xa</option><option value=\"7\">Hun trùng</option><option value=\"8\">Dân cư</option><option value=\"9\">Trả hộ</option><option value=\"10\">Bảo hiểm</option><option value=\"11\">Môi giới, hoa hồng</option><option value=\"12\">Đóng gói</option><option value=\"13\">Trả trước, tạm ứng</option><option value=\"14\">Trợ giúp</option><option value=\"15\">Loại khác</option></select></td><td><input class=\"form-control input-sm cuocPhi_soTien\" type=\"number\" id=\"cuocPhi_soTien_" + currentNumber_cuocPhi + "\" onchange=\"tinhTienVat(" + currentNumber_cuocPhi + ")\"></td><td><select class=\"form-control input-sm no_padding_left_right cuocPhi_maTienTe\" id=\"cuocPhi_maTienTe_" + currentNumber_cuocPhi + "\" onchange=\"changeOtherCurrency(" + currentNumber_cuocPhi + ")\"><option value=\"\">Chọn</option><option value=\"USD\">USD</option><option value=\"VND\">VND</option><option value=\"EUR\">EUR</option><option value=\"GBP\">GBP</option><option value=\"HKD\">HKD</option><option value=\"AUD\">AUD</option><option value=\"CAD\">CAD</option><option value=\"SGD\">SGD</option><option value=\"CNY\">CNY</option><option value=\"CHF\">CHF</option><option value=\"DKK\">DKK</option><option value=\"IDR\">IDR</option><option value=\"INR\">INR</option><option value=\"JPY\">JPY</option><option value=\"KHR\">KHR</option><option value=\"KRW\">KRW</option><option value=\"LAK\">LAK</option><option value=\"MOP\">MOP</option><option value=\"MYR\">MYR</option><option value=\"NOK\">NOK</option><option value=\"NZD\">NZD</option><option value=\"RUB\">RUB</option><option value=\"SEK\">SEK</option><option value=\"THB\">THB</option><option value=\"TRY\">TRY</option><option value=\"TWD\">TWD</option></select></td><td><select class=\"form-control input-sm no_padding_left_right\" id=\"cuocPhi_vatPercent_" + currentNumber_cuocPhi + "\"  onchange=\"changeVatPercent(" + currentNumber_cuocPhi + ")\"><option value=\"\">Chọn</option><option value=\"1\">5</option><option value=\"2\">10</option><option value=\"2\">15</option><option value=\"2\">20</option><option value=\"2\">0</option><option value=\"2\">KCT</option></select></td><td> <input class=\"form-control input-sm cuocPhi_tienVat\" type=\"number\" id=\"cuocPhi_tienVat_" + currentNumber_cuocPhi + "\" disabled></td></tr>");
    remove_Chosen_LoaiCuocPhi();
}

function remove_Chosen_LoaiCuocPhi() {
    for (var i = currentNumber_cuocPhi - 1; i >= 1; i--) {
        var valueSelected = $('#cuocPhi_loaiCuocPhi_' + i).val()
        $("#cuocPhi_loaiCuocPhi_" + currentNumber_cuocPhi + " option[value='" + valueSelected+ "']").remove();
    }
}

function removeLastRow_cuocPhi() {
    if (currentNumber_cuocPhi > 1) {
        $('#table_CuocPhi tr:last').remove();
        currentNumber_cuocPhi--;
    }
    //tinh lại gia tri
    reCalculate_SoTien_VAT_TongCuoc();
}

function reCalculate_SoTien_VAT_TongCuoc() {
    //sotien
    var totalSoTien = 0;
    $('.cuocPhi_soTien').each(function (index, element) {
        // element == this
        if ($.isNumeric($(element).val())) {
            totalSoTien = parseFloat(totalSoTien);
            totalSoTien += parseFloat($(element).val());
        };
    });
    $('#cuocPhi_totalSoTien').text(parseFloat(totalSoTien).toFixed(2));

    var totalSoTienVAT = 0;
    $('.cuocPhi_tienVat').each(function (index, element) {
        // element == this
        if ($.isNumeric($(element).val())) {
            var temp = $(element).val();
            var temp2 = parseFloat($(element).val());
            totalSoTienVAT = parseFloat(totalSoTienVAT) + parseFloat($(element).val());
        };
    });
    $('#cuocPhi_totalTienVat').text(parseFloat(totalSoTienVAT).toFixed(2));

    var maTien = $('#cuocPhi_maTienTe_1').val() == "" ? "" : $('#cuocPhi_maTienTe_1').find(':selected').text()
    $('#cuocPhi_totalTongCuoc').text((totalSoTienVAT + totalSoTien) + ' ' + maTien);
}

function addMoreThongTinNguoiGuiNhan() {
    $("#myModal").modal()
}

