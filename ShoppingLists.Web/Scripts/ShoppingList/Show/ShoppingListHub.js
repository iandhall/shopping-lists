/*global ListItemModel */
"use strict";

function getShoppingListHub(urls, serviceExceptions) {

    var shoppingListHub = $.connection.shoppingListHub, shoppingListModel;

    shoppingListHub.client.shoppingListUpdated = function (jsShoppingListModel) {
        ko.mapping.fromJS(jsShoppingListModel, {}, shoppingListModel);
    };

    shoppingListHub.client.listItemDeleted = function (listItemId) {
        var existingListItem = shoppingListModel.findListItem(listItemId);
        shoppingListModel.ListItems.splice(shoppingListModel.ListItems.indexOf(existingListItem), 1);
    };

    shoppingListHub.client.listItemUpdated = function (jsListItemModel) {
        var existingListItem = shoppingListModel.findListItem(jsListItemModel.Id);
        existingListItem.fromJS(jsListItemModel); // Update the existing ListItem with the new model sent by the server.
    };

    shoppingListHub.client.listItemCreated = function (jsListItemModel) {
        var listItemModel = new ListItemModel(jsListItemModel);
        shoppingListModel.ListItems.push(listItemModel);
    };

    shoppingListHub.client.serviceException = function (error) {
        console.log("Service error: " + error);
        switch (error) {
        case serviceExceptions.ListItemAlreadyExistsException:
            bootbox.alert("Item has already been added to the shopping list.");
            break;
        case serviceExceptions.ShoppingListTitleDuplicateException:
            bootbox.alert("A shopping list with this name already exists.");
            break;
        case serviceExceptions.PermissionNotFoundException:
            bootbox.alert("You don't have permission to do that.");
            break;
        case serviceExceptions.ScriptInjectionException:
            bootbox.alert("Please ensure there aren't any HTML tags in your input.");
            break;
        default:
            bootbox.alert("A server error occurred.");
            break;
        }
    };

    shoppingListHub.client.allListItemsUnpicked = function () {
        ko.utils.arrayForEach(shoppingListModel.ListItems(), function (listItem) {
            listItem.StatusId(1);
        });
    };

    shoppingListHub.client.viewAccessRemoved = function () {
        console.log("viewAccessRemoved");
        bootbox.alert("You no longer have permission to view " + shoppingListModel.Title() + ".", function () {
            window.location.href = urls.index;
        });
    };

    shoppingListHub.client.viewerAdded = function (username) {
        console.log("viewerAdded " + username);
        if (username !== shoppingListModel.CurrentUsername()) {
            shoppingListModel.viewers.push(username);
            shoppingListModel.viewerAlert(username + " is viewing.");
            $(".alert").delay(100).addClass("in").fadeOut(4000).css({ display: "block" });
        }
    };

    shoppingListHub.client.viewerRemoved = function (username) {
        console.log("viewerRemoved " + username);
        if (username !== shoppingListModel.CurrentUsername()) {
            shoppingListModel.viewers.remove(username);
            shoppingListModel.viewerAlert(username + " is no longer viewing.");
            $(".alert").delay(100).addClass("in").fadeOut(4000).css({ display: "block" });
        }
    };

    shoppingListHub.client.allCurrentViewersReceived = function (currentViewers) {
        shoppingListModel.viewers().length = 0;
        ko.utils.arrayForEach(currentViewers, function (username) {
            if (username !== shoppingListModel.CurrentUsername()) {
                shoppingListModel.viewers.push(username);
            }
        });
    };

    $.connection.hub.reconnected(function () {
        console.log("hub reconnected");
        shoppingListHub.server.startViewingList(shoppingListModel.Id()).done(function () {
            console.log("startViewingList done");
        });
    });

    $.connection.hub.disconnected(function () { // Occurs after a period of retrying the connection.
        console.log("hub disconnected");
    });

    $.connection.hub.connectionSlow(function () {
        console.log("connection.hub.connectionSlow");
    });

    $.connection.hub.error(function (error) {
        console.log("connection.hub.error: " + error);
        if (error.message === "WebSocket closed.") { // Occurs the instant the web socket connection is lost.
            bootbox.alert("Connection to the server has been lost!");
        }
    });

    $.connection.hub.stateChanged(function (change) {
        console.log("connection.hub.stateChanged: " + change.newState);
        //if (change.newState == signalR.connectionState.reconnecting) {
        //    shoppingListModel.storage = localStorage;
        //} else if (change.newState == signalR.connectionState.connected) {
        //    shoppingListModel.storage = serverStorage;
        //}
    });

    shoppingListHub.start = function (shoppingListModelToUse) {
        shoppingListModel = shoppingListModelToUse;
        $.connection.hub.start({ transport: 'webSockets' }, function () {
            console.log("hub connection started");

            shoppingListHub.server.startViewingList(shoppingListModel.Id()).done(function () {
                console.log("startViewingList done");
            });
        });
    };

    return shoppingListHub;
}