
var groupApp = angular.module('groupApp', ['ngRoute', 'ui.bootstrap']);




groupApp.controller("groupParticipants", function ($scope, $http, $modal, $log) {
    $scope.newGroupM = [];

    $scope.selection = 'individuals';
    loadData();
    $scope.AddedRecipantEmails = '';
    $scope.AddedRecipantUserIds = '';
    function loadData() {
        $http.get('/Group/LoadGroups').success(function (data) {
            $scope.groups = data.Groups;
            $scope.individuals = data.Individuals;
            $scope.classes = data.Classes;
            var userIds = $('#Members').val();
            
            angular.forEach(userIds.split(","), function (value, key) {
                var index = $scope.individuals.indexOf(
                      $scope.individuals.filter(function (t) {
                          return t.UserId == value;
                      })[0]);
                console.log(userIds);
                //console.log($scope.individuals);
                if (index !== -1) {

                    var index1 = $scope.newGroupM.indexOf(
                      $scope.newGroupM.filter(function (t) {
                          return t.UserId == $scope.individuals[index].UserId;
                      })[0]);
                    if (index1 == -1) {
                        $scope.newGroupM.push($scope.individuals[index]);
                    }
                }
            });

            angular.forEach($scope.newGroupM, function (value, key) {
                $scope.AddedRecipantEmails += ($scope.AddedRecipantEmails == '' ? value.Email : ";" + value.Email);
                $scope.AddedRecipantUserIds += ($scope.AddedRecipantUserIds == '' ? value.UserId : "," + value.UserId);
            });
            // alert($scope.AddedRecipantEmails);
            $('#GroupMembers').val($scope.AddedRecipantEmails);
        });
    };

    $scope.AddRecipantDone = function () {
        $scope.AddedRecipantEmails = '';
        $scope.AddedRecipantUserIds = '';
        angular.forEach($scope.newGroupM, function (value, key) {
            $scope.AddedRecipantEmails += ($scope.AddedRecipantEmails == '' ? value.Email : ";" + value.Email);
            $scope.AddedRecipantUserIds += ($scope.AddedRecipantUserIds == '' ? value.UserId : "," + value.UserId);
        });
        $('#GroupMembers').val($scope.AddedRecipantEmails);
        $('#Members').val($scope.AddedRecipantUserIds);
        $('#dialog').modal('hide');
    };

    $scope.RemoveAll = function () {
        $scope.newGroupM = [];
    };

    $scope.removeIndividual = function (ind) {

        var index = $scope.newGroupM.indexOf(
           $scope.newGroupM.filter(function (t) {
               return t.UserName === ind.UserName;
           })[0]);

        if (index !== -1) {
            $scope.newGroupM.splice(index, 1);
        }
    };

    $scope.addGroup = function (group) {

        $http.get('/Group/LoadMembersOfGroup?groupId=' + group.GroupId)
            .success(function (result) {
                angular.forEach(result.GroupMembers, function (value, key) {
                    var index = $scope.newGroupM.indexOf(
                          $scope.newGroupM.filter(function (t) {
                              return t.UserId === value.UserId;
                          })[0]);

                    if (index == -1) {
                        $scope.newGroupM.push(value);
                    }
                });

            }).error(function (error) {

                alertify.error(error);
            });


    };

    $scope.addIndividual = function (ind) {

        $scope.newGroupM.push(ind);
    };

    $scope.AddAllIndividuals = function () {
        angular.forEach($scope.individuals, function (value, key) {
            $scope.newGroupM.push(value);
        });
    };

    $scope.addClass = function (clas) {
        $http.get('/Group/LoadMembersOfClass?classId=' + clas.ClassId)
            .success(function (result) {
                angular.forEach(result.ClassMembers, function (value, key) {
                    var index = $scope.newGroupM.indexOf(
                          $scope.newGroupM.filter(function (t) {
                              return t.UserId === value.UserId;
                          })[0]);

                    if (index == -1) {
                        $scope.newGroupM.push(value);
                    }
                });

            }).error(function (error) {

                alertify.error(error);
            });

        ////angular.forEach(clas.Members, function (value, key) {
        ////    $scope.newGroupM.push(value);
        ////});
    };
});







