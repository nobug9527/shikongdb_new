
//首页
function firstPage(m) {
    if (m == "0") {
        getGZ(1);
    } else if (m == "zp") {
        Premium(1);
    } else {
        var category = $('input[name=hddx]:checked').val();
        if (category == "SingleGoods") {
            goodsSearch(1);
        } else if (category == "Brand") {
            brandSearch(1);
        } else {
            return false;
        }
    }
}

//末页
function lastPage(m) {
    if (m == "0") {
        var count = document.getElementById('pageCount_list').innerHTML;
        var pgCount = parseInt(count);
        getGZ(pgCount);
    } else if (m == "zp") {
        var count = document.getElementById('B16').innerHTML;
        var pgCount = parseInt(count);
        Premium(pgCount);
    } else {
        var count = document.getElementById('B4').innerHTML;
        var pgCount = parseInt(count);
        var category = $('input[name=hddx]:checked').val();
        if (category == "SingleGoods") {
            goodsSearch(pgCount);
        } else if (category == "Brand") {
            brandSearch(pgCount);
        } else {
            return false;
        }
    }

}

//下一页
function nextPage(m) {
    var pgCount;
    var index;
    var count;
    var pgNum;
    if (m == "0") {
        index = document.getElementById('pageIndex_list').innerHTML;
        count = document.getElementById('pageCount_list').innerHTML;
        pgNum = parseInt(index);
        pgCount = parseInt(count);
        if (pgCount === 0) {
            return false;
        }
        pgNum = pgNum + 1 > pgCount ? pgCount : pgNum + 1;
        getGZ(pgNum);
    } else if (m == "zp") {
        index = document.getElementById('B15').innerHTML;
        count = document.getElementById('B16').innerHTML;
        pgNum = parseInt(index);
        pgCount = parseInt(count);
        if (pgCount === 0) {
            return false;
        }
        pgNum = pgNum + 1 > pgCount ? pgCount : pgNum + 1;
        Premium(pgNum);
    } else {
        index = document.getElementById('B3').innerHTML;
        count = document.getElementById('B4').innerHTML;
        pgNum = parseInt(index);
        pgCount = parseInt(count);
        if (pgCount === 0) {
            return false;
        }
        pgNum = pgNum + 1 > pgCount ? pgCount : pgNum + 1;
        var category = $('input[name=hddx]:checked').val();
        if (category === "SingleGoods") {
            goodsSearch(pgNum);
        } else if (category === "Brand") {
            brandSearch(pgNum);
        } else {
            return false;
        }
    }
}

//上一页
function prePage(m) {
    if (m == "0") {
        var index = document.getElementById('pageIndex_list').innerHTML;
        var pgNum = parseInt(index);
        pgNum = pgNum == 1 ? 1 : pgNum - 1;
        getGZ(pgNum);
    } else if (m == "zp") {
        var index = document.getElementById('B15').innerHTML;
        var count = document.getElementById('B16').innerHTML;
        var pgNum = parseInt(index);
        var pgCount = parseInt(count);
        pgNum = pgNum == 1 ? 1 : pgNum - 1;
        Premium(pgNum);
    } else {
        var index = document.getElementById('B3').innerHTML;
        var pgNum = parseInt(index);
        pgNum = pgNum == 1 ? 1 : pgNum - 1;
        var category = $('input[name=hddx]:checked').val();
        if (category == "SingleGoods") {
            goodsSearch(pgNum);
        } else if (category == "Brand") {
            brandSearch(pgNum);
        } else {
            return false;
        }
    }
}

//显示总页数、当前页、总条数
function setRecordCount(m, page, total, index1) {
    if (m == "0") {
        document.getElementById('pageSize_list').innerHTML = page;
        document.getElementById('recordCount_list').innerHTML = total;
        document.getElementById('pageIndex_list').innerHTML = index1;
        if (parseInt(total / page) > 0 && total % page == 0) {
            document.getElementById('pageCount_list').innerHTML = parseInt(total / page);
        }
        if (parseInt(total / page) >= 0 && total % page > 0) {
            document.getElementById('pageCount_list').innerHTML = Math.ceil(total / page);
        }
    } else if (m == "zp") {
        document.getElementById('B13').innerHTML = page;
        document.getElementById('B14').innerHTML = total;
        document.getElementById('B15').innerHTML = index1;
        if (parseInt(total / page) > 0 && total % page == 0) {
            document.getElementById('B16').innerHTML = parseInt(total / page);
        }
        if (parseInt(total / page) >= 0 && total % page > 0) {
            document.getElementById('B16').innerHTML = Math.ceil(total / page);
        }
    } else {
        document.getElementById('B1').innerHTML = page;
        document.getElementById('B2').innerHTML = total;
        document.getElementById('B3').innerHTML = index1;
        if (parseInt(total / page) > 0 && total % page == 0) {
            document.getElementById('B4').innerHTML = parseInt(total / page);
        }
        if (parseInt(total / page) >= 0 && total % page > 0) {
            document.getElementById('B4').innerHTML = Math.ceil(total / page);
        }
    }
}

function alertFun(text, callback, type) {
    if (type == undefined)//s=success,w=waring,f=fail
        type = 's';
    layalert(type, text, '信息', '确定', function () {
        if (typeof callback == 'function')
            callback();
    });
}
function confirmFun(text, callback) {
    layconfirm(text, '提示', '确认', function () {
        if (typeof callback == 'function')
            callback();
    }, '取消', function () { });
}