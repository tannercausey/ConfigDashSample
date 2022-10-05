namespace ConfigDashSample.Shared.Models;
public class TileItem
{
	public string Title { get; set; }
	public string Content { get; set; }
	public bool Visible { get; set; } = true;
	public int RowSpan { get; set; }
	public int ColSpan { get; set; }
}