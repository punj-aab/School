﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using Dapper;
using System.Collections.Generic;

namespace StudentTracker.Core.Repositories
{
    public abstract class BaseRepository
    {
        protected static void SetIdentity<T>(IDbConnection connection, Action<T> setId)
        {
            dynamic identity = connection.Query("SELECT @@IDENTITY AS Id").Single();
            T newId = (T)identity.Id;
            setId(newId);
        }

        protected static IDbConnection OpenConnection()
        {
            IDbConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString);
            connection.Open();
            return connection;
        }

        public List<T> Get<T>(string sql)
        {
            List<T> objlist = new List<T>();
            using (IDbConnection connection = OpenConnection())
            {
                return objlist = connection.Query<T>(sql).ToList();
            }
        }

        public List<T> Find<T>(string sql, long id)
        {
            List<T> objList = new List<T>();
            using (IDbConnection connection = OpenConnection())
            {
                return objList = connection.Query<T>(sql, new { id = id }).ToList();
            }
        }

        public List<T> Find<T>(string sql, int id)
        {
            List<T> objList = new List<T>();
            using (IDbConnection connection = OpenConnection())
            {
                return objList = connection.Query<T>(sql, new { id = id }).ToList();
            }
        }

        public T SingleOrDefault<T>(string sql, long id)
        {
            T obj;
            using (IDbConnection connection = OpenConnection())
            {
                return obj = connection.Query<T>(sql, new { id = id }).SingleOrDefault();
            }
        }

        public T ExecuteSP<T>(string sp, object parameters)
        {
            T obj;
            using (IDbConnection connection = OpenConnection())
            {
                return obj = connection.Query<T>(sp, parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }

        public int ExecuteQuery(string query, object parameters)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Execute(query, param: parameters);
            }
        }
        //public List<T> ExecuteSP<T>(string sp, object parameters)
        //{
        //    using (IDbConnection connection = OpenConnection())
        //    {
        //        return connection.Query<T>(sp, parameters, commandType: CommandType.StoredProcedure).ToList();
        //    }
        //}

    }
}