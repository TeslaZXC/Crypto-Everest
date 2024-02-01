<?php
$filepath = "data/_users.json";
$data = json_decode(file_get_contents('php://input'), true);

$users = file_exists($filepath) ? json_decode(file_get_contents($filepath), true) : [];

$foundUser = array_filter($users, function ($user) use ($data) {
    return $user['id'] == $data['id'];
});

if (count($foundUser) > 0) {
    $foundUserKey = array_keys($foundUser)[0];
    $users[$foundUserKey] = $data;
} else {
    $users[] = $data;
}

file_put_contents($filepath, json_encode($users, JSON_UNESCAPED_UNICODE));
?>
