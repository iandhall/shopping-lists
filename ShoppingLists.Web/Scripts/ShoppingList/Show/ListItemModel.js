"use strict";

function ListItemModel(jsListItemModel, shoppingListId) {

    this.fromJS = function (jsListItemModel) {
        ko.mapping.fromJS(jsListItemModel, {}, this);
    };

    if (jsListItemModel !== null) {
        // Existing:
        this.fromJS(jsListItemModel);
    } else {
        // New:
        if (shoppingListId === undefined) {
            throw "A value for shoppingListId is requred if jsListItemModel is not supplied.";
        }
        this.Id = ko.observable(0); // Set to 0 otherwise SingnalR fails to read the model on the server.
        this.Description = ko.observable();
        this.Quantity = ko.observable(1);
        this.StatusId = ko.observable(1);
        this.ShoppingListId = ko.observable(shoppingListId);
    }

    // Sets the CSS class of the element representing this ListItem to the picked state.
    this.getCssClass = ko.computed(function () {
        if (this.StatusId() === 1) {
            return "btn-default";
        }
        if (this.StatusId() === 2) {
            return "btn-success";
        }
        throw "Unexpected ListItemModel.StatusId: " + this.StatusId();
    }, this);

    this.caption = ko.computed(function () {
        var caption = this.Description();
        if (this.Quantity() > 1) {
            caption += " X " + this.Quantity();
        }
        return caption;
    }, this);

    this.toJS = function () {
        return ko.mapping.toJS(this);
    };
}
