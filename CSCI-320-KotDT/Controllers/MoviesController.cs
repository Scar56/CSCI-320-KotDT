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

        public MoviesController(IQueryHandler IQuery)
        {
            QueryHandler = IQuery;
        }

        // GET: Movies
        public ActionResult Index(string search, int pageNum = 0, int pageSize = 25)
        {
            int temp = pageNum * pageSize;
            string queryString = "SELECT title, release_year, running_time, id FROM movies order by (select score from moviescore where moviescore.id = movies.id), title limit " +
                pageSize.ToString() + " offset " + temp.ToString();
            if (!String.IsNullOrEmpty(search))
            {
                queryString = "SELECT title, release_year, running_time, id FROM movies where lower(title) like lower('%" + search + "%') order by (select score from moviescore where moviescore.id = movies.id), title limit " +
                pageSize.ToString() + " offset " + temp.ToString();
            }
            Console.WriteLine(queryString);

            var cmd = QueryHandler.query(queryString);
            List<Movie> movies = new List<Movie>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.Write(reader.FieldCount);

                    String title = reader.GetString(0);
                    Int32 ry = reader.GetInt32(1);
                    Int32 rt = reader.GetInt32(2);
                    Int32 id = reader.GetInt32(3);
                    Movie movie = new Movie(title, ry, rt, id);
                    movies.Add(movie);

                }
            }
            return View(movies);

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            string query = "select title, release_year, running_time from movies where id = " + id.ToString();

            var cmd = QueryHandler.query(query);
            Movie movie = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    String title = reader.GetString(0);
                    Int32 ry = reader.GetInt32(1);
                    Int32 rt = reader.GetInt32(2);
                    movie = new Movie(title, ry, rt, (int)id);
                }

            }

            if (movie == null)
            {
                return HttpNotFound();
            }

            query = "select created_by, dislike_count, like_count, score, review_text from review where movie_id = " + id.ToString();
            cmd = QueryHandler.query(query);
            List<Review> reviews = new List<Review>();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Review r = new Review();
                    r.username = reader.GetString(0);
                    r.dislike_count = reader.GetInt32(1);
                    r.like_count = reader.GetInt32(2);
                    r.score = reader.GetFloat(3);
                    r.review_text = reader.GetString(4);
                    reviews.Add(r);
                }
            }
            movie.reviews = reviews;
            return View(movie);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }
    }
}
