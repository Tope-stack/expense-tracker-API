using System.Text.Json.Serialization;

namespace ExpenseTracker.Models.DTO
{
    public class ExpenseDto
    {
        public Guid Id { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string MerchantName { get; set; }
        public int Amount { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Category Category { get; set; }
    }

    public class CreateExpenseDto : CreateUpdateExpenseDto
    {

    }

    public class UpdateExpenseDto : CreateUpdateExpenseDto
    {

    } 

    public abstract class CreateUpdateExpenseDto
    {
        public DateTime ExpenseDate { get; set; }
        public string MerchantName { get; set; }
        public int Amount { get; set; }
        public Category Category { get; set; }
    }
}
