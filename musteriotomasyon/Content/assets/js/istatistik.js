function Satislar() {
    var url = '/Home/SatislariGetir';
    $('#satistablo').html("");
    var thead = '<thead><tr><th style="width: 50px">Müşteri</th><th style="width: 300px;">GenelToplam</th><th>Tarih</th><th>Garanti Bitiş</th><th>Detay</th></tr></thead >'
    $('#satistablo').html(thead);
    $.getJSON(url, function (data) {
        var tbody = '<tbody>';
        $('#satistablo').append(tbody);
        var say = 0;
        for (var item in data.Result) {
            say++;
            var tarihTurkce = new Date(data.Result[item].Tarih);
            var options = { weekday: 'short', year: 'numeric', month: 'short', day: '2-digit' };
            var user = '<tr><td style="width:40px;">' + data.Result[item].Musteri + '</td><td>' + data.Result[item].GenelToplam + '</td><td style="display:none">' + data.Result[item].Tarih + '</td><td>' + tarihTurkce.toLocaleDateString('tr-TR', options) + '</td><td>' + data.Result[item].GarantiBitis + '</td> <td><a href="/Satis/SatisDetay/' + data.Result[item].DetayID + '" class="badge badge-flat border-primary text-primary-600 "><i class="icon-stack-text"></i></a> </td></tr>';
            $('#satistablo').append(user);
        }
        $('#toplam').append(say);

    });
}
function myFunction() {
    var input, filter, table, tr, td, i, txtValue, start, end, a, b;
    input = document.getElementById("myInputstart");
    filter = input.value.toUpperCase();
    table = document.getElementById("satistablo");
    tr = table.getElementsByTagName("tr");
    start = new Date(document.getElementById("myInputstart1").value);
    end = new Date(document.getElementById("myInputend1").value);
    var say = 0;
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[2];
        if (td) {
            txtValue = new Date(td.innerText);

            //alert("çalıştı");

            var end1 = new Date(end.setDate(end.getDate() + 1));
            if ((txtValue >= start && txtValue <= end1)) {
                say++;
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}