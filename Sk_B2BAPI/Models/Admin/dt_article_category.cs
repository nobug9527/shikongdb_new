using System;
using System.Linq;
using System.Text;

namespace Sk_B2BAPI.Models.Admin
{
    ///<summary>
    ///channel_id=7的是商品分类
    ///</summary>
    public partial class dt_article_category
    {
            public dt_article_category(){


            }
           /// <summary>
           /// Desc:自增ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int id {get;set;}

           /// <summary>
           /// Desc:频道ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int channel_id {get;set;}

           /// <summary>
           /// Desc:类别标题
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string title {get;set;}

           /// <summary>
           /// Desc:调用别名
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string call_index {get;set;}

           /// <summary>
           /// Desc:父类别ID
           /// Default:0
           /// Nullable:True
           /// </summary>           
           public int? parent_id {get;set;}

           /// <summary>
           /// Desc:类别ID列表(逗号分隔开)
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string class_list {get;set;}

           /// <summary>
           /// Desc:类别深度
           /// Default:0
           /// Nullable:True
           /// </summary>           
           public int? class_layer {get;set;}

           /// <summary>
           /// Desc:排序数字
           /// Default:99
           /// Nullable:True
           /// </summary>           
           public int? sort_id {get;set;}

           /// <summary>
           /// Desc:URL跳转地址
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string link_url {get;set;}

           /// <summary>
           /// Desc:图片地址
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string img_url {get;set;}

           /// <summary>
           /// Desc:备注说明
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string content {get;set;}

           /// <summary>
           /// Desc:SEO标题
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string seo_title {get;set;}

           /// <summary>
           /// Desc:SEO关健字
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string seo_keywords {get;set;}

           /// <summary>
           /// Desc:SEO描述
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string seo_description {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string img_url_1 {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string img_url_2 {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string entid {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string lastmodifytime {get;set;}

    }
}
