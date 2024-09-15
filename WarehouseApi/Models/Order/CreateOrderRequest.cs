using WarehouseApi.Enums;
using System.Text.Json.Serialization;

namespace WarehouseApi.Models.Order
{
    public class CreateOrderRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OutOfStockModeEnum Mode { get; set; }
    }
}
