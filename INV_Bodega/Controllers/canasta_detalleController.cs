using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using INV_Bodega.Models;

namespace INV_Bodega.Controllers
{
    public class canasta_detalleController : Controller
    {
        private tecnologiaEntities db = new tecnologiaEntities();

        // GET: canasta_detalle
        public ActionResult Index()
        {
            var canasta_detalle = db.canasta_detalle.Include(c => c.canasta);
            return View(canasta_detalle.ToList());
        }

        // GET: canasta_detalle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            canasta_detalle canasta_detalle = db.canasta_detalle.Find(id);
            if (canasta_detalle == null)
            {
                return HttpNotFound();
            }
            return View(canasta_detalle);
        }

        // GET: canasta_detalle/Create
        public ActionResult Create()
        {
            ViewBag.rowid_canasta = new SelectList(db.canasta, "rowid", "bloque");
            return View();
        }

        // POST: canasta_detalle/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "rowid,rowid_canasta,barras,cant")] canasta_detalle canasta_detalle)
        {
            if (ModelState.IsValid)
            {
                db.canasta_detalle.Add(canasta_detalle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.rowid_canasta = new SelectList(db.canasta, "rowid", "bloque", canasta_detalle.rowid_canasta);
            return View(canasta_detalle);
        }

        // GET: canasta_detalle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            canasta_detalle canasta_detalle = db.canasta_detalle.Find(id);
            if (canasta_detalle == null)
            {
                return HttpNotFound();
            }
            ViewBag.rowid_canasta = new SelectList(db.canasta, "rowid", "bloque", canasta_detalle.rowid_canasta);
            return View(canasta_detalle);
        }

        // POST: canasta_detalle/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "rowid,rowid_canasta,barras,cant")] canasta_detalle canasta_detalle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(canasta_detalle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.rowid_canasta = new SelectList(db.canasta, "rowid", "bloque", canasta_detalle.rowid_canasta);
            return View(canasta_detalle);
        }

        // GET: canasta_detalle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            canasta_detalle canasta_detalle = db.canasta_detalle.Find(id);
            if (canasta_detalle == null)
            {
                return HttpNotFound();
            }
            return View(canasta_detalle);
        }

        // POST: canasta_detalle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            canasta_detalle canasta_detalle = db.canasta_detalle.Find(id);
            db.canasta_detalle.Remove(canasta_detalle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
