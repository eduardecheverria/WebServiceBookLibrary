<?php
use biblioteca\Conexion;

require 'vendor/autoload.php';
    $json=$_POST['json'];
    $action=$_POST['action'];
    $conexion= new Conexion();
    $conexion->setAction($action);
    $conexion->setJsonRequest($json);
    $conexion->hacerConexion();
    $json=$conexion->respuestaGenerica;
    $code=$json->getCode();
    $message=$json->getMessage();
    $data=$json->getData();
    $status=$json->getStatus();
    $response=array("code"=>$code,"message"=>$message,"data"=>$data,"status"=>$status);
    $response=json_encode($response);
    echo $response;
?>