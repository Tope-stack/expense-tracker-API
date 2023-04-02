using System.Text.Json.Serialization;

namespace ExpenseTracker.Models.Domain
{
    public class Expense
    {
        public Guid Id { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string MerchantName { get; set; }
        public int Amount { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Category Category { get; set; }
    }
}
