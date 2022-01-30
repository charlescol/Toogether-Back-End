using System;
using System.Collections.Generic;
using System.Text;

namespace AppModel.Storage
{
    public class ReferencedItem<TId, TItem>
    {
        public TId _id { get; set; }
        public TItem Item {get; set;}

        public ReferencedItem(TId id, TItem item )
        {
            _id = id;
            Item = item;
        }
    }
}
