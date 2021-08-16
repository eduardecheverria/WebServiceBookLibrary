<?php
namespace biblioteca;
class UserInfo 
{
    var $correo='';
    var $nombre='';
    var $rol="";
    var $telefono="";

    function getCorreo(){
        return $this->correo;
    }
    function setCorreo($correo){
        $this->correo=$correo;
    }
    function getNombre(){
        return $this->nombre;
    }
    function setNombre($nombre){
        $this->nombre=$nombre;
    }
    function getRol(){
        return $this->rol;
    }
    function setRol($rol){
        $this->rol=$rol;
    }
    function getTelefono(){
        return $this->correo;
    }
    function setTelefono($telefono){
        $this->telefono=$telefono;
    }
    function __construct($correo,$nombre,$rol,$telefono){
        $this->correo=$correo;
        $this->nombre=$nombre;
        $this->rol=$rol;
        $this->telefono=$telefono;
    }
}

class RespuestaGetUsers{
    var $code=0;
    var $message="";
    var $data=array();
    var $status="";

    function getCode(){
        return $this->code;
    }
    function setCode($code){
        $this->code=$code;
    }
    function getMessage(){
        return $this->message;
    }
    function setMessage($message){
        $this->code=$message;
    }
    function getData(){
        return $this->data;
    }
    function setData($data){
        $this->data=$data;
    }
    function getStatus(){
        return $this->status;
    }
    function setStatus($status){
        $this->status=$status;
    }
    function __construct($code,$message,$data,$status){
        $this->code=$code;
        $this->message=$message;
        $this->data=$data;
        $this->status=$status;
    }
}
class RespuestaGetUsersInfo{
    var $code=0;
    var $message="";
    var $data=array();
    var $status="";

    function getCode(){
        return $this->code;
    }
    function setCode($code){
        $this->code=$code;
    }
    function getMessage(){
        return $this->message;
    }
    function setMessage($message){
        $this->code=$message;
    }
    function getData(){
        return $this->data;
    }
    function setData($data){
        $this->data=$data;
    }
    function getStatus(){
        return $this->status;
    }
    function setStatus($status){
        $this->status=$status;
    }
    function __construct($code,$message,$data,$status){
        $this->code=$code;
        $this->message=$message;
        $this->data=$data;
        $this->status=$status;
    }
}


class RespuestaGenerica{
    var $code=0;
    var $message="";
    var $data="";
    var $status="";

    function getCode(){
        return $this->code;
    }
    function setCode($code){
        $this->code=$code;
    }
    function getMessage(){
        return $this->message;
    }
    function setMessage($message){
        $this->code=$message;
    }
    function getData(){
        return $this->data;
    }
    function setData($data){
        $this->data=$data;
    }
    function getStatus(){
        return $this->status;
    }
    function setStatus($status){
        $this->status=$status;
    }
    function __construct($code,$message,$data,$status){
        $this->code=$code;
        $this->message=$message;
        $this->data=$data;
        $this->status=$status;
    }
}
class Conexion{
    var $action="";
    var $respuestaGenerica;
    var $respuestaGetUsers;
    var $respuestaGetUsersInfo;
    var $jsonRequest;

    function getAction(){
        return $this->action;
    }
    function setAction($action){
        $this->action=$action;
    }
    function getJsonRequest(){
        return $this->jsonRequest;
    }
    function setJsonRequest($jsonRequest){
        $this->jsonRequest=$jsonRequest;
    }
    function getRespuestaGenerica(){
        return $this->respuestaGenerica;
    }
    function setRespuestaGenerica($respuestaGenerica){
        $this->respuestaGenerica=$respuestaGenerica;
    }
    function getRespuestaGetUsers(){
        return $this->respuestaGetUsers;
    }
    function setRespuestaGetUsers($respuestaGetUsers){
        $this->respuestaGetUsers=$respuestaGetUsers;
    }
    function getRespuestaGetUsersInfo(){
        return $this->respuestaGetUsersInfo;
    }
    function setRespuestaGetUsersInfo($respuestaGetUsersInfo){
        $this->respuestaGetUsersInfo=$respuestaGetUsersInfo;
    }
    function hacerConexion(){
        if($this->getAction()==="getUsers"||$this->getAction()==="getUsersInfo"){
            $action=$this->getAction();
            $url = "http://localhost:54344/api/".$action;
            $ch = curl_init();
            curl_setopt($ch, CURLOPT_URL, $url); 
            curl_setopt($ch, CURLOPT_RETURNTRANSFER, true); 
            curl_setopt($ch, CURLOPT_HEADER, 0); 
            $response = curl_exec($ch); 
            curl_close($ch); 
            $response=json_decode($response,true);
            if($this->getAction()==="getUsers"){
                $respuestaGetUsers= new RespuestaGetUsers($response['code'],$response['message'],$response['data'],$response['status']);
                $this->setRespuestaGetUsers($respuestaGetUsers);
            }
            else{
                $respuestaGetUsersInfo= new RespuestaGetUsersInfo($response['code'],$response['message'],$response['data'],$response['status']);
                $this->setRespuestaGetUsersInfo($respuestaGetUsersInfo);
            }
        }
        else{
            $action=$this->getAction();
            $url = "http://localhost:54344/api/".$action;
            $ch = curl_init($url);
            $json=json_decode($this->getJsonRequest());
            $payload = json_encode($json);
            curl_setopt($ch, CURLOPT_POSTFIELDS, $payload);
            curl_setopt($ch, CURLOPT_HTTPHEADER, array('Content-Type:application/json'));
            curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
            $result = curl_exec($ch);
            curl_close($ch);
            $response=json_decode($result,true);
            $respuestaGenerica= new RespuestaGenerica($response['code'],$response['message'],$response['data'],$response['status']);
            $this->setRespuestaGenerica($respuestaGenerica);
        }
        
    }
}

?>