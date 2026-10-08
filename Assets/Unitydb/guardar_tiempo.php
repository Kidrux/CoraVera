<?php

$conexion = new mysqli("localhost","root","","coravera");

$usuario = $_POST["usuario"];
$nivel = $_POST["nivel"];
$tiempo = $_POST["tiempo"];

$sql = "INSERT INTO tiempos(usuario,nivel,tiempo)
VALUES('$usuario','$nivel','$tiempo')";

if($conexion->query($sql))
{
    echo json_encode(["status"=>"success"]);
}
else
{
    echo json_encode(["status"=>"error"]);
}

?>