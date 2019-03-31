using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CacheRedisBaloncesto.Models {
    public class Jugador {
        public int IdJugador { get; set; }
        public String Nombre { get; set; }
        public String Equipo { get; set; }
        public String Posicion { get; set; }
        public String Imagen { get; set; }
    }
}