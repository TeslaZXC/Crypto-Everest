<?php
$filepath = "data/_users.json";
$json_data = file_get_contents($filepath);
$users = json_decode($json_data, true);

usort($users, function($a, $b) {
    return $b['score'] - $a['score'];
});

$top_10 = array_slice($users, 0, 10);

echo json_encode($top_10, JSON_UNESCAPED_UNICODE);
?>