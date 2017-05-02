using Core.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class DataEntityHelper<T> where T : IModel
    {
        ITableEntity _entity = null;
        T _model = default(T);
        public DataEntityHelper(Guid id)
        {
            switch (typeof(T).ToString())
            {
                case "Core.Models.IUser":
                    _entity = new UserEntity(id);
                    break;
                case "Core.Models.IVendor":
                    _entity = new VendorEntity(id);
                    break;
                case "Core.Models.IEvent":
                    _entity = new EventEntity(id);
                    break;
                case "Core.Models.IProject":
                    _entity = new ProjectEntity(id);
                    break;
                case "Core.Models.IArrangement":
                    _entity = new ArrangementEntity(id);
                    break;
            }
        }

        public DataEntityHelper(T model)
        {
            switch (typeof(T).ToString())
            {
                case "Core.Models.IUser":
                    _entity = new UserEntity((IUser)model);
                    break;
                case "Core.Models.IVendor":
                    _entity = new VendorEntity((IVendor)model);
                    break;
                case "Core.Models.IEvent":
                    _entity = new EventEntity((IEvent)model);
                    break;
                case "Core.Models.IProject":
                    _entity = new ProjectEntity((IProject)model);
                    break;
                case "Core.Models.IArrangement":
                    _entity = new ArrangementEntity((IArrangement)model);
                    break;
            }
            _model = model;
        }

        public async Task<T> Save()
        {
            return await ((TableEntityBase<T>)_entity).Save();
        }

        public async Task<List<T>> Fetch(string propertyName, string operation, string value)
        {
            return await ((TableEntityBase<T>)_entity).Fetch(propertyName, operation, value);
        }

        public async Task<List<T>> FetchAll()
        {
            return await ((TableEntityBase<T>)_entity).FetchAll();
        }

        public async Task<int> Delete()
        {
            return await ((TableEntityBase<T>)_entity).Delete();
        }

        public async Task<T> FetchById()
        {
            return await ((TableEntityBase<T>)_entity).FetchById();
        }

        public async Task<List<T>> FetchQuery(string query)
        {
            return await ((TableEntityBase<T>)_entity).FetchByQuery(query);
        }
    }
}
