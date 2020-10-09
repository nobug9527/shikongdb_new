//添加运费模板，提交表单
//function doSubmit(){
	
//	//构造费用及选择地区字符串
//	doBeforeSubmit();
	
//	var flag = false;
//	var shippingId=$("#shippingId").val();
//	if(shippingId==31){//门店自提只能选择普通配送
//		flag=validStoreForm();//表单验证
//	}else{
//		flag=validForm();//表单验证
//	}
	
//	if(flag){//表单验证通过
//		var codFeeRate = $.trim($("#codFeeRate").val());
//		if(!isNotNull(codFeeRate)){
//			$("#codFeeRate").val(0);
//		}
//		if($("#submittype").val()=="add"){
//			var url = ctx + "/administrator/addShippingTemplate.html?t="+new Date().getTime();
//			$.ajax({
//		         type: "POST",
//		         url:url,
//		         data:$('#shippingTempForm').serialize(),// 要提交的表单 
//		         success: function(data) {
//		        	 var json = $.parseJSON(data);
//		        	 if(json.code=="0"){
//		        		 alert("成功添加运费模板!");
//		        		 location.href=ctx + "/administrator/shippingTemplateList.html";
//		        	 }else if(json.code=="10001"){
//		        		 alert("该快递公司已存在模板!");
//		        		 location.href=ctx + "/administrator/shippingTemplateList.html";
//		        	 }
//		         }
//		     });
//		}else{
//			$("#shippingTempForm").submit();
//		}
//	}
//}

//表单验证
function validForm(){
	var templateType = $("input[name=templateType]:checked").val();
	if(!isNotNull($.trim($("#name").val()))){
		alert("请正确填写模板名称!");
		$("#name").focus();
		return false;
	}
	//支持普通配送
	if(templateType==1 || templateType==2){
		var normalSet = $("#normalSettings").val();
		if(!isNotNull(normalSet)){
			alert("请至少填写一条普通配送指定地区运费!");
			return false;
		}
	}
	
	//填写费用不能为0
	var flag = true;
	var flag2 = true;
	
	$.each($("#normalSet tr"),function(idx,ele){
		$.each($(ele).find(".inputT"),function(index,obj){
			if(index==0 || index==2){
				if(parseFloat($.trim($(obj).val()))<=0 && flag){
					flag=false;				
				}
			}
			if(index==1 || index==3){
				if(parseFloat($.trim($(obj).val()))<=0 && flag2){
					flag2=false;
				}
			}
		});
	});
	if(!flag){
		alert("普通配送:所填写的首重、续重都必须大于0!");
		return false;
	}
	if(!flag2){
		if(confirm("普通配送:所填写的首费、续费有为0的项，请确认是否仍然提交")){
			return true;
		}else{
		return false;
		}
	}
	
	
	 flag = true;
	 flag2 = true;
	$.each($("#codSet tr"),function(idx,ele){
		$.each($(ele).find(".inputT"),function(index,obj){
			if(index==0 || index==2){
				if(parseFloat($.trim($(obj).val()))<=0 && flag){
					flag=false;
				}
			}
			if(index==1 || index==3){
				if(parseFloat($.trim($(obj).val()))<=0 && flag2){
					flag2=false;
				}
			}
		});
	});
	if(!flag){
		alert("货到付款: 所填写的首重、续重都必须大于0!");
		return false;
	}
	if(!flag2){
		if(confirm("货到付款:所填写的首费、续费有为0的项，请确认是否仍然提交")){
			return true;
		}else{
		return false;
		}
	}
	
	
	//支持货到付款
	if(templateType==1 || templateType==3){
		var codSet = $("#codSettings").val();
		/*var codFeeRate = $.trim($("#codFeeRate").val());
		if(!isNotNull(codFeeRate)){
			alert("请正确填写货到付款费率!");
			$("#codFeeRate").focus();
			return false;
		}else{
			if(parseFloat(codFeeRate)<=0){
				alert("货到付款费率必须大于0!");
				$("#codFeeRate").focus();
				return false;
			}
		}*/
		if(!isNotNull(codSet)){
			alert("请至少填写一条货到付款指定地区运费!");
			return false;
		}
	}
	
	return true;
}

//门店自提表单验证
function validStoreForm(){
	var templateType = $("input[name=templateType]:checked").val();
	if(!isNotNull($.trim($("#name").val()))){
		alert("请正确填写模板名称!");
		$("#name").focus();
		return false;
	}
	//支持普通配送
	if(templateType==2){
		var normalSet = $("#normalSettings").val();
		if(!isNotNull(normalSet)){
			alert("请至少填写一条普通配送指定地区运费!");
			return false;
		}
	}else{
		alert("门店自提只能选择普通配送");
		return false;
	}
	
	//填写费用不能为0
	var flag = false;
	$.each($("#normalSet tr"),function(idx,ele){
		$.each($(ele).find(".inputT"),function(index,obj){
			if(parseFloat($.trim($(obj).val()))>0){
				flag = true;
				return false;//break each
			}
		});
		if(flag){
			return false;//break each
		}
	});
	if(flag){
		alert("普通配送: 所填写的首重、首费、续重、续费都必须等于0!");
		return false;
	}
	
	return true;
}

function doInit(){
	//选择模板类型
	$("input[name=templateType]").click(function(){
		if($(this).val()==1){//普通和到付
			$("#mbNormal").show();
			$("#mbCod").show();
		}else if($(this).val()==2){//普通
			$("#mbNormal").show();
			$("#mbCod").hide();
		}else if($(this).val()==3){//到付
			$("#mbNormal").hide();
			$("#mbCod").show();
		}
	});
	
	//费用输入框限制只能输入数字和小数点
	inputNumOnly("inputT");
	
	$("#addNormalArea").click(function(){
		var trId = "tr" + new Date().getTime() + "" + getMathRand(6);
		//增加指定地区
		var str = "<tr id=\"" + trId + "\">";
		    //str = str + "<td><asp:TextBox ID=\"field_control_sub_title\" runat=\"server\" CssClass=\"input normal\" datatype=\"*0-255\" sucmsg=\" \" /></td>"
			str = str + "<td><input type=\"text\" name=\"firstWeight\" id=\"firstWeight\" class=\"inputT\"/></td>";
			str = str + "<td><input type=\"text\" name=\"firstFee\" id=\"firstFee\" class=\"inputT\"/></td>";
			str = str + "<td><input type=\"text\" name=\"secondWeight\" id=\"secondWeight\"  class=\"inputT\"/></td>";
			str = str + "<td><input type=\"text\" name=\"secondFee\" id=\"secondFee\" class=\"inputT\"/></td>";
			//str = str + "<td><input type=\"hidden\" name=\"citys\"/><span class=\"selcitys\">未指定地区</span> <span class=\"areaEdit\"><a class=\"areaSelect\" href=\"javascript:void(0);\">编辑</a></span></td>";
			str = str + "<td><input type=\"hidden\" id=\"regionValid\" name=\"regionValid\" value=\"1\"/><input type=\"hidden\" id=\"citys\" name=\"citys\"/><span class=\"selcitys\">未指定地区</span></td>";
			//str = str + "<td>&nbsp;</td>";
			str = str + "<td style=\"text-align:center;\"><a class=\"areaSelect\" id=\"areaSelect\" href=\"javascript:void(0);\">编辑地区</a> <br/> <a class=\"delTr\" href=\"javascript:void(0);\">删除</a></td>";
			str = str + "</tr>";
		 $('#normalSet tr:eq(1)').after(str);
		 $("#"+trId).find("input[name=firstWeight]").focus();
		 $(".inputT").attr("maxlength",8);
	});
	
	$("#addCodArea").click(function(){
		var trId = "tr" + new Date().getTime() + "" + getMathRand(6);
		//增加指定地区
		var str = "<tr id=\""+trId +"\">";
			str = str + "<td><input type=\"text\" name=\"firstWeight\" class=\"inputT\"/></td>";
			str = str + "<td><input type=\"text\" name=\"firstFee\" class=\"inputT\"/></td>";
			str = str + "<td><input type=\"text\" name=\"secondWeight\" class=\"inputT\"/></td>";
			str = str + "<td><input type=\"text\" name=\"secondFee\" class=\"inputT\"/></td>";
			//str = str + "<td><input type=\"hidden\" name=\"citys\"/><span class=\"selcitys\">未指定地区</span><span class=\"areaEdit\"><a class=\"areaSelect\" href=\"javascript:void(0);\">编辑</a></span></td>";
			str = str + "<td><input type=\"hidden\" name=\"regionValid\" value=\"1\"/><input type=\"hidden\" name=\"citys\"/><span class=\"selcitys\">未指定地区</span></td>";
			str = str + "<td>&nbsp;</td>";
			str = str + "<td style=\"text-align:center;\"><a class=\"areaSelect\" href=\"javascript:void(0);\">编辑地区</a> <br/> <a class=\"delTr\" href=\"javascript:void(0);\">删除</a></td>";
			str = str + "</tr>";
		 $("#codSet tr:eq(1)").after(str);
		 $("#"+trId).find("input[name=firstWeight]").focus();
		 $(".inputT").attr("maxlength",8);
	});
	
	//删除指定地区
	$(".delTr").live("click", function() {
		$(this).parent().parent().remove();
	});
	
	//弹出省份/城市选择
	$(".areaSelect").live("click",function(){
	    //debugger;
		 //标识弹出城市选择框对应哪个TR
		 //$("#currentTrId").val($(this).parent().parent().parent().attr("id"));
		$("#currentTrId").val($(this).parent().parent().attr("id"));
		 //已选省份设为disabled
		 doInitAreaSelectDiv();
		 
		 $("#areaSelectDiv").dialog("open");
		 $(".ui-dialog").css({"top":"6%","left":"22%"});
	});
	
	$("#areaSelectDiv").dialog({//编辑地区弹出框
        autoOpen: false,
        resizable: false,
        modal: true,
        width: 760,
        draggable: false,
        position: [100,300],
        buttons: {
            "确定": function () {
                //debugger;
                var citysStr = getSelectedAreaData();
                //debugger;
                if (citysStr != null && citysStr != "") {
                    //debugger;
               		var trId = $("#currentTrId").val();
                	$("#"+trId).find("input[name=citys]").val(citysStr);
                	showCitys(trId,citysStr);
                	
                	//重设未设置运费地区
                	var tblId = $("#"+trId).parent().parent().attr("id");
                	doParseUnSetMailArea(tblId,areaInfo);
                }
                resetAreaSelectDiv();//清空选择
                $(this).dialog("close");
            }, "取消": function() {
            	resetAreaSelectDiv();//清空选择
    			$(this).dialog("close"); 
            } 
        }
	});
	
	//加载地区数据
	loadAreas();
	
}//end doInit



//已选省份设为disabled
//citys: 当前tr中 已经选择的省份和城市集合字符串
function doInitAreaSelectDiv(){
	$("#areaSelectDiv").find("input[type=checkbox]").removeAttr("disabled");
	$("#areaSelectDiv").find("input[type=checkbox]").removeAttr("checked");
	//$("#areaSelectDiv").find("input[name=proId]").removeAttr("disabled");
	$("#areaSelectDiv").find(".selcity").show();
	
	var curTrId = $("#currentTrId").val();
	var tblId = $("#"+curTrId).parent().parent().attr("id");
	var trCount = $("#"+tblId +" tr").length;
	
	var result = new Array();//所有选中的省份
	var resultCity = new Array();//所有选中的城市
	
	var proJoin = "@@";
	var proCityJoin = "$$";
	
	var selectedProIds = new Array();//当前tr选中的省份
	var selectedCitys = new Array();//当前tr选中的城市
	
	if(trCount>3){//超过3行，表示有设置运费区域
		var size = trCount-3;
		var proCount = 0;
		var selProCount = 0;//选中的省份计数器
		for(var i=0;i<size;i++){
			var $tr = $("#"+tblId +" tr:eq("+(i+2)+")");
			var citys = $tr.find("input[name=citys]").val();
			var arr = citys.split(proJoin);
			if($tr.attr("id")!=curTrId){
				for(var j=0;j<arr.length;j++){
					var proIdName = arr[j].split(proCityJoin)[0];//省ID=省名称
					var proId = proIdName.split("=")[0];
					result[proCount] = proId;
					resultCity[proCount] = arr[j].split(proCityJoin)[1];
					proCount ++;
				}
			}else{
				//当前tr,设置已选中省份和城市
				for(var j=0;j<arr.length;j++){
					var proIdName = arr[j].split(proCityJoin)[0];//省ID=省名称
					var proId = proIdName.split("=")[0];
					selectedProIds[selProCount] = proId;
					//城市ID=城市名称,城市ID=城市名称,城市ID=城市名称
					selectedCitys[selProCount] = arr[j].split(proCityJoin)[1];
					selProCount ++;
				}
			}
		}
	}//end trCount>3 if
	
	$.each($("#areaSelectDiv").find("input[name=proId]"),function(idx,ele){
		var proId = $(ele).val();
		
		if(result.contains(proId)){
			for(var i=0;i<result.length;i++){
				//var idx = result.indexOf(proId);
				if(result[i]==proId){
					var idx = i;
					var cityIdNameArr = resultCity[idx].split(",");//城市ID=城市名称
					var cityCount = $("#pro"+proId).find("input[type=checkbox]").size();
					if(cityIdNameArr.length==cityCount){//选中城市数和省份城市数比较
						$(ele).attr("disabled",true);
						$.each($("#areaSelectDiv").find(".selcity"),function(index,obj){
							if(proId==$(obj).attr("rel")){
								$(obj).hide();
								return false;
							}
						});
					}else{
						var tmpIds = new Array();
						for(var h=0;h<cityIdNameArr.length;h++){
							tmpIds[h] = cityIdNameArr[h].split("=")[0];
						}
						$.each($("#pro"+proId).find("input[type=checkbox]"),function(count,mobj){
							if(tmpIds.contains($(this).val()))$(this).attr("disabled",true);
						});
						
						if($("#pro"+proId).find("input[type=checkbox]:disabled").size()==cityCount){
							$(ele).attr("disabled",true);
							$.each($("#areaSelectDiv").find(".selcity"),function(index,obj){
								if(proId==$(obj).attr("rel")){
									$(obj).hide();
									return false;
								}
							});
						}
					}
				}
			}
		}
		
		/*if(result.contains(proId)){
			$(ele).attr("disabled",true);
			
			$.each($("#areaSelectDiv").find(".selcity"),function(index,obj){
				var rel = $(obj).attr("rel");
				if(proId==rel){
					$(obj).hide();
					return false;
				}
			});
		}*/
		
		//勾选已经选中的城市和省份
		if(selectedProIds.contains(proId)){
			$(ele).attr("checked",true);//省份勾选
			
			var idx = selectedProIds.indexOf(proId);
			var cityIdNameArr = selectedCitys[idx].split(",");//城市ID=城市名称
			var cityIds = new Array();
			
			for(var i=0;i<cityIdNameArr.length;i++){
				cityIds[i] = cityIdNameArr[i].split("=")[0];
			}
			$.each($("#pro"+proId).find("input[type=checkbox]"),function(count,mobj){
				if(cityIds.contains($(this).val()) && !$(this).attr("disabled"))$(this).attr("checked",true);
			});
			
			cityIds.length>0?$("#citycount"+proId).text("("+cityIds.length+")"):"";
		}
	});
	
}

//解析城市弹出框返回的字符串，并生成城市名称显示
function showCitys(trId,citysStr){
	var proJoin = "@@";
	var cityJoin = "$$";
	var endJoin = ",";
	var proList = citysStr.split(proJoin);
	var str = "<ul class=\"showcitys\">";
	for(var i=0;i<proList.length;i++){
		var pro = proList[i].split(cityJoin)[0];
		str = str + "<li><span>"+pro.split("=")[1]+"：</span> ";
		var citys = proList[i].split(cityJoin)[1];
		var tmpCity = "";
		if(citys!=null && citys!=""){
			var cityArr = citys.split(endJoin);
			for(var j=0;j<cityArr.length;j++){
				tmpCity = tmpCity + "、" + cityArr[j].split("=")[1];
			}
			if(tmpCity.length>0)tmpCity = tmpCity.substring(1);
		}
		str = str + tmpCity + "</li>"
	}
	str = str + "</ul>";
	$("#"+trId).find("td").find(".selcitys").html(str);
}//end showCitys

//获取选择省份城市信息(返回格式如下所示：)
// proId=proName$$cityId=cityName,cityId=cityName@@proId=proName$$cityId=cityName,cityId=cityName
// 2001=广东省$$3001=广州,3002=梅州@@2002=湖南省$$3003=长沙,3004=株洲
function getSelectedAreaData(){
	var result = "";
	var proJoin = "@@";
	var cityJoin = "$$";
	var endJoin = ",";
	var $proList = $("#areaSelectDiv").find("input[name=proId]:checked");
	$.each($proList,function(idx,ele){
		var proId = $(ele).val();
		var proName = $(ele).attr("relname");
		var $cityList = $("#pro" + proId).find("input[type=checkbox]:checked");
		if($cityList.size()>0){
			result = result + proJoin + (proId+"="+proName) + cityJoin;
			var citystr = "";
			$.each($cityList,function(index,obj){
				citystr = citystr + endJoin + $(obj).val() + "=" + $(obj).attr("relname");
			});
			if(citystr.length>0){
				citystr = citystr.substring(1);
			}
			result = result + citystr;
		}else{
			result = result + proJoin + (proId+"="+proName);
		}
		
	});
	
	return result.length>0?result.substring(2):result;
}//end getSelectedAreaData



//点编辑弹出城市选择框加载省份城市数据
function loadAreas(){
	//华东，华北，华中，华南，东北，西北，西南,港澳台
/*var areaInfo = [
				{
				    "areaId": "1001",
				    "name": "华东",
				    "pros": [{"areaId" : "2001", "name" : "广东","pros" :[{"areaId" : "3001", "name" : "广州"},{"areaId" : "3002", "name" : "梅州"}]},{"areaId" : "2002", "name" : "江苏"},
						    {"areaId" : "2003", "name" : "上海"},{"areaId" : "2004", "name" : "江苏"},
						    {"areaId" : "2005", "name" : "上海"},{"areaId" : "2006", "name" : "苏州","pros" :[{"areaId" : "3001", "name" : "广州"},{"areaId" : "3006", "name" : "梅州"}]}
						    ],
				},
				{
				    "areaId": "1002",
				    "name": "华中",
				    "pros": [{"areaId" : "2005", "name" : "北京"},{"areaId" : "2002", "name" : "天津"}],
				}
				];*/

		var str = "<ul>";
		$.each(areaInfo,function(idx,ele){
			var substr = "";
			$.each(ele.pros,function(index,obj){
				substr = substr + " <label><input type=\"checkbox\" value=\""+obj.areaId+"\" relname=\""+obj.name+"\" name=\"proId\" id=\"proId"+obj.areaId+"\"/>" + obj.name + "</label><span class=\"citycount\" id=\"citycount"+obj.areaId+"\"></span>";
				substr = substr + " <span rel=\""+obj.areaId+"\" class=\"selcity\" style=\"width:12px;height:12px;\">&nbsp;&nbsp;</span>";
				if(obj!=undefined && obj.pros!=undefined && obj.pros.length>0){
					substr = substr + "<div class=\"citylist\" rel=\"0\" id=\"pro"+obj.areaId+"\"><ul>";
					$.each(obj.pros,function(index,city){
						if(obj.name=='四川省' && city.name=='重庆市')return ;//作用同continue;
						substr = substr + "<li>";
						substr = substr + "<input type=\"checkbox\" rel=\""+obj.areaId+"\" relname=\""+city.name+"\" value=\""+city.areaId+"\" name=\"cityId\"/>" + city.name;
						substr = substr + "</li>";
					});
					substr = substr + "</ul></div>";
				}
			});
			str = str + "<li><input type=\"checkbox\" value=\""+ele.areaId+"\" name=\"areaId\"/> <span>"+ele.name+"</span>  " +substr+"</li>";
			str = str + "<li class=\"clear\" style=\"margin:0;\"></li>"
		});
		str = str +  "</ul>"
		$("#areaSelectDiv").html(str);
		
	
	//显示或隐藏下拉的城市数据
	$("#areaSelectDiv").find("span.selcity").live("click",function(){
		var proId = $(this).attr("rel");
		$("#areaSelectDiv").find("label").css("background-color","");
		$.each($(this).parent().find(".citylist"),function(idx,ele){
			if($(ele).attr("id")!="pro"+proId)$(ele).attr("rel",0);
		});
		
		if($("#pro"+proId).attr("rel")==0){
			//收起所有城市div
			$(".citylist").slideUp(0);
			$("#pro"+proId).slideDown(100);
			$("#pro"+proId).attr("rel",1);
			$("#proId"+proId).parent().css("background-color","#FF6600");
		}else{
			$("#pro"+proId).slideUp(100);
			$("#pro"+proId).attr("rel",0);
			$("#proId"+proId).parent().css("background-color","");
		}
	});
	
	//点击选择区域
	$("input[type=checkbox][name=areaId]").live("click",function(){
		var $parent = $(this).parent();
		var $proList = $parent.find("label").find("input[name=proId]");
		if($(this).attr("checked")){
			$.each($proList,function(ii,ee){
				var disabled = $(ee).attr("disabled");
				if(disabled!="disabled" && disabled!=true){
					var proId = $(ee).val();
					$(ee).attr("checked",true);
					var $cityList = $("#pro" + proId).find("input[type=checkbox]");
					var selCount = 0;
					$.each($cityList,function(t,k){
						if(!$(k).attr("disabled")){
							$(k).attr("checked",true);
							selCount++;
						}
					});
					selCount>0?$("#citycount"+proId).text("("+selCount+")"):"";
				}
			});
		}else{
			$.each($proList,function(ii,ee){
				var disabled = $(ee).attr("disabled");
				if(disabled!="disabled" && disabled!=true){
					$(ee).removeAttr("checked");
					var proId = $(ee).val();
					var $cityList = $("#pro" + proId).find("input[type=checkbox]");
					$cityList.removeAttr("checked");
					$("#citycount"+proId).text("");
				}
			});
		}
	});
	
	//点击选择省份
	$("input[type=checkbox][name=proId]").live("click",function(){
		var proId = $(this).val();
		var $cityList = $("#pro" + proId).find("input[type=checkbox]");
		if($(this).attr("checked")){
			var selCount = 0;
			$.each($cityList,function(t,k){
				if(!$(k).attr("disabled")){
					$(k).attr("checked",true);
					selCount++;
				}
			});
			selCount>0?$("#citycount"+proId).text("("+selCount+")"):"";
		}else{
			$(this).parent().parent().find("input[name=areaId]").removeAttr("checked");
			$("#pro" + proId).find("input[type=checkbox]").removeAttr("checked");
			$("#citycount"+proId).text("");
		}
	});
	
	//点击选择城市
	$("input[type=checkbox][name=cityId]").live("click",function(){
		var proId = $(this).attr("rel");
		var $cityListUL = $(this).parent().parent();
		var selcount = $cityListUL.find("input[type=checkbox]:checked").size();
		(selcount>0)?$("#proId"+proId).attr("checked",true):$("#proId"+proId).removeAttr("checked");
		$("#citycount"+proId).text("("+selcount+")");
	});
}//end loadAreas

//订单表单前构造设置运费相关字符串
function doBeforeSubmit(){
	//模板类型：1.普通和到付,2.普通,3.到付
	var templateType = parseInt($("input[name=templateType]:checked").val());
	var result = "";
	
	if(templateType==1 || templateType==2){//普通
		var trCount = $("#normalSet tr").length;
		if(trCount>3){//超过3行，表示有设置运费区域
			result = createSubmitSettings("normalSet");//1.普通配送
			$("#normalSettings").val(result);
		}
	}
	
	if(templateType==1 || templateType==3){//到付
		var trCount = $("#codSet tr").length;
		if(trCount>3){//超过3行，表示有设置运费区域
			result = createSubmitSettings("codSet");//2.货到付款
			$("#codSettings").val(result);
		}
	}
}

//提交表单前构造选择的省份城市字符串
//tblId: normalSet.普通配送,codSet.货到付款
function createSubmitSettings(tblId){
	var feeCityJoin="$=$";//费用与城市分割
	var setJoin = "@=@";//各设置之间的分割
	
	var trCount = $("#"+tblId +" tr").length;
	var result = "";
	if(trCount>3){//超过3行，表示有设置运费区域
		var size = trCount-3;
		for(var i=0;i<size;i++){
			var $tr = $("#"+tblId +" tr:eq("+(i+2)+")");
			var firstWeight = $.trim($tr.find("input[name=firstWeight]").val());
			var firstFee = $.trim($tr.find("input[name=firstFee]").val());
			var secondWeight = $.trim($tr.find("input[name=secondWeight]").val());
			var secondFee = $.trim($tr.find("input[name=secondFee]").val());
			var citys = $tr.find("input[name=citys]").val();
			var regionValid = $tr.find("input[name=regionValid]").val();
			if(isNotNull(firstWeight) && isNotNull(firstFee) 
					&& isNotNull(secondWeight) && isNotNull(secondFee) 
					&& isNotNull(citys)){
				//格式： 1,10,1,10$=$citys
				result = result + setJoin + firstWeight + "," + firstFee + "," + secondWeight + "," + secondFee + "," + regionValid + feeCityJoin + citys;
			}
		}
		if(result.length>0)result = result.substring(3);
	}
	
	return result;
}

//解析未设置邮费地区的显示(新增或修改)
//tblId: normalSet或codSet
//areaInfo: 所有城市的json数据
function doParseUnSetMailArea(tblId,areaInfo){
	
	if($("#"+tblId +" tr").length==3)return;
	
	var $citys = $("#"+tblId +" tr").find("input[name=citys]");
	var proCityJoin = "$$";
	var proJoin = "@@";
	var proArr = new Array();//一维数据:[省ID,省ID,省ID]
	var citysArr = new Array();//二维数组: [市ID,市ID],[市ID,市ID]
	
	var proSelCount = 0;
	$.each($citys,function(idx,ele){
		var proCitys = $(ele).val().split(proJoin);
		for(var k=0;k<proCitys.length;k++){
			var arr = proCitys[k].split(proCityJoin);
			var proId = arr[0].split("=")[0];
			var cityArr = new Array();
			if(isNotNull(arr[1])){
				var tmpArr = arr[1].split(",");
				for(var i=0;i<tmpArr.length;i++){
					cityArr[i] = tmpArr[i].split("=")[0];
				}
			}
			proArr[proSelCount] = proId
			citysArr[proSelCount] = new Array();
			citysArr[proSelCount] = cityArr;
			proSelCount++;
		}
	});
	
	var unSelPro = new Array();//一维数据:[省ID=省名称,省ID=省名称,省ID=省名称]
	var unSelCity = new Array();//二维数组: [市ID=市名称,市ID=市名称],[市ID=市名称,市ID=市名称]
	var proCount = 0;
	$.each(areaInfo,function(idx,ele){
		var pros = ele.pros;//区域中的省份
		$.each(pros,function(index,obj){
			var proId = obj.areaId;
			var proName = obj.name;
			var citys = obj.pros;//省份中的城市
			if(proArr.contains(proId)){//当前proId在选中的省份之中
				var proIdIndex = proArr.indexOf(proId);
				var selCityCount = citysArr[proIdIndex].length;
				if(obj.pros.length!=selCityCount){//只选了部分城市
					unSelPro[proCount] = proId + "=" + proName;
					unSelCity[proCount] = new Array();
					var cityCount = 0;
					$.each(citys,function(curIdx,e){
						if(!citysArr[proIdIndex].contains(e.areaId)){//当前e.areaId不在选中的城市之中
							unSelCity[proCount][cityCount] = e.areaId + "=" + e.name;
							cityCount ++ ;
						}
					});
					proCount ++;
				}
			}else{//当前proId不在选中的省份之中
				unSelPro[proCount] = proId + "=" + proName;
				unSelCity[proCount] = new Array();
				$.each(citys,function(curIdx,e){
					unSelCity[proCount][curIdx] = e.areaId + "=" + e.name;
				});
				proCount ++;
			}
		}); //end pros each
	});//end areaInfo each
	
	//构造显示未选的地区
	showUnSelectCitys(tblId,unSelPro,unSelCity);
}

//解析城市弹出框返回的字符串，并生成城市名称显示
function showUnSelectCitys(tblId,unSelProArr,unSelCityArr){
	var endJoin = "、";
	var str = "<ul class=\"showcitys\">";
	for(var i=0;i<unSelProArr.length;i++){
		var citys = unSelCityArr[i];
		if(citys==0)continue;
		var pro = unSelProArr[i].split("=");
		str = str + "<li><span>"+pro[1]+"：</span> ";
		var tmpCity = "";
		for(var j=0;j<citys.length;j++){
			tmpCity = tmpCity + endJoin + citys[j].split("=")[1];
		}
		if(tmpCity.length>0)tmpCity = tmpCity.substring(1);
		str = str + tmpCity + "</li>"
	}
	str = str + "</ul>";
	$("#"+tblId + "UnSelCitys").html(str);
}//end showCitys

//解析未设置邮费地区的显示(列表页面)
//tblId: normal或cod
//shippingTemplateId: 运费模板主键ID
//areaInfo: 所有城市的json数据
function showUnSetMailAreaForList(tblId,shippingTemplateId,areaInfo){
	
	var $citys = $("#mbDtl_" + shippingTemplateId).find("input[name="+tblId+"HiddenCitys]");
	var proCityJoin = "$$";
	var proJoin = "@@";
	var proArr = new Array();//一维数据:[省ID,省ID,省ID]
	var citysArr = new Array();//二维数组: [市ID,市ID],[市ID,市ID]
	
	var proSelCount = 0;
	$.each($citys,function(idx,ele){
		var proCitys = $(ele).val().split(proJoin);
		for(var k=0;k<proCitys.length;k++){
			var arr = proCitys[k].split(proCityJoin);
			var proId = arr[0].split("=")[0];
			var cityArr = new Array();
			if(isNotNull(arr[1])){
				var tmpArr = arr[1].split(",");
				for(var i=0;i<tmpArr.length;i++){
					cityArr[i] = tmpArr[i].split("=")[0];
				}
			}
			if(!proArr.contains(proId)){
				proArr[proSelCount] = proId
				citysArr[proSelCount] = new Array();
				citysArr[proSelCount] = cityArr;
				proSelCount++;
			}else{
				var idx = proArr.indexOf(proId);
				citysArr[idx].push(cityArr);
			}
		}
	});
	
	var unSelPro = new Array();//一维数据:[省ID=省名称,省ID=省名称,省ID=省名称]
	var unSelCity = new Array();//二维数组: [市ID=市名称,市ID=市名称],[市ID=市名称,市ID=市名称]
	var proCount = 0;
	$.each(areaInfo,function(idx,ele){
		var pros = ele.pros;//区域中的省份
		$.each(pros,function(index,obj){
			var proId = obj.areaId;
			var proName = obj.name;
			var citys = obj.pros;//省份中的城市
			if(proArr.contains(proId)){//当前proId在选中的省份之中
				
				for(var i=0;i<proArr.length;i++){
					if(proArr[i]==proId){
						var proIdIndex = i;
						var selCityCount = citysArr[proIdIndex].length;
						if(obj.pros.length!=selCityCount){//只选了部分城市
							unSelPro[proCount] = proId + "=" + proName;
							unSelCity[proCount] = new Array();
							var cityCount = 0;
							$.each(citys,function(curIdx,e){
								if(!citysArr[proIdIndex].contains(e.areaId)){//当前e.areaId不在选中的城市之中
									unSelCity[proCount][cityCount] = e.areaId + "=" + e.name;
									cityCount ++ ;
								}
							});
							proCount ++;
						}
					}
				}
				
				
				
				/*var proIdIndex = proArr.indexOf(proId);
				var selCityCount = citysArr[proIdIndex].length;
				if(obj.pros.length!=selCityCount){//只选了部分城市
					unSelPro[proCount] = proId + "=" + proName;
					unSelCity[proCount] = new Array();
					var cityCount = 0;
					$.each(citys,function(curIdx,e){
						if(!citysArr[proIdIndex].contains(e.areaId)){//当前e.areaId不在选中的城市之中
							unSelCity[proCount][cityCount] = e.areaId + "=" + e.name;
							cityCount ++ ;
						}
					});
					proCount ++;
				}*/
			}else{//当前proId不在选中的省份之中
				unSelPro[proCount] = proId + "=" + proName;
				unSelCity[proCount] = new Array();
				$.each(citys,function(curIdx,e){
					unSelCity[proCount][curIdx] = e.areaId + "=" + e.name;
				});
				proCount ++;
			}
		}); //end pros each
	});//end areaInfo each
	
	//构造显示未选的地区
	showUnSelectCitys(tblId,unSelPro,unSelCity);
}

//编辑初始化
function initEditShipping(){
	//启用运费地区配送
	$(".openArea").live("click",function(idx,ele){
		if(confirm("请确定是否启用该地区配送？")){
			 var id = $(this).attr("rel");
			 $.get(ctx + "/administrator/openArea.html?t="+new Date().getTime(), { id: id},
			  function(data){
			    	var json = $.parseJSON(data);
			    	if(json.success=="0"){
			    		location.href = location.href;
			    	}else{
			    		alert("启用地区配送失败!");
			    	}
			  });
		}
	});
	
	//停用运费地区配送
	$(".closeArea").live("click",function(idx,ele){
		if(confirm("请确定是否停止该地区配送？")){
			 var id = $(this).attr("rel");
			 $.get(ctx + "/administrator/closeArea.html?t="+new Date().getTime(), { id: id},
			  function(data){
			    	var json = $.parseJSON(data);
			    	if(json.success=="0"){
			    		location.href = location.href;
			    	}else{
			    		alert("停止地区配送失败!");
			    	}
			  });
		}
	});
}

//运费模块列表：初始化
function initShippingList(){
	
	//设置满包邮输入框只能输入数字
	inputNumOnly("inputT");
	
	//设置已定制快递公司为　disabled
	var idsArr = shippingIds.split(",");
	$.each($("#setShippingDiv").find("input[name=shipId]"),function(idx,ele){
		if(idsArr.contains($(ele).val()))$(ele).attr("disabled",true);
	});
	
	//快递公司列表：全选/全不选
	$("#selall").click(function(){
		var checked = $(this).attr("checked");
		$.each($("#setShippingDiv").find("input[name=shipId]"),function(idx,ele){
			var disabled = $(ele).attr("disabled");
			if(checked){
				if(disabled!='disabled' && disabled != true ){
					$(ele).attr("checked",checked);
				}
			}else{
				$(ele).removeAttr("checked");
			}
		});
	});
	
	//删除定制
	$(".delshipping").live("click",function(idx,ele){
		if(confirm("请您确定是否不再使用该快递公司，取消后该快递公司的运费模板将删除！")){
			 var id = $(this).attr("rel");
			 $.get(ctx + "/administrator/deleteShipping.html?t="+new Date().getTime(), { id: id},
			  function(data){
			    	var json = $.parseJSON(data);
			    	if(json.success=="0"){
			    		alert("您已成功删除所选的快递公司定制!");
			    		location.href = location.href;
			    	}else{
			    		alert("删除快递公司定制失败!");
			    	}
			  });
		}
	});
	
	//启用运费模板
	$(".openshipping").live("click",function(idx,ele){
		if(confirm("请确定是否启用该快递公司？")){
			 var id = $(this).attr("rel");
			 $.get(ctx + "/administrator/openShipping.html?t="+new Date().getTime(), { id: id},
			  function(data){
			    	var json = $.parseJSON(data);
			    	if(json.success=="0"){
			    		location.href = location.href;
			    	}else{
			    		alert("启用运费模板失败!");
			    	}
			  });
		}
	});
	
	//停用运费模板
	$(".closeshipping").live("click",function(idx,ele){
		if(confirm("请确定是否停用该快递公司？停用后，提交订单时用户无法选择该快递！")){
			 var id = $(this).attr("rel");
			 $.get(ctx + "/administrator/closeShipping.html?t="+new Date().getTime(), { id: id},
			  function(data){
			    	var json = $.parseJSON(data);
			    	if(json.success=="0"){
			    		location.href = location.href;
			    	}else{
			    		alert("停用运费模板失败!");
			    	}
			  });
		}
	});
	
	//查看未设置邮费地区
	$(".showUnSelect").live("click",function(){
		 $("#curShipName").text($(this).attr("shipName"));
		 var shippingTemplateId = $(this).attr("rel");
		 var templateType = $.trim($(this).attr("templateType"));
		 if(templateType=="1" || templateType=="2"){
			 showUnSetMailAreaForList("normal",shippingTemplateId,areaInfo);//普通配送
			 $(".normalTr").show();
		 }else{
			 $(".normalTr").hide();
		 }
		 if(templateType=="1" || templateType=="3"){
			 showUnSetMailAreaForList("cod",shippingTemplateId,areaInfo);//货到付款
			 $(".codTr").show();
		 }else{
			 $(".codTr").hide();
		 }
		 
		 $("#unSelectCitysDiv").dialog("open");
		 $(".ui-dialog").css({"top":"6%","left":"22%"});
	});
	//弹出定制快递框
	$(".shippingBtn").live("click",function(){
		 $("#setShippingDiv").dialog("open");
		 $(".ui-dialog").css({"top":"6%","left":"22%"});
	});
	//弹出满包邮框
	$(".feeMailBtn").live("click",function(){
		 $("#showShipName").text($(this).attr("shipName"));
		 $("#currentId").val($(this).attr("rel"));
		 $("#freeMailAmount").val($(this).parent().prev().text());
		 $("#freeMailDiv").dialog("open");
		 $(".ui-dialog").css({"top":"6%","left":"22%"});
		 
		 //未设置满包邮,不显示取消按钮
		 var freeMailAmount = $("#freeMailAmount").val();
		 if(freeMailAmount==""){
			 $("#cancelFreeMail").hide();
		 }else{
			 $("#cancelFreeMail").show();
		 }
	});
	
	$("#unSelectCitysDiv").dialog({//查看未设置邮费的地区
        autoOpen: false,
        resizable: false,
        modal: true,
        width: 680,
        draggable: false,
        position: [100,300],
        buttons: {
            "知道了": function() { 
               
                $(this).dialog("close");
            }
        }
	});
	
	$("#freeMailDiv").dialog({//设置满包邮
        autoOpen: false,
        resizable: false,
        modal: true,
        width: 500,
        draggable: false,
        position: [100,300],
        buttons: {
            "确定": function() {
            	 var amount = $.trim($("#freeMailAmount").val());
            	 var id = $.trim($("#currentId").val());
            	 if(amount=="" || parseFloat(amount)<=0){
            		 alert("请正确填写买满金额！");
            		 $("#freeMailAmount").focus();
            		 return false;
            	 }else{
	            	 $.get(ctx + "/administrator/freeMailSet.html?t="+new Date().getTime(), {id: id,amount:amount},
	   					  function(data){
	   					    	var json = $.parseJSON(data);
	   					    	if(json.success=="0"){
	   					    		alert("设置成功！");
	   					    		location.href = location.href;
	   					    	}else{
	   					    		alert("设置满包邮失败!");
	   					    	}
	   					  });
            	 }
            },
            "取消": function() { 
                $(this).dialog("close");
            }
        }
	});
	
	$("#setShippingDiv").dialog({//新增物流定制
        autoOpen: false,
        resizable: false,
        modal: true,
        width: 680,
        draggable: false,
        position: [100,300],
        buttons: {
            "提交定制": function() {
            	var ids = "";
            	$.each($("#setShippingDiv").find("input[name=shipId]"),function(idx,ele){
					if($(ele).attr("checked"))ids = ids + "," + $(ele).val();
				});
            	ids = ids.substring(1)||"";
            	if(ids==""){
            		alert("请至少选择一个快递公司!");
					return false;
            	}else if(ids.split(",").length>1){
					alert("只能选择一个快递公司!");
					return false;
				}else{
					//38如果是顺丰安心递则需要单独处理，先判断商家填写内容是否完整，然后自动生成普通快递和货到付款运费模板
					//39如果是京东快递则需要单独处理，先判断商家填写内容是否完整，然后自动生成普通快递和货到付款运费模板
					if(ids==38 || ids==50){
						var dataIntegrity=$("#dataIntegrity").val();
						if(dataIntegrity=="yes"){//填写完整直接自动生成
							var cityid=$("#cityid").val();
            				var merchantid=$("#merchantid").val();
							$.getJSON(ctx + "/administrator/createTemplate.html?t="+new Date().getTime(), 
							{
								cityid:cityid,merchantid:merchantid,shippingid:ids
							},
							function(data){
								var msg=data.msg;
						    	if(msg=="success"){
						    		location.href = location.href;
						    	}else{
						    		alert(msg);
						  		}
							});
						}else{//填写不完整需先完善商家信息
							$("#merchantInfoDiv").dialog("open");
							$("#shippingid").val(ids);
		 					$("#merchantInfoDiv .ui-dialog").css({"top":"6%","left":"22%"});
						}
					}else{
						$.get(ctx + "/administrator/setShipping.html?t="+new Date().getTime(), {ids: ids},
						  function(data){
						    	var json = $.parseJSON(data);
						    	if(json.success=="0"){
						    		location.href = location.href;
						    	}else{
						    		alert("新增快递公司定制失败!");
						    	}
						  });
					}
	                $(this).dialog("close");
                }
            },
            "取消": function() { 
               
                $(this).dialog("close");
            }
        }
	});
	
	$("#merchantInfoDiv").dialog({//商家发货信息
        autoOpen: false,
        resizable: false,
        modal: true,
        width: 650,
        draggable: false,
        position: [100,300],
        buttons: {
            "提交": function() {
            	var province=$("#province").val()||"";
            	var city=$("#city").val()||"";
            	var sender=$("#sender").val()||"";
            	var sendAddress=$("#sendAddress").val()||"";
            	var sendEmail=$("#sendEmail").val()||"";
            	var sendTelephone=$("#sendTelephone").val()||"";
            	var shippingid=$("#shippingid").val()||"";
            	if(province==""||city==""){
            		alert("所在地区不能为空");
            		return false;
            	}
            	if(sender==""){
            		alert("发件人姓名不能为空");
            		return false;
            	}
            	if(sendAddress==""){
            		alert("发件地址不能为空");
            		return false;
            	}
            	if(sendEmail==""){
            		alert("发件邮编不能为空");
            		return false;
            	}
            	if(sendTelephone==""){
            		alert("发件人联系电话不能为空");
            		return false;
            	}
            	if(shippingid==""){
            		alert("快递信息有误，请刷新页面后提交");
            		return false;
            	}
            	var provinceid=$("#provinceid").val();
            	var provincename=$("#provincename").val();
            	var cityid=$("#cityid").val();
            	var cityname=$("#cityname").val();
            	var merchantid=$("#merchantid").val();
            	$.getJSON(ctx + "/administrator/updateMerchantCreateTemplate.html?t="+new Date().getTime(), 
				{
					provinceid:provinceid,provincename:provincename,
					cityid:cityid,cityname:cityname,
					sender:sender,sendAddress:sendAddress,
					sendEmail:sendEmail,sendTelephone:sendTelephone,
					merchantid:merchantid,shippingid:shippingid
				},
				function(data){
					var msg=data.msg;
			    	if(msg=="success"){
			    		location.href = location.href;
			    	}else{
			    		alert(msg);
			  		}
				});
            },
            "取消": function() { 
            	$(this).dialog("close");
            }
        }
	});
}

//取消满包邮
function cancelFreeMail(){
	$("#cancelFreeMail").click(function(){
		if(confirm("您确定取消该快递的满额包邮吗？")){
			 var id = $.trim($("#currentId").val());
			 $.get(ctx + "/administrator/cancelFreeMail.html?t="+new Date().getTime(), {id: id},
			  function(data){
			    	var json = $.parseJSON(data);
			    	if(json.success=="0"){
			    		location.href = location.href;
			    	}else{
			    		alert("取消满额包邮失败!");
			    	}
			  });
		}
	});
}

//重置城市选择弹出层
function resetAreaSelectDiv(){
	$("#areaSelectDiv").find("input[type=checkbox]").removeAttr("checked");
	$("#areaSelectDiv").find(".citycount").text("");
	$(".citylist").slideUp(0);
	//$("#pro"+proId).slideDown(100);
	//$("#pro"+proId).attr("rel",1);
	$("#areaSelectDiv").find("input[name=proId]").parent().css("background-color","");
	
}

//运费模板列表：展开/隐藏详细
function openPanel(id){
    if($('#mbDtl_'+id).css('display')=='none'){
       $('#mbDtl_'+id).css('display','');
       //minus_icon,plus_icon
      // $('#icon_open_'+id).attr('class', 'minus_icon'); 
       $('#icon_open_'+id).html("收起详细");
       //$('#span_open_'+oderno).html("关闭面板"); 
    }else{
       $('#mbDtl_'+id).css('display','none');
       //minus_icon,plus_icon
       $('#icon_open_'+id).html("展开详细");
       //$('#icon_open_'+id).attr('class', 'plus_icon'); 
       //$('#span_open_'+oderno).html("展开面板"); 
    }
}

function isNotNull(str){
	if(str==undefined || str==null || str==""){
		return false
	}else{
		return true;
	}
}

function templateTypeSelect(templateType,shippingId){
    $.get(ctx + "/administrator/cancelFreeMail.html?t="+new Date().getTime(), {shippingId: shippingId},
			  function(data){
			    	var json = $.parseJSON(data);
			    	if(json.success=="0"){
			    		location.href = location.href;
			    	}else{
			    		alert("取消满额包邮失败!");
			    	}
			  });
    var templateTypes=$('input:radio[name="templateType"]:checked').val();
    if(templateTypes == "3" && templateType != "3"){
      alert("该快递设置了单品免邮，更换类型后，设置的单品免邮将失效，请确定是否更换类型！（单品免邮不适用于货到付款）");
    }
}

/**
 * 只能输入数字
 * cssClass: css样式的class名称, 如：class='inputT'，则cssClass为 inputT
 */
function inputNumOnly(cssClass){
	//费用输入框限制只能输入数字和小数点
	$("." + cssClass).live("keyup",function(){     
        var tmptxt=$(this).val();
        $(this).val(tmptxt.replace(/[^\d.]/g,''));
        //$(this).val(tmptxt.replace(/\D|^0/g,''));     
    }).bind("paste",function(){
        var tmptxt=$(this).val();
        $(this).val(tmptxt.replace(/[^\d.]/g,'')); 
        //$(this).val(tmptxt.replace(/\D|^0/g,''));     
    }).css("ime-mode", "disabled"); 
}

//获取随机数
function getMathRand(count) 
{ 
	var num=""; 
	for(var i=0;i<count;i++) 
	{ 
		num+=Math.floor(Math.random()*10); 
	} 
	return num;
}

Array.prototype.contains = function(item){
  return RegExp("\\b"+item+"\\b").test(this);
};
	
Array.prototype.indexOf = function(value) {
    var a = this;//为了增加方法扩展适应性
    for (var i = 0; i < a.length; i++) {
        if (a[i] == value)return i;
    }
}