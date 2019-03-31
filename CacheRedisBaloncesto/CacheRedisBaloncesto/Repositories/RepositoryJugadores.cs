using CacheRedisBaloncesto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace CacheRedisBaloncesto.Repositories {
    public class RepositoryJugadores {
        String uri;
        public RepositoryJugadores(String uri) {
            this.uri = uri;
        }
        public List<Jugador> GetJugadores() {
            XDocument docxml = XDocument.Load(this.uri);
            var consulta = from datos in docxml.Descendants("jugador")
                           select new Jugador{
                               IdJugador = int.Parse(datos.Element("idjugador").Value),
                               Nombre = datos.Element("nombre").Value,
                               Equipo = datos.Element("equipo").Value,
                               Posicion = datos.Element("posicion").Value,
                               Imagen = datos.Element("imagen").Value
                           };
            return consulta.ToList();
        }
        public Jugador BuscarJugador(int idjugador) {
            return this.GetJugadores().SingleOrDefault(z => z.IdJugador == idjugador);
        }
    }
}