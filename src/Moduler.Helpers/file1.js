function splitArrayIntoChunksOfLen(arr, len) {
    var chunks = [], i = 0, n = arr.length;
    while (i < n) {
        chunks.push(arr.slice(i, i += len));
    }
    return chunks;
}
var alphabet = ['a', 'b', 'c', 'd', 'e', 'f'];
var alphabetPairs = splitArrayIntoChunksOfLen(alphabet, 2); //split into chunks of two


function getCombination(result, arr, len) {
    if (result == null) {
        result = [];
    }
    var chunks = [], i = 0, n = arr.length;
    while (i < n) {
        chunks.push(arr.slice(i, i += len));
    }
    result.push(chunks);
    return result;
}
getCombination(null, alphabet, 3); //split into chunks of two


function getCombination(result, arr, len) {
    if (result == null) {
        result = [];
    }
    var group = [];
    for (var i = 0; i < len; i++) {
        group.push(arr.slice(i, i += len));
    }
    result.push(group);
    return result;
}
var alphabet = ['a', 'b', 'c', 'd', 'e', 'f'];
getCombination(null, alphabet, 2); //split into chunks of two

var alphabet = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'];
function getCombination(result, arr, len) {
    if (result == null) {
        result = [];
    }
    console.log("gelen istek:", result, arr, len);
    if (arr.length == 0) {
        return result;
    }
    var temp = arr.slice(); //shallow copy
    console.log("temp:", temp);
    var group = [];
    for (var i = 0; i < len; i++) {
        group.push(temp[0]);
        temp.shift();
    }
    result.push(group);
    //if (arr.length>0) {
    //    result.concat(getCombination(result, arr.shift(), len));
    //}
    return result;
}
getCombination(null, alphabet, 3);


var alphabet = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'];
function recursive(arr) {
    console.log(arr);
    arr.shift();
    //console.log(arr);
    if (arr.length > 0) {
        recursive(arr);
    }
}
recursive(alphabet);


var alphabet = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'];
function recursive(arr) {
    console.log(arr);
    arr.shift();
    //console.log(arr);
    if (arr.length > 0) {
        recursive(arr);
    }
}
recursive(alphabet);