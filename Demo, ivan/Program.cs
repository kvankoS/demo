using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.SignalR.Protocol;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

List<Order> repo =
[
   new Order (11,2,5,2006, "gg", "бан","взорвался","взорвался","Василий","bam","Даниил"),
];


bool isUpdateStatus = false;
string message = "";

app.MapGet("/", () => {
    if (isUpdateStatus)
    {
        string buffer = message;
        isUpdateStatus = false;
        message = "";
        return Results.Json(new OrderUpdateStatusDto(repo, buffer));
    }
    else
        return Results.Json(repo);
});
app.MapPost("/", (Order Z) => repo.Add(Z));
app.MapPut("/{number}", (int number, OrderUpdateDTO dto) =>
{
    Order buffer = repo.Find(o => o.Number == number);
    if (buffer == null)

        if (buffer.Status != dto.Stasus)
            buffer.Status = dto.Stasus;
    if (buffer.Master != dto.Master)
        buffer.Master = dto.Master;
    if (buffer.Descripption != dto.Descpription)
        buffer.Descripption = dto.Descpription;
    {
        buffer.Status = dto.Stasus;
        isUpdateStatus = true;
        message += "Статус заявки номер" + buffer.Number + "изменен\n";
        if (buffer.Status == "завершено")
            buffer.EndDate = DateTime.Now;
    }
    if (dto.Comment != null || dto.Comment != "")
        buffer.Comments.Add(dto.Comment);

});
app.MapPut("/{number}", (int num) => repo.Find(o => o.Number == num));
app.MapGet("/stat/complCount", () => repo.FindAll(o => o.Status == "завершено"));
app.MapGet("/stat/problemTypes", () =>
{
    Dictionary<string, int> result = [];
    foreach (var o in repo)

        if (result.ContainsKey(o.ProblemType)) result[o.ProblemType]++;
        else result[o.ProblemType] = 1;
    return result;


});
app.MapGet("/stat/avrg", () => {
    double timeSum = 0;
    int oCount = 0;
    foreach (var o in repo)
    {
        if (o.Status == "завершино")
            timeSum += o.TimeInDay;
        oCount++;
    }
    return oCount > 0 ? timeSum / oCount : 0;
});
app.Run();

class OrderUpdateDTO
{
    string stasus;
    String descpription;
    string master;
    string comment;

    public string Stasus { get => stasus; set => stasus = value; }
    public string Descpription { get => descpription; set => descpription = value; }
    public string Master { get => master; set => master = value; }
    public string Comment { get => comment; set => comment = value; }
}

record class OrderUpdateStatusDto(List<Order> repo, string messege);
class Order
{
    int number;
    string device;
    string problemType;
    string descripption;
    string client;
    string status;
    string master;
    string Comment;
    int phonenumber;

    public Order(int number, int day, int month, int year, string device, string problemType, string descripption, string client, string status, string master)
    {
        Number = number;
        Device = device;
        ProblemType = problemType;
        Descripption = descripption;
        Client = client;
        Status = status;
        Master = "Не назначен";
    }
    public int Number { get => number; set => number = value; }
    public string Device { get => device; set => device = value; }
    public string ProblemType { get => problemType; set => problemType = value; }
    public string Descripption { get => descripption; set => descripption = value; }
    public string Client { get => client; set => client = value; }
    public string Status { get => status; set => status = value; }
    public string Master { get => master; set => master = value; }
    public List<string> Comments { get; set; } = [];
    public int Phonenumber { get => phonenumber; set => phonenumber = value; }
}