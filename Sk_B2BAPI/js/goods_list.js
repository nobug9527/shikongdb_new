$(function () {
    $(".open").click(function () {
        $(".ul_list1").hide();
        $(".ul_list2").show();
        $(".up_ico").css("background-position","-75px -31px")

    });
    $(".close").click(function () {
        $(".ul_list1").show();
        $(".ul_list2").hide();
        $(".up_ico").css("background-position","-50px 0px")
    })

    //左侧列表效果


    $(".list_next ul li").mouseover(function () {
        $(this).addClass("first").siblings("li").removeClass("first");

    });


    $("table tr td.com").click(function () {
        $(this).addClass("sort").siblings().removeClass("sort");
    })

	//导航条效果
    $('ul.nav_title li a').click(function(){
        $(this).addClass("checked").parent().siblings().find("a").removeClass("checked");

   });


	//左侧导航
    $(".title_nav").hover(function () {
        $(".left_nav").stop().show();
        $(".title_nav .title_ico").hide();


    }, function () {
        $(".left_nav").stop().hide();
        $(".title_nav .title_ico").show();

    });


    //左侧导航
    $(".nav_list .child_li").hover( function () {
        var cur =$(this).index();
        $(this).addClass("on");
        $(".box").show();
        $(".box li").eq(cur).show().siblings("li").hide();
        $(this).prev().find("p").css("border-bottom",0);


    }, function () {

        $(this).removeClass("on");
        $(this).prev().find("p").css("border-bottom","1px solid #e0e0e0");

    });


    $(".left_nav").hover(function () {
        $(".box").show();
    }, function () {
        $(".box").hide();
    });
    $(".box li").hover(function () {
        var cur=$(this).index();
        var _this= $(".nav_list .child_li");

        _this.eq(cur).addClass("on");
        _this.eq(cur-1).find("p").css("border-bottom",0)

    }, function () {
        var cur=$(this).index();
        var _this= $(".nav_list .child_li");
        _this.eq(cur).removeClass("on");
        _this.eq(cur-1).removeClass("on").find("p").css("border-bottom","1px solid #e0e0e0");
    });

	$(".jiyao").hover(function () {
        $(".box").addClass("big_box");
    }, function () {
        $(".box").removeClass("big_box");
    });
   
	$(".ul_list1 ul li").click(function () {
        $(this).addClass("on_click").siblings().removeClass("on_click");

    });

	$(".cart_tab ul li.child").hover(function () {
        $(this).addClass("on_check").siblings().removeClass("on_check");

    }, function () {
        $(this).removeClass("on_check");
    });

	$(".cart_tab ul li.child_1").hover(function () {
        $(this).addClass("on_check").siblings().removeClass("on_check");

    }, function () {
        $(this).removeClass("on_check");
    });

	$(".table2_td2").hover(function () {
        $(this).find(".a_img").show();
    }, function () {
        $(".a_img").hide();
    });

	// 推荐厂家效果
	$(".list_top img").each(function(k,img){
        new JumpObj(img,10);
        //第一个参数代表元素对象
        //第二个参数代表抖动幅度
    });

});