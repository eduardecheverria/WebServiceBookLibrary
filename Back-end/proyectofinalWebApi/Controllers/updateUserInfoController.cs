using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BibliotecaClases;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Text.RegularExpressions;
using NJsonSchema;
using Newtonsoft.Json;

namespace proyectofinalWebApi.Controllers
{
    public class updateUserInfoController : ApiController
    {
        // POST: api/updateUserInfo
        public async Task<Respuesta> Post(setUserInfoRequest request)
        {
            Respuesta response = await updateUserInfo(request.user, request.pass, request.searchedUser, request.userInfoJSON);
            return response;
        }

        public async Task<string> autenticar(string user, string pass, IFirebaseClient client)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync("usuarios/" + user + "");
                Usuario resultado = response.ResultAs<Usuario>();
                string result = "";
                if (resultado.pass != pass)
                {
                    result = "error contraseña no reconocida";
                }
                else
                {
                    result = "success";
                }

                return result;
            }
            catch (Exception e)
            {
                string result = "error usuario no identificado";
                return result;
            }

        }
        public bool stringValid(string user)
        {
            Regex r = new Regex("^[a-zA-Z0-9]*$");
            if (r.IsMatch(user))
            {
                if (user.Contains(" "))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> userExists(string user, IFirebaseClient client)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync("usuarios/" + user + "");
                Usuario resultado = response.ResultAs<Usuario>();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public async Task<UserInfo> getUserInfo(string user, IFirebaseClient client)
        {
            FirebaseResponse response = await client.GetTaskAsync("usuariosInfo/" + user);
            UserInfo resultado = response.ResultAs<UserInfo>();
            return resultado;
        }
        public async Task<bool> userInfoExists(string user, IFirebaseClient client)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync("usuariosInfo/" + user);
                UserInfo resultado = response.ResultAs<UserInfo>();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<RespuestaCode> getRespuestaCode(int code, IFirebaseClient client)
        {
            FirebaseResponse response = await client.GetTaskAsync("respuestas/" + code.ToString());
            RespuestaCode resultado = response.ResultAs<RespuestaCode>();
            return resultado;

        }
        public async Task<bool> validateJsonString(string json)
        {
            var schemaFromFile = await JsonSchema.FromFileAsync("C:/Users/52222/source/repos/protectofinal2/schema.json");
            var errors = schemaFromFile.Validate(json);
            int contadorErrores = 0;
            foreach (var error in errors)
            {
                contadorErrores++;
            }
            if (contadorErrores > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = Newtonsoft.Json.Linq.JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public async Task<Respuesta> updateUserInfo(string user, string pass, string searchedUser, string userInfoJson)

        {
            // firebase config
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "LjT7xmFcKv1XppLCarGaGwr1PXI5cZp4QrlOf4OH",
                BasePath = "https://productosws-c9f47-default-rtdb.firebaseio.com/"
            };
            IFirebaseClient client = new FireSharp.FirebaseClient(config);

            //autenticacion
            string autenticado = await autenticar(user, pass, client);
            if (autenticado == "success")
            {
                bool userInfoIsDefined = await userInfoExists(user, client);
                if (userInfoIsDefined)
                {
                    UserInfo userinfo = await getUserInfo(user, client);
                    if (userinfo.rol == "rh")
                    {
                        bool searchedUserExists = await userExists(searchedUser, client);
                        if (searchedUserExists)
                        {
                            bool searchedUserInfoIsDefined = await userInfoExists(searchedUser, client);
                            if (searchedUserInfoIsDefined == true)
                            {
                                bool jsonValido = IsValidJson(userInfoJson);
                                if (jsonValido == true)
                                {
                                    bool jsonBienFormado = await validateJsonString(userInfoJson);
                                    if (jsonBienFormado == true)
                                    {
                                        UserInfo searchedUserInfo = JsonConvert.DeserializeObject<UserInfo>(userInfoJson);
                                        FirebaseResponse responseSetNewUserInfo = await client.UpdateTaskAsync("usuariosInfo/" + searchedUser, searchedUserInfo);
                                        RespuestaCode message = await getRespuestaCode(403, client);
                                        DateTime localDate = DateTime.Now;
                                        Respuesta respuesta = new Respuesta(403, message.mensaje, localDate.ToString(), "success");
                                        return respuesta;
                                    }
                                    else
                                    {
                                        RespuestaCode message = await getRespuestaCode(304, client);
                                        Respuesta respuestaError = new Respuesta(304, message.mensaje, "", "error");
                                        return respuestaError;
                                    }
                                }
                                else
                                {
                                    RespuestaCode message = await getRespuestaCode(305, client);
                                    Respuesta respuestaError = new Respuesta(305, message.mensaje, "", "error");
                                    return respuestaError;
                                }
                            }
                            else
                            {
                                RespuestaCode message = await getRespuestaCode(507, client);
                                Respuesta respuestaError = new Respuesta(507, message.mensaje, "", "error");
                                return respuestaError;
                            }
                        }
                        else
                        {
                            RespuestaCode message = await getRespuestaCode(505, client);
                            Respuesta respuestaError = new Respuesta(505, message.mensaje, "", "error");
                            return respuestaError;
                        }
                    }
                    else
                    {
                        RespuestaCode message = await getRespuestaCode(504, client);
                        Respuesta respuestaError = new Respuesta(504, message.mensaje, "", "error");
                        return respuestaError;
                    }
                }
                else
                {
                    RespuestaCode message = await getRespuestaCode(504, client);
                    Respuesta respuestaError = new Respuesta(504, message.mensaje, "", "error");
                    return respuestaError;
                }
            }
            else
            {
                if (autenticado == "error contraseña no reconocida")
                {
                    RespuestaCode message = await getRespuestaCode(501, client);
                    Respuesta respuestaError = new Respuesta(501, message.mensaje, "", "error");
                    return respuestaError;
                }
                else if (autenticado == "error usuario no identificado")
                {
                    RespuestaCode message = await getRespuestaCode(500, client);
                    Respuesta respuestaError = new Respuesta(500, message.mensaje, "", "error");
                    return respuestaError;
                }
                else
                {
                    RespuestaCode message = await getRespuestaCode(999, client);
                    Respuesta respuestaError = new Respuesta(999, message.mensaje, "", "error");
                    return respuestaError;
                }

            }
        }
    }
}
