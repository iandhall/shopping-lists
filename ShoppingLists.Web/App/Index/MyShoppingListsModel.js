/*global post, ShoppingListOverviewModel */
"use strict";

function MyShoppingListsModel(jsMyShoppingListsModel, urls) {

    var shoppingListOverviewModelMapping = {
        create: function (options) {
            return new ShoppingListOverviewModel(options.data);
        }
    };

    ko.mapping.fromJS(jsMyShoppingListsModel, { MyLists: shoppingListOverviewModelMapping, ListsSharedWithMe: shoppingListOverviewModelMapping }, this);

    this.createNewList = function () {
        window.location.href = urls.create;
    };

    this.showList = function (listModel) {
        window.location.href = urls.show + listModel.Id();
    };

    this.deleteList = function (listModel) {
        bootbox.confirm("Are you sure you want to delete " + listModel.Title() + "?", function (result) {
            if (result) {
                post(urls.deleteAction, { id: listModel.Id() }, this, function () {
                    this.MyLists.splice(this.MyLists.indexOf(listModel), 1);
                }, function (error) {
                    console.log(error);
                    bootbox.alert(error);
                });
            }
        }.bind(this));
    };

    this.ignoreList = function (listModel) {
        bootbox.confirm("Are you sure you want to ignore " + listModel.Title() + "?", function (result) {
            if (result) {
                post(urls.ignore, { id: listModel.Id() }, this, function () {
                    this.ListsSharedWithMe.splice(this.ListsSharedWithMe.indexOf(listModel), 1);
                });
            }
        }.bind(this));
    };

    ko.applyBindings(this);
}
