using System.ComponentModel.Design;
using EstanteLivrosMongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        ConectionDB conectionDB = new("mongodb", "localhost", 27017);

        var conectionPath = conectionDB.PathConection(conectionDB);
        MongoClient mongoClient = new MongoClient(conectionPath);

        var baseDatas = mongoClient.GetDatabase("EstanteLivros");
        var collection = baseDatas.GetCollection<BsonDocument>("Livros");

        int op = 1;
        do
        {
            op = Menu();
            switch (op)
            {
                case 0:
                    Console.WriteLine("Saindo..");
                    op = 0;
                    break;
                case 1:
                    InsertBook(CreateBook(), collection);
                    break;
                case 2:
                    Book book = FindByName(collection);
                    if (book != null)
                    {
                        Console.WriteLine(book.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Não há registros com esse título");
                    }
                    break;
                case 3:
                    ListBooks(collection);
                    break;
                case 4:
                    UpdateBook(collection);
                    break;
                case 5:
                    DeleteBook(collection);
                    break;
                default:
                    Console.WriteLine("Valor inválido");
                    break;
            }
        } while (op != 0);
    }

    // Menu
    private static int Menu()
    {
        Console.WriteLine("\nEscolha a opção desejada" +
                          "\n[1] Inserir novo livro." +             //Create
                          "\n[2] Buscar livro por nome." +          //Read
                          "\n[3] Buscar todos os livros" +
                          "\n[4] Editar o nome do livro." +         //Update
                          "\n[5] Apagar registro de livro." +       //Delete                
                          "\n[0] Sair");
        int op = int.Parse(Console.ReadLine());
        return op;
    }

    // Inserir livro no banco
    private static void InsertBook(Book book, IMongoCollection<BsonDocument> collection)
    {
        var bookDoc = new BsonDocument
        {
            {"Title", book.Title},
            {"DatePublication", book.Dt_Publication},
            {"Publisher", book.Publisher},
            {"ISBN", book.Isbn },
            {"Authors", book.Author},
            {"Lendo", book.Lendo},
            {"Emprestado", book.Emprestado}
        };
        collection.InsertOne(bookDoc);
    }

    // Criar livro
    private static Book CreateBook()
    {
        Console.WriteLine("Título do livro:");
        string t = Console.ReadLine();

        Console.WriteLine("Data de publicação do livro:");
        string dt = Console.ReadLine();

        Console.WriteLine("Editora do livro:");
        string e = Console.ReadLine();

        Console.WriteLine("ISBN do livro:");
        int i = int.Parse(Console.ReadLine());

        Console.WriteLine("Autor do livro:");
        string a = Console.ReadLine();

        Book book = new Book();
        book.Title = t;
        book.Dt_Publication = t;
        book.Publisher = e;
        book.Isbn = i;
        book.Author = a;
        return book;
    }

    // Listar todos os livros do banco
    private static void ListBooks(IMongoCollection<BsonDocument> collection)
    {
        var booksLst = collection.Find(new BsonDocument()).ToList();
        booksLst.ForEach(x => Console.WriteLine(x.ToString()));
    }

    // Busca por nome do livro
    private static Book FindByName(IMongoCollection<BsonDocument> collection)
    {
        Book book = new Book();
        Console.WriteLine("\nQual o títuo a ser buscado?");
        string s = Console.ReadLine();

        var filtro = Builders<BsonDocument>.Filter.Regex("Title", s);
        var b = collection.Find(filtro).FirstOrDefault();
        var bo = BsonSerializer.Deserialize<Book>(b);

        if (bo != null)
        {
            return bo;
        }
        return null;
    }

    // Editar o nome do livro
    private static void UpdateBook(IMongoCollection<BsonDocument> collection)
    {
        Console.WriteLine("Qual o nome do livro a ser editado:");
        string s = Console.ReadLine();

        var filtro = Builders<BsonDocument>.Filter.Regex("Title", s);
        var b = collection.Find(filtro).FirstOrDefault();

        Console.WriteLine("Qual o novo título:");
        string s2 = Console.ReadLine();

        var update = Builders<BsonDocument>.Update.Set("Title", s2);

        collection.UpdateOne(filtro, update);
    }

    // Deletar Registro de livro
    private static void DeleteBook(IMongoCollection<BsonDocument> collection)
    {
        Console.WriteLine("Qual o nome do livro a ser deletado dos registros?");
        string s = Console.ReadLine();

        var filtro = Builders<BsonDocument>.Filter.Regex("Title", s);
        collection.FindOneAndDelete(filtro);
    }
}