using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IapStore
{
    public class Product
    {
        public Product(string id, string localizedPrice, bool owned)
        {
            Id = id;
            LocalizedPrice = localizedPrice;
            Owned = owned;
        }

        public string Id
        {
            get;
            private set;
        }

        public string LocalizedPrice
        {
            get;
            private set;
        }

        public bool Owned
        {
            get;
            private set;
        }
    }
}
