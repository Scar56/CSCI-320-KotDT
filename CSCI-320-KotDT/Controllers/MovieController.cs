﻿using System;
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
        public ActionResult Index(string search)
        {
            string queryString = "SELECT title, release_year, running_time, id FROM movies order by title limit 30";
            if (!String.IsNullOrEmpty(search))
            {
                queryString = "SELECT title, release_year, running_time, id FROM movies where lower(title) like lower('%" + search + "%') order by title limit 30";
            }


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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

            string query_reviews = "select review_text, score, like_count, dislike_count, created_by from review where movie_id = " + id.ToString();
            cmd = QueryHandler.query(query_reviews);
            List<Review> reviews = new List<Review>();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Review review = new Review();
                    if (!reader.IsDBNull(0))
                        review.review_text = reader.GetString(0);
                    if (!reader.IsDBNull(1))
                        review.score = (float)reader.GetDecimal(1);
                    if (!reader.IsDBNull(2))
                        review.like_count = reader.GetInt32(2);
                    if (!reader.IsDBNull(3))
                        review.dislike_count = reader.GetInt32(3);
                    if (!reader.IsDBNull(4))
                        review.username = reader.GetString(4);
                    reviews.Add(review);
                }

            }
            return View(movie, reviews);
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
