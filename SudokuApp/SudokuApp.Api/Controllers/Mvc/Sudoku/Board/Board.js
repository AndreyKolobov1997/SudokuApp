var boardId = null;
var fillBoardArray = null;
var fullBoardArray = null;
var urls = null;
var arrayOfInvalidValues = [];
var arrayOfValidValues = [];
var isAliveGame = null;

$(document).ready(Init);

function Init() {
    var scriptData = JSON.parse($(".script-data").html());

    urls = scriptData.urls;
    boardId = scriptData.boardId;
    fillBoardArray = scriptData.boardFilled.split('');
    fullBoardArray = scriptData.boardFull.split('');

    isAliveGame = scriptData.isAliveGame;

    var containerTable = document.getElementById('container-table');
    var table = document.createElement('table');

    var counterForInputId = 0;

    for (var i = 0; i < 9; i++) {
        var tr = document.createElement('tr');
        for (var j = 0; j < 9; j++) {
            var input = document.createElement('input');
            input.setAttribute('maxlength', '1');
            input.setAttribute('id', counterForInputId);

            if (!isAliveGame) {
                input.setAttribute('disabled', isAliveGame);
            }

            if (fullBoardArray[counterForInputId] === fillBoardArray[counterForInputId]) {
                input.value = fillBoardArray[counterForInputId];
                input.setAttribute('disabled', 'true');
            }

            $(input).change(function (event) {
                GetInputId(event);
            });

            $(input).change(function (event) {
                GetInCorrectValue(event);
            });

            $(input).keydown(function (event) {
                ValidationOfEnteredValues(event);
            });

            counterForInputId++;
            var td = document.createElement('td');
            td.appendChild(input);
            tr.appendChild(td);
        }

        table.appendChild(tr);
    }

    containerTable.appendChild(table);

    $('#js-board-save').click(function (event) {
        BoardSave(event);
    });

    $('#js-board-newgame').click(function (event) {
        CreateNewBoard(event);
    });
}

function GetInputId(event) {
    var inputId = event.target.id;
    var inputValue = event.target.value;

    fillBoardArray[inputId] = inputValue;
}

function GetInCorrectValue(event) {
    var inputId = event.target.id;
    var inputValue = event.target.value;

    document.getElementById(inputId).style.color = 'black';

    if (inputValue !== fullBoardArray[inputId]) {
        arrayOfInvalidValues[arrayOfInvalidValues.length] = inputId;
    }
    else {
        arrayOfValidValues[arrayOfValidValues.length] = inputId;
    }
}

function ValidationOfEnteredValues(event) {
    if (event.keyCode === 46 || event.keyCode === 8 || event.keyCode === 9 || event.keyCode === 27 ||
        (event.keyCode === 65 && event.ctrlKey === true) ||
        (event.keyCode >= 35 && event.keyCode <= 39)) {
    }
    else {
        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
            event.preventDefault();
        }
    }
}

function selfRandom(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

$(document).ready(function () {
    $("#js-board-chek").click(function () {
        for (var i = 0; i < arrayOfInvalidValues.length; i++) {
            document.getElementById(arrayOfInvalidValues[i]).style.color = 'red';
        }
        for (var j = 0; j < arrayOfValidValues.length; j++) {
            document.getElementById(arrayOfValidValues[j]).style.color = 'green';
            document.getElementById(arrayOfValidValues[j]).setAttribute('disabled', 'true');
        }
        arrayOfInvalidValues = [];

        if (JSON.stringify(fullBoardArray) === JSON.stringify(fillBoardArray)) {
            alert("Поздравляем, Вы прошли игру, можете начать новую нажав на кнопку 'Новая игра'.");
            $('#js-board-newgame').click(function (event) {
                CreateNewBoard(event);
            });
        }
    });
});

$(document).ready(function () {
    $("#js-board-help").click(function () {
        var arrayOfUserPromptValues = [];
        for (var i = 0; i < fullBoardArray.length; i++) {
            if (fullBoardArray[i] !== fillBoardArray[i]) {
                arrayOfUserPromptValues[arrayOfUserPromptValues.length] = fullBoardArray[i];
            }
            else arrayOfUserPromptValues[arrayOfUserPromptValues.length] = '';
        }

        do {
            var rand = selfRandom(0, 80);
        }
        while (arrayOfUserPromptValues[rand] === '');
        document.getElementById(rand).value = arrayOfUserPromptValues[rand];
        fillBoardArray[rand] = arrayOfUserPromptValues[rand];
        
        if (JSON.stringify(fullBoardArray) === JSON.stringify(fillBoardArray)) {
            document.getElementById("js-board-help").setAttribute('disabled', !isAliveGame);
        }
    });
});

function BoardSave() {
    $.ajax({
        url: urls.boardSave,
        type: 'POST',
        data: {
            boardId: boardId,
            newFilledBoard: fillBoardArray.toString().replace(new RegExp(",", 'g'), "")
        },
        success: function (result) {
            alert('Доска успешно сохранена!');
        },
        error: function (result) {
            alert('Произошла ошибка!');
        }
    });
}

function CreateNewBoard() {
    $.ajax({
        url: urls.createNewBoard,
        type: 'POST',
        data: {
            boardId: boardId
        },
        success: function (result) {
            alert('Доска создана!');
            location.reload();
        },
        error: function (result) {
            alert('Произошла ошибка!');
        }
    });
}
