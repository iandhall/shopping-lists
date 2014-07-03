"use strict";

function PermissionModel(jsPermissionModel) {

    ko.mapping.fromJS(jsPermissionModel, {}, this);

    this.getCssClass = ko.computed(function () {
        if (this.Selected()) {
            return "btn-success";
        }
        return "btn-default";
    }, this);
}
