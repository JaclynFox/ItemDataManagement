using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace additem
{
    public class Function
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private String tablename = "Cars";
        public async Task<String> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            try
            {
                Dictionary<String, AttributeValue> dict = new Dictionary<string, AttributeValue>();
                Car car = JsonConvert.DeserializeObject<Car>(input.Body);
                dict["itemId"] = new AttributeValue { S = car.ItemID };
                dict["description"] = new AttributeValue { S = car.Description };
                dict["rating"] = new AttributeValue { N = car.Rating.ToString() };
                dict["type"] = new AttributeValue { S = car.Type };
                dict["company"] = new AttributeValue { S = car.Company };
                dict["lastinstanceofword"] = new AttributeValue { S = car.LastInstanceOfWord };
                PutItemRequest req = new PutItemRequest(tablename, dict);
                PutItemResponse res = await client.PutItemAsync(req);
                return res.ToString();
            }
            catch (Exception e) {
                return "Error parsing JSON. Referr to API documentation and make sure your input is formatted correctly. Exception message follows:\n" + e.Message;
            }
        }
    }
}
