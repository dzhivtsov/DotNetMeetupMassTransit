using System;
using System.Threading.Tasks;

namespace MassTransitDemo.OrderProcessor
{
    internal class WarehouseService : IWarehouseService
    {
        public async Task ReserveProducts(string product, int quantity)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
