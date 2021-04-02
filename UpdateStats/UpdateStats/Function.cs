using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace UpdateStats
{
    public class Function
    {
        string returns = String.Empty;
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private String tablename = "RatingsByType";
        public async Task<String> FunctionHandler(DynamoDBEvent input, ILambdaContext context)
        {
            Table table = Table.LoadTable(client, tablename);
            List<Car> cars = new List<Car>();
            List<DynamoDBEvent.DynamodbStreamRecord> records = (List<DynamoDBEvent.DynamodbStreamRecord>)input.Records;
            if (records.Count > 0)
            {
                DynamoDBEvent.DynamodbStreamRecord record = records[0];
                if (record.EventName.Equals("INSERT"))
                {
                    Document doc = Document.FromAttributeMap(record.Dynamodb.NewImage);
                    Car car = JsonConvert.DeserializeObject<Car>(doc.ToJson());
                    GetItemResponse res = await client.GetItemAsync(tablename, new Dictionary<string, AttributeValue>
                    {
                        { "type", new AttributeValue {S = car.Type} }
                    });
                    RatingsStats stats = JsonConvert.DeserializeObject<RatingsStats>(Document.FromAttributeMap(res.Item).ToJson());
                    var request = new UpdateItemRequest
                    {
                        TableName = tablename,
                        Key = new Dictionary<string, AttributeValue>
                            {
                                { "type", new AttributeValue{ S = car.Type } }
                            },
                        AttributeUpdates = new Dictionary<string, AttributeValueUpdate>()
                            {
                                { "count", new AttributeValueUpdate { Action="ADD", Value = new AttributeValue{N = "1" } } },
                                { "total", new AttributeValueUpdate { Action="ADD", Value = new AttributeValue{N = car.Rating.ToString() } } },
                                { "average", new AttributeValueUpdate { Action="PUT", Value = new AttributeValue{
                                    N = ((stats.TotalRating + car.Rating)/(stats.Count + 1)).ToString()
                                } } }
                            }
                    };
                    await client.UpdateItemAsync(request);
                    returns = "Updated";
                }
            }
            if (returns != String.Empty)
                return returns;
            else
                return "Error";
        }
    }
}
