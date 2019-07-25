using System;
using System.Threading.Tasks;

namespace MassTransitDemo.OrderProcessor
{
    internal interface IWarehouseService
    {
        Task ReserveProducts(string product, int quantity);
    }
}