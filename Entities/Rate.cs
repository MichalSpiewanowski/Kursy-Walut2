namespace KursyWalut2.Entities
{
    public class Rate
    {
        public int Id { get; set; }
        public string? Currency {  get; set; }
        public string Code { get; set; }
        public double? Mid { get; set; }
        public double? Bid { get; set; }
        public double? Ask { get; set; }
    }
}
