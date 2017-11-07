"use strict";

function ShoppingListOverviewModel(jsShoppingListOverviewModel) {

    ko.mapping.fromJS(jsShoppingListOverviewModel, {}, this); // Convert plain Javascript object to KO obect with observable fields.

    this.getShowListTooltip = ko.computed(function () {
        return "Show " + this.Title();
    }, this);

    this.getDeleteListTooltip = ko.computed(function () {
        return "Delete " + this.Title();
    }, this);

    this.getIgnoreListTooltip = ko.computed(function () {
        return "Ignore " + this.Title();
    }, this);

    this.getCaption = ko.computed(function () {
        return this.Title() + " (" + this.Creator() + ")";
    }, this);
}
