namespace AngularShop.API.Helpers
{
    public class UserParams
    {

        private const int MaxpageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxpageSize) ? MaxpageSize : value; }
        }

        public int UserId { get; set; }
        public string OrderBy { get; set; }


    }
}