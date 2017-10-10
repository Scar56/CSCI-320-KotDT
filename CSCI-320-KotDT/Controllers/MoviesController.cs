using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using CSCI_320_KotDT.Models;
using ControllerDI.Interfaces;

namespace CSCI_320_KotDT.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IQueryHandler QueryHandler;

        private MovieDBContext db = new MovieDBContext();

        public MoviesController(IQueryHandler IQuery)
        {
            QueryHandler = IQuery;
        }

        // GET: Movies
        public ActionResult Index()
        {
            string queryString = "SELECT title, release_year, running_time FROM movies order by title limit 30";

           

            var cmd = QueryHandler.query(queryString);
            List<Movie> movies = new List<Movie>();
            using (var reader = cmd.ExecuteReader())
            {
                Console.Write(reader.GetName(0));
                Console.Write(reader.GetName(1));
                Console.Write(reader.GetName(2));
                while (reader.Read())
                {
                    Console.Write(reader.FieldCount);

                    String title = reader.GetString(0);
                    Int32 ry = reader.GetInt32(1);
                    Int32 rt = reader.GetInt32(2);
                    Movie movie = new Movie(title, ry, rt);
                    movies.Add(movie);

                }
            }
            return View(movies);

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
             //   db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
