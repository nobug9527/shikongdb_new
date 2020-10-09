using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace Sk_B2BAPI.Tool
{
    public class XmlOperation
    {
        private static string _Path;
        public static string Path
        {
            get
            {
                _Path = string.Format("{0}Base.xml", AppDomain.CurrentDomain.BaseDirectory);
                return _Path;
            }
        }
        /// <summary>
        /// 读取指定子节点的内容（二级）
        /// </summary>
        /// <param name="snode">父节点</param>
        /// <param name="tnode">子节点</param>
        /// <returns></returns>
        public static string ReadXml(string snode,string tnode)
        {
            try
            {
                var xmlValue = from xl in XElement.Load(Path).Element(snode).Elements(tnode)
                               select xl.Value;
                return xmlValue.First();
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "XmlOperation/ReadXml", ex.Message.ToString());
                return "";
            }
        }
    }

    public enum NodeType
    {
        [Description("Null")]
        Null,
        [Description ("InventoryPlace")]
        InventoryPlace,
        [Description("PricePlace")]
        PricePlace,
        [Description("PackagePlace")]
        PackagePlace
    }
}