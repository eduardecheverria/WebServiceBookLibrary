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
    public class getUsersInfoController : ApiController
    {
        // GET: api/getUsers
        [HttpPost]
        public async Task<RespuestaGetUsersInfo> GetAllUsers()
        {
            return await getUsersInfo();
        }


        public async Task<RespuestaCode> getRespuestaCode(int code, IFirebaseClient client)
        {
            FirebaseResponse response = await client.GetTaskAsync("respuestas/" + code.ToString());
            RespuestaCode resultado = response.ResultAs<RespuestaCode>();
            return resultado;

        }

        public async Task<RespuestaGetUsersInfo> getUsersInfo()

        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "LjT7xmFcKv1XppLCarGaGwr1PXI5cZp4QrlOf4OH",
                BasePath = "https://productosws-c9f47-default-rtdb.firebaseio.com/"
            };
            IFirebaseClient client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = await client.GetTaskAsync("usuariosInfo");
            Dictionary<string, UserInfo> result = response.ResultAs<Dictionary<string, UserInfo>>();
            int contador = 0;
            foreach (var usuario in result)
            {
                contador++;
            }
            UserInfo[] array_usuarios = new UserInfo[contador];
            string[] array_keys = new string[contador];
            int nuevoContador = 0;
            foreach (var usuario in result)
            {
                array_usuarios[nuevoContador] = usuario.Value;
                array_keys[nuevoContador] = usuario.Key;
                nuevoContador++;
            }
            RespuestaCode message = await getRespuestaCode(404, client);
            DateTime localDate = DateTime.Now;
            RespuestaGetUsersInfo respuesta = new RespuestaGetUsersInfo(404, message.mensaje, array_usuarios, "success");
            return respuesta;



        }
    }
}
