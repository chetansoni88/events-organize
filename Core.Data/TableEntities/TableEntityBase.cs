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
        internal TableEntityBase(T model)
        {
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
            return await client.Fetch(TableName, propertyName, operation, value);
        }

        internal virtual T ConvertToModel()
        {
            throw new NotImplementedException();
        }

        internal virtual void PopulateFromModel(T model)
        {
            throw new NotImplementedException();
        }

        internal static T ExtractModel(DynamicTableEntity entity)
        {
            foreach(var key in entity.Properties.Keys)
            {

            }
            return default(T);
        }
    }
}
