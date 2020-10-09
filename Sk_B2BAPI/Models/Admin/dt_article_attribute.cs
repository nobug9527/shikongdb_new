using System;
using System.Linq;
using System.Text;

namespace Sk_B2BAPI.Models.Admin
{
    ///<summary>
    ///商品信息表
    ///</summary>
    public partial class dt_article_attribute
    {
            public dt_article_attribute(){


            }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string entid {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int article_id {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string sub_title {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string goodscode {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string generic {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string author {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string goods_no {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? point {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string drug_factory {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string approval_number {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Commodity_type {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string drug_spec {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Origin_mnemonic_code {get;set;}

           /// <summary>
           /// Desc:
           /// Default:1
           /// Nullable:True
           /// </summary>           
           public decimal? big_package {get;set;}

           /// <summary>
           /// Desc:
           /// Default:1
           /// Nullable:True
           /// </summary>           
           public decimal? min_package {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string package_unit {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Storage_conditions {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string category {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string mnemonic_code {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string dosage_form {get;set;}

           /// <summary>
           /// Desc:
           /// Default:0
           /// Nullable:True
           /// </summary>           
           public decimal? rate {get;set;}

           /// <summary>
           /// Desc:单价
           /// Default:0
           /// Nullable:True
           /// </summary>           
           public decimal? price {get;set;}

           /// <summary>
           /// Desc:
           /// Default:N
           /// Nullable:True
           /// </summary>           
           public string min_package_astrict {get;set;}

           /// <summary>
           /// Desc:状态（是否上架）
           /// Default:1
           /// Nullable:True
           /// </summary>           
           public int? status {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string changjbz {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string brandId {get;set;}

           /// <summary>
           /// Desc:最后更新时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string lastmodifytime {get;set;}

           /// <summary>
           /// Desc:商品类型（药品、器械、食品）
           /// Default:0
           /// Nullable:True
           /// </summary>           
           public int? kind {get;set;}

           /// <summary>
           /// Desc:
           /// Default:N
           /// Nullable:True
           /// </summary>           
           public string packControl {get;set;}

           /// <summary>
           /// Desc:是否拆零
           /// Default:N
           /// Nullable:True
           /// </summary>           
           public string scattered {get;set;}

           /// <summary>
           /// Desc:
           /// Default:1
           /// Nullable:True
           /// </summary>           
           public decimal? scatteredPackage {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string businessCategory {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string ypfl {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string GoodsOrigin {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string province_kz {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string cjdwmch {get;set;}

    }
}
