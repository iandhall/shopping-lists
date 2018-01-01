/*global post, ShoppingListOverviewModel */
"use strict";

function ShoppingListsIndexModel(jsShoppingListsIndexModel) {

    var shoppingListOverviewModelMapping = {
        create: function (options) {
            return new ShoppingListOverviewModel(options.data);
        }
    };

    ko.mapping.fromJS(jsShoppingListsIndexModel, { MyLists: shoppingListOverviewModelMapping, ListsSharedWithMe: shoppingListOverviewModelMapping }, this);

    this.createNewList = function () {
        ajax.post("/api/shopping-lists/", null, this)
            .done(function (shoppingList) {
                window.location.href = "/ShoppingLists/Show/" + shoppingList.Id;
            }).fail(utils.onAjaxFailure);
    };

    this.showList = function (listModel) {
        window.location.href = "/ShoppingLists/Show/" + listModel.Id();
    };

    this.deleteList = function (listModel) {
        bootbox.confirm("Are you sure you want to delete " + listModel.Title() + "?", function (result) {
            if (result) {
                ajax.del("/api/shopping-lists/" + listModel.Id(), null, this)
                    .done(function () {
                        this.MyLists.splice(this.MyLists.indexOf(listModel), 1);
                    }).fail(utils.onAjaxFailure);
            }
        }.bind(this));
    };

    this.ignoreList = function (listModel) {
        bootbox.confirm("Are you sure you want to ignore " + listModel.Title() + "?", function (result) {
            if (result) {
                ajax.put("/api/shopping-lists/" + listModel.Id() + "/ignore", null, this)
                    .done(function () {
                        this.ListsSharedWithMe.splice(this.ListsSharedWithMe.indexOf(listModel), 1);
                    }).fail(utils.onAjaxFailure);
            }
        }.bind(this));
    };

    ko.applyBindings(this, $("#IndexModelRoot").get(0));
}