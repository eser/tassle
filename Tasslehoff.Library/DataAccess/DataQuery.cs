// -----------------------------------------------------------------------
// <copyright file="DataQuery.cs" company="-">
// Copyright (c) 2013 larukedi (eser@sent.com). All rights reserved.
// </copyright>
// <author>larukedi (http://github.com/larukedi/)</author>
// -----------------------------------------------------------------------

//// This program is free software: you can redistribute it and/or modify
//// it under the terms of the GNU General Public License as published by
//// the Free Software Foundation, either version 3 of the License, or
//// (at your option) any later version.
//// 
//// This program is distributed in the hope that it will be useful,
//// but WITHOUT ANY WARRANTY; without even the implied warranty of
//// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//// GNU General Public License for more details.
////
//// You should have received a copy of the GNU General Public License
//// along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace Tasslehoff.Library.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    /// <summary>
    /// DataQuery class.
    /// </summary>
    public class DataQuery
    {
        // fields

        /// <summary>
        /// The database
        /// </summary>
        private Database database;

        /// <summary>
        /// The destination table
        /// </summary>
        private string destinationTable;

        /// <summary>
        /// The fields
        /// </summary>
        private IList<string> fields;

        /// <summary>
        /// The parameters
        /// </summary>
        private IList<DbParameter> parameters;

        /// <summary>
        /// The where statement
        /// </summary>
        private string whereStatement;
        
        /// <summary>
        /// The output statement
        /// </summary>
        private string outputStatement;

        /// <summary>
        /// The group by statement
        /// </summary>
        private string groupByStatement;

        /// <summary>
        /// The order by statement
        /// </summary>
        private string orderByStatement;

        /// <summary>
        /// The limit
        /// </summary>
        private int limit;

        /// <summary>
        /// The SQL string
        /// </summary>
        private string sqlString;
    
        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQuery"/> class.
        /// </summary>
        /// <param name="database">The database</param>
        public DataQuery(Database database)
        {
            this.database = database;

            this.fields = new List<string>();
            this.parameters = new List<DbParameter>();
        }

        // methods

        /// <summary>
        /// Sets the destination table.
        /// </summary>
        /// <param name="destinationTable">The destination table</param>
        /// <returns>Chain reference</returns>
        public DataQuery SetDestinationTable(string destinationTable)
        {
            this.destinationTable = destinationTable;
            return this;
        }

        /// <summary>
        /// Adds the fields.
        /// </summary>
        /// <param name="fields">The fields</param>
        /// <returns>Chain reference</returns>
        public DataQuery AddFields(params string[] fields)
        {
            foreach (string field in fields)
            {
                this.fields.Add(field);
            }

            return this;
        }

        /// <summary>
        /// Adds the fields.
        /// </summary>
        /// <param name="fields">The fields</param>
        /// <returns>Chain reference</returns>
        public DataQuery AddFields(IEnumerable<string> fields)
        {
            foreach (string field in fields)
            {
                this.fields.Add(field);
            }

            return this;
        }

        /// <summary>
        /// Adds all fields.
        /// </summary>
        /// <param name="tablePrefix">The table prefix</param>
        /// <returns>Chain reference</returns>
        public DataQuery AddAllFields(string tablePrefix = null)
        {
            if (!string.IsNullOrEmpty(tablePrefix))
            {
                this.fields.Add(tablePrefix + ".*");
                return this;
            }

            this.fields.Add("*");
            return this;
        }

        /// <summary>
        /// Adds the parameters.
        /// </summary>
        /// <param name="parameters">The parameters</param>
        /// <returns>Chain reference</returns>
        public DataQuery AddParameters(params DbParameter[] parameters)
        {
            foreach (DbParameter parameter in parameters)
            {
                if (parameter.Value == null)
                {
                    parameter.Value = DBNull.Value;
                }

                this.parameters.Add(parameter);
            }

            return this;
        }

        /// <summary>
        /// Adds the parameters.
        /// </summary>
        /// <param name="parameters">The parameters</param>
        /// <returns>Chain reference</returns>
        public DataQuery AddParameters(IEnumerable<DbParameter> parameters)
        {
            foreach (DbParameter parameter in parameters)
            {
                if (parameter.Value == null)
                {
                    parameter.Value = DBNull.Value;
                }

                this.parameters.Add(parameter);
            }

            return this;
        }

        /// <summary>
        /// Adds the parameters.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <returns>Chain reference</returns>
        public DataQuery AddParameters(string key, object value)
        {
            object fieldValue = value;
            if (fieldValue == null)
            {
                fieldValue = DBNull.Value;
            }

            this.parameters.Add(this.database.GetParameter(key, fieldValue));
            return this;
        }

        /// <summary>
        /// Adds the parameters.
        /// </summary>
        /// <param name="parameters">The parameters</param>
        /// <returns>Chain reference</returns>
        public DataQuery AddParameters(params KeyValuePair<string, object>[] parameters)
        {
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                object fieldValue = parameter.Value;
                if (fieldValue == null)
                {
                    fieldValue = DBNull.Value;
                }

                this.parameters.Add(this.database.GetParameter(parameter.Key, fieldValue));
            }

            return this;
        }

        /// <summary>
        /// Adds the parameters.
        /// </summary>
        /// <param name="parameters">The parameters</param>
        /// <returns>Chain reference</returns>
        public DataQuery AddParameters(IEnumerable<KeyValuePair<string, object>> parameters)
        {
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                object fieldValue = parameter.Value;
                if (fieldValue == null)
                {
                    fieldValue = DBNull.Value;
                }

                this.parameters.Add(this.database.GetParameter(parameter.Key, fieldValue));
            }

            return this;
        }

        /// <summary>
        /// Sets the where.
        /// </summary>
        /// <param name="whereStatement">The where statement</param>
        /// <returns>Chain reference</returns>
        public DataQuery SetWhere(string whereStatement)
        {
            this.whereStatement = whereStatement;
            return this;
        }

        /// <summary>
        /// Sets the output.
        /// </summary>
        /// <param name="outputStatement">The output statement</param>
        /// <returns>Chain reference</returns>
        public DataQuery SetOutput(string outputStatement)
        {
            this.outputStatement = outputStatement;
            return this;
        }

        /// <summary>
        /// Sets the group by.
        /// </summary>
        /// <param name="groupByStatement">The group by statement</param>
        /// <returns>Chain reference</returns>
        public DataQuery SetGroupBy(string groupByStatement)
        {
            this.groupByStatement = groupByStatement;
            return this;
        }

        /// <summary>
        /// Sets the order by.
        /// </summary>
        /// <param name="orderByStatement">The order by statement</param>
        /// <returns>Chain reference</returns>
        public DataQuery SetOrderBy(string orderByStatement)
        {
            this.orderByStatement = orderByStatement;
            return this;
        }

        /// <summary>
        /// Sets the limit.
        /// </summary>
        /// <param name="limit">The limit</param>
        /// <returns>Chain reference</returns>
        public DataQuery SetLimit(int limit)
        {
            this.limit = limit;
            return this;
        }

        /// <summary>
        /// Selects the query.
        /// </summary>
        /// <returns>Chain reference</returns>
        public DataQuery SelectQuery()
        {
            StringBuilder queryText = new StringBuilder();

            queryText.Append("SELECT ");
            if (this.limit > 0)
            {
                queryText.Append("TOP(");
                queryText.Append(this.limit);
                queryText.Append(") ");
            }

            bool isFirstField = true;
            foreach (string field in this.fields)
            {
                if (isFirstField)
                {
                    isFirstField = false;
                }
                else
                {
                    queryText.Append(", ");
                }

                queryText.Append(field);
            }

            queryText.Append(" FROM ");
            queryText.Append(this.destinationTable);

            if (this.whereStatement != null)
            {
                queryText.Append(" WHERE ");
                queryText.Append(this.whereStatement);
            }

            if (this.groupByStatement != null)
            {
                queryText.Append(" GROUP BY ");
                queryText.Append(this.groupByStatement);
            }

            if (this.orderByStatement != null)
            {
                queryText.Append(" ORDER BY ");
                queryText.Append(this.orderByStatement);
            }

            this.sqlString = queryText.ToString();
            return this;
        }

        /// <summary>
        /// Inserts the query.
        /// </summary>
        /// <returns>Chain reference</returns>
        public DataQuery InsertQuery()
        {
            StringBuilder queryText = new StringBuilder();

            queryText.Append("INSERT INTO ");
            queryText.Append(this.destinationTable);
            queryText.Append(" (");

            bool isFirstField = true;
            foreach (string field in this.fields)
            {
                if (isFirstField)
                {
                    isFirstField = false;
                }
                else
                {
                    queryText.Append(", ");
                }

                queryText.Append(field);
            }

            queryText.Append(")");

            if (this.outputStatement != null)
            {
                queryText.Append(" OUTPUT ");
                queryText.Append(this.outputStatement);
            }

            queryText.Append(" VALUES (");

            isFirstField = true;
            foreach (string field in this.fields)
            {
                if (isFirstField)
                {
                    isFirstField = false;
                }
                else
                {
                    queryText.Append(", ");
                }

                queryText.Append('@');
                queryText.Append(field);
            }

            queryText.Append(")");

            this.sqlString = queryText.ToString();
            return this;
        }

        /// <summary>
        /// Updates the query.
        /// </summary>
        /// <param name="manualSet">if set to <c>true</c> [manual set]</param>
        /// <returns>Chain reference</returns>
        public DataQuery UpdateQuery(bool manualSet = false)
        {
            StringBuilder queryText = new StringBuilder();

            queryText.Append("UPDATE ");
            if (this.limit > 0)
            {
                queryText.Append("TOP(");
                queryText.Append(this.limit);
                queryText.Append(") ");
            }

            queryText.Append(this.destinationTable);

            queryText.Append(" SET ");
            bool isFirstField = true;
            foreach (string field in this.fields)
            {
                if (isFirstField)
                {
                    isFirstField = false;
                }
                else
                {
                    queryText.Append(", ");
                }

                queryText.Append(field);
                if (!manualSet)
                {
                    queryText.Append("=@");
                    queryText.Append(field);
                }
            }

            if (this.outputStatement != null)
            {
                queryText.Append(" OUTPUT ");
                queryText.Append(this.outputStatement);
            }

            if (this.whereStatement != null)
            {
                queryText.Append(" WHERE ");
                queryText.Append(this.whereStatement);
            }

            this.sqlString = queryText.ToString();
            return this;
        }

        /// <summary>
        /// Deletes the query.
        /// </summary>
        /// <returns>Chain reference</returns>
        public DataQuery DeleteQuery()
        {
            StringBuilder queryText = new StringBuilder();

            queryText.Append("DELETE ");
            if (this.limit > 0)
            {
                queryText.Append("TOP(");
                queryText.Append(this.limit);
                queryText.Append(") ");
            }

            queryText.Append("FROM ");
            queryText.Append(this.destinationTable);

            if (this.outputStatement != null)
            {
                queryText.Append(" OUTPUT ");
                queryText.Append(this.outputStatement);
            }

            if (this.whereStatement != null)
            {
                queryText.Append(" WHERE ");
                queryText.Append(this.whereStatement);
            }

            this.sqlString = queryText.ToString();
            return this;
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="commandBehavior">The command behavior</param>
        /// <param name="function">The function</param>
        public void ExecuteReader(CommandBehavior commandBehavior = CommandBehavior.Default, Action<DbDataReader> function = null)
        {
            this.database.ExecuteReader(this.sqlString, CommandType.Text, commandBehavior, this.parameters, function);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <returns>Scalar result</returns>
        public object ExecuteScalar()
        {
            return this.database.ExecuteScalar(this.sqlString, CommandType.Text, this.parameters);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <returns>NonQuery result</returns>
        public int ExecuteNonQuery()
        {
            return this.database.ExecuteNonQuery(this.sqlString, CommandType.Text, this.parameters);
        }

        /// <summary>
        /// Gets the SQL string.
        /// </summary>
        /// <returns>Query string</returns>
        public string GetSqlString()
        {
            return this.sqlString;
        }
    }
}
