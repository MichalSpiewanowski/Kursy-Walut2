namespace KursyWalut2.Entities
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public string Table { get; set; }
        public string No { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public ICollection<Rate> Rates { get; set; }
    }
}
