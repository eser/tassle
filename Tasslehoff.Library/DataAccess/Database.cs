//
//  Database.cs
//
//  Author:
//       larukedi <eser@sent.com>
//
//  Copyright (c) 2013 larukedi
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Data.Common;
using System.Data;
using System.Collections.Generic;

namespace Tasslehoff.Library.DataAccess
{
    public class Database
    {
        public const int DEFAULT_COMMAND_TIMEOUT = 3600;

        private readonly string connectionString;
        private DbProviderFactory dbProviderFactory;

        public Database (string databaseDriver, string connectionString)
        {
            this.dbProviderFactory = DbProviderFactories.GetFactory(databaseDriver);
            this.connectionString = connectionString;
        }

        public DbConnection GetConnection()
        {
            DbConnection _connection = this.dbProviderFactory.CreateConnection();
            if(_connection == null) {
                throw new NotImplementedException();
            }

            _connection.ConnectionString = this.connectionString;
            if(_connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken) {
                _connection.Open();
            }
            
            return _connection;
        }

        public DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType = CommandType.Text, int commandTimeout = Database.DEFAULT_COMMAND_TIMEOUT) {
            DbCommand _result = connection.CreateCommand();
            if(_result == null) {
                throw new NotImplementedException();
            }
            
            _result.CommandText = commandText;
            _result.CommandType = commandType;
            _result.CommandTimeout = commandTimeout;
            
            return _result;
        }

        public DbParameter GetParameter(string name, object value) {
            DbParameter _result = this.dbProviderFactory.CreateParameter();
            if(_result == null) {
                throw new NotImplementedException();
            }
            
            _result.ParameterName = name;
            _result.Value = value;
            
            return _result;
        }

        public void ExecuteReader(string commandText, CommandType commandType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default, IEnumerable<DbParameter> parameters = null, Action<DbDataReader> function = null) {
            using(DbConnection _connection = this.GetConnection()) {
                using(DbCommand _command = this.GetCommand(_connection, commandText, commandType)) {
                    if(parameters != null) {
                        // Parallel.ForEach<DbParameter>(parameters, _parameter => {
                        foreach(DbParameter _parameter in parameters) {
                            _command.Parameters.Add(_parameter);
                        }
                    }
                    
                    using(DbDataReader _reader = _command.ExecuteReader(commandBehavior)) {
                        try {
                            if(function != null) {
                                function.Invoke(_reader);
                            }
                        }
                        finally {
                            _reader.Close();
                        }
                    }
                }
                
                // _connection.Close();
            }
        }

        public object ExecuteScalar(string commandText, CommandType commandType = CommandType.Text, IEnumerable<DbParameter> parameters = null) {
            object _result;
            
            using(DbConnection _connection = this.GetConnection()) {
                using(DbCommand _command = this.GetCommand(_connection, commandText, commandType)) {
                    if(parameters != null) {
                        // Parallel.ForEach<DbParameter>(parameters, _parameter => {
                        foreach(DbParameter _parameter in parameters) {
                            _command.Parameters.Add(_parameter);
                        }
                    }
                    
                    _result = _command.ExecuteScalar();
                }
                
                // _connection.Close();
            }
            
            return _result;
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.Text, IEnumerable<DbParameter> parameters = null) {
            int _result;
            
            using(DbConnection _connection = this.GetConnection()) {
                using(DbCommand _command = this.GetCommand(_connection, commandText, commandType)) {
                    if(parameters != null) {
                        // Parallel.ForEach<DbParameter>(parameters, _parameter => {
                        foreach(DbParameter _parameter in parameters) {
                            _command.Parameters.Add(_parameter);
                        }
                    }
                    
                    _result = _command.ExecuteNonQuery();
                }
                
                // _connection.Close();
            }
            
            return _result;
        }
    }
}

