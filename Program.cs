using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);//aqui estamos deixando pronto para o banco poder usar

var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

/*Esses daqui em baixo são end points que são partes do codigo
usados para encaminhar até o final*/
app.MapGet("/", () => "Hello World 2");
/*Aqui o c# ja transforma ele em json*/
app.MapPost("/user", ()=> new {Name="Stephany Batista", Age=35});
app.MapGet("/AddHeader", (HttpResponse response)=> {
    response.Headers.Add("Teste","Stephany Batista");
    return "Retorno";
    });



/*parte get que pega da aplicação*/
//api.app.com/users?datastart={date}&dateend={date}
app.MapGet("/getproduct", ([FromQuery] string dateStart,[FromQuery] string dateEnd)=>{
    return dateStart + " - "+ dateEnd;
});



/*get que pega dadods direto do cabeçalho*/
//api.app.com/users?datastart={date}&dateend={date}
app.MapGet("/getproductbyheader", (HttpRequest request)=>{
    return request.Headers["product-code"].ToString();
});


/*Metodos principais abaixo*/
/*Parte de Post da aplicação usada para postar dados da aplicação*/
app.MapPost("/products", (ProductRequest productRequest, ApplicationDbContext context ) => {
    var category = context.Categories.Where(c => c.Id == productRequest.CategoryId).First();
    var product = new Product{
        Code = productRequest.Code,
        Name = productRequest.Name,
        Description = productRequest.Description,
        Category = category
    };
    context.Products.Add(product);
    context.SaveChanges();
    return Results.Created($"/products/{product.Id}", product.Id);
});

/*get que é usado para pegar um dado especifico, ele retorna um do tipo produto*/
//api.app.com/users/{code}
app.MapGet("/products/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);
    return product;
});
/*metodo usado para fazer o put, mudar o dado de um determinado numero*/
app.MapPut("/products", (Product product) => {
    var productSaved = ProductRepository.GetBy(product.Code);
    productSaved.Name = product.Name;
});
/*Delete usado para deletar um dados*/
app.MapDelete("/products/{code}",([FromRoute] string code) => {
    var productSaved = ProductRepository.GetBy(code);
    ProductRepository.Remove(productSaved);
});


//Agora vai retornar o nome do banco de dados
app.MapGet("/configuration/database", (IConfiguration configuration) =>{
    return Results.Ok($"{configuration["database:connection"]} / {configuration["database:port"]}");
});


app.Run();

#region Classes do banco

public static class ProductRepository{ 

    public static List<Product> Products {get;set;}

    public static void Init(IConfiguration configuration){
        var products = configuration.GetSection("Products").Get<List<Product>>();
        Products = products;
    }

    public static void Add(Product product){
        if (Products == null)
            Products = new List<Product>();
        Products.Add(product);
    }

    public static Product GetBy(string code){
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product product){
            Products.Remove(product);
        }
}

public class Category{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Tag{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ProductId { get; set; }
}

public class Product{
    public int Id { get; set; }
    public string Code { get; set;}
    public string Name { get; set;}
    public string Description { get; set;}
    public Category Category { get; set;}
    public int CategoryId { get; set;}
    public List<Tag> Tags { get; set; }
}
/*criando conexão com o banco de dados*/
public class ApplicationDbContext : DbContext{

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    //aqui em baixo tem um construtor para o banco de dados
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){

    }
    
// essa função é usada para definir os criterios das variaveis deuma tabela
    protected override void OnModelCreating(ModelBuilder builder){
            builder.Entity<Product>()
            .Property(p => p.Description).HasMaxLength(500).IsRequired(false);
            builder.Entity<Product>()
            .Property(p => p.Name).HasMaxLength(120).IsRequired();
            builder.Entity<Product>()
            .Property(p => p.Code).HasMaxLength(20).IsRequired();
            builder.Entity<Category>()
            .ToTable("Categories");
        }
}
#endregion






