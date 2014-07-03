/*global post, UserModel, UserPermissionsEditModel */
"use strict";

function SharingModel(jsSharingModel, urls, serviceExceptions) {

    var userModelMapping, reportServiceError, userPermissionsEditModel;

    userModelMapping = {
        create: function (options) {
            return new UserModel(options.data);
        }
    };

    ko.mapping.fromJS(jsSharingModel, { Users: userModelMapping }, this);

    // Closure to display the user error message with extraErrorInfo:
    reportServiceError = function (extraErrorInfo) {
        return function (error) {
            console.log("Service error: " + error.responseJSON);
            if (extraErrorInfo === undefined) {
                extraErrorInfo = "<unknown>";
            }
            switch (error.responseJSON) {
            case serviceExceptions.UserNotFoundException:
                bootbox.alert("Couldn't find the user " + extraErrorInfo + " on the system.");
                break;
            case serviceExceptions.PermissionAlreadyExistsException:
                bootbox.alert("List is already shared with " + extraErrorInfo + ".");
                break;
            case serviceExceptions.PermissionNotFoundException:
                bootbox.alert("You don't have permission to share this list.");
                break;
            case serviceExceptions.ShareWithListCreatorException:
                bootbox.alert(extraErrorInfo + " is the list creator and already has access.");
                break;
            case serviceExceptions.ShareWithYourselfException:
                bootbox.alert(extraErrorInfo + " is you and you can't share with yourself.");
                break;
            default:
                bootbox.alert("A server error occured.");
                break;
            }
        };
    };

    this.shareWithUser = function () {
        bootbox.prompt({
            title: "Enter the name of a user who will share this list:",
            callback: function (promptResult) {
                if (promptResult !== null) {
                    if (promptResult.trim().length === 0) {
                        bootbox.alert("The username can't be blank.");
                        return;
                    }
                    post(urls.shareWithUser, { shoppingListId: this.Id(), username: promptResult }, this, function (newUser) {
                        var userModel = new UserModel(newUser);
                        this.Users.push(userModel);
                        userPermissionsEditModel.show(userModel, true);
                    }, reportServiceError(promptResult));
                }
            }.bind(this)
        });
    };

    this.backToList = function () {
        window.location.href = urls.show + this.Id();
    };

    this.backToMyLists = function () {
        window.location.href = urls.index;
    };

    this.userEditModel = ko.mapping.fromJS({
        formTitle: null,
        permissionModels: ko.observableArray(),
        userModel: null
    });

    this.removeUser = function (userModel) {
        post(urls.removeSharingUser, { shoppingListId: this.Id(), userSharingModel: ko.mapping.toJS(userModel) }, this, function () {
            this.Users.splice(this.Users.indexOf(userModel), 1);
        }, reportServiceError());
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
    userPermissionsEditModel = new UserPermissionsEditModel(this, urls, reportServiceError());
}
