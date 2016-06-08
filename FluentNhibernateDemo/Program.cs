using System;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate.Cfg;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;

namespace FluentNhibernateDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var msSqlConfiguration = MsSqlConfiguration.MsSql2008
                .ConnectionString("Server=.;Database=cbj;Trusted_Connection=True;");

            Configuration cfg = null;
            var buildSessionFactory = Fluently.Configure()
                .Database(msSqlConfiguration)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Book>())
                .ExposeConfiguration(c => cfg = c)
                .BuildSessionFactory();

            new SchemaExport(cfg).Execute(true, true, false);

            var currentSession = buildSessionFactory.OpenSession();

            var books = currentSession.Query<Book>()
                .ToList();
            currentSession.Save(new Book
            {
                Name = "book"
            });
            currentSession.Flush();

            Console.Write(books);
            Console.ReadKey();
        }
    }

    class Book
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
    }

    class BookMap : ClassMap<Book>
    {
        public BookMap()
        {
            Table("books");
            Id(_ => _.Id).Column("id");
            Map(_ => _.Name).Column("name");
        }
    }
}
