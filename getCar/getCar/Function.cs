using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DocumentModel;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace getCar
{

    public class Function
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private String tablename = "Cars";
        public async Task<Car> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            Dictionary<String, String> cr = (Dictionary<String, String>)input.QueryStringParameters;
            String itemId = String.Empty;
            cr.TryGetValue("itemId", out itemId);
            GetItemResponse res = await client.GetItemAsync(tablename, new Dictionary<string, AttributeValue>
            {
                { "itemId", new AttributeValue {S = itemId} }
            });
            Car car = JsonConvert.DeserializeObject<Car>(Document.FromAttributeMap(res.Item).ToJson());
            return car;
        }
    }
}