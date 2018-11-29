using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsCSharp;
using EventPropsClasses;
using CustomerDB = EventDBClasses.ProductSQLDB;
using System.Data;

namespace EventClasses
{
    public class Customer : BaseBusiness
    {
        #region Properties

        public int ID => ((CustomerProps)mProps).customerID;

        public string Name
        {
            get
            {
                return ((CustomerProps)mProps).name;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).name))
                {
                    if (value.Length >= 1 && value.Length <= 100)
                    {
                        mRules.RuleBroken("Name", false);
                        ((CustomerProps)mProps).name = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Name must be between 1 and 100 characters");
                    }
                }
            }
        }
        public string Address
        {
            get
            {
                return ((CustomerProps)mProps).address;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).address))
                {
                    if (value.Length >= 1 && value.Length <= 50)
                    {
                        mRules.RuleBroken("Address", false);
                        ((CustomerProps)mProps).address = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Address must be between 1 and 50 characters");
                    }
                }
            }
        }

        public string City
        {
            get
            {
                return ((CustomerProps)mProps).city;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).city))
                {
                    if (value.Length >= 1 && value.Length <= 20)
                    {
                        mRules.RuleBroken("City", false);
                        ((CustomerProps)mProps).city = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("City must be between 1 and 50 characters");
                    }
                }
            }
        }

        public string State
        {
            get
            {
                return ((CustomerProps)mProps).state;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).state))
                {
                    if (value.Length == 2)
                    {
                        ((CustomerProps)mProps).state = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("State must be 2 charcters");
                    }
                }
            }
        }

        public string ZipCode
        {
            get
            {
                return ((CustomerProps)mProps).zipCode;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).zipCode))
                {
                    if (value.Length >= 5 && value.Length <= 15)
                    {
                        ((CustomerProps)mProps).zipCode = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Zip Code must be between 5 and 15 numbers");
                    }
                }
            }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Default constructor - does nothing.
        /// </summary>
        public Customer() : base() { }
        

        /// <summary>
        /// One arg constructor.
        /// Calls methods SetUp(), SetRequiredRules(), 
        /// SetDefaultProperties() and BaseBusiness one arg constructor.
        /// </summary>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Customer(string cnString) : base(cnString) { }
        
        /// <summary>
        /// Two arg constructor.
        /// Calls methods SetUp() and Load().
        /// </summary>
        /// <param name="key">ID number of a record in the database.
        /// Sent as an arg to Load() to set values of record to properties of an 
        /// object.</param>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Customer(int key, string cnString) : base(key, cnString) { }
        
        public Customer(int key) : base(key) { }
        
        // *** I added these 2 so that I could create a 
        // business object from a properties object
        // I added the new constructors to the base class
        public Customer(CustomerProps props) : base(props) { }
        
        public Customer(CustomerProps props, string cnString) : base(props, cnString) { }
        
        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("Name", true);
            mRules.RuleBroken("Address", true);
            mRules.RuleBroken("City", true);
            mRules.RuleBroken("State", true);
            mRules.RuleBroken("ZipCode", true);
        }

        protected override void SetDefaultProperties() { }
        
        protected override void SetUp()
        {
            mProps = new CustomerProps();
            mOldProps = new CustomerProps();

            if (this.mConnectionString == "")
            {
                mdbReadable = new CustomerDB();
                mdbWriteable = new CustomerDB();
            }

            else
            {
                mdbReadable = new CustomerDB(this.mConnectionString);
                mdbWriteable = new CustomerDB(this.mConnectionString);
            }
        }
        public override object GetList() => throw new NotImplementedException();
        #endregion
    }
}

