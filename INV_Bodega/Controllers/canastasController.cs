using INV_Bodega.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace INV_Bodega.Controllers
{
    public class canastasController : Controller
    {
        private tecnologiaEntities db = new tecnologiaEntities();

        //-----------------------LOGIN-----------------------//
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Nombre, string Contraseña)
        {
            var user = db.Usuario.FirstOrDefault(e => e.NombreUsuario == Nombre && e.Contraseña == Contraseña);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Nombre, true);
                Session["usuario"] = Nombre;
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", new { message = "No reconocimos sus datos" });
            }

        }

        //---------------DETALLE CANASTA----------------//
        [Authorize]
        public ActionResult detalle_canasta(string barras)
        {
            
            List<codigoCLS> Listproductos = (from f in db.SKU
                                             where f.barras == barras
                                             select new codigoCLS
                                             {
                                                 Barras = f.barras,
                                                 Referencia = f.referencia,
                                                 Talla = f.talla,
                                                 Color = f.color
                                             }).ToList();
            //------------------------------ lista para retornar ----------------------------------------//
            ViewBag.rowid_canasta = new SelectList(db.canasta, "rowid", "bloque");

            var micookie = ControllerContext.HttpContext.Request.Cookies["TIKECTCOOKIE"];
            if (micookie != null)
            {
                ViewBag.miCookie = micookie.Value;
            }
            var micookies = ControllerContext.HttpContext.Request.Cookies["cantidad"];
            if (micookies != null)
            {
                ViewBag.miCookies = micookies.Value;
            }
            var micookiess = ControllerContext.HttpContext.Request.Cookies["barras"];
            if (micookiess != null)
            {
                string nombre = Session["usuario"].ToString();

                string rowid_v = Session["row_idcanasta"].ToString();

                int id_r = Convert.ToInt32(rowid_v);
                var miCookiess = micookiess.Value;
                string barra = miCookiess + "";
                List<codigoCLS> Listproductoss = (from f in db.Canastas
                                                  where f.usuario == nombre
                                                  where f.rowid_canasta == id_r
                                                  select new codigoCLS
                                                  {
                                                      Barras = f.barras,
                                                      Referencia = f.referencia,
                                                      Talla = f.talla,
                                                      Color = f.color,
                                                      Cant = f.Cant
                                                      
                                                  }).ToList();
                ViewBag.rowid_canasta = new SelectList(db.canasta, "rowid", "bloque");
                return View(Listproductoss);
            }
            return View(Listproductos);
        }

        [Authorize]
        [HttpPost]
        public ActionResult detalle_canasta([Bind(Include = "rowid,rowid_canasta,barras,cant")] canasta_detalle canasta_detalle, string barras)
        {
            string rowid_v = Session["row_idcanasta"].ToString();
            int id_r = Convert.ToInt32(rowid_v);
            int valor = 1;

            //-------------------- lista para las tablas ------------------------------//
            //List<codigoCLS> Detalles = (from j in db.Canastas
            //                            where j.rowid_canasta == id_r
            //                            select new codigoCLS
            //                            {
            //                                Referencia = j.referencia,
            //                                Talla = j.talla,
            //                                Color = j.color
            //                            }).ToList();
            ////------------------------- tabla -------------------------------------------------------- //
            List<codigoCLS> Listproductos = (from f in db.Canastas       
                                             where f.rowid_canasta == id_r
                                             where f.barras == barras
                                             select new codigoCLS
                                             {
                                                 Referencia = f.referencia,
                                                 Talla = f.talla,
                                                 Color = f.color,
                                                 Cant = f.Cant                                      
                                             }).ToList();

            //-------------------------Contador y guarda los datos--------------------//

            if (barras != null)
            {
                List<codigoCLS> liscod = null;

                liscod = (from f in db.canasta_detalle
                          where f.barras == canasta_detalle.barras
                          where f.rowid_canasta == id_r
                          select new codigoCLS
                          {
                              Barras = f.barras
                          }).ToList();

                int count = liscod.Count() + 1;
                string cants = count + "";
                ViewBag.total = count;

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, cants, DateTime.Now, DateTime.Now.AddMinutes(10), true, cants);
                String Encrypt = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookies = new HttpCookie("cantidad", cants);
                ControllerContext.HttpContext.Response.SetCookie(cookies);
                Response.Cookies.Add(cookies);
                ViewBag.cookies = cants;              

                FormsAuthenticationTicket ticketes = new FormsAuthenticationTicket(1, barras, DateTime.Now, DateTime.Now.AddMinutes(10), true, barras);
                String Encryptes = FormsAuthentication.Encrypt(ticketes);
                HttpCookie cookiess = new HttpCookie("barras", barras);
                ControllerContext.HttpContext.Response.SetCookie(cookiess);
                Response.Cookies.Add(cookiess);
                ViewBag.cookiess = barras;

                if (ModelState.IsValid)
                {
                    var barra_var = db.SKU.FirstOrDefault(e => e.barras == barras);

                    if (barra_var != null)
                    {
                        canasta_detalle.cant = valor;
                        canasta_detalle.rowid_canasta = id_r;
                        db.canasta_detalle.Add(canasta_detalle);
                        db.SaveChanges();
                        return RedirectToAction("detalle_canasta", "canastas");

                    }
                    else
                    {
                        TempData["testmsg"] = "<script>  swal('Error al escanear','Codigo no registrado','error')  </script>";
                        return RedirectToAction("detalle_canasta", "canastas");
                    }

                }
            }

            return View(Listproductos);

        }


        //-----------------------CANASTA-----------------------//
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.rowid_bodega = new SelectList((from s in db.Bodegas.ToList()
                                                   orderby s.id
                                                   select new
                                                   {
                                                       rowid = s.rowid,
                                                       fullname = s.id + " - " + s.bodega
                                                   }),
                "rowid",
                "fullname",
                null);

            //VALIDA LA COOKIE//
            //var micookie = ControllerContext.HttpContext.Request.Cookies["TIKECTCOOKIE"];
            //if (micookie != null)
            //{
            //    ViewBag.miCookie = micookie.Value;
            //}
            return View();
        }

        // GET: canastas
        [Authorize]
        public ActionResult Index()
        {
            //var micookie = ControllerContext.HttpContext.Request.Cookies["TIKECTCOOKIE"];
            string nombre = Session["usuario"].ToString();
           
           
            List<BodegaCLS> bodega = (from d in db.canasta
                                      join a in db.Bodegas on d.rowid_bodega equals a.rowid
                                      where d.usuario == nombre
                                      select new BodegaCLS
                                      {
                                          Pasillo = d.pasillo,
                                          Bloque = d.bloque,
                                          Canasta = d.canasta1,
                                          Usuario = d.usuario,
                                          Bodega = a.bodega
                                          //Rowid_canasta = d.rowid
                                          


                                      }).ToList();
            //-------------------------------------------------modal----------------------------------------------------------------------------------//
            if (nombre == db.canasta.FirstOrDefault().usuario)
                
            {
                string rowid_v = Session["row_idcanasta"].ToString();
                int id_r = Convert.ToInt32(rowid_v);
                List<codigoCLS> Listmodal = (from f in db.Canastas
                                                  where f.usuario == nombre
                                                  where f.rowid_canasta !=0
                                                  select new codigoCLS
                                                  {
                                                      Barras = f.barras,
                                                      Referencia = f.referencia,
                                                      Talla = f.talla,
                                                      Color = f.color,
                                                      Cant = f.Cant,
                                                      

                                                  }).ToList();
               
                return View(Listmodal);
            }
            return View(bodega);
        }

        // GET: canastas/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            canasta canasta = db.canasta.Find(id);
            if (canasta == null)
            {
                return HttpNotFound();
            }
            return View(canasta);
        }

        // POST: canastas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        [Authorize]
        public ActionResult Create([Bind(Include = "rowid,rowid_bodega,pasillo,bloque,canasta1,usuario,fecha,estado")] canasta canasta)
        {
            string usu = Session["usuario"].ToString();
            DateTime ahora = DateTime.Now;
            if (ModelState.IsValid)
            {
                canasta.usuario = usu;
                canasta.fecha = ahora;
                db.canasta.Add(canasta);
                db.SaveChanges();

                string idcanasta = canasta.rowid + "";
                Session["row_idcanasta"] = idcanasta;
                return RedirectToAction("detalle_canasta");
            }
            else
            {
                return RedirectToAction("Create");
            }

        }

        // GET: canastas/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            canasta canasta = db.canasta.Find(id);
            if (canasta == null)
            {
                return HttpNotFound();
            }
            return View(canasta);
        }

        //[HttpPost]
        //public ActionResult estado(int? id)
        //{
        //    string rowid_v = Session["row_idcanasta"].ToString();
        //    int id_r = Convert.ToInt32(rowid_v);
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var resultado = (from p in db.canasta
        //                     where p.rowid == id_r
        //                     select p).SingleOrDefault();

        //    resultado.estado = 1;
        //    db.SaveChanges();

        //    ViewBag.id = id;
        //    return RedirectToAction("Create");
        //}

        // POST: canastas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "rowid,rowid_bodega,pasillo,bloque,canasta1,usuario")] canasta canasta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(canasta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(canasta);
        }

        // GET: canastas/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            canasta canasta = db.canasta.Find(id);
            if (canasta == null)
            {
                return HttpNotFound();
            }
            return View(canasta);
        }

        // POST: canastas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            canasta canasta = db.canasta.Find(id);
            db.canasta.Remove(canasta);
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
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            Session.Remove("usuario");
            return RedirectToAction("Login");

        }
    }
}
