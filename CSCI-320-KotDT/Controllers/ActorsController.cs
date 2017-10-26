using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CSCI_320_KotDT.Models;
using ControllerDI.Interfaces;

namespace CSCI_320_KotDT.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IQueryHandler QueryHandler;

        public ActorsController(IQueryHandler IQuery)
        {
            QueryHandler = IQuery;
        }

        // GET: Actors
        public ActionResult Index()
        {
            return View();
        }

        // GET: Actors/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string query = "select actors.role, movies.title, movies.id, actors.billing_position  from actors inner join movies on actors.performed_in = movies.title "
                            + "WHERE actors.name LIKE '" + id +"' ORDER BY actors.billing_position";
            List<string> movies = new List<string>();

            var cmd = QueryHandler.query(query);
            Actor actor = new Actor(id);
            List<string> movieTitles = new List<string>();
            List<string> roles = new List<string>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    String movie = reader.GetString(0);
                    String role = reader.GetString(1);
                    Int32 movieId = reader.GetInt32(2);

                    actor.Filmography.Add(new Tuple<Movie, string>(new Movie(movie, movieId), role));
                }

            }
            if (actor == null)
            {
                return HttpNotFound();
            }

            return View(actor);
        }

        // GET: Actors/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Actors/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = new Actor("Who Knows", "Who Cares");
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }


        // GET: Actors/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = new Actor("Who Knows", "Who Cares");
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }
    }
}
