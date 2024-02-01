<?php
$filepath = "data/_promocodes.json";
$data = json_decode(file_get_contents('php://input'), true);

$promocodes = file_exists($filepath) ? json_decode(file_get_contents($filepath), true) : [];

$foundCodes = array_filter($promocodes, function ($promocodes) use ($data) {
    return $promocodes['created'] == $data['created'];
});

if (count($foundCodes) > 0) {
    echo array_values($foundCodes)[0]['code'];
    exit();
}

$promocodes[] = $data;

file_put_contents($filepath, json_encode($promocodes, JSON_UNESCAPED_UNICODE));
?>
