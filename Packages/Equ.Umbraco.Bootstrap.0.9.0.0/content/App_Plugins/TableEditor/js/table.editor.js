angular.module('umbraco').controller('TableEditorController', function ($scope) {
    
    //$scope.control.value = null;

    var emptyCellModel = '{"value": ""}';
    var defaultModel = {
        useFirstRowAsHeader: false,
        useLastRowAsFooter: false,
        tableStyle: null,
        columnStylesSelected: [
           null,
           null
        ],
        rowStylesSelected: [
           null,
           null,
           null
        ],
        cells: [
            [{ value: "" }, { value: "" }],
            [{ value: "" }, { value: "" }],
            [{ value: "" }, { value: "" }],
        ]
    }

    //ini
    $scope.control.value = $scope.control.value || defaultModel;
    $scope.showAdvanced = false;
    $scope.control.editor.config.tableStyles = parseStyleConfig($scope.control.editor.config.tableStyles);
    $scope.control.editor.config.columnStyles = parseStyleConfig($scope.control.editor.config.columnStyles);
    $scope.control.editor.config.rowStyles = parseStyleConfig($scope.control.editor.config.rowStyles);
 
    $scope.canAddRow = function () {
        if (isNaN(parseInt($scope.control.editor.config.maxRows, 10))) {
            return true;
        }

        return ($scope.control.editor.config.maxRows > $scope.control.value.cells.length);
    }

    $scope.addRow = function ($index) {
        if ($scope.canAddRow()) {
            var newRow = [];
            
            for (var i = 0; i < getColumnCount() ; i++) {
                newRow.push(emptyCellModel);
            }

            $scope.control.value.cells.splice($index + 1, 0, JSON.parse("[" + newRow.join(',') + "]"));
        }
    }

    $scope.canAddColumn = function () {

        if (isNaN(parseInt($scope.control.editor.config.maxColumns, 10))) {
            return true;   
        }

        return ($scope.control.editor.config.maxColumns > getColumnCount());
    }

    $scope.addColumn = function ($index) {
        if ($scope.canAddColumn()) {

            //style
            $scope.control.value.columnStylesSelected.splice($index + 1, 0, null);

            //cells
            for (var i in $scope.control.value.cells) {
                $scope.control.value.cells[i].splice($index + 1, 0, JSON.parse(emptyCellModel));
            }
        }
    }

    $scope.canRemoveRow = function () {
        return ($scope.control.value.cells.length > 1);
    }

    $scope.removeRow = function ($index) {
        if ($scope.canRemoveRow()) {
            if (confirm("Are you sure you'd like to remove this row?")) {
                $scope.control.value.cells.splice($index, 1);
            }
        }
    }

    $scope.canRemoveColumn = function () {
        return getColumnCount() > 1;
    }

    $scope.removeColumn = function ($index) {
        if ($scope.canRemoveColumn()) {
            if (confirm("Are you sure you'd like to remove this column?")) {
                $scope.control.value.columnStylesSelected.splice($index, 1);

                for (var i in $scope.control.value.cells) {
                    $scope.control.value.cells[i].splice($index, 1);
                }
            }
        }
    }

    $scope.canSort = function () {
        return ($scope.control.value.cells.length > 1);
    }

    //sort config
    $scope.sortableOptions = {
        axis: 'y',
        cursor: "move",
        handle: ".handle",
        update: function (ev, ui) {

        },
        stop: function (ev, ui) {

        }
    };

    $scope.clearTable = function () {
        if (confirm("Are you sure you wish to remove everything from the table?")) {
            $scope.control.value = defaultModel;
        }
    }

    function getColumnCount() {
        return $scope.control.value.cells[0].length;
    }

    function parseStyleConfig(configString) {
		if(!configString)
			return;
	
		if (typeof configString == 'string') {
		    //Col Style 1,col-style-1
		    var lines = configString.split(';');
		    var styles = [{ label: "None", value: null }];

		    for (var i in lines) {
		        var style = {};
		        var temp = lines[i].split(',');

		        if (temp[0].trim() != "" && temp[1].trim() != "") {
		            style.label = temp[0].trim();
		            style.value = temp[1].trim();

		            styles.push(style);
		        }
		    }

		    return styles;
		}
		else
		{
		    return configString;
		}
    }
}).directive('tableEditorRowControl', function () {

    var linker = function (scope, element, attrs) {

        var $rowControls = jQuery(element).find("td.row-buttons-td div");
        var $rowStyle = jQuery(element).find("td.row-style");
        var selectActive = false;

        $rowStyle.find("select").focus(function () {
            selectActive = true;
        });

        $rowStyle.find("select").blur(function () {
            selectActive = false;
        });

        element.bind('mouseover', function () {
            selectActive = false;
            $rowControls.show();
            
            element.addClass("row-highlighted");
			
			if($rowStyle.find('option').length > 1) {
				$rowStyle.css('visibility', 'visible');
			}
        });

        element.bind('mouseout', function () {
            if (selectActive == false) {
                $rowControls.hide();
                $rowStyle.css('visibility', 'hidden');
				//$rowStyle.find('select').hide();
                element.removeClass("row-highlighted");
            }
        });
    }

    return {
        restrict: "A",
        link: linker
    }
}).directive('tableEditorColumnControl', function () {

    var linker = function (scope, element, attrs) {

        //had to encapsulate all of the jquery in each function due to the dynamic nature of the prop editor

        element.bind('mouseover', function () {
            var $td = jQuery(element);
            var index = $td.index() + 1;
            var $table = $td.closest("table");
            var $tds = $table.find("tbody td:nth-child(" + index + ")");
            var $th1 = $table.find("thead tr:nth-child(1) th:nth-child(" + (index) + ")");
            var $th2 = $table.find("thead tr:nth-child(2) th:nth-child(" + (index) + ")");
            $tds.addClass("col-highlighted");
            $th1.addClass("col-highlighted");
            $th2.addClass("col-highlighted");
			
			$th1.css('visibility', 'visible');
			
			if($th1.find('option').length > 1) {
				$th1.find('select').css('visibility', 'visible');
			}	
        });

        element.bind('mouseout', function () {
            var $td = jQuery(element);
            var index = $td.index() + 1;
            var $table = $td.closest("table");
            var $tds = $table.find("tbody td:nth-child(" + index + ")");
            var $th1 = $table.find("thead tr:nth-child(1) th:nth-child(" + index + ")");
            var $th2 = $table.find("thead tr:nth-child(2) th:nth-child(" + (index) + ")");
            $tds.removeClass("col-highlighted");
            $th1.removeClass("col-highlighted");
            $th2.removeClass("col-highlighted");
			$th1.css('visibility', 'hidden');
            $th1.find('select').css('visibility', 'hidden');
        });
    }

    return {
        restrict: "A",
        link: linker
    }
});
