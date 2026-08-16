using DataModel;
using FileIO;
using Parser;
using System.Text.Json;

HTTP_Client clientRequest = new HTTP_Client();
JsonSerialize jsonInput = new JsonSerialize();
string file = "post.json";
var text = await clientRequest.HTTPRequestAsync();
var post = await jsonInput.JsonWriteAsync(file, text);
Console.WriteLine($"Title: {post.Title}");
Console.WriteLine($"Completed: {post.Completed}");
