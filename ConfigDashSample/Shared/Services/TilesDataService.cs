using ConfigDashSample.Shared.Models;

namespace ConfigDashSample.Shared.Services;
public class TilesDataService
{
	public Task<List<TileItem>> GetTilesDataAsync()
	{
		return Task.FromResult(
			new List<TileItem>
			{
				new TileItem()
				{
					Title = "TopX Report",
					Content = "TopX",
					RowSpan = 2,
					ColSpan = 1
				},
				new TileItem()
				{
					Title = "Console Events",
					Content = "ConsoleEvents",
					RowSpan = 2,
					ColSpan = 1
				},
				new TileItem()
				{
					Title = "Risks",
					Content = "Risks",
					RowSpan = 2,
					ColSpan = 1
				},
				new TileItem()
				{
					Title = "Active Incidents",
					Content = "ActiveIncidents",
					RowSpan = 1,
					ColSpan = 1
				},
				new TileItem()
				{
					Title = "Weekly Recap-Downloads",
					Content = "WeeklyRecap",
					RowSpan = 2,
					ColSpan = 2
				},
				new TileItem()
				{
					Title = "Performance Trend",
					Content = "PerformanceTrend",
					RowSpan = 2,
					ColSpan = 2
				},
				new TileItem()
				{
					Title = "Top 5 Episodes",
					Content = "TopEpisodes",
					RowSpan = 2,
					ColSpan = 4
				}
			}
		);
	}
}