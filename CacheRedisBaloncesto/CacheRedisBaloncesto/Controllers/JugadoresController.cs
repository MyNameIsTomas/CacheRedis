using CacheRedisBaloncesto.Models;
using CacheRedisBaloncesto.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CacheRedisBaloncesto.Controllers {
    public class JugadoresController : Controller {
        RepositoryRedis reporedis;
        public JugadoresController() {
            this.reporedis = new RepositoryRedis();
        }
        public ActionResult AlmacenarFavoritos(int idjugador) {
            //Buscamos si ya tenemos el producto en redis
            if (this.reporedis.BuscarJugador(idjugador) != null) {
                //El producto existe y nos lo llevamos a detalles con el mensaje
                TempData["MENSAJE"] = "El jugador ya existen en la Cache";
                return RedirectToAction("DetallesJugador", new { idjugador = idjugador });
            } else {
                //Recuperamos el producto del Xml
                Jugador jugador = this.GetRepo().BuscarJugador(idjugador);
                //Almacenamos en Redis Cache
                this.reporedis.AlmacenarJugador(jugador);
                return RedirectToAction("Index");
            }
        }
        public ActionResult EliminarFavoritos(int idjugador){
            this.reporedis.EliminarJugadorCache(idjugador);
            return RedirectToAction("JugadoresRedis");
        }
        public RepositoryJugadores GetRepo() {
            Uri uri = HttpContext.Request.Url;
            String url = uri.Scheme + "://" + uri.Authority + "/Documentos/jugadores.xml";
            return new RepositoryJugadores(url);
        }
        // GET: Jugadores
        public ActionResult Index() {
            return View(this.GetRepo().GetJugadores());
        }
        public ActionResult DetallesJugador(int idjugador) {
            ViewBag.Mensaje = TempData["MENSAJE"];
            return View(this.GetRepo().BuscarJugador(idjugador));
        }
        public ActionResult JugadoresRedis() {
            return View(this.reporedis.GetJugadoresCacheRedis());
        }
    }
}