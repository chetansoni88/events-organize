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

        public T Model
        {
            get
            {
                return _model;
            }
        }
        internal TableEntityBase(Guid id)
        {
            PartitionKey = TableName;
            RowKey = id.ToString();
            Id = id;
        }

        internal TableEntityBase(T model) : this(model.Id)
        {
            if (model.Id == Guid.Empty)
            {
                Guid id = Guid.NewGuid();
                model.Id = id;
                RowKey = id.ToString();
            }
            _model = model;
            Id = model.Id;
            PopulateFromModel(model);
        }

        internal virtual string TableName { get { throw new NotImplementedException(); } }

        internal virtual async Task<T> Save()
        {
            var client = new AzureTableClient<TableEntityBase<T>, T>("");
            var result = await client.Save(this);
            return _model;
        }

        internal virtual async Task<List<T>> Fetch(string propertyName, string operation, string value)
        {
            var client = new AzureTableClient<TableEntityBase<T>, T>("");
            var entities = await client.FetchByCriteria(TableName, propertyName, operation, value);
            return ExtractModels(entities);
        }

        internal virtual async Task<List<T>> FetchByQuery(string queryText)
        {
            var client = new AzureTableClient<TableEntityBase<T>, T>("");
            var entities = await client.FetchByQueryText(TableName, queryText);
            return ExtractModels(entities);
        }

        internal virtual async Task<List<T>> FetchAll()
        {
            var client = new AzureTableClient<TableEntityBase<T>, T>("");
            var entities = await client.FetchAll(TableName);
            return ExtractModels(entities);
        }

        internal virtual async Task<T> FetchById()
        {
            var client = new AzureTableClient<TableEntityBase<T>, T>("");
            var entities = await client.FetchById(TableName, Id);
            return entities.Count > 0 ? ExtractModels(entities)[0] : default(T);
        }

        internal virtual async Task<int> Delete()
        {
            var client = new AzureTableClient<TableEntityBase<T>, T>("");
            var r = await client.Delete(this);
            return r != null && r.Result != null ? 1 : 0;
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

        public Guid Id { get; set; }
    }
}
