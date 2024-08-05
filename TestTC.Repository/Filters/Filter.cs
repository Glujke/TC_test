using TestTC.Repository.Enums;

namespace TestTC.Repository.Filters
{
    public class Filter
    {
        public Filter() { }
        public Filter(DateTime? date, MoreEqualsLess moreLessEqualsDate, int? priorityId, MoreEqualsLess moreLessEqualsPriority, bool? isReady)
        {
            this.Date = date;
            this.moreLessEqualsDate = moreLessEqualsDate;
            this.priorityId = priorityId;
            this.moreLessEqualsPriority = moreLessEqualsPriority;
            this.IsReady= isReady;
        }

        public DateTime? Date { get; set; }
        public MoreEqualsLess moreLessEqualsDate { get; set; }
        public int? priorityId { get; set; }
        public MoreEqualsLess moreLessEqualsPriority { get; set; }
        public bool? IsReady { get; set; }
    }
}