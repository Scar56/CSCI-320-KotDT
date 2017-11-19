using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
                return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            string query = "(SELECT movie_id, performed_in, role  FROM actors WHERE name LIKE '" + id.ToString() + "' ";
            query += "ORDER BY billing_position)";
            query += "union all select movie_id, Directed, 'Director' as role from directors where name like '" + id + "' ";

            var cmd = QueryHandler.query(query);
            Actor actor = new Actor(id);
                                            
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return HttpNotFound();
                }
                
                
                while (reader.Read())
                {
                    Int32 movieId = reader.GetInt32(0);
                    String title = reader.GetString(1);
                    String role = reader.GetString(2);
                    
                    actor.Roles.Add(new Tuple<string, Movie>(role, new Movie(title, movieId)));
                }

            }
            
            return View(actor);
        }

        // GET: Actors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Actors/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = null;
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Performed_in,Role")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
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
            Actor actor = null;
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Actor actor = null;
            return RedirectToAction("Index");
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
