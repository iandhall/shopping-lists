/*global ListItemModel */
"use strict";

function ListItemEditModel(hub, shoppingListId) {

    var committing = false, validatedSelf;

    this.validationOn = ko.observable(false);
    this.formTitle = ko.observable();
    this.originalListItem = null;
    this.editing = null;

    this.description = ko.observable("").extend({
        required: true
    });

    this.descriptionErrorOn = ko.computed(function () {
        return (!this.description.isValid()) && this.validationOn();
    }, this);

    this.quantity = ko.observable().extend({
        required: true,
        pattern: {
            message: 'Quantity can contain only numbers.',
            params: '^[0-9]+$'
        },
        min: 1,
        max: 2147483647
    });

    this.quantityErrorOn = ko.computed(function () {
        return (!this.quantity.isValid()) && this.validationOn();
    }, this);

    this.show = function (listItemModel) {
        committing = false;
        this.validationOn(false);
        if (listItemModel !== undefined) {
            this.editing = true;
            this.formTitle("Change item details");
            this.description(listItemModel.Description());
            this.quantity(listItemModel.Quantity());
            this.originalListItem = listItemModel; // Store a reference to the original ListItemModel to retain additional information for posting back to the server.
        } else {
            this.editing = false;
            this.formTitle("Add new item");
            this.description("");
            this.quantity(1);
            this.originalListItem = null;
        }
        $("#ListItemEditForm").modal({ backdrop: "static" }).on('shown.bs.modal', function () {
            $("#ListItemEditForm-description").focus();
        });
    };

    validatedSelf = ko.validatedObservable(this);

    this.commit = function () {
        var jsListItem;

        if (committing) {
            return; // Prevent commit from being triggered twice when the return key is pressed twice quickly.
        }
        if (!validatedSelf.isValid()) {
            console.log("this.validationOn(true)");
            this.validationOn(true);
            return;
        }
        committing = true;

        $("#ListItemEditForm").modal("hide");

        if (this.originalListItem === null) {
            // Create a new ListItemModel.
            jsListItem = new ListItemModel(null, shoppingListId).toJS();
        } else {
            // Copy the original ListItemModel so we preserve fields like Id, ShoppingListId etc.
            jsListItem = this.originalListItem.toJS();
        }

        // Update the JavaScript ListItemModel with the ListItemEditModel changes:
        jsListItem.Description = this.description();
        jsListItem.Quantity = this.quantity();

        // Post the new/updated ListItemModel back to the server:
        if (this.editing) {
            hub.server.updateListItem(jsListItem);
        } else {
            hub.server.createListItem(jsListItem);
        }
    };

    ko.applyBindingsWithValidation(this, $("#ListItemEditForm").get(0), {
        decorateElementOnModified: false,
        insertMessages: false,
        messagesOnModified: false
    });

    // Confirm the dialog when the Return key is pressed.
    $("#ListItemEditForm").on("keyup", function (e) {
        if (e.which === 13) {
            this.commit();
        }
    }.bind(this));
}
