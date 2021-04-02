using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace getStatsForType
{
    public class Function
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private String tablename = "RatingsByType";
        public async Task<RatingsStats> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            Dictionary<String, String> cr = (Dictionary<String, String>)input.QueryStringParameters;
            String type = String.Empty;
            cr.TryGetValue("type", out type);
            GetItemResponse res = await client.GetItemAsync(tablename, new Dictionary<string, AttributeValue>
            {
                { "type", new AttributeValue {S = type} }
            });
            RatingsStats rs = JsonConvert.DeserializeObject<RatingsStats>(Document.FromAttributeMap(res.Item).ToJson());
            return rs;
        }
    }
}