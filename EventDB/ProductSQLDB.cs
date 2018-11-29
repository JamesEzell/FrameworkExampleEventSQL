using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventPropsClasses;
using ToolsCSharp;

using System.Data;
using System.Data.SqlClient;

// *** I use an "alias" for the ado.net classes throughout my code
// When I switch to an oracle database, I ONLY have to change the actual classes here
using DBBase = ToolsCSharp.BaseSQLDB;
using DBConnection = System.Data.SqlClient.SqlConnection;
using DBCommand = System.Data.SqlClient.SqlCommand;
using DBParameter = System.Data.SqlClient.SqlParameter;
using DBDataReader = System.Data.SqlClient.SqlDataReader;
using DBDataAdapter = System.Data.SqlClient.SqlDataAdapter;

namespace EventDBClasses
{
    public class ProductSQLDB : DBBase, IReadDB, IWriteDB
    {
        #region Constructors

        public ProductSQLDB() : base() { }
        public ProductSQLDB(string cnString) : base(cnString) { }
        public ProductSQLDB(DBConnection cn) : base(cn) { }

        #endregion

        // Implementation of methods required by the interfaces
        // Notice that they use ADO.NET objects and call methods in the SQL base class
        #region IReadDB Members
        /// <summary>
        /// </summary>
        /// 
        public IBaseProps Retrieve(Object key)
        {
            DBDataReader data = null;
            ProductProps props = new ProductProps();
            DBCommand command = new DBCommand();

            command.CommandText = "usp_ProductSelect";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters["@ProductID"].Value = (Int32)key;

            try
            {
                data = RunProcedure(command);
                if (!data.IsClosed)
                {
                    if (data.Read())
                    {
                        props.SetState(data);
                    }
                    else
                        throw new Exception("Record does not exist in the database.");
                }
                return props;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (data != null)
                {
                    if (!data.IsClosed)
                        data.Close();
                }
            }
        } //end of Retrieve()

        // retrieves a list of objects
        public object RetrieveAll(Type type)
        {
            List<ProductProps> list = new List<ProductProps>();
            DBDataReader reader = null;
            ProductProps props;

            try
            {
                reader = RunProcedure("usp_EventSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        props = new ProductProps();
                        props.SetState(reader);
                        list.Add(props);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }
        #endregion

        #region IWriteDB Members
        /// <summary>
        /// </summary>
        public IBaseProps Create(IBaseProps p)
        {
            int rowsAffected = 0;
            ProductProps props = (ProductProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_EventCreate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters.Add("@ProductCode", SqlDbType.Char);
            command.Parameters.Add("@Description", SqlDbType.NVarChar);
            command.Parameters.Add("@UnitPrice", SqlDbType.Money);
            command.Parameters.Add("@OnHandQuantity", SqlDbType.Int);
            command.Parameters[0].Direction = ParameterDirection.Output;
            command.Parameters["@ProductCode"].Value = props.code;
            command.Parameters["@Description"].Value = props.description;
            command.Parameters["@UnitPrice"].Value = props.price;
            command.Parameters["@OnHandQuantity"].Value = props.quantity;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.productID = (int)command.Parameters[0].Value;
                    props.ConcurrencyID = 1;
                    return props;
                }
                else
                    throw new Exception("Unable to insert record. " + props.ToString());
            }
            catch (Exception e)
            {
                // log this error
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }

        /// <summary>
        /// </summary>
        public bool Delete(IBaseProps p)
        {
            ProductProps props = (ProductProps)p;
            int rowsAffected = 0;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_EventDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters["@ProductID"].Value = props.productID;
            command.Parameters["@ConcurrencyID"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    return true;
                }
                else
                {
                    string message = "Record cannot be deleted. It has been edited by another user.";
                    throw new Exception(message);
                }

            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        } // end of Delete()

        /// <summary>
        /// </summary>
        public bool Update(IBaseProps p)
        {
            int rowsAffected = 0;
            ProductProps props = (ProductProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductUpdate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters.Add("@ProductCode", SqlDbType.Char);
            command.Parameters.Add("@ProductDescription", SqlDbType.VarChar);
            command.Parameters.Add("@ProductPrice", SqlDbType.Money);
            command.Parameters.Add("@ProductQuantity", SqlDbType.Int);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters["@ProductID"].Value = props.productID;
            command.Parameters["@ProductCode"].Value = props.code;
            command.Parameters["@ProductDescription"].Value = props.description;
            command.Parameters["@ProductPrice"].Value = props.price;
            command.Parameters["@ProductQuantity"].Value = props.quantity;
            command.Parameters["@ConcurrencyID"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ConcurrencyID++;
                    return true;
                }
                else
                {
                    string message = "Record cannot be updated. It has been edited by another user.";
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        } // end of Update()
        #endregion

        // the version of the delete called from the 
        // static method in the Business Class.  It ignores the concurrency id
        public void Delete(int key)
        {
            int rowsAffected = 0;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductStaticDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters["@ProductID"].Value = key;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected != 1)
                {
                    string message = "Record was not deleted. Perhaps the key you specified does not exist.";
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                // log this error
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }


        // Shows you how to use a data table rather than a list of objects
        public DataTable RetrieveTable()
        {
            DataTable t = new DataTable("ProductList");
            DBDataReader reader = null;
            DataRow row;

            t.Columns.Add("ID", System.Type.GetType("System.Int32"));
            t.Columns.Add("Description", System.Type.GetType("System.String"));
            t.Columns.Add("Code", System.Type.GetType("System.String"));
            t.Columns.Add("Price", System.Type.GetType("System.Decimal"));
            t.Columns.Add("Quantity", System.Type.GetType("System.Int"));

            try
            {
                reader = RunProcedure("usp_ProductSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        row = t.NewRow();
                        row["ID"] = reader["ProductId"];
                        row["Description"] = reader["ProductDescription"];
                        row["Code"] = reader["ProductCode"];
                        row["Price"] = reader["ProductPrice"];
                        row["Quantity"] = reader["ProductQuantity"];
                        t.Rows.Add(row);
                    }
                }
                t.AcceptChanges();
                return t;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }
    }
}
