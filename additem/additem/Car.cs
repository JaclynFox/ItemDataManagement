using System;
using Newtonsoft.Json;

public class Car
{
	private String _itemID, _description, _type, _company, _word;
	private Decimal _rating;
	public String ItemID { get => _itemID; set => _itemID = value; }
	public String Description { get => _description; set => _description = value; }
	public String Type { get => _type; set => _type = value; }
	public String Company { get => _company; set => _company = value; }
	public Decimal Rating { get => _rating; set => _rating = value; }
	public String LastInstanceOfWord { get => _word; set => _word = value; }
	public Car(){}
	[JsonConstructor]
	public Car(String itemId, String description, String type, String company, Decimal rating)
    {
		ItemID = itemId;
		Description = description;
		Type = type;
		Company = company;
		if (Company == "B")
			Rating = rating / 2;
		else if (rating >= 1)
			Rating = rating;
		else
			Rating = 1;
		Func<string, string> FindWord = w =>
		{
			String word = String.Empty;
			String[] words = w.Split(' ');
			for (int i = words.Length - 1; i >= 0; i--)
			{
				foreach (char c in words[i].ToLower())
					if (c == 'e' || c == 'o')
					{
						word = words[i].ToLower();
						i = -1;
						break;
					}
			}
			return word;
		};
		LastInstanceOfWord = FindWord(description);
	}
	public override String ToString()
    {
		return "{\n\t\"itemID\": \"" + ItemID + "\",\n\t\"description\": \"" + Description + "\",\n\t\"type\": \"" + 
			Type + "\",\n\t\"company\": \"" + Company + "\",\n\t\"rating\": " + Rating + "\"\n}";
    }
}
