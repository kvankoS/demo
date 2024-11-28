var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


List<Order> repo = new()
{
    new Order(2, (2003), "gh", "не робит", "Жмышенко Валерий Альбертовичь", 890053345),
};

app.MapGet("/", () => Order);
app.MapPost("/", (Order Z) => repo.Add(Z));
app.MapPut("/{number}", (int number, OrderUpdateDTO dto) =>
{
    Order buffer = repo.Find(o => o.Number == number);
    if (buffer == null)

        if (buffer.status != dto.Stasus)
            buffer.status = dto.Stasus;
    if (buffer.descripption != dto.Descpription)
        buffer.descripption = dto.Descpription;
    {
        buffer.Status = dto.Stasus;
        isUpdateStatus = true;
        message += "Новая заявка" + buffer.Number + "в процессе ремонта\n";
        if (buffer.Status == "завершено")
            buffer.EndDate = DateTime.Now;
    }
    if (dto.Comment != null || dto.Comment != "")
        buffer.Comments.Add(dto.Comment);

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
    string status;
    string descripption;
    string master;

    public string Status { get => status; set => status = value; }
    public string Descripption { get => descripption; set => descripption = value; }
    public string Master { get => master; set => master = value; }
}
  
class Order
{
    int number;
    int StartDate;
    string model;
    string Description;
    string client;
    int phonenumber;

    public Order(int number, int startDate1, string model, string description1, string client, int phonenumber)
    {
        Number = number;
        StartDate1 = startDate1;
        Model = model;
        Description1 = description1;
        Client = client;
        Phonenumber = phonenumber;
    }

    public int Number { get => number; set => number = value; }
    public int StartDate1 { get => StartDate; set => StartDate = value; }
    public string Model { get => model; set => model = value; }
    public string Description1 { get => Description; set => Description = value; }
    public string Client { get => client; set => client = value; }
    public int Phonenumber { get => phonenumber; set => phonenumber = value; }
}
