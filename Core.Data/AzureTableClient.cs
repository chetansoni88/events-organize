using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using System.Threading.Tasks;
using Core.Models;

namespace Core.Data
{
    internal class AzureTableClient<T, K> where T : TableEntityBase<K>
                                          where K : IModel
    {
        private string _conn = "DefaultEndpointsProtocol=https;AccountName=eventorganizesa;AccountKey=GU04KZd7m/X0Eroq4xm45ik10uv/f4BaCDYuwYFllZLarYAqAU+F57JChhBrc6D35UKKeEPF6p8mKLSeBFOdCQ==;EndpointSuffix=core.windows.net";
        private string _tableName = string.Empty;

        internal AzureTableClient(string connection)
        {
            //_conn = connection;
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
            var client = GetClient();
            // Retrieve a reference to the table.
            CloudTable table = client.GetTableReference(_tableName);
            // Create the table if it doesn't exist.
            await table.CreateIfNotExistsAsync();
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

        internal async Task<List<K>> Fetch(string tableName, string propertyName, string operation, string value)
        {
            _tableName = tableName;
            var table = await GetTableReference();

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery query = new TableQuery().Where(TableQuery.GenerateFilterCondition(propertyName, operation, value));

            // Initialize the continuation token to null to start from the beginning of the table.
            TableContinuationToken continuationToken = null;
            var list = new List<K>();
            do
            {
                // Retrieve a segment (up to 1,000 entities).
                var tableQueryResult =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQueryResult.ContinuationToken;
                foreach (var r in tableQueryResult.Results)
                {
                    list.Add(((T)Convert.ChangeType(r, typeof(T))).ConvertToModel());
                }
                // Loop until a null continuation token is received, indicating the end of the table.
            } while (continuationToken != null);
            return list;
        }
    }
}
