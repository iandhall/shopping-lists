/*global post, UserModel, UserPermissionsEditModel */
"use strict";

function SharingModel(jsSharingModel) {

    var userModelMapping, userPermissionsEditModel;

    userModelMapping = {
        create: function (options) {
            return new UserModel(options.data);
        }
    };

    ko.mapping.fromJS(jsSharingModel, { Users: userModelMapping }, this);

    this.shareWithUser = function () {
        bootbox.prompt({
            title: "Enter the name of a user who will share this list:",
            callback: function (promptResult) {
                if (promptResult !== null) {
                    if (promptResult.trim().length === 0) {
                        bootbox.alert("The username can't be blank.");
                        return;
                    }
                    ajax.put("/api/shopping-lists/" + this.Id() + "/share/" + promptResult, null, this)
                        .done(function (newUser) {
                            var userModel = new UserModel(newUser);
                            this.Users.push(userModel);
                            userPermissionsEditModel.show(userModel, true);
                        }).fail(utils.onAjaxFailure);
                }
            }.bind(this)
        });
    };

    this.backToList = function () {
        window.location.href = "/ShoppingLists/Show" + this.Id();
    };

    this.backToMyLists = function () {
        window.location.href = "/ShoppingLists/Index";
    };

    this.userEditModel = ko.mapping.fromJS({
        formTitle: null,
        permissionModels: ko.observableArray(),
        userModel: null
    });

    this.removeUser = function (userModel) {
        ajax.put("/api/shopping-lists/" + this.Id() + "/unshare/" + userModel.Id(), null, this)
            .done(function() {
                this.Users.splice(this.Users.indexOf(userModel), 1);
            }).fail(utils.onAjaxFailure);
    };

    this.editUserPermissions = function (userModel) {
        userPermissionsEditModel.show(userModel);
    };

    this.getHeading = ko.computed(function () {
        return this.Title() + " is currently shared with:";
    }, this);

    // Apply Knockout bindings.
    ko.applyBindings(this, $("#SharingModelRoot").get(0));

    // Create the UserPermissionsEditModel (applies KO bindings to the modal popup form):
    userPermissionsEditModel = new UserPermissionsEditModel(this);
}
