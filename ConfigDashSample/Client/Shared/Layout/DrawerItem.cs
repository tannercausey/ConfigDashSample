namespace ConfigDashSample.Client.Shared.Layout;

public class DrawerItem
{
	public string Text { get; set; } = "Default Text";
	public string Icon { get; set; } = "home";
	public string Url { get; set; } = "/";
	public string Group { get; set; } = "app";

	public bool Expanded { get; set; }
	public int Level { get; set; }
	public IEnumerable<DrawerItem>? Children { get; set; }

	public bool AuthNeeded { get; set; }
	public string RoleNeeded { get; set; } = default!;
}