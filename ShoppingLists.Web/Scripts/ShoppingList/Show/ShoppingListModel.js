/*global ListItemModel, ListItemEditModel */
"use strict";

function ShoppingListModel(jsShoppingListModel, hub, urls, permissions) {

    var listItemMapping, tools, listItemEditModel;

    listItemMapping = {
        create: function (options) {
            return new ListItemModel(options.data);
        },
        key: function (data) {
            return ko.utils.unwrapObservable(data.Id);
        }
    };

    // Map the entire ShoppingList from a plain Javascript object to a Knockout observed object.
    ko.mapping.fromJS(jsShoppingListModel, { ListItems: listItemMapping }, this);

    this.permissions = permissions;
    this.viewers = ko.observableArray();
    this.viewerAlert = ko.observable();
    this.currentTool = null;

    tools = {
        pickItem: "pickItem",
        unpickItem: "unpickItem",
        editItem: "editItem",
        removeItem: "removeItem"
    };

    this.backToMyLists = function () {
        window.location.href = urls.index;
    };

    this.shareList = function () {
        if (!this.checkPermissionAlert(permissions.Share)) {
            return false; // Prevents the link from firing.
        }
        window.location.href = urls.share + this.Id();
    };

    this.changeListTitle = function () {
        if (!this.checkPermissionAlert(permissions.Edit)) {
            return;
        }
        bootbox.prompt({
            title: "Change this shopping list's title:",
            value: this.Title(),
            callback: function (promptResult) {
                if (promptResult === null) {
                    return;
                }
                if (promptResult.trim().length === 0) {
                    bootbox.alert("The list title can't be blank.");
                    return;
                }
                hub.server.update({ Id: this.Id(), Title: promptResult }); // Pass shoppingList without ListItems.
            }.bind(this)
        });
    };

    this.checkPermission = function (permissionTypeId) {
        if (this.CurrentUserPermissions().indexOf(permissionTypeId) !== -1) {
            return true;
        }
        return false;
    };

    this.checkPermissionAlert = function (permissionTypeId) {
        if (!this.checkPermission(permissionTypeId)) {
            bootbox.alert("The owner of this list hasn't given you permission to do that.");
            return false;
        }
        return true;
    };

    this.getEnabledStyle = function (permissionTypeId) {
        if (!this.checkPermission(permissionTypeId)) {
            return { color: "grey" };
        }
        return {};
    };

    this.setTool = function (toolName) {
        this.currentTool = toolName;
    };

    this.itemButtonClick = function (listItem) {
        switch (this.currentTool) {
        case tools.pickItem:
            this.pickListItem(listItem);
            break;
        case tools.unpickItem:
            this.unpickListItem(listItem);
            break;
        case tools.editItem:
            this.editListItem(listItem);
            break;
        case tools.removeItem:
            this.removeListItem(listItem);
            break;
        default:
            throw "Unexpected ShoppingListModel.currentTool - " + this.currentTool;
        }
    };

    this.addListItem = function () {
        if (!this.checkPermissionAlert(permissions.AddListItems)) {
            return;
        }
        listItemEditModel.show();
    };

    this.editListItem = function (listItem) {
        if (!this.checkPermissionAlert(permissions.EditListItems)) {
            return;
        }
        listItemEditModel.show(listItem);
    };

    this.unpickAllListItems = function () {
        if (!this.checkPermissionAlert(permissions.PickOrUnpickListItems)) {
            return;
        }
        bootbox.confirm("Are you sure you want to unpick all items?", function (result) {
            if (result) {
                hub.server.unpickAllListItems(this.Id());
            }
        }.bind(this));
    };

    this.removeListItem = function (listItem) {
        if (!this.checkPermissionAlert(permissions.RemoveListItems)) {
            return;
        }
        bootbox.confirm("Are you sure you want to remove " + listItem.Description() + "?", function (result) {
            if (result) {
                hub.server.deleteListItem(listItem.toJS());
            }
        }.bind(this));
    };

    this.pickListItem = function (listItem) {
        if (!this.checkPermissionAlert(permissions.PickOrUnpickListItems)) {
            return;
        }
        hub.server.pickListItem(listItem.toJS());
    };

    this.unpickListItem = function (listItem) {
        if (!this.checkPermissionAlert(permissions.PickOrUnpickListItems)) {
            return;
        }
        hub.server.unpickListItem(listItem.toJS());
    };

    this.findListItem = function (listItemId) {
        var listItem = ko.utils.arrayFirst(this.ListItems(), function (currentListItem) { // Find the existing ListItem within the array on the view model.
            return currentListItem.Id() === listItemId;
        });
        if (!listItem) {
            throw "findListItem: Existing ListItem not found - listItemId=" + listItemId;
        }
        return listItem;
    };

    // Apply Knockout bindings.
    ko.applyBindings(this, $("#ListModelRoot").get(0));

    // Create the ListItemEditModel (applies KO bindings to the modal popup form).
    listItemEditModel = new ListItemEditModel(hub, this.Id());

    // Call the checked radio's click handler to select the default list item tool.
    $('input[type="radio"][name="ListItemTools"]:checked').click();
}
