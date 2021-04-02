using System;
using Newtonsoft.Json;

public class RatingsStats
{
	private decimal _averageRating, _totalRating;
	private int _count;
	private string _type;
	public decimal AverageRating { get => _averageRating; set => _averageRating = value; }
	public decimal TotalRating { get => _totalRating; set => _totalRating = value; }
	public int Count { get => _count; set => _count = value; }
	public string Type { get => _type; set => _type = value; }
	public RatingsStats(){}
	[JsonConstructor]
	public RatingsStats(decimal average, decimal total, int count, string type)
    {
		AverageRating = average;
		TotalRating = total;
		Count = count;
		Type = type;
    }
}
