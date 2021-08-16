using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Text.RegularExpressions;
using NJsonSchema;
using Newtonsoft.Json;
using BibliotecaClases;
using System.Threading.Tasks;

namespace proyectofinalWebApi.Controllers
{
    public class getUsersController : ApiController
    {
        // GET: api/getUsers
        [HttpPost]
        public async Task<RespuestaGetUsers> GetAllUsers()
        {
            return await getUsers();
        }

        
        public async Task<RespuestaCode> getRespuestaCode(int code, IFirebaseClient client)
        {
            FirebaseResponse response = await client.GetTaskAsync("respuestas/" + code.ToString());
            RespuestaCode resultado = response.ResultAs<RespuestaCode>();
            return resultado;

        }

        public async Task<RespuestaGetUsers> getUsers()

        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "LjT7xmFcKv1XppLCarGaGwr1PXI5cZp4QrlOf4OH",
                BasePath = "https://productosws-c9f47-default-rtdb.firebaseio.com/"
            };
            IFirebaseClient client = new FireSharp.FirebaseClient(config);

            //autenticacion

            FirebaseResponse response = await client.GetTaskAsync("usuarios");
            Dictionary<string, Usuario> result = response.ResultAs<Dictionary<string, Usuario>>();
            int contador = 0;
            foreach (var usuario in result)
            {
                contador++;
            }
            string[] array = new string[contador];
            int nuevoContador = 0;
            foreach (var usuario in result)
            {
                array[nuevoContador] = usuario.Value.nomUsuario;
                nuevoContador++;
            }
            RespuestaCode message = await getRespuestaCode(404, client);
            DateTime localDate = DateTime.Now;
            Usuario prueba = new Usuario();

            RespuestaGetUsers respuesta = new RespuestaGetUsers(404, message.mensaje, array, "success");
            return respuesta;



        }
    }
}
