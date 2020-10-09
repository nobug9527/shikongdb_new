$(function () {

    //右侧在线咨询

    $(".services").hover(function () {

        $(this).addClass("hover");

    }, function () {

        $(this).removeClass("hover");

    }) ;



   

    //当滚动条的位置处于距顶部100像素以下时，跳转链接出现，否则消失

    $(function () {

        // $(window).scroll(function(){
        //
        //     if ($(window).scrollTop()>100){
        //
        //         $("#totop").fadeIn();
        //
        //     }
        //
        //     else
        //
        //     {
        //
        //         $("#totop").fadeOut();
        //
        //     }
        //
        // });

        //当点击跳转链接后，回到页面顶部位置

        $("#totop").click(function(){

            $('body,html').animate({scrollTop:0},600);

            return false;

        });

    });


    //二维码显示隐藏

    $(".attention").hover(function () {

        $(this).addClass("a_tab");

        $(".pic").show();

        $(this).css({

            "color":"#6c6c6c"

        });

    }, function () {

        $(".pic").hide();

        $(this).removeClass("a_tab");

    });
    
    
        $(".phone-app").hover(function () {

        $(this).addClass("a_tab");

        $(".app-ewm").show();

        $(this).css({

            "color":"#6c6c6c"

        });

    }, function () {

        $(".app-ewm").hide();

        $(this).removeClass("a_tab");

    });

	

	//顶部关闭广告

	$(".site-bn-bar-close").click(function () {

        $(".site-bn-bar").remove();

    });

    
    //左侧导航

    $(".category-list .child-li").hover( function () {

        var cur =$(this).index();

        $(this).addClass("on");

        $(".category-list-child").show();

        $(".category-list-child li").eq(cur).show().siblings("li").hide();



        $(this).prev().find("p").css("border-bottom",0);
        $(this).find("p").css("border-bottom",0);

    }, function () {

        $(this).removeClass("on");

//      $(this).prev().find("p").css("border-bottom","1px solid #e0e0e0");




    });


    $(".category-list").hover(function () {

        $(".category-list-child").show();

    }, function () {

        $(".category-list-child").hide();

    });

     $(".category-list-child").hover(function () {

       $(this).show();


    }, function () {

        $(this).hide();
    });



    $(".category-list-child li").hover(function () {

        var cur=$(this).index();

        var _this= $(".category-list .child-li");

        _this.eq(cur).addClass("on");

        _this.eq(cur-1).find("p").css("border-bottom",0)



    }, function () {

        var cur=$(this).index();

        var _this= $(".category-list .child-li");

        _this.eq(cur).removeClass("on");

        _this.eq(cur-1).removeClass("on").find("p").css("border-bottom","1px solid #e0e0e0");

    });



    $(".jiyao").hover(function () {

        $(".category-list-child").addClass("big_box");

    }, function () {

        $(".category-list-child").removeClass("big_box");

    });

    // 推荐厂家效果

    $(".business-recommend img").each(function(k,img){

        new JumpObj(img,10);


    });

     //左侧列表效果


    $(".week-sales ul li").mouseover(function () {

        $(this).addClass("first").siblings("li").removeClass("first");

    });

     //弹出广告效果

    $('.close').click(function(){

        $('.alert_mark').hide(0);

        $('.content_mark_div').hide(0);

    });

    $("#J_payonline").click(function () {

        $('.alert_mark').show(0);

        $('.content_mark_div').show(0).css("filter","alpha(opacity=60)");

    });






});



//商家
    $(".category-list1 .child-li1").hover( function () {

        var cur =$(this).index();

        $(this).addClass("on");

        $(".category-list-child1").show();

        $(".category-list-child1 li").eq(cur).show().siblings("li").hide();



        $(this).prev().find("p").css("border-bottom",0);
        $(this).find("p").css("border-bottom",0);

    }, function () {

        $(this).removeClass("on");

        $(this).prev().find("p").css("border-bottom","1px solid #e0e0e0");
        $(this).find("p").css("border-bottom","1px solid #e0e0e0");




    });


    $(".category-list1").hover(function () {

        $(".category-list-child1").show();

    }, function () {

        $(".category-list-child1").hide();

    });

     $(".category-list-child1").hover(function () {

       $(this).show();


    }, function () {

        $(this).hide();
    });



    $(".category-list-child1 li").hover(function () {

        var cur=$(this).index();

        var _this= $(".category-list1 .child-li1");

        _this.eq(cur).addClass("on");

        _this.eq(cur-1).find("p").css("border-bottom",0)



    }, function () {

        var cur=$(this).index();

        var _this= $(".category-list1 .child-li1");

        _this.eq(cur).removeClass("on");

        _this.eq(cur-1).removeClass("on").find("p").css("border-bottom","1px solid #e0e0e0");

    });



    $(".jiyao1").hover(function () {

        $(".category-list-child1").addClass("big_box1");

    }, function () {

        $(".category-list-child1").removeClass("big_box1");

    });
