using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackOrders.Test.Integration.ApiBase;

namespace TrackOrders.Test.Integration.Shared
{
    [CollectionDefinition("Test collection")]
    public class SharedTestCollection : ICollectionFixture<TrackOrdersApiFactory>
    {
    }
}
