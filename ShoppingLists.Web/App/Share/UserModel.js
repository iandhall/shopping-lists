"use strict";

function UserModel(jsUserModel) {
    ko.mapping.fromJS(jsUserModel, {}, this); // Convert plain Javascript object to KO obect with observable fields.

    this.getShareWithUserTooltip = ko.computed(function () {
        return "Edit " + this.UserName() + "'s permissions";
    }, this);

    this.getRemoveUserTooltip = ko.computed(function () {
        return "Remove " + this.UserName() + "'s access to the shopping list";
    }, this);
}
