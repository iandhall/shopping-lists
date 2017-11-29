/*global post, PermissionModel */
"use strict";

function UserPermissionsEditModel(sharingModel) {

    var committing = false, permissionModelMapping;

    this.formTitle = ko.observable();
    this.permissionModels = ko.observableArray();
    this.userModel = null;

    permissionModelMapping = {
        create: function (options) {
            return new PermissionModel(options.data);
        }
    };
    
    this.show = function (userModel, shouldDefaultPermissions) {
        committing = false;
        if (shouldDefaultPermissions === undefined) {
            shouldDefaultPermissions = false;
        }
        ajax.get("/api/shopping-lists/" + sharingModel.Id() + "/permissions/" + userModel.Id() + "/" + shouldDefaultPermissions.toString(), this)
            .done(function (permissionModels) {
                this.permissionModels().length = 0; // Clear all array elements from last time.
                ko.utils.arrayForEach(permissionModels, function (permissionModel) { // Populate array with new permissions from the server.
                    this.permissionModels.push(ko.mapping.fromJS(permissionModel, permissionModelMapping));
                }.bind(this));
                this.formTitle("Allow " + userModel.UserName() + " to...");
                this.userModel = userModel;
                $("#UserPermissionsEditForm").modal({ backdrop: "static" });
            }).fail(utils.onAjaxFailure);
    };

    this.commit = function () {
        var data;
        if (committing) {
            return; // Prevent commit from being triggered twice when the return key is pressed twice quickly.
        }
        committing = true;
        $("#UserPermissionsEditForm").modal("hide");
        data = { selectedPermissionIds: [] };
        ko.utils.arrayForEach(this.permissionModels(), function (permissionModel) { // Populate plain JS array of permission IDs to post to the server.
            if (permissionModel.Selected()) {
                data.selectedPermissionIds.push(permissionModel.PermissionTypeId());
            }
        });
        ajax.put("/api/shopping-lists/" + sharingModel.Id() + "/permissions/" + this.userModel.Id(), data, this)
            .fail(utils.onAjaxFailure);
    };

    ko.applyBindings(this, $("#UserPermissionsEditForm").get(0));

    // Confirm the dialog when the Return key is pressed.
    $("#UserPermissionsEditForm").on("keyup", function (e) {
        if (e.which === 13) {
            this.commit();
        }
    }.bind(this));
}
