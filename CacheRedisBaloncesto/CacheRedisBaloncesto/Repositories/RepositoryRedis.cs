using CacheRedisBaloncesto.Helpers;
using CacheRedisBaloncesto.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CacheRedisBaloncesto.Repositories {
    public class RepositoryRedis {
        IDatabase bbdd;
        public String keyredis;
        public RepositoryRedis() {
            //Creamos Bbdd en Redis
            this.bbdd = AccesoCacheRedis.Connection.GetDatabase();
            this.keyredis = "Mvc@Jugador@";
        }
        public List<Jugador> GetJugadoresCacheRedis() {
            List<Jugador> listajugadores = new List<Jugador>();
            IDatabase cacheredis = AccesoCacheRedis.Connection.GetDatabase();
            //RECUPERAMOS EL PUNTO DE ENTRADA PARA NUESTRA APLICACION EN EL CACHE REDIS
            var endpoints = AccesoCacheRedis.Connection.GetEndPoints();
            //RECUPERAMOS EL SERVIDOR CON EL PRIMER ENDPOINT Y UNICO DE NUESTRA APP
            IServer server = AccesoCacheRedis.Connection.GetServer(endpoints.First());
            //RECUPERAMOS TODAS LAS KEYS DE NUESTRO CACHE REDIS
            IEnumerable<RedisKey> claves = server.Keys();
            if (claves.Count() != 0) {
                //TENEMOS CLAVES ALMACENADAS Y RECUPERAMOS CADA UNO DE LOS OBJETOS POR LAS CLAVES
                foreach (RedisKey key in claves) {
                    String jugadorcache = cacheredis.StringGet(key);
                    //DESERIALIZAMOS EL OBJETO JSON A PRODUCTO
                    Jugador jugador = JsonConvert.DeserializeObject<Jugador>(jugadorcache);
                    listajugadores.Add(jugador);
                }
                return listajugadores;
            } else {
                return null;
            }
        }

        public void AlmacenarJugador(Jugador jugador) {
            String json = JsonConvert.SerializeObject(jugador);
            String idjug = this.keyredis + jugador.IdJugador;
            this.bbdd.StringSet(idjug, json);
        }
        public Jugador BuscarJugador(int idjugador) {
            String idjug = this.keyredis + idjugador;
            RedisValue value = this.bbdd.StringGet(idjug);
            if (value.HasValue) {
                String json = this.bbdd.StringGet(idjug);
                Jugador producto = JsonConvert.DeserializeObject<Jugador>(json);
                return producto;
            } else {
                return null;
            }
        }
        public void EliminarJugadorCache(int idjugador) {
            String idjug = this.keyredis + idjugador;
            IDatabase cacheredis = AccesoCacheRedis.Connection.GetDatabase();
            cacheredis.KeyDelete(idjug);
        }
    }
}