using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CacheRedisBaloncesto.Helpers {
    public static class AccesoCacheRedis {
        private static String CadenaConexionRedis = ConfigurationManager.AppSettings["cacheazureredis"];

        private static Lazy<ConnectionMultiplexer> CrearConexion = new Lazy<ConnectionMultiplexer>(() => {
            return ConnectionMultiplexer.Connect(CadenaConexionRedis);
        });

        public static ConnectionMultiplexer Connection {
            get {
                return CrearConexion.Value;
            }
        }
    }
}