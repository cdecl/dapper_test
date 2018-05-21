using System;
using System.Data;
using System.Data.SqlClient;
using GLASS;
using Dapper;

namespace vsnetcore_con
{
    struct Movie
    {
        public string movieid;
        public string title;
    }

    struct Sysobject
    {
        public string id;
        public string name;
    }

    class Program
    {
        static string CONN = "Server=10.20.10.215;UID=dev;PWD=devmember;Database=Glass";
        static GLASS.AdoNetSql GetConn()
        {
            var ado = new GLASS.AdoNetSql();
            ado.Open(CONN);
            return ado;
        }

        static void ADOTest()
        {
            var ado = GetConn();
            var ds = ado.ExecuteDataSet(new SqlCommand("select Top 10 movieid, title from Glass..movies"));

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                var movieid = r["movieid"].ToString();
                var title = r["title"].ToString();

                Console.WriteLine($"{movieid} : {title}");
            }

        }

        static void DapperTest()
        {
            var conn = new SqlConnection(CONN);
            //var Movies = conn.Query<Movie>("select Top 10 movieid, title from Glass..movies ");
            var Sysobject = conn.Query<Sysobject>("select name, id from sys.sysobjects where type like @type + '%'", new { type = "U" });

            foreach (var m in Sysobject)
            {
                Console.WriteLine($"{m.id} : {m.name}");
            }
        }

        static void Main(string[] args)
        {
            try
            {
                //ADOTest();
                DapperTest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Program END");
        }
    }
}
    