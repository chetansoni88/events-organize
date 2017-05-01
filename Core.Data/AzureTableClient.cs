using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using System.Threading.Tasks;
using Core.Models;

namespace Core.Data
{
    internal class TableMapper
    {
        private TableMapper()
        {

        }

        private static TableMapper _mapper = null;
        private static object _lock = new object();
        internal static TableMapper Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_mapper == null)
                    {
                        lock (_lock)
                        {
                            _mapper = new TableMapper();
                            _mapper.Map = new Dictionary<string, CloudTable>();
                        }
                    }
                }
                return _mapper;
            }
        }

        internal Dictionary<string, CloudTable> Map
        {
            get;
            private set;
        }
    }
    internal class AzureTableClient<T, K> where T : TableEntityBase<K>
                                          where K : IModel
    {
        private string _conn = "DefaultEndpointsProtocol=https;AccountName=eventorganizesa;AccountKey=GU04KZd7m/X0Eroq4xm45ik10uv/f4BaCDYuwYFllZLarYAqAU+F57JChhBrc6D35UKKeEPF6p8mKLSeBFOdCQ==;EndpointSuffix=core.windows.net";
        private string _tableName = string.Empty;

        internal AzureTableClient(string connection)
        {
            if (!string.IsNullOrEmpty(connection.Trim()))
                _conn = connection;
        }

        CloudTableClient GetClient()
        {
            // Retrieve the storage account from the connection string.
            var storageAccount = CloudStorageAccount.Parse(_conn);
            // Create the table client.
            return storageAccount.CreateCloudTableClient();
        }

        async Task<CloudTable> GetTableReference()
        {
            CloudTable table = null;
            if (TableMapper.Instance.Map.ContainsKey(_tableName))
            {
                table = TableMapper.Instance.Map[_tableName];
            }
            else
            {
                var client = GetClient();
                // Retrieve a reference to the table.
                table = client.GetTableReference(_tableName);
                // Create the table if it doesn't exist.
                await table.CreateIfNotExistsAsync();
                TableMapper.Instance.Map.Add(_tableName, table);
            }
            return table;
        }

        internal async Task<T> Save(T t)
        {
            _tableName = t.TableName;
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.InsertOrReplace(t);
            var table = await GetTableReference();

            // Execute the insert operation.
            var result = await table.ExecuteAsync(insertOperation);
            return (T)result.Result;
        }

        internal async Task<List<DynamicTableEntity>> FetchByCriteria(string tableName, string propertyName,
            string operation, string value)
        {
            return await FetchByQueryText(tableName, TableQuery.GenerateFilterCondition(propertyName, operation, value));
        }

        async Task<List<DynamicTableEntity>> FetchByQueryText(string tableName, string queryText)
        {
            _tableName = tableName;
            var table = await GetTableReference();

            TableQuery query = new TableQuery().Where(queryText);

            // Initialize the continuation token to null to start from the beginning of the table.
            TableContinuationToken continuationToken = null;
            var list = new List<DynamicTableEntity>();
            do
            {
                // Retrieve a segment (up to 1,000 entities).
                var tableQueryResult =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQueryResult.ContinuationToken;
                list.AddRange(tableQueryResult.Results);
                // Loop until a null continuation token is received, indicating the end of the table.
            } while (continuationToken != null);
            return list;
        }

        internal async Task<List<DynamicTableEntity>> FetchById(string tableName, Guid id)
        {
            return await FetchByQueryText(tableName, TableQuery.GenerateFilterConditionForGuid("Id", "eq", id));
        }

        internal async Task<List<DynamicTableEntity>> FetchAll(string tableName)
        {
            return await FetchByQueryText(tableName, TableQuery.GenerateFilterCondition("PartitionKey", "eq", tableName));
        }

        internal async Task<TableResult> Delete(T entity)
        {
            _tableName = entity.TableName;
            var table = await GetTableReference();
            var results = await FetchById(entity.TableName, entity.Id);
            if (results.Count > 0)
            {
                var op = TableOperation.Delete(results[0]);
                return await table.ExecuteAsync(op);
            }
            return null;
        }
    }
}
