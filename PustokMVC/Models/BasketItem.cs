namespace PustokMVC.Models
{
	public class BasketItem:BaseEntity
	{
		public int BookId { get; set; }
		public string AppUserId { get; set; }
		public int Count {  get; set; }
		public AppUser appUser { get; set; }
		public Book Book { get; set; }
	}
}
