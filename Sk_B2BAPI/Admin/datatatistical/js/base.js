$(function () {
    echart_6();
    //湖南省高铁
    function echart_6() {
           // 基于准备好的dom，初始化echarts实例
            var myChart = echarts.init(document.getElementById('chart_6'));
            //加载地图
                myChart.setOption({
                    series: [{
                        type: 'map',
                        mapType: 'hunan'
                    }]
                });

                var geoCoordMap = {
                    '怀化站': [109.999867,27.518949],
                    '吉首站': [109.741528,28.332629],
                    '张家界站': [110.491722,29.112001],
                    '常德站': [111.701486,29.076683],
                    '益阳站': [112.348741,28.544124],
                    '岳阳站': [113.126486,29.382401],
                    '长沙站': [113.019455,28.200103],
                    '株洲站': [113.163141,27.8418],
                    '湘潭站': [112.91977,27.882141],
                    '邵阳站': [111.467859,27.21915],
                    '娄底站': [112.012438,27.745506],
                    '衡阳站': [112.63809,26.895225],
                    '永州站': [111.577632,26.460144],
                    '郴州站': [113.039396,25.81497]
                };

                var goData = [
                    [{
                        name: '怀化站'

                    }, {
                        id: 1,
                        name: '吉首站',
                        value: 60
                    }],
                    [{
                        name: '吉首站'

                    }, {
                        id: 1,
                        name: '张家界站',
                        value: 70
                    }],
                    [{
                        name: '张家界站'

                    }, {
                        id: 1,
                        name: '常德站',
                        value: 77
                    }],
                    [{
                        name: '常德站'

                    }, {
                        id: 1,
                        name: '岳阳站',
                        value: 70
                    }],
                    [{
                        name: '常德站'

                    }, {
                        id: 1,
                        name: '益阳站',
                        value: 65
                    }],
                    [{
                        name: '常德站'

                    }, {
                        id: 1,
                        name: '邵阳站',
                        value: 80
                    }],
                    [{
                        name: '益阳站'

                    }, {
                        id: 1,
                        name: '长沙站',
                        value: 95
                    }],
                    [{
                        name: '益阳站'

                    }, {
                        id: 1,
                        name: '娄底站',
                        value: 72
                    }],
                    [{
                        name: '长沙站'

                    }, {
                        id: 1,
                        name: '株洲站',
                        value: 80
                    }],
                    [{
                        name: '长沙站'

                    }, {
                        id: 1,
                        name: '湘潭站',
                        value: 90
                    }],
                    [{
                        name: '长沙站'

                    }, {
                        id: 1,
                        name: '衡阳站',
                        value: 88
                    }],
                    [{
                        name: '湘潭站'

                    }, {
                        id: 1,
                        name: '娄底站',
                        value: 72
                    }],
                    [{
                        name: '娄底站'

                    }, {
                        id: 1,
                        name: '怀化站',
                        value: 80
                    }],
                    [{
                        name: '邵阳站'

                    }, {
                        id: 1,
                        name: '永州站',
                        value: 74
                    }],
                    [{
                        name: '衡阳站'

                    }, {
                        id: 1,
                        name: '邵阳站',
                        value: 80
                    }],
                    [{
                        name: '衡阳站'

                    }, {
                        id: 1,
                        name: '永州站',
                        value: 74
                    }],
                    [{
                        name: '衡阳站'

                    }, {
                        id: 1,
                        name: '郴州站',
                        value: 70
                    }],
                ];
                //值控制圆点大小
                var backData = [
                    [{
                        name: '吉首站'
                    }, {
                        id: 2,
                        name: '怀化站',
                        value: 80
                    }],
                    [{
                        name: '常德站'

                    }, {
                        id: 1,
                        name: '张家界站',
                        value: 70
                    }],
                    [{
                        name: '岳阳站'

                    }, {
                        id: 1,
                        name: '常德站',
                        value: 77
                    }],
                    [{
                        name: '益阳站'

                    }, {
                        id: 1,
                        name: '常德站',
                        value: 77
                    }],
                    [{
                        name: '邵阳站'

                    }, {
                        id: 1,
                        name: '常德站',
                        value: 77
                    }],
                    [{
                        name: '长沙站'

                    }, {
                        id: 1,
                        name: '益阳站',
                        value: 65
                    }],
                    [{
                        name: '娄底站'

                    }, {
                        id: 1,
                        name: '益阳站',
                        value: 65
                    }],
                    [{
                        name: '株洲站'

                    }, {
                        id: 1,
                        name: '长沙站',
                        value: 95
                    }],
                    [{
                        name: '湘潭站'

                    }, {
                        id: 1,
                        name: '长沙站',
                        value: 95
                    }],
                    [{
                        name: '衡阳站'

                    }, {
                        id: 1,
                        name: '长沙站',
                        value: 95
                    }],
                    [{
                        name: '娄底站'

                    }, {
                        id: 1,
                        name: '湘潭站',
                        value: 90
                    }],
                    [{
                        name: '怀化站'

                    }, {
                        id: 1,
                        name: '娄底站',
                        value: 72
                    }],
                    [{
                        name: '永州站'

                    }, {
                        id: 1,
                        name: '邵阳站',
                        value: 80
                    }],
                    [{
                        name: '邵阳站'

                    }, {
                        id: 1,
                        name: '衡阳站',
                        value: 88
                    }],
                    [{
                        name: '永州站'

                    }, {
                        id: 1,
                        name: '衡阳站',
                        value: 88
                    }],
                    [{
                        name: '郴州站'

                    }, {
                        id: 1,
                        name: '衡阳站',
                        value: 88
                    }],
                ];

                var arcAngle = function(data) {
                    var j, k;
                    for (var i = 0; i < data.length; i++) {
                        var dataItem = data[i];
                        if (dataItem[1].id == 1) {
                            j = 0.2;
                            return j;
                        } else if (dataItem[1].id == 2) {
                            k = -0.2;
                            return k;
                        }
                    }
                }

                var convertData = function(data) {
                    var res = [];
                    for (var i = 0; i < data.length; i++) {
                        var dataItem = data[i];
                        var fromCoord = geoCoordMap[dataItem[0].name];
                        var toCoord = geoCoordMap[dataItem[1].name];
                        if (dataItem[1].id == 1) {
                            if (fromCoord && toCoord) {
                                res.push([{
                                    coord: fromCoord,
                                }, {
                                    coord: toCoord,
                                    value: dataItem[1].value //线条颜色
                                }]);
                            }
                        } else if (dataItem[1].id == 2) {
                            if (fromCoord && toCoord) {
                                res.push([{
                                    coord: fromCoord,
                                }, {
                                    coord: toCoord
                                }]);
                            }
                        }
                    }
                    return res;
                };

                var color = ['#fff', '#FF1493', '#00FF00'];
                var series = [];
                [
                    ['1', goData],
                    ['2', backData]
                ].forEach(function(item, i) {
                    series.push({
                        name: item[0],
                        type: 'lines',
                        zlevel: 2,
                        symbol: ['arrow', 'arrow'],
                        //线特效配置
                        effect: {
                            show: true,
                            period: 6,
                            trailLength: 0.1,
                            symbol: 'arrow', //标记类型
                            symbolSize: 5
                        },
                        lineStyle: {
                            normal: {
                                width: 1,
                                opacity: 0.4,
                                curveness: arcAngle(item[1]), //弧线角度
                                color: '#fff'
                            }
                        },
                        data: convertData(item[1])
                    }, {
                        type: 'effectScatter',
                        coordinateSystem: 'geo',
                        zlevel: 2,
                        //波纹效果
                        rippleEffect: {
                            period: 2,
                            brushType: 'stroke',
                            scale: 3
                        },
                        label: {
                            normal: {
                                show: true,
                                color: '#fff',
                                position: 'right',
                                formatter: '{b}'
                            }
                        },
                        //终点形象
                        symbol: 'circle',
                        //圆点大小
                        symbolSize: function(val) {
                            return val[2] / 8;
                        },
                        itemStyle: {
                            normal: {
                                show: true
                            }
                        },
                        data: item[1].map(function(dataItem) {
                            return {
                                name: dataItem[1].name,
                                value: geoCoordMap[dataItem[1].name].concat([dataItem[1].value])
                            };
                        })

                    });

                });

                option = {
                    title: {
                        text: '',
                        subtext: '',
                        left: 'center',
                        textStyle: {
                            color: '#fff'
                        }
                    },
                    tooltip: {
                        trigger: 'item',
                        formatter: "{b}"
                    },
                    //线颜色及飞行轨道颜色
                    visualMap: {
                        show: false,
                        min: 0,
                        max: 100,
                        color: ['#fff']
                    },
                    //地图相关设置
                    geo: {
                        map: 'hunan',
                        //视角缩放比例
                        zoom: 1,
                        //显示文本样式
                        label: {
                            normal: {
                                show: false,
                                textStyle: {
                                    color: '#fff'
                                }
                            },
                            emphasis: {
                                textStyle: {
                                    color: '#fff'
                                }
                            }
                        },
                        //鼠标缩放和平移
                        roam: true,
                        itemStyle: {
                            normal: {
                                //          	color: '#ddd',
                                borderColor: 'rgba(147, 235, 248, 1)',
                                borderWidth: 1,
                                areaColor: {
                                    type: 'radial',
                                    x: 0.5,
                                    y: 0.5,
                                    r: 0.8,
                                    colorStops: [{
                                        offset: 0,
                                        color: 'rgba(175,238,238, 0)' // 0% 处的颜色
                                    }, {
                                        offset: 1,
                                        color: 'rgba(	47,79,79, .2)' // 100% 处的颜色
                                    }],
                                    globalCoord: false // 缺省为 false
                                },
                                shadowColor: 'rgba(128, 217, 248, 1)',
                                shadowOffsetX: -2,
                                shadowOffsetY: 2,
                                shadowBlur: 10
                            },
                            emphasis: {
                                areaColor: '#389BB7',
                                borderWidth: 0
                            }
                        }
                    },
                    series: series
                };
                myChart.setOption(option);
    }
    //湖南总货运量
    function echart_7() {
        var myChart = echarts.init(document.getElementById('chart_7'));
        myChart.clear();
        option = {
            title: {
                text: ''
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data:['货运量','铁路货运量','国家铁路货运量','地方铁路货运量','合资铁路货运量','公路货运量','水运货运量'],
                textStyle:{
                    color: '#fff'
                },
                top: '4%'
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            toolbox: {
                orient: 'vertical',
                right: '1%',
                top: '2%',
                iconStyle: {
                    color: '#FFEA51',
                    borderColor: '#FFA74D',
                    borderWidth: 1,
                },
                feature: {
                    saveAsImage: {},
                    magicType: {
                        show: true,
                        type: ['line','bar','stack','tiled']
                    }
                }
            },
            xAxis: {
                type: 'category',
                boundaryGap: false,
                data: ['2012年','2013年','2014年','2015年','2016年'],
                splitLine: {
                    show: false
                },
                axisLine: {
                    lineStyle: {
                        color: '#fff'
                    }
                }
            },
            yAxis: {
                name: '单位(万吨)',
                type: 'value',
                splitLine: {
                    show: false
                },
                axisLine: {
                    lineStyle: {
                        color: '#fff'
                    }
                }
            },
            color: ['#FF4949','#FFA74D','#FFEA51','#4BF0FF','#44AFF0','#4E82FF','#584BFF','#BE4DFF','#F845F1'],
            series: [
                {
                    name:'货运量',
                    type:'line',
                    data:[219130, 198009, 209946, 198024, 210586]
                },
                {
                    name:'铁路货运量',
                    type:'line',
                    data:[21010, 22469, 20619, 17843, 16313]
                },
                {
                    name:'国家铁路货运量',
                    type:'line',
                    data:[17866, 19354, 17589, 17709, 18589]
                },
                {
                    name:'地方铁路货运量',
                    type:'line',
                    data:[3034, 2845, 2712, 2790, 2812]
                },
                {
                    name:'合资铁路货运量',
                    type:'line',
                    data:[111, 271, 318, 327, 349]
                },
                {
                    name:'公路货运量',
                    type:'line',
                    data:[195530, 172492, 185286,175637,189822]
                },
                {
                    name:'水运货运量',
                    type:'line',
                    data:[2590, 3048, 4041,4544,4451]
                }
            ]
        };
        myChart.setOption(option);
    }
    //湖南货物周转量
    function echart_8() {
        var myChart = echarts.init(document.getElementById('chart_8'));
        myChart.clear();
        option = {
            title: {
                text: ''
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data:['铁路货物周转量','国家铁路货物周转量','地方铁路货物周转量','合资铁路货物周转量','公路货物周转量','水运货物周转量'],
                textStyle:{
                    color: '#fff'
                },
                top: '4%'
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            toolbox: {
                orient: 'vertical',
                right: '1%',
                top: '2%',
                iconStyle: {
                    color: '#FFEA51',
                    borderColor: '#FFA74D',
                    borderWidth: 1,
                },
                feature: {
                    saveAsImage: {},
                    magicType: {
                        show: true,
                        type: ['line','bar','stack','tiled']
                    }
                }
            },
            color: ['#FF4949','#FFA74D','#FFEA51','#4BF0FF','#44AFF0','#4E82FF','#584BFF','#BE4DFF','#F845F1'],
            xAxis: {
                type: 'category',
                boundaryGap: false,
                data: ['2014年','2015年','2016年','2017年','2018年'],
                splitLine: {
                    show: false
                },
                axisLine: {
                    lineStyle: {
                        color: '#fff'
                    }
                }
            },
            yAxis: {
                name: '亿吨公里',
                type: 'value',
                splitLine: {
                    show: false
                },
                axisLine: {
                    lineStyle: {
                        color: '#fff'
                    }
                }
            },
            series: [
                {
                    name:'铁路货物周转量',
                    type:'line',
                    data:[3961.88, 4233.63, 4183.14, 3633.01, 3704.47]
                },
                {
                    name:'国家铁路货物周转量',
                    type:'line',
                    data:[3374.76, 3364.76, 3274.76, 3371.82, 3259.87]
                },
                {
                    name:'地方铁路货物周转量',
                    type:'line',
                    data:[14.77, 15.17, 13.17, 14.56, 15.84]
                },
                {
                    name:'合资铁路货物周转量',
                    type:'line',
                    data:[686.17,847.26,895.22,865.28,886.72]
                },
                {
                    name:'公路货物周转量',
                    type:'line',
                    data:[6133.47, 6577.89, 7019.56,6821.48,7294.59]
                },
                {
                    name:'水运货物周转量',
                    type:'line',
                    data:[509.60, 862.54, 1481.77,1552.79,1333.62]
                }
            ]
        };
        myChart.setOption(option);
    }
    //湖南运输线长度
    function echart_9() {
        var myChart = echarts.init(document.getElementById('chart_9'));
        myChart.clear();
        option = {
            title: {
                text: ''
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data:['铁路营业里程','公路里程','等级公路里程','高速等级公路里程','一级等级公路里程','二级等级公路里程','等外公路公路里程'],
                textStyle:{
                    color: '#fff'
                },
                top: '4%'
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            toolbox: {
                orient: 'vertical',
                right: '1%',
                top: '2%',
                iconStyle: {
                    color: '#FFEA51',
                    borderColor: '#FFA74D',
                    borderWidth: 1,
                },
                feature: {
                    saveAsImage: {},
                    magicType: {
                        show: true,
                        type: ['line','bar','stack','tiled']
                    }
                }
            },
            xAxis: {
                type: 'category',
                boundaryGap: false,
                data: ['2014年','2015年','2016年','2017年','2018年'],
                splitLine: {
                    show: false
                },
                axisLine: {
                    lineStyle: {
                        color: '#fff'
                    }
                }
            },
            yAxis: {
                name: '万公里',
                type: 'value',
                splitLine: {
                    show: false
                },
                axisLine: {
                    lineStyle: {
                        color: '#fff'
                    }
                }
            },
            color: ['#FF4949','#FFA74D','#FFEA51','#4BF0FF','#44AFF0','#4E82FF','#584BFF','#BE4DFF','#F845F1'],
            series: [
                {
                    name:'铁路营业里程',
                    type:'line',
                    data:[0.56, 0.63, 0.63, 0.70, 0.70]
                },
                {
                    name:'公路里程',
                    type:'line',
                    data:[16.30, 17.45, 17.92, 18.46, 18.84]
                },
                {
                    name:'等级公路里程',
                    type:'line',
                    data:[15.54, 16.77, 17.29, 17.86, 18.26]
                },
                {
                    name:'高速等级公路里程',
                    type:'line',
                    data:[0.51, 0.56, 0.59, 0.63, 0.65]
                },
                {
                    name:'一级等级公路里程',
                    type:'line',
                    data:[0.47,0.48,0.51,0.54,0.56]
                },
                {
                    name:'二级等级公路里程',
                    type:'line',
                    data:[1.76, 1.85, 1.93, 1.97, 1.99]
                },
                {
                    name:'等外公路公路里程',
                    type:'line',
                    data:[0.76, 0.68, 0.63, 0.60, 0.58]
                }
            ]
        };
        myChart.setOption(option);
    }
    //湖南省快递业务量
    function echart_10(){
        var myChart = echarts.init(document.getElementById('chart_10'));
        myChart.clear();

        option = {
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b}: {c} ({d}%)"
            },
            legend: {
                orient: 'vertical',
                x: 'left',
                top: '2%',
                left: '1%',
                textStyle: {
                    color: '#fff'
                },
                data:[
                      '国际','省外','省内',
                     ]
            },
            color: ['#FF4949','#FFA74D','#4BF0FF','#44AFF0','#4E82FF','#584BFF','#BE4DFF','#F845F1','#4BF0FF','#44AFF0'],
            series: [
                {
                    name:'业务量(万件)',
                    type:'pie',
                    selectedMode: 'single',
                    radius: [0, '15%'],
                    center: ['28%','28%'],
                    label: {
                        normal: {
                            position: 'inner'
                        }
                    },
                    labelLine: {
                        normal: {
                            show: false
                        }
                    },
                    data:[
                        {value:90392.39, name:'2018年业务量(90392.39万件)'},
                    ]
                },
                {
                    name:'业务量(万件)',
                    type:'pie',
                    radius: ['20%', '30%'],
                    center: ['28%','28%'],
                    label: {
                        normal: {
                            formatter: '{a|{a}}{abg|}\n{hr|}\n  {b|{b}：}{c}  {per|{d}%}  ',
                            backgroundColor: '#eee',
                            borderColor: '#aaa',
                            borderWidth: 1,
                            borderRadius: 4,
                            rich: {
                                a: {
                                    color: '#999',
                                    lineHeight: 22,
                                    align: 'center'
                                },
                                hr: {
                                    borderColor: '#aaa',
                                    width: '100%',
                                    borderWidth: 0.5,
                                    height: 0
                                },
                                b: {
                                    fontSize: 16,
                                    lineHeight: 33
                                },
                                per: {
                                    color: '#eee',
                                    backgroundColor: '#334455',
                                    padding: [2, 4],
                                    borderRadius: 2
                                }
                            }
                        }
                    },
                    data:[
                        {value:464.43, name:'国际'},
                        {value:68575.6, name:'省外'},
                        {value:21352.36, name:'省内'},
                    ]
                },
                {
                    name:'业务量(万件)',
                    type:'pie',
                    selectedMode: 'single',
                    radius: [0, '15%'],
                    center: ['70%','28%'],
                    label: {
                        normal: {
                            position: 'inner'
                        }
                    },
                    labelLine: {
                        normal: {
                            show: false
                        }
                    },
                    data:[
                        {value:54911.94, name:'2017年业务量(54911.94万件)'},
                    ]
                },
                {
                    name:'业务量(万件)',
                    type:'pie',
                    radius: ['20%', '30%'],
                    center: ['70%','28%'],
                    label: {
                        normal: {
                            formatter: '{a|{a}}{abg|}\n{hr|}\n  {b|{b}：}{c}  {per|{d}%}  ',
                            backgroundColor: '#eee',
                            borderColor: '#aaa',
                            borderWidth: 1,
                            borderRadius: 4,
                            rich: {
                                a: {
                                    color: '#999',
                                    lineHeight: 22,
                                    align: 'center'
                                },
                                hr: {
                                    borderColor: '#aaa',
                                    width: '100%',
                                    borderWidth: 0.5,
                                    height: 0
                                },
                                b: {
                                    fontSize: 16,
                                    lineHeight: 33
                                },
                                per: {
                                    color: '#eee',
                                    backgroundColor: '#334455',
                                    padding: [2, 4],
                                    borderRadius: 2
                                }
                            }
                        }
                    },
                    data:[
                        {value:278.5, name:'国际'},
                        {value:37111.03, name:'省外'},
                        {value:17522.41, name:'省内'},
                    ]
                },
                {
                    name:'业务量(万件)',
                    type:'pie',
                    selectedMode: 'single',
                    radius: [0, '15%'],
                    center: ['28%','70%'],
                    label: {
                        normal: {
                            position: 'inner'
                        }
                    },
                    labelLine: {
                        normal: {
                            show: false
                        }
                    },
                    data:[
                        {value:34019.15, name:'2016年业务量(34019.15万件)'},
                    ]
                },
                {
                    name:'业务量(万件)',
                    type:'pie',
                    radius: ['20%', '30%'],
                    center: ['28%','70%'],
                    label: {
                        normal: {
                            formatter: '{a|{a}}{abg|}\n{hr|}\n  {b|{b}：}{c}  {per|{d}%}  ',
                            backgroundColor: '#eee',
                            borderColor: '#aaa',
                            borderWidth: 1,
                            borderRadius: 4,
                            rich: {
                                a: {
                                    color: '#999',
                                    lineHeight: 22,
                                    align: 'center'
                                },
                                hr: {
                                    borderColor: '#aaa',
                                    width: '100%',
                                    borderWidth: 0.5,
                                    height: 0
                                },
                                b: {
                                    fontSize: 16,
                                    lineHeight: 33
                                },
                                per: {
                                    color: '#eee',
                                    backgroundColor: '#334455',
                                    padding: [2, 4],
                                    borderRadius: 2
                                }
                            }
                        }
                    },
                    data:[
                        {value:163.72, name:'国际'},
                        {value:26841.29, name:'省外'},
                        {value:7014.14, name:'省内'},
                    ]
                },
                {
                    name:'业务量(万件)',
                    type:'pie',
                    selectedMode: 'single',
                    radius: [0, '15%'],
                    center: ['70%','70%'],
                    label: {
                        normal: {
                            position: 'inner'
                        }
                    },
                    labelLine: {
                        normal: {
                            show: false
                        }
                    },
                    data:[
                        {value:20755.74, name:'2015年业务量(20755.74万件)'},
                    ]
                },
                {
                    name:'业务量(万件)',
                    type:'pie',
                    radius: ['20%', '30%'],
                    center: ['70%','70%'],
                    label: {
                        normal: {
                            formatter: '{a|{a}}{abg|}\n{hr|}\n  {b|{b}：}{c}  {per|{d}%}  ',
                            backgroundColor: '#eee',
                            borderColor: '#aaa',
                            borderWidth: 1,
                            borderRadius: 4,
                            rich: {
                                a: {
                                    color: '#999',
                                    lineHeight: 22,
                                    align: 'center'
                                },
                                hr: {
                                    borderColor: '#aaa',
                                    width: '100%',
                                    borderWidth: 0.5,
                                    height: 0
                                },
                                b: {
                                    fontSize: 16,
                                    lineHeight: 33
                                },
                                per: {
                                    color: '#eee',
                                    backgroundColor: '#334455',
                                    padding: [2, 4],
                                    borderRadius: 2
                                }
                            }
                        }
                    },
                    data:[
                        {value:129.65, name:'国际'},
                        {value:18072.54, name:'省外'},
                        {value:2553.55, name:'省内'},
                    ]
                },
            ]
        };

        myChart.setOption(option);
    }
    //湖南省公路营运
    function echart_11(){
        var myChart = echarts.init(document.getElementById('chart_11'));
        myChart.clear();

        option = {
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b}: {c} ({d}%)"
            },
            legend: {
                x: 'left',
                top: '2%',
                left: '1%',
                textStyle: {
                    color: '#fff'
                },
                data:['公路营运载客','公路营运载货']
            },
            color: ['#FF4949','#FFA74D','#4BF0FF','#44AFF0','#4E82FF','#584BFF','#BE4DFF','#F845F1'],
            series: [
                {
                    name:'公路营运',
                    type:'pie',
                    selectedMode: 'single',
                    radius: [0, '15%'],
                    center: ['28%','28%'],
                    label: {
                        normal: {
                            position: 'inner'
                        }
                    },
                    labelLine: {
                        normal: {
                            show: false
                        }
                    },
                    data:[
                        {value:145.18, name:'2018年公路营运拥有量(145.18万辆)'},
                    ]
                },
                {
                    name:'汽车拥有量(万辆)',
                    type:'pie',
                    radius: ['20%', '30%'],
                    center: ['28%','28%'],
                    label: {
                        normal: {
                            formatter: '{a|{a}}{abg|}\n{hr|}\n  {b|{b}：}{c}  {per|{d}%}  ',
                            backgroundColor: '#eee',
                            borderColor: '#aaa',
                            borderWidth: 1,
                            borderRadius: 4,
                            position: 'outside',
                            rich: {
                                a: {
                                    color: '#999',
                                    lineHeight: 22,
                                    align: 'center'
                                },
                                hr: {
                                    borderColor: '#aaa',
                                    width: '100%',
                                    borderWidth: 0.5,
                                    height: 0
                                },
                                b: {
                                    fontSize: 16,
                                    lineHeight: 33
                                },
                                per: {
                                    color: '#eee',
                                    backgroundColor: '#334455',
                                    padding: [2, 4],
                                    borderRadius: 2
                                }
                            }
                        }
                    },
                
                    data:[
                        {value:142.65, name:'公路营运载客'},
                        {value:2.53, name:'公路营运载货'},
                    ]
                },
                {
                    name:'公路营运',
                    type:'pie',
                    selectedMode: 'single',
                    radius: [0, '15%'],
                    center: ['70%','28%'],
                    label: {
                        normal: {
                            position: 'inner'
                        }
                    },
                    labelLine: {
                        normal: {
                            show: false
                        }
                    },
                    data:[
                        {value:142.47, name:'2017年公路营运拥有量(142.47万辆)'}
                    ]
                },
                {
                    name:'汽车拥有量(万辆)',
                    type:'pie',
                    radius: ['20%', '30%'],
                    center: ['70%','28%'],
                    label: {
                        normal: {
                            formatter: '{a|{a}}{abg|}\n{hr|}\n  {b|{b}：}{c}  {per|{d}%}  ',
                            backgroundColor: '#eee',
                            borderColor: '#aaa',
                            borderWidth: 1,
                            borderRadius: 4,
                            position: 'outside',
                            rich: {
                                a: {
                                    color: '#999',
                                    lineHeight: 22,
                                    align: 'center'
                                },
                                hr: {
                                    borderColor: '#aaa',
                                    width: '100%',
                                    borderWidth: 0.5,
                                    height: 0
                                },
                                b: {
                                    fontSize: 16,
                                    lineHeight: 33
                                },
                                per: {
                                    color: '#eee',
                                    backgroundColor: '#334455',
                                    padding: [2, 4],
                                    borderRadius: 2
                                }
                            }
                        }
                    },
                
                    data:[
                        {value:139.95, name:'公路营运载客'},
                        {value:2.52, name:'公路营运载货'},
                        // {value:137.96, name:'2014年公路营运载客汽车拥有量(万辆)'},
                        // {value:2.65, name:'2014年公路营运载货汽车拥有量(万辆)'},
                        // {value:131.48, name:'2013年公路营运载客汽车拥有量(万辆)'},
                        // {value:2.97, name:'2013年公路营运载货汽车拥有量(万辆)'}
                    ]
                },
                {
                    name:'公路营运',
                    type:'pie',
                    selectedMode: 'single',
                    radius: [0, '15%'],
                    center: ['28%','70%'],
                    label: {
                        normal: {
                            position: 'inner'
                        }
                    },
                    labelLine: {
                        normal: {
                            show: false
                        }
                    },
                    data:[
                        {value:140.61, name:'2016年公路营运拥有量(140.61万辆)'},
                        // {value:142.47, name:'2015年公路营运拥有量(142.47万辆)'},
                        // {value:140.61, name:'2014年公路营运拥有量(140.61万辆)'},
                        // {value:134.45, name:'2013年公路营运拥有量(134.45万辆)'},
                    ]
                },
                {
                    name:'汽车拥有量(万辆)',
                    type:'pie',
                    radius: ['20%', '30%'],
                    center: ['28%','70%'],
                    label: {
                        normal: {
                            formatter: '{a|{a}}{abg|}\n{hr|}\n  {b|{b}：}{c}  {per|{d}%}  ',
                            backgroundColor: '#eee',
                            borderColor: '#aaa',
                            borderWidth: 1,
                            borderRadius: 4,
                            position: 'outside',
                            rich: {
                                a: {
                                    color: '#999',
                                    lineHeight: 22,
                                    align: 'center'
                                },
                                hr: {
                                    borderColor: '#aaa',
                                    width: '100%',
                                    borderWidth: 0.5,
                                    height: 0
                                },
                                b: {
                                    fontSize: 16,
                                    lineHeight: 33
                                },
                                per: {
                                    color: '#eee',
                                    backgroundColor: '#334455',
                                    padding: [2, 4],
                                    borderRadius: 2
                                }
                            }
                        }
                    },
                
                    data:[
                        {value:137.96, name:'公路营运载客'},
                        {value:2.65, name:'公路营运载货'},
                        // {value:137.96, name:'2014年公路营运载客汽车拥有量(万辆)'},
                        // {value:2.65, name:'2014年公路营运载货汽车拥有量(万辆)'},
                        // {value:131.48, name:'2013年公路营运载客汽车拥有量(万辆)'},
                        // {value:2.97, name:'2013年公路营运载货汽车拥有量(万辆)'}
                    ]
                },
                {
                    name:'公路营运',
                    type:'pie',
                    selectedMode: 'single',
                    radius: [0, '15%'],
                    center: ['70%','70%'],
                    label: {
                        normal: {
                            position: 'inner'
                        }
                    },
                    labelLine: {
                        normal: {
                            show: false
                        }
                    },
                    data:[
                        {value:134.45, name:'2015年公路营运拥有量(134.45万辆)'},
                    ]
                },
                {
                    name:'汽车拥有量(万辆)',
                    type:'pie',
                    radius: ['20%', '30%'],
                    center: ['70%','70%'],
                    label: {
                        normal: {
                            formatter: '{a|{a}}{abg|}\n{hr|}\n  {b|{b}：}{c}  {per|{d}%}  ',
                            backgroundColor: '#eee',
                            borderColor: '#aaa',
                            borderWidth: 1,
                            borderRadius: 4,
                            position: 'outside',
                            rich: {
                                a: {
                                    color: '#999',
                                    lineHeight: 22,
                                    align: 'center'
                                },
                                hr: {
                                    borderColor: '#aaa',
                                    width: '100%',
                                    borderWidth: 0.5,
                                    height: 0
                                },
                                b: {
                                    fontSize: 16,
                                    lineHeight: 33
                                },
                                per: {
                                    color: '#eee',
                                    backgroundColor: '#334455',
                                    padding: [2, 4],
                                    borderRadius: 2
                                }
                            }
                        }
                    },
                
                    data:[
                        {value:131.48, name:'公路营运载客'},
                        {value:2.97, name:'公路营运载货'},
                        // {value:137.96, name:'2014年公路营运载客汽车拥有量(万辆)'},
                        // {value:2.65, name:'2014年公路营运载货汽车拥有量(万辆)'},
                        // {value:131.48, name:'2013年公路营运载客汽车拥有量(万辆)'},
                        // {value:2.97, name:'2013年公路营运载货汽车拥有量(万辆)'}
                    ]
                }
            ]
        };

        myChart.setOption(option);
    }
    //湖南省城市公共交通
    function echart_12() {
        var myChart = echarts.init(document.getElementById('chart_12'));
        myChart.clear();
        option = {
            title: {
                text: ''
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data:['公共交通运营数','运营线路总长度','公共交通客运总量'],
                textStyle:{
                    color: '#fff'
                },
                top: '4%'
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            toolbox: {
                orient: 'vertical',
                right: '1%',
                top: '2%',
                iconStyle: {
                    color: '#FFEA51',
                    borderColor: '#FFA74D',
                    borderWidth: 1,
                },
                feature: {
                    saveAsImage: {},
                    magicType: {
                        show: true,
                        type: ['line','bar','stack','tiled']
                    }
                }
            },
            xAxis: {
                type: 'category',
                boundaryGap: false,
                data: ['2014年','2015年','2016年','2017年','2018年'],
                splitLine: {
                    show: false
                },
                axisLine: {
                    lineStyle: {
                        color: '#fff'
                    }
                }
            },
            yAxis: {
                name: '万公里',
                type: 'value',
                splitLine: {
                    show: false
                },
                axisLine: {
                    lineStyle: {
                        color: '#fff'
                    }
                }
            },
            color: ['#FF4949','#FFA74D','#FFEA51','#4BF0FF','#44AFF0','#4E82FF','#584BFF','#BE4DFF','#F845F1'],
            series: [
                {
                    name:'公共交通运营数',
                    type:'line',
                    data:[16493,17498, 15977, 18927, 21479]
                },
                {
                    name:'运营线路总长度',
                    type:'line',
                    data:[18812, 19647, 20305, 22940, 26077]
                },
                {
                    name:'公共交通客运总量',
                    type:'line',
                    data:[203954, 202727, 205342, 187208, 186048]
                },
            ]
        };
        myChart.setOption(option);
    }
    
    $('.t_btn3').click(function(){
        $('.center_text').css('display', 'none');
        $('.t_cos3').css('display', 'block');
        echart_4();
    });
    $('.t_btn4').click(function(){
        $('.center_text').css('display', 'none');
        $('.t_cos6').css('display', 'block');
        echart_6();
    });
    $('.t_btn5').click(function(){
        $('.center_text').css('display', 'none');
        $('.t_cos4').css('display', 'block');
        echart_1();
    });
    $('.t_btn6').click(function(){
        $('.center_text').css('display', 'none');
        $('.t_cos5').css('display', 'block');
        echart_3();
    });
    $('.t_btn7').click(function(){
        $('.center_text').css('display', 'none');
        $('.t_cos7').css('display', 'block');
        echart_7();
    });
    $('.t_btn8').click(function(){
        $('.center_text').css('display', 'none');
        $('.t_cos8').css('display', 'block');
        echart_8();
    });
    $('.t_btn9').click(function(){
        $('.center_text').css('display', 'none');
        $('.t_cos9').css('display', 'block');
        echart_9();
    });
    $('.t_btn10').click(function(){
        $('.center_text').css('display', 'none');
        $('.t_cos10').css('display', 'block');
        echart_10();
    });
    $('.t_btn11').click(function(){
        $('.center_text').css('display', 'none');
        $('.t_cos11').css('display', 'block');
        echart_11();
    });
    $('.t_btn12').click(function(){
        $('.center_text').css('display', 'none');
        $('.t_cos12').css('display', 'block');
        echart_12();
    });
    $('.t_btn13').click(function(){
        $('.center_text').css('display', 'none');
        $('.t_cos13').css('display', 'block');
        echart_13();
    });
    $('.t_btn14').click(function(){
        $('.center_text').css('display', 'none');
        $('.t_cos14').css('display', 'block');
        echart_14();
    });
    //获取地址栏参数
    $(function(){
        function getUrlParms(name){
            var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
                if(r!=null)
                return unescape(r[2]);
                return null;
            }
            var id = getUrlParms("id");  
            if(id == 2){
                $('.center_text').css('display', 'none');
                $('.t_cos10').css('display', 'block');
                echart_10();
            }
            if(id == 3){
                $('.center_text').css('display', 'none');
                $('.t_cos11').css('display', 'block');
                echart_11();
            }
            if(id == 4){
                $('.center_text').css('display', 'none');
                $('.t_cos1').css('display', 'block');
                echart_2();
            }
            if(id == 5){
                $('.center_text').css('display', 'none');
                $('.t_cos6').css('display', 'block');
                echart_6();
            }
            if(id == 6){
                $('.center_text').css('display', 'none');
                $('.t_cos4').css('display', 'block');
                echart_1();
            }
            if(id == 7){
                $('.center_text').css('display', 'none');
                $('.t_cos8').css('display', 'block');
                echart_8();
            }
            if(id == 8){
                $('.center_text').css('display', 'none');
                $('.t_cos12').css('display', 'block');
                echart_12();
            }
            if(id == 9){
                $('.center_text').css('display', 'none');
                $('.t_cos13').css('display', 'block');
                echart_13();
            }
    });
});
