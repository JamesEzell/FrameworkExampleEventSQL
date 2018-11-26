using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsCSharp;
using System.IO;
using DBDataReader = System.Data.SqlClient.SqlDataReader;//creates an alias to allow for easy switching of database products
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace EventPropsClasses
{
    public class ProductProps : IBaseProps
    {
        #region instance variables
        /// <summary>
        /// 
        /// </summary>
        public int ID = Int32.MinValue;

        /// <summary>
        /// 
        /// </summary>
        public int quantity = 0;

        /// <summary>
        /// 
        /// </summary>
        public string code = "";


        /// <summary>
        /// 
        /// </summary>
        public decimal price = 0.0m;

        /// <summary>
        /// 
        /// </summary>
        public string description = "";

        /// <summary>
        /// ConcurrencyID. See main docs, don't manipulate directly
        /// </summary>
        public int ConcurrencyID = 0;
        #endregion

        public ProductProps() {}
        
        public object Clone()
        {
            ProductProps p = new ProductProps();
            p.ID = this.ID;
            p.quantity = this.quantity;
            p.code = this.code;
            p.price = this.price;
            p.description = this.description;
            p.ConcurrencyID = this.ConcurrencyID;
            return p;
        }

        public string GetState()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, this);
            return writer.GetStringBuilder().ToString();
        }

        public void SetState(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringReader reader = new StringReader(xml);
            ProductProps p = (ProductProps)serializer.Deserialize(reader);
            this.ID = p.ID;
            this.quantity = p.quantity;
            this.code = p.code;
            this.price = p.price;
            this.description = p.description;
            this.ConcurrencyID = p.ConcurrencyID;
        }

        public void SetState(DBDataReader dr)
        {
            this.ID = (Int32)dr["ProductID"];
            this.quantity = (Int32)dr["OnHandQuantity"];
            this.code = (string)dr["ProductCode"];
            this.price = (decimal)dr["UnitPrice"];
            this.description = (string)dr["Description"];
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }
    }
}
