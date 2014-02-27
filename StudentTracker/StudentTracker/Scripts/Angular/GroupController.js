// create the module and name it scotchApp
var creatGroupApp = angular.module('creatGroupApp', ['ngRoute']);
var controllers = {};

controllers.newGroupController = function ($scope) {
    $scope.members = [];
};
controllers.individualController = function ($scope) {
        $scope.individuals = [];
    };

controllers.groupController = function ($scope) {
        $scope.groups = [];
    };

controllers.classController = function ($scope) {
    $scope.classes = [];
};

creatGroupApp.controller(controllers);
// configure our routes
scotchApp.config(function ($routeProvider) {
    $routeProvider

        // route for the home page
        .when('/', {

            controller: 'individualsController'
        })

        // route for the about page
        .when('/groups', {

            controller: 'groupController'
        })

        // route for the contact page
        .when('/class', {

            controller: 'classController'
        });
});

