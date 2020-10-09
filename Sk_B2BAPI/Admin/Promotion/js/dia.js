$(function(){  
    var screenwidth,screenheight,mytop,getPosLeft,getPosTop  
    screenwidth = $(window).width();  
    screenheight = $(window).height();  
    //获取滚动条距顶部的偏移  
    mytop = $(document).scrollTop();  
    //计算弹出层的left  
    getPosLeft = screenwidth/2 - 260;  
    //计算弹出层的top  
    getPosTop = screenheight/2 - 150;  
    //css定位弹出层  
    $("#box").css({"left":getPosLeft,"top":getPosTop});  
    //当浏览器窗口大小改变时...  
    $(window).resize(function(){  
         screenwidth = $(window).width();  
       screenheight = $(window).height();  
        mytop = $(document).scrollTop();  
       getPosLeft = screenwidth/2 - 260;  
        getPosTop = screenheight/2 - 150;  
        $("#box").css({"left":getPosLeft,"top":getPosTop+mytop});  
    });  
  
    //当拉动滚动条时...  
    $(window).scroll(function(){  
        screenwidth = $(window).width();  
       screenheight = $(window).height();  
       mytop = $(document).scrollTop();  
       getPosLeft = screenwidth/2 - 260;  
        getPosTop = screenheight/2 - 150;  
       $("#box").css({"left":getPosLeft,"top":getPosTop+mytop});  
    });  
  
    //点击链接弹出窗口  
    $("#popup").click(function(){  
        $("#box").fadeIn("fast");  
       //获取页面文档的高度  
       var docheight = $(document).height();  
       //追加一个层，使背景变灰  
        $("body").append("<div id='greybackground'></div>");  
       $("#greybackground").css({"opacity":"0.5","height":docheight});  
       return false;  
    });  
  
    //点击关闭按钮  
    $("#closeBtn").click(function() {  
        $("#box").hide();  
      //删除变灰的层  
       $("#greybackground").remove();  
        return false;  
    });  
  
});  