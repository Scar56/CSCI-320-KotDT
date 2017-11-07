using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using CSCI_320_KotDT.Models;
using ControllerDI.Interfaces;
using System.Diagnostics;
using System.Collections;

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
            string queryString = "SELECT title, release_year, running_time," + 
            "id FROM movies order by (select score from moviescore where movies.id = moviescore.id), title limit " +
                pageSize.ToString() + " offset " + temp.ToString();
            if (!String.IsNullOrEmpty(search))
            {
                queryString = "SELECT title, release_year, running_time, id FROM" + 
                "movies where lower(title) like lower('%" + search  + "%')" + 
                "order by (select score from moviescore where movies.id = moviescore.id), title limit " +
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

            query = "SELECT name, role FROM actors WHERE movie_id = '" + movie.MovieId + "'";
            //query += "ORDER BY (SELECT count(name) FROM actors";                                  //TODO: Order by popularity
            cmd = QueryHandler.query(query);
            List<Actor> cast = new List<Actor>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    String name = reader.GetString(0);
                    String role = reader.GetString(1);
                    cast.Add(new Actor(name, role));
                }

            }

            movie.Cast = cast;

            query = "select created_by, dislike_count, like_count, score, Review_Text, review_id from review where movie_id = " + id.ToString();
            cmd = QueryHandler.query(query);
            List<Review> reviews = new List<Review>();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Review r = new Review(movie.MovieId, QueryHandler);
                    r.CreatedBy = reader.GetString(0);
                    r.DislikeCount = reader.GetInt32(1);
                    r.LikeCount = reader.GetInt32(2);
                    r.Score = reader.GetFloat(3);
                    r.ReviewText = reader.GetString(4);
                    r.Id = reader.GetInt32(5);
                    reviews.Add(r);
                }
            }
            movie.Reviews = reviews;
            movie.NewReview = new Review(movie.MovieId, QueryHandler);

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReview(Review review)
        {
            if (ModelState.IsValid)
            {
                String query = "insert into review (review_id, Review_Text, score, created_by, movie_id) values ( nextval('review_review_id_seq'),'" +
                    review.ReviewText + "'," + review.Score.ToString() + ",'" + review.CreatedBy + "'," + review.MovieId + ")";

                return RedirectToAction("Details", new { id = review.MovieId });
            }
            Debug.Print("here");
            return RedirectToAction("Details", new { id = review.MovieId });
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Performed_in,Role")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            return View(actor);
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
