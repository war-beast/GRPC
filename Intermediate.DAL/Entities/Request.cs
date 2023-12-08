namespace Intermediate.DAL.Entities
{
    public class Request : BaseEntity
	{
        public DateTime CallDateTime { get; set; }

        public string AppUserName { get; set; }

        public string RequestName { get; set; }

        public IEnumerable<int> RequestDigits { get; set; }

        public int? RequestInteger { get; set; }
    }
}
