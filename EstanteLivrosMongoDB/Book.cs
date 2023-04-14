using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EstanteLivrosMongoDB
{
    [BsonIgnoreExtraElements]
    internal class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("DatePublication")]
        public string Dt_Publication { get; set; }

        [BsonElement("Publisher")]
        public string Publisher { get; set; }

        [BsonElement("ISBN")]
        public int Isbn { get; set; }

        [BsonElement("Author")]
        public string Author { get; set; }

        [BsonElement("Lendo")]
        public bool Lendo { get; set; }

        [BsonElement("Emprestado")]
        public bool Emprestado { get; set; }

        public Book()
        {
            Lendo = false;
            Emprestado = false;
        }

        public override string ToString()
        {
            return $"\n°°° DADOS DO LIVRO ººº" +
                   $"\n     Título do Livro: {Title}" +
                   $"\n     Data de Publicação: {Dt_Publication}" +
                   $"\n     Editora: {Publisher}" +
                   $"\n     Autor: {Author}" +
                   $"\n     ISBN: {Isbn}" +
                   $"\n     Em leitura: {Lendo}" +
                   $"\n     Emprestado: {Emprestado}";
        }
    }
}
