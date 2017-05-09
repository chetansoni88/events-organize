using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Core.Data
{
    internal class TokenEntity : TableEntityBase<IToken>
    {
        internal TokenEntity(Guid id):base(id)
        {

        }

        internal TokenEntity(IToken token) : base(token)
        {

        }

        public Guid UserId { get; set; }

        internal override IToken ConvertToModel()
        {
            IToken t = new Token();
            t.Id = Id;
            t.UserId = UserId;
            return t;
        }

        internal override List<IToken> ExtractModels(List<DynamicTableEntity> entities)
        {
            List<IToken> list = new List<IToken>();
            foreach (var entity in entities)
            {
                IToken t = new Token();
                t.Id = entity.Properties["Id"].GuidValue.Value;
                t.UserId = entity.Properties["UserId"].GuidValue.Value;
                list.Add(t);
            }
            return list;
        }

        internal override void PopulateFromModel(IToken model)
        {
            Id = model.Id;
            UserId = model.UserId;
        }

        internal override string TableName => "tokens";
    }
}
