function fileBtn() {
    $('#file').click();
}
function PostFile(obj) {
    var file = obj.files[0];
    var msg = new FormData();
    msg.append("file", file);
    $.ajax({
        type: 'POST',
        cache: false,
        url: 'ashx/PrizePictureUpload.ashx',
        data: msg,
        processData: false,  // 不处理数据
        contentType: false,  // 不设置内容类型
        success: function (date) {
            var s = JSON.parse(date);
            if (s.return_code == "0") {
                //$('#pictureText').val($('#file').val());
                $('#pictureText').val(s.data[1].picture);
                $('#hiddenUrl').val(s.data[1].picture);
                alertFun(s.data[0].message, function () { }, 's');
            } else if (s.return_code == "1") {
                alertFun(s.data[0].message, function () { }, 'f');
            }
        }
    });
}