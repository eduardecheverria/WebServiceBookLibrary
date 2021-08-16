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
    public class updateUserController : ApiController
    {


        // POST: api/updateUser
        [HttpPost]
        public async Task<Respuesta> Post(updateUserRequest request)
        {
            Respuesta response = await updateUser(request.user, request.pass,request.oldUser, request.newUser, request.newPass);
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
        public async Task<Respuesta> updateUser(string user, string pass, string oldUser, string newUser, string newPass)

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
                        if (stringValid(newUser))
                        {
                            if (stringValid(newPass))
                            {
                                bool newUserExists = await userExists(newUser, client);
                                if (newUserExists == false)
                                {
                                    bool oldUserExists = await userExists(oldUser, client);
                                    if (oldUserExists)
                                    {
                                        bool oldUserInfoIsDefined = await userInfoExists(oldUser, client);
                                        if (oldUserInfoIsDefined == true)
                                        {
                                            UserInfo oldUserinfo = await getUserInfo(oldUser, client);
                                            FirebaseResponse responseDeleteUserInfo = await client.DeleteTaskAsync("usuariosInfo/" + oldUser);
                                            FirebaseResponse responseSetNewUserInfo = await client.SetTaskAsync("usuariosInfo/" + newUser, oldUserinfo);
                                        }
                                        Usuario nuevoUsuario = new Usuario();
                                        nuevoUsuario.nomUsuario = newUser;
                                        nuevoUsuario.pass = newPass;
                                        FirebaseResponse responseDelete = await client.DeleteTaskAsync("usuarios/" + oldUser);
                                        SetResponse response = await client.SetTaskAsync("usuarios/" + newUser, nuevoUsuario);
                                        Usuario result = response.ResultAs<Usuario>();
                                        RespuestaCode message = await getRespuestaCode(401, client);
                                        DateTime localDate = DateTime.Now;
                                        Respuesta respuesta = new Respuesta(401, message.mensaje, localDate.ToString(), "success");
                                        return respuesta;
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
                                    RespuestaCode message = await getRespuestaCode(508, client);
                                    Respuesta respuestaError = new Respuesta(508, message.mensaje, "", "error");
                                    return respuestaError;
                                }
                            }
                            else
                            {
                                RespuestaCode message = await getRespuestaCode(502, client);
                                Respuesta respuestaError = new Respuesta(502, message.mensaje, "", "error");
                                return respuestaError;
                            }
                        }
                        else
                        {
                            RespuestaCode message = await getRespuestaCode(503, client);
                            Respuesta respuestaError = new Respuesta(503, message.mensaje, "", "error");
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
