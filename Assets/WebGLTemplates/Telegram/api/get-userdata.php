<?php
$filepath = "data/_users.json";
$userid = $_GET['id'];

$data_array = file_exists($filepath) ? json_decode(file_get_contents($filepath), true) : [];

foreach ($data_array as $user) {
    if ($user['id'] == $userid) {
        echo json_encode($user);
        break;
    }
}
?>