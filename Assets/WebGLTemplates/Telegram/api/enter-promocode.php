<?php
$promoCodesFile = 'data/_promocodes.json';
$promoCodesArray = json_decode(file_get_contents($promoCodesFile), true);

$code = $_GET['code'];
$id = $_GET['id'];

$promoCodeIndex = -1;

if (!isset($code) || empty($code)) {
    echo 'Failed to load data';
    exit();
}

foreach ($promoCodesArray as $index => $promoCode) {
    if ($promoCode['code'] == $code) {
        $promoCodeIndex = $index;
        break;
    }
}

if ($promoCodeIndex == -1) {
    echo 'No promocode found';
    exit();
}

$promocodeСreator = $promoCodesArray[$promoCodeIndex]['created'];
if ($promocodeСreator == $id) {
    echo 'You cannot use your promocode';
    exit();
}

array_splice($promoCodesArray, $promoCodeIndex, 1);

$updatedPromoCodesJson = json_encode($promoCodesArray, JSON_UNESCAPED_UNICODE);
file_put_contents($promoCodesFile, $updatedPromoCodesJson);

$usersFile = 'data/_users.json';
$fileContent = file_get_contents($usersFile);
$users = json_decode($fileContent, true);

$userIndex = array_search($promocodeСreator, array_column($users, 'id'));

if ($userIndex !== false) {
    $users[$userIndex]['health'] += 5;
    $updatedFileContent = json_encode($users, JSON_UNESCAPED_UNICODE);
    file_put_contents($usersFile, $updatedFileContent);
}

echo 'Success';
?>