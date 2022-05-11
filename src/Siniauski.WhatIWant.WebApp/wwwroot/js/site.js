$(document).on('click', 'button[id^="FriendButton"]', function (e) {
    var button = $(e.target);
    var userId = this.id.substring(13);
    var url;
    var isSendButton = button.hasClass("btn-friend-action-send");
    var isAcceptButton = button.hasClass("btn-friend-action-accept");
    var isCancelButton = button.hasClass("btn-friend-action-cancel");
    var isDeleteButton = button.hasClass("btn-friend-action-delete");
    var classToRemove;
    var classToAdd;
    var nextText;
    var successText;
    var errorText;

    if (isSendButton || isAcceptButton) {
        url = "Friend/Create";
        if (isSendButton) {
            classToRemove = "btn-friend-action-send";
            classToAdd = "btn-friend-action-cancel";
            nextText = "Отменить приглашение";
            successText = "Приглашение отправлено!";
            errorText = "Ошибка при отправке приглашения!";
        }
        else {
            classToRemove = "btn-friend-action-accept";
            classToAdd = "btn-friend-action-delete";
            nextText = "Удалить из друзей";
            successText = "Приглашение принято!";
            errorText = "Ошибка при принятии приглашения!";
        }
    }
    if (isCancelButton || isDeleteButton) {
        url = "Friend/Delete";
        if (isCancelButton) {
            classToRemove = "btn-friend-action-cancel";
            classToAdd = "btn-friend-action-send";
            nextText = "Добавить в друзья";
            successText = "Приглашение отменено!";
            errorText = "Ошибка при отмене приглашения!";
        }
        else {
            classToRemove = "btn-friend-action-delete";
            classToAdd = "btn-friend-action-accept";
            nextText = "Принять приглашение";
            successText = "Пользователь удален из друзей!";
            errorText = "Ошибка при удалении пользователя из друзей!";
        }
    }

    var employee = new Object();
    employee.FirstUserId = "FirstUserId";
    employee.SecondUserId = userId;

    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(employee),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            if (JSON.parse(JSON.stringify(response)).status == 0) {
                button.removeClass(classToRemove);
                button.addClass(classToAdd);
                button.text(nextText);
                $("#modalMessageText").text(successText);
                $("#modalMessage").modal('show');
            }
            else {
                $("#modalErrorMessageText").text(errorText);
                $("#modalErrorMessage").modal('show');
            }
        },
        error: function () {
            $("#modalErrorMessageText").text(errorText);
            $("#modalErrorMessage").modal('show');
        },
    });
});

$(document).on('click', 'button[id$="FriendsButton"]', function (e) {
    var button = $(e.target);
    var url = "Friend";
    console.log(this.id);
    var title = "Мои друзья";
    switch (this.id) {
        case "SearchFriendsButton":
            url += "/Search";
            title = "Поиск друзей";
            break;
        case "OutgoingInviteFriendsButton":
            url += "/Outgoing";
            title = "Исходящие заявки";
            break;
        case "IncomingInviteFriendsButton":
            url += "/Incoming";
            title = "Входящие заявки";
            break;
    }
    $.ajax({
        type: "GET",
        url: url,
        dataType: "html",
        success: function (result) {
            $("#BodyList").html(result);
            $("#Title").text(title);
        }
    });
});

$(document).on('click', 'button[id$="WishesButton"]', function (e) {
    var button = $(e.target);
    var url = "Wishes";
    console.log(this.id);
    switch (this.id) {
        case "MyWishesButton":
            url += "/My";
            break;
        case "FriendsWishesButton":
            url += "/Friends";
            break;
        case "UnfulfilledWishesButton":
            url += "/Unfulfilled";
            break;
    }
    $.ajax({
        type: "GET",
        url: url,
        dataType: "html",
        success: function (result) {
            $("#BodyList").html(result);
        }
    });
});

$(document).on('click', 'button[id="AddNewWish"]', function (e) {
    var button = $(e.target);
    var url = "Wishes/Create";
    console.log("Hello!");
    $.ajax({
        type: "GET",
        url: url,
        dataType: "html",
        success: function (result) {
            $("#BodyList").html(result);
        }
    });
});

$(document).on('click', 'button[id="CreateWish"]', function (e) {
    var button = $(e.target);
    var url = "Wishes/Create";
    var employee = new Object();
    employee.Name = $("#WishName").val().trim();
    employee.Description = $("#WishDescription").val().trim();
    employee.UserId = "";
    employee.IsDone = false;

    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(employee),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            if (JSON.parse(JSON.stringify(response)).status == 0) {
                $("#modalMessageText").text(JSON.parse(JSON.stringify(response)).message);
                $("#modalMessage").modal('show');
            }
            else {
                $("#modalErrorMessageText").text(JSON.parse(JSON.stringify(response)).message);
                $("#modalErrorMessage").modal('show');
            }
        },
        error: function () {
            $("#modalErrorMessageText").text("Ошибка при создании желания!");
            $("#modalErrorMessage").modal('show');
        },
    });
});

$(document).on('click', 'button[id^="MyWishDelete"]', function (e) {
    var wishId = this.id.substring(13);
    var url = "Wishes/Delete";
    var employee = new Object();
    employee.WishId = wishId;
    var button = $(e.target);
    var errorText = "Ошибка при удалении желания!";

    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(employee),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            if (JSON.parse(JSON.stringify(response)).status == 0) {
                $("#modalMessageText").text(JSON.parse(JSON.stringify(response)).message);
                $("#modalMessage").modal('show');
                button.text("Удалено");
                button.removeClass('btn-danger');
                button.addClass('btn-secondary');
                button.prop('disabled', true);
                $("#MyWishSetAsDone_" + wishId).hide();
                $("#MyWishDone_" + wishId).hide();
            }
            else {
                $("#modalErrorMessageText").text(JSON.parse(JSON.stringify(response)).message);
                $("#modalErrorMessage").modal('show');
            }
        },
        error: function () {
            $("#modalErrorMessageText").text(errorText);
            $("#modalErrorMessage").modal('show');
        },
    });
});

$(document).on('click', 'button[id^="FriendsWishBlock"]', function (e) {
    var button = $(e.target);
    var wishId = this.id.substring(17);
    var url;
    var isBlockButton = button.hasClass("btn-wish-action-block");
    var isUnblockButton = button.hasClass("btn-wish-action-unblock");
    var classToRemove;
    var classToAdd;
    var nextText;
    var errorText = "Ошибка выполнения запроса!"

    if (isBlockButton) {
        url = "Wishes/Block";
        classToRemove = "btn-wish-action-block";
        classToAdd = "btn-wish-action-unblock";
        nextText = "Отказаться от исполнения";
    }
    if (isUnblockButton) {
        url = "Wishes/Unblock";
        classToRemove = "btn-wish-action-unblock";
        classToAdd = "btn-wish-action-block";
        nextText = "Исполнить";
    }

    var employee = new Object();
    employee.UserId = "UserId";
    employee.WishId = wishId;

    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(employee),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            if (JSON.parse(JSON.stringify(response)).status == 0) {
                button.removeClass(classToRemove);
                button.addClass(classToAdd);
                button.text(nextText);
                $("#modalMessageText").text(JSON.parse(JSON.stringify(response)).message);
                $("#modalMessage").modal('show');
            }
            else {
                $("#modalErrorMessageText").text(JSON.parse(JSON.stringify(response)).message);
                $("#modalErrorMessage").modal('show');
            }
        },
        error: function () {
            $("#modalErrorMessageText").text(errorText);
            $("#modalErrorMessage").modal('show');
        },
    });
});

$(document).on('click', 'button[id^="MyWishSetAsDone"]', function (e) {
    var wishId = this.id.substring(16);
    var url = "Wishes/Done";
    var employee = new Object();
    employee.UserId = "UserId"
    employee.WishId = wishId;
    var button = $(e.target);

    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(employee),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            if (JSON.parse(JSON.stringify(response)).status == 0) {
                button.text("Исполнено");
                button.removeClass('btn-success');
                button.addClass('btn-secondary');
                button.prop('disabled', true);
                $("#modalMessageText").text(JSON.parse(JSON.stringify(response)).message);
                $("#modalMessage").modal('show');
            }
            $("#modalErrorMessageText").text(JSON.parse(JSON.stringify(response)).message);
            $("#modalErrorMessage").modal('show');
        },
        error: function () {
            $("#modalErrorMessageText").text(JSON.parse(JSON.stringify(response)).message);
            $("#modalErrorMessage").modal('show');
        },
    });
});

function makeid(length) {
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for (var i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() *
            charactersLength));
    }
    return result;
}

$(document).on('click', 'label[id="BrowseImage"]', function (e) {
    var fileDialog = $('<input type="file" accept=".jpg, .jpeg, .png">');
    fileDialog.click();
    fileDialog.on("change", onFileSelected);
    return false;
});

var onFileSelected = function (e) {
    var file = $(this)[0].files[0];
    var ext = file.name.substr(file.name.lastIndexOf('.') + 1);
    var randName = makeid(40) + "." + ext;
    var data = new FormData();
    data.append(randName, file);
    $.ajax({
        type: "POST",
        url: '/Account/UploadAvatar',
        contentType: false,
        processData: false,
        data: data,
        success: function (response) {
            if (JSON.parse(JSON.stringify(response)).status == 0) {
                $("#inputAvatar").val(file.name);
                $("#avatarServerName").val(randName);
            }
            else {
                $("#inputAvatar").val(JSON.parse(JSON.stringify(response)).message);
            }
        },
        error: function () {
            $("#inputAvatar").val("Ошибка загрузки!");
        },
    });

};

$(document).ajaxError(function (event, xhr, settings, error) {
    if (xhr.status == 401) {
        window.location = '/Account/Login';
    }
});