<?php
use biblioteca\Conexion;

require 'vendor/autoload.php';
    $action='getUsersInfo';
    $conexion= new Conexion();
    $conexion->setAction($action);
    $conexion->hacerConexion();
    $json=$conexion->respuestaGetUsersInfo;
    $code=$json->getCode();
    $message=$json->getMessage();
    $data=$json->getData();
    $status=$json->getStatus();
    $response=array("code"=>$code,"message"=>$message,"data"=>$data,"status"=>$status);
    $response=json_encode($response);
    echo $response;
?>