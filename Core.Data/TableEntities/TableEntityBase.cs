using Core.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    internal abstract class TableEntityBase<T> : TableEntity where T : IModel
    {
        T _model = default(T);
        internal TableEntityBase(T model)
        {
            _model = model;
            PartitionKey = TableName;
            RowKey = model.Id.ToString();
            PopulateFromModel(model);
        }

        internal virtual string TableName { get { throw new NotImplementedException(); } }

        internal async Task<T> Save()
        {
            var client = new AzureTableClient<TableEntityBase<T>, T>("");
            var result = await client.Save(this);
            return result.ConvertToModel();
        }

        internal async Task<List<T>> Fetch(string propertyName, string operation, string value)
        {
            var client = new AzureTableClient<TableEntityBase<T>, T>("");
            var entities = await client.FetchByCriteria(TableName, propertyName, operation, value);
            return ExtractModels(entities);
        }

        internal async Task<List<T>> FetchAll()
        {
            var client = new AzureTableClient<TableEntityBase<T>, T>("");
            var entities = await client.FetchAll(TableName);
            return ExtractModels(entities);
        }

        internal async Task<T> FetchById()
        {
            var client = new AzureTableClient<TableEntityBase<T>, T>("");
            var entities = await client.FetchById(TableName, _model.Id);
            return entities.Count > 0 ? ExtractModels(entities)[0] : default(T);
        }

        internal async Task<int> Delete()
        {
            var client = new AzureTableClient<TableEntityBase<T>, T>("");
            var r = await client.Delete(this);
            return r.Result != null ? 1 : 0;
        }

        internal virtual T ConvertToModel()
        {
            throw new NotImplementedException();
        }

        internal virtual void PopulateFromModel(T model)
        {
            throw new NotImplementedException();
        }

        internal virtual List<T> ExtractModels(List<DynamicTableEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
