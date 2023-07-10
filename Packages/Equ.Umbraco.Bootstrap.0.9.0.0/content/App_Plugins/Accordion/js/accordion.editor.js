angular.module('umbraco').controller('AccordionEditorController', function ($scope) {

    var defaultModel = [{
        'title': '',
        'content':''
    }]

    //init
    $scope.control.value = $scope.control.value || defaultModel;

    $scope.addNewPane = function () {
        $scope.control.value.push({
            'title': '',
            'content':''
        });
    };

    $scope.remove = function (index) {
        $scope.control.value.splice(index, 1);
    }

    $scope.canMoveUp = function (index) {
        if( index > 0 )
        {
            return true;
        }

        return false;
    }

    $scope.moveUp = function (index) {
        if ($scope.canMoveUp(index)) {
            $scope.control.value = MoveItemInArray($scope.control.value, index, index - 1)
        }
    }

    $scope.canMoveDown = function (index) {
        if (index < $scope.control.value.length -1 ) {
            return true;
        }

        return false;
    }

    $scope.moveDown = function (index) {
        if ($scope.canMoveDown(index)) {
            $scope.control.value = MoveItemInArray($scope.control.value, index, index + 1)
        }
    }

    function MoveItemInArray(array, oldIndex, newIndex) {
        if (newIndex >= array.length) {
            var k = newIndex - array.length;
            while ((k--) + 1) {
                array.push(undefined);
            }
        }
        array.splice(newIndex, 0, array.splice(oldIndex, 1)[0]);
        return array;
    };


}).directive('paneEditorControl', function () {

    var linker = function (scope, element, attrs) {
        var $pane = jQuery(element)
        var selectActive = false;


        element.bind('mouseover', function () {
            selectActive = false;
            $pane.addClass('hover');
        });

        element.bind('mouseout', function () {
            if (selectActive == false) {
                $pane.removeClass('hover');
            }
        });
    }

    return {
        restrict: "A",
        link: linker
    }
});
