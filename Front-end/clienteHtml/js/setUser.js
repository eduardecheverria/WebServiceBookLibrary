function enviarDatos(event){
    event.preventDefault();
    const newUser=document.getElementById("newUser").value;
    const newPass=document.getElementById("newPass").value;
    const user=localStorage.getItem('user');
    const pass=localStorage.getItem('pass');
    document.getElementById("grid").style.display="flex";
    const json='{"user":"'+user+'","pass":"'+pass+'","newUser":"'+newUser+'","newPass":"'+newPass+'"}';
    $.ajax({
        type:"POST", // la variable type guarda el tipo de la peticion GET,POST,..
        url:"../respuestaGenerica.php", //url guarda la ruta hacia donde se hace la peticion
        data:{json:json,action:"setUser"}, // data recive un objeto con la informacion que se enviara al servidor
        success:function(datos){ //success es una funcion que se utiliza si el servidor retorna informacion
            console.log
            setTimeout(function(){
                document.getElementById("cargando").style.display="none";
                document.getElementById("respuesta").style.display="flex";
                const newjson= JSON.parse(datos);
                document.getElementById("code").innerHTML= newjson.code;
                document.getElementById("message").innerHTML= newjson.message;
                document.getElementById("data").innerHTML= newjson.data;
                document.getElementById("status").innerHTML= newjson.status;
            }, 1500);
             
         }
    })
}
function enviarDatosActualizar(event){
    event.preventDefault();
    const oldUser=document.getElementById("oldUser").value;
    const newUser=document.getElementById("newUser").value;
    const newPass=document.getElementById("newPass").value;
    const user=localStorage.getItem('user');
    const pass=localStorage.getItem('pass');
    document.getElementById("grid").style.display="flex";
    const json='{"user":"'+user+'","pass":"'+pass+'","oldUser":"'+oldUser+'","newUser":"'+newUser+'","newPass":"'+newPass+'"}';
    $.ajax({
        type:"POST", // la variable type guarda el tipo de la peticion GET,POST,..
        url:"../respuestaGenerica.php", //url guarda la ruta hacia donde se hace la peticion
        data:{json:json,action:"updateUser"}, // data recive un objeto con la informacion que se enviara al servidor
        success:function(datos){ //success es una funcion que se utiliza si el servidor retorna informacion
            
            setTimeout(function(){
                document.getElementById("cargando").style.display="none";
                document.getElementById("respuesta").style.display="flex";
                const newjson= JSON.parse(datos);
                document.getElementById("code").innerHTML= newjson.code;
                document.getElementById("message").innerHTML= newjson.message;
                document.getElementById("data").innerHTML= newjson.data;
                document.getElementById("status").innerHTML= newjson.status;
            }, 1500);
             
         }
    })
}
function setUserInfo(event){
    event.preventDefault();
    const correo=document.getElementById("correo").value;
    const nombre=document.getElementById("nombre").value;
    const rol=document.getElementById("rol").value;
    const telefono=document.getElementById("telefono").value;
    const searchedUser=document.getElementById("searchedUser").value;
    const user=localStorage.getItem('user');
    const pass=localStorage.getItem('pass');
    const userInfoJSON="{'correo': '"+correo+"','nombre': '"+nombre+"','rol': '"+rol+"','telefono': '"+telefono+"'}"
    document.getElementById("grid").style.display="flex";
    const json='{"user":"'+user+'","pass":"'+pass+'","searchedUser":"'+searchedUser+'","userInfoJSON":"'+userInfoJSON+'"}';
    $.ajax({
        type:"POST", // la variable type guarda el tipo de la peticion GET,POST,..
        url:"../respuestaGenerica.php", //url guarda la ruta hacia donde se hace la peticion
        data:{json:json,action:"setUserInfo"}, // data recive un objeto con la informacion que se enviara al servidor
        success:function(datos){ //success es una funcion que se utiliza si el servidor retorna informacion
            
            setTimeout(function(){
                document.getElementById("cargando").style.display="none";
                document.getElementById("respuesta").style.display="flex";
                const newjson= JSON.parse(datos);
                document.getElementById("code").innerHTML= newjson.code;
                document.getElementById("message").innerHTML= newjson.message;
                document.getElementById("data").innerHTML= newjson.data;
                document.getElementById("status").innerHTML= newjson.status;
            }, 1500);
             
         }
    })
}
function updateUserInfo(event){
    event.preventDefault();
    const correo=document.getElementById("correo").value;
    const nombre=document.getElementById("nombre").value;
    const rol=document.getElementById("rol").value;
    const telefono=document.getElementById("telefono").value;
    const searchedUser=document.getElementById("searchedUser").value;
    const user=localStorage.getItem('user');
    const pass=localStorage.getItem('pass');
    const userInfoJSON="{'correo': '"+correo+"','nombre': '"+nombre+"','rol': '"+rol+"','telefono': '"+telefono+"'}"
    document.getElementById("grid").style.display="flex";
    const json='{"user":"'+user+'","pass":"'+pass+'","searchedUser":"'+searchedUser+'","userInfoJSON":"'+userInfoJSON+'"}';
    $.ajax({
        type:"POST", // la variable type guarda el tipo de la peticion GET,POST,..
        url:"../respuestaGenerica.php", //url guarda la ruta hacia donde se hace la peticion
        data:{json:json,action:"updateUserInfo"}, // data recive un objeto con la informacion que se enviara al servidor
        success:function(datos){ //success es una funcion que se utiliza si el servidor retorna informacion
            
            setTimeout(function(){
                document.getElementById("cargando").style.display="none";
                document.getElementById("respuesta").style.display="flex";
                const newjson= JSON.parse(datos);
                document.getElementById("code").innerHTML= newjson.code;
                document.getElementById("message").innerHTML= newjson.message;
                document.getElementById("data").innerHTML= newjson.data;
                document.getElementById("status").innerHTML= newjson.status;
            }, 1500);
             
         }
    })
}
function getUsers(){
    
    $.ajax({
        type:"GET", // la variable type guarda el tipo de la peticion GET,POST,..
        url:"../respuestaGetUsers.php", //url guarda la ruta hacia donde se hace la peticion
        success:function(datos){ //success es una funcion que se utiliza si el servidor retorna informacion
           // console.log(datos);
            const newjson= JSON.parse(datos);
            console.log(newjson.data);
            newjson.data.forEach(element => {
                var node = document.createElement("LI");                 // Create a <li> node
                var textnode = document.createTextNode(element);         // Create a text node
                node.appendChild(textnode);                              // Append the text to <li>
                document.getElementById("usuarios").appendChild(node);
            });
            
         }
    })
}
function getUsersInfo(){
    
    $.ajax({
        type:"GET", // la variable type guarda el tipo de la peticion GET,POST,..
        url:"../respuestaGetUsersInfo.php", //url guarda la ruta hacia donde se hace la peticion
        success:function(datos){ //success es una funcion que se utiliza si el servidor retorna informacion
           // console.log(datos);
            const newjson= JSON.parse(datos);
            console.log(newjson.data);
            newjson.data.forEach(element => {
                var table = document.getElementById("usuarios");
                var row = table.insertRow(1);
                var cell1 = row.insertCell(0);
                var cell2 = row.insertCell(1);
                var cell3 = row.insertCell(2);
                var cell4 = row.insertCell(3);
                cell1.innerHTML = element.correo;
                cell2.innerHTML = element.nombre;
                cell3.innerHTML = element.rol;
                cell4.innerHTML = element.telefono;
            });
            
         }
    })
}
function cerrar_grid(){
    document.getElementById("grid").style.display="none";
    document.getElementById("cargando").style.display="flex";
    document.getElementById("respuesta").style.display="none";
}