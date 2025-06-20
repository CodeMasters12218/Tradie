namespace Tradie.Models.Users
{
    public class ReviewsWithPendingCountViewModel<T>
    {
        public List<T> Reviews { get; set; }
        public int PendingCount { get; set; }
    }

}
