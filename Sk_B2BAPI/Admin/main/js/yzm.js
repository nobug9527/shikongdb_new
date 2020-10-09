createCode()

var code;
function createCode() {  //函数体
    code = "";
    var codeLength = 4; //验证码的长度
    var checkCode = document.getElementById("checkCode");
    var codeChars = new Array(
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
        'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
        'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z');
    for (var i = 0; i < codeLength; i++) {
        var charNum = Math.floor(Math.random() * 52);//设置随机产生
        code += codeChars[charNum];
    }
    if (checkCode) {
        checkCode.className = "code";
        checkCode.innerHTML = code;
    }
}