<?php

use biblioteca\Conexion;

require 'vendor/autoload.php';

    $conexion= new Conexion();
    $conexion->setAction("getUsersInfo");
    $conexion->setJsonRequest('{
        "user":"pruebas1",
        "pass":"12345678a",
        "newUser":"pruebas8",
        "newPass":"12345678a"
    }');
    $conexion->hacerConexion();
    // API URL

?>