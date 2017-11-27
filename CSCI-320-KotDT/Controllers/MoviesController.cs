using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using CSCI_320_KotDT.Models;
using ControllerDI.Interfaces;
using System.Diagnostics;
using System.Collections;
using Npgsql;

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
            NpgsqlCommand cmd;
            int temp = pageNum * pageSize;
            string queryString = "SELECT title, release_year, running_time," +
            "id FROM movies " + Movie.OrderingString() +" limit " +
                pageSize.ToString() + " offset " + temp.ToString();
            if (!String.IsNullOrEmpty(search))
            {
                search = search.Replace(' ', '%');
                queryString = "SELECT title, release_year, running_time, id FROM " +
                "movies where lower(title) like lower( @0 ) " + Movie.OrderingString() + " limit " +
                pageSize.ToString() + " offset " + temp.ToString();
                cmd = QueryHandler.query(queryString, "%" + search + "%");
            }
            else
            {
                cmd = QueryHandler.query(queryString);
            }
            Console.WriteLine(queryString);


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

            string query = " select title, release_year, running_time, score from movies left join moviescore using (id) where movies.id = " + id.ToString();

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
                    if (!reader.IsDBNull(3))
                        movie.Score = (float)reader.GetDouble(3);
                }
            }

            if (movie == null)
            {
                return HttpNotFound();
            }

            query = "select name from directors where movie_id = " + id.ToString();
            cmd = QueryHandler.query(query);
            using (var reader = cmd.ExecuteReader())
            {

                while(reader.Read())
                {
                    movie.Directors.Add(reader.GetString(0));
                }
            }

            query = "select genre from genres where id = " + id.ToString();
            cmd = QueryHandler.query(query);
            using (var reader = cmd.ExecuteReader())
            {
                int count = 0;
                while (reader.Read())
                {
                    movie.Genres[count] = reader.GetString(0);
                    count++;
                }
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

            query = "select created_by, dislike_count, like_count, score, review_text, review_id from review where movie_id = " + id.ToString()
                + " order by like_count, score desc";
            cmd = QueryHandler.query(query);
            string idquery = "";
            Dictionary<int, Review> reviews = new Dictionary<int, Review>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Review r = new Review();
                    r.CreatedBy = reader.GetString(0);
                    r.DislikeCount = reader.GetInt32(1);
                    r.LikeCount = reader.GetInt32(2);
                    r.Score = reader.GetFloat(3);
                    r.ReviewText = reader.GetString(4);
                    r.Id = reader.GetInt32(5);
                    r.Comments = new List<Comment>();
                    reviews.Add(r.Id, r);
                    idquery += " or parent_review_id = " + r.Id;
                }
            }
            if (idquery != "")
            {
                query = "select comment_text, time_posted, \"user\", parent_review_id from comment where " + idquery.Substring(3) + " order by time_posted";
                Review tempReview = new Review();
                cmd = QueryHandler.query(query);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Comment comment = new Comment();
                        comment.Text = reader.GetString(0);
                        comment.Posted = reader.GetDateTime(1);
                        comment.CreatedBy = reader.GetString(2);
                        reviews.TryGetValue(reader.GetInt32(3), out tempReview);
                        tempReview.Comments.Add(comment);
                    }
                }
            }
            var temp = System.Web.HttpContext.Current.Session["UserID"];
            if (temp != null){
                query = "select exists(select follower from follows_movie where follower = '" + ((User)temp).username +  "' AND movie_id = " + id + ")";
                    cmd = QueryHandler.query(query);
                    var reader = cmd.ExecuteReader();
                reader.Read();
                ViewBag.Following = reader.GetBoolean(0);
            }

            Review[] array = new Review[reviews.Values.Count];
            reviews.Values.CopyTo(array, 0);
            movie.Reviews = array;
            movie.NewReview = new Review(movie.MovieId);

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReview(Review review)
        {
            
            if (ModelState.IsValid)
            {
                String query = "insert into review (review_id, review_text, score, created_by, movie_id) values ( nextval('review_review_id_seq')," +
                    "@0, @1, @2, @3)";

                var cmd = QueryHandler.query(query, review.ReviewText, review.Score, review.CreatedBy, review.MovieId);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Details", new { id = review.MovieId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(Comment comment, string returnURL)
        {
            if (ModelState.IsValid)
            {
                String query = "insert into comment (comment_text, time_posted, \"user\", parent_review_id) values (" +
                    "@0, @1, @2, @3)";

                var cmd = QueryHandler.query(query, comment.Text, comment.Posted, comment.CreatedBy, comment.ParentID);
                cmd.ExecuteNonQuery();
            }
            return Redirect(returnURL);
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

        public ActionResult Like(int? id)
        {
            string queryString = "UPDATE  review set like_count = like_count+1 where review_id = " + id;
            Console.WriteLine(id);
            QueryHandler.nonQuery(queryString);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Dislike(int? id)
        {
            string queryString = "UPDATE  review set dislike_count = dislike_count+1 where review_id = " + id;
            QueryHandler.nonQuery(queryString);
            return RedirectToAction("Index", "Home");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }

        public ActionResult follow(int id) {
            string currUser = ((User)System.Web.HttpContext.Current.Session["UserID"]).username;
            string query = "INSERT INTO follows_movie " +
                               "(follower, movie_id) " +
                                    "VALUES('" + currUser + "', " + id + ")";
            QueryHandler.nonQuery(query);
            return RedirectToAction("Details", "Movies", new{id = id});
        }

        public ActionResult unfollow(int id) {
            string currUser = ((User)System.Web.HttpContext.Current.Session["UserID"]).username;
            string query = "DELETE FROM follows_movie " +
                "WHERE (follower = '" + currUser + "' AND  movie_id = " + id + ")";
            QueryHandler.nonQuery(query);
            return RedirectToAction("Details", "Movies", new{id = id});
        }

    }
}
